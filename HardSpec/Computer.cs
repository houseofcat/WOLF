using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryTOTMEM);

            foreach (ManagementObject item in searcher.Get())
            {
                double memSize = 0;
                double.TryParse(item["TotalPhysicalMemory"].ToString(), out memSize);

                //Convert from Bytes to Gigabytes
                memSize = memSize / 1041741824;

                TotalMemory = memSize.ToString("0.##") + " GB " + "(" + intMemModules + "x DIMM)";
            }
        }

        private void funcPopulateRAM()
        {
            SelectQuery QueryMEM = new SelectQuery("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryMEM);

            intMemModules = 0;

            foreach (ManagementObject obj in searcher.Get())
            {
                MEM temp = new MEM(obj);

                listMEM.Add(temp);

                MemorySpeed = temp.MEMSpeed;

                intMemModules += 1;
            }
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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryCPUs);

            foreach (ManagementObject procInfo in searcher.Get())
            {
                CPU temp = new CPU(procInfo);

                listCPU.Add(temp);
            }
        }

        public void funcPopulateMB()
        {
            try
            {
                SelectQuery QueryMBs = new SelectQuery("SELECT * FROM Win32_BaseBoard");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryMBs);

                foreach (ManagementObject mbInfo in searcher.Get())
                {
                    Motherboard temp = new Motherboard(mbInfo);

                    mb = temp;
                }
            }
            catch(Exception Bug)
            {
                MessageBox.Show(Bug.ToString());
            }
        }

        public void funcPopulateGPUs()
        {
            SelectQuery QueryGPUs = new SelectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryGPUs);

            foreach (ManagementObject gpuInfo in searcher.Get())
            {
                GPU temp = new GPU(gpuInfo);

                listGPU.Add(temp);
            }
        }

        public void funcPopulateSoundDevices()
        {
            SelectQuery QuerySDs = new SelectQuery("SELECT * FROM Win32_SoundDevice");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QuerySDs);

            foreach (ManagementObject sdInfo in searcher.Get())
            {
                SoundDevice temp = new SoundDevice(sdInfo);

                listSD.Add(temp);
            }
        }

        public void funcPopulateMonitors()
        {
            SelectQuery QueryMONs = new SelectQuery("SELECT * FROM Win32_PnPEntity WHERE Service='monitor'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryMONs);

            foreach (ManagementObject monInfo in searcher.Get())
            {
                PNPMonitor temp = new PNPMonitor(monInfo);

                listMons.Add(temp);
            }
        }

        public void funcPopulateTPs()
        {
            SelectQuery QueryTPs = new SelectQuery("SELECT * FROM Win32_TemperatureProbe");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryTPs);

            foreach (ManagementObject tpInfo in searcher.Get())
            {
                ThermalProbe temp = new ThermalProbe(tpInfo);

                listTP.Add(temp);
            }
        }

        public void funcPopulateBIOSes()
        {
            SelectQuery QueryBIOSes = new SelectQuery("SELECT * FROM Win32_BIOS");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryBIOSes);

            foreach (ManagementObject BIOSInfo in searcher.Get())
            {
                BIOS temp = new BIOS(BIOSInfo);

                listBIOS.Add(temp);
            }
        }

        public void funcPopulateSMBIOSes()
        {
            SelectQuery QuerySMBIOSes = new SelectQuery("SELECT * FROM Win32_SMBIOSMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QuerySMBIOSes);

            foreach (ManagementObject SMInfo in searcher.Get())
            {
                SMBIOS temp = new SMBIOS(SMInfo);

                listSMBIOS.Add(temp);
            }
        }

        private void funcPopulateNICs()
        {
            SelectQuery QueryNICs = new SelectQuery("SELECT * FROM WIN32_NetworkAdapter");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryNICs);
            
            foreach (ManagementObject Adapt in searcher.Get())
            {
                NIC temp = new NIC(Adapt);

                listNIC.Add(temp);
            }
        }

        private void funcPopulateLOGDRVs()
        {
            SelectQuery QueryDRVs = new SelectQuery("SELECT * FROM WIN32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryDRVs);

            foreach (ManagementObject Drive in searcher.Get())
            {
                LOGDRV temp = new LOGDRV(Drive);

                listLOGDRV.Add(temp);
            }
        }

        private void funcPopulateDSKDRVs()
        {
            SelectQuery QueryDRVs = new SelectQuery("SELECT * FROM WIN32_DiskDrive");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryDRVs);

            foreach (ManagementObject Drive in searcher.Get())
            {
                DSKDRV temp = new DSKDRV(Drive);

                listDSKDRV.Add(temp);
            }
        }

        private void funcPopulateOSes()
        {
            SelectQuery QueryOS = new SelectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryOS);

            foreach (ManagementObject item in searcher.Get())
            {
                OS temp = new OS(item);

                listOS.Add(temp);
            }
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
