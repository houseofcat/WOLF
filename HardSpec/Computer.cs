using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolf.HardSpec
{
    class Computer : IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                os.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //Class constants.
        public OS os = new OS();
        public CPU cpu = new CPU();
        public Motherboard mb = new Motherboard();

        public List<OS> listOS = new List<OS>();

        public List<NetCon> ActCons = new List<NetCon>();
        public List<NetCon> InactCons = new List<NetCon>();

        public List<BIOS> listBIOS = new List<BIOS>();
        public List<SMBIOS> listSMBIOS = new List<SMBIOS>();
        public List<CPU> listCPU = new List<CPU>();
        public List<GPU> listGPU = new List<GPU>();
        public List<SoundDevice> listSD = new List<SoundDevice>();
        public List<ThermalProbe> listTP = new List<ThermalProbe>();
        public List<PNPMonitor> listMons = new List<PNPMonitor>();
        public List<MEM> listMEM = new List<MEM>();
        public List<NIC> listNIC = new List<NIC>();
        public List<LOGDRV> listLOGDRV = new List<LOGDRV>();
        public List<DSKDRV> listDSKDRV = new List<DSKDRV>();

        public string TotalMemory = "";
        public string MemorySpeed = "";
        public double dblTotalMemory = 0;
        public ulong uint64TotalMemory = 0;
        public short intMemModules = 0;

        private int intActiveCons = 0;
        private int intInactiveCons = 0;
        private int intProcCount = 0;

        //private int intCPUCount = 1;

        public Computer()
        {

        }

        public void funcLoadComputer()
        {
            intProcCount = Environment.ProcessorCount;

            funcPopulateCons();
            funcPopulateRAM();
            funcPopulateMB();
            funcPopulateNICs();
            funcPopulateLOGDRVs();
            funcPopulateDSKDRVs();
            funcPopulateGPUs();
            funcPopulateSoundDevices();
            funcPopulateMonitors();
            funcPopulateTPs();
            funcPopulateBIOSes();
            funcPopulateSMBIOSes();
            funcPopulateOSes();

            funcMisc();
        }

        public void funcPopulateCons()
        {
            foreach (NetworkInterface con in NetworkInterface.GetAllNetworkInterfaces())
            {
                NetCon temp = new NetCon(con);

                if ((temp.getConnName() != string.Empty) && (temp.getConnName() != null))
                {
                    if (temp.IsNICUp())
                    {
                        addActiveCon();
                        ActCons.Add(temp);
                    }
                }
            }
        }

        //Populate temporary/miscellaneous system info till its ready
        //for an individual function.
        private void funcMisc()
        {
            SelectQuery QueryTOTMEM = new SelectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryTOTMEM).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                double memSize = 0;
                if (double.TryParse(mo["TotalPhysicalMemory"]?.ToString() ?? "", out memSize))
                {
                    //Convert from Bytes to Gigabytes
                    memSize = memSize / 1041741824;

                    TotalMemory = memSize.ToString("0.##") + " GB " + "(" + intMemModules + "x DIMM)";
                }
            });
        }

        private void funcPopulateRAM()
        {
            SelectQuery QueryMEM = new SelectQuery("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryMEM).Get();

            intMemModules = 0;

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                MEM temp = new MEM(mo);

                listMEM.Add(temp);

                MemorySpeed = temp.MEMSpeed;

                intMemModules += 1;
            });
        }

        public string getTotalMemorySize()
        {
            return TotalMemory;
        }

        //Set to public so it can be handled in a background worker thread
        //controlled by GUI.  Will hang the UI for 1.5 seconds unless put
        //on a different thread.
        public void funcPopulateCPUs()
        {
            SelectQuery QueryCPUs = new SelectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryCPUs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                CPU temp = new CPU(mo);

                listCPU.Add(temp);
            });
        }

        public void funcPopulateMB()
        {
            SelectQuery QueryMBs = new SelectQuery("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryMBs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                Motherboard temp = new Motherboard(mo);

                mb = temp;
            });
        }

        public void funcPopulateGPUs()
        {
            SelectQuery QueryGPUs = new SelectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryGPUs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                GPU temp = new GPU(mo);

                listGPU.Add(temp);
            });
        }

        public void funcPopulateSoundDevices()
        {
            SelectQuery QuerySDs = new SelectQuery("SELECT * FROM Win32_SoundDevice");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QuerySDs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                SoundDevice temp = new SoundDevice(mo);

                listSD.Add(temp);
            });
        }

        public void funcPopulateMonitors()
        {
            SelectQuery QueryMONs = new SelectQuery("SELECT * FROM Win32_PnPEntity WHERE Service='monitor'");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryMONs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                PNPMonitor temp = new PNPMonitor(mo);

                listMons.Add(temp);
            });
        }

        public void funcPopulateTPs()
        {
            SelectQuery QueryTPs = new SelectQuery("SELECT * FROM Win32_TemperatureProbe");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryTPs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                ThermalProbe temp = new ThermalProbe(mo);

                listTP.Add(temp);
            });
        }

        public void funcPopulateBIOSes()
        {
            SelectQuery QueryBIOSes = new SelectQuery("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryBIOSes).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                BIOS temp = new BIOS(mo);

                listBIOS.Add(temp);
            });
        }

        public void funcPopulateSMBIOSes()
        {
            SelectQuery QuerySMBIOSes = new SelectQuery("SELECT * FROM Win32_SMBIOSMemory");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QuerySMBIOSes).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                SMBIOS temp = new SMBIOS(mo);

                listSMBIOS.Add(temp);
            });
        }

        private void funcPopulateNICs()
        {
            SelectQuery QueryNICs = new SelectQuery("SELECT * FROM WIN32_NetworkAdapter");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryNICs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                NIC temp = new NIC(mo);

                listNIC.Add(temp);
            });
        }

        private void funcPopulateLOGDRVs()
        {
            SelectQuery QueryDRVs = new SelectQuery("SELECT * FROM WIN32_LogicalDisk");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryDRVs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                LOGDRV temp = new LOGDRV(mo);

                listLOGDRV.Add(temp);
            });
        }

        private void funcPopulateDSKDRVs()
        {
            SelectQuery QueryDRVs = new SelectQuery("SELECT * FROM WIN32_DiskDrive");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryDRVs).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                DSKDRV temp = new DSKDRV(mo);

                listDSKDRV.Add(temp);
            });
        }

        private void funcPopulateOSes()
        {
            SelectQuery QueryOS = new SelectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryOS).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                OS temp = new OS(mo);

                listOS.Add(temp);
            });
        }

        private void addActiveCon()
        {
            intActiveCons += 1;
        }

        private void addInactiveCon()
        {
            intInactiveCons += 1;
        }

        public int getActiveCons()
        {
            return intActiveCons;
        }

        public int getInactiveCons()
        {
            return intInactiveCons;
        }

    }
}
