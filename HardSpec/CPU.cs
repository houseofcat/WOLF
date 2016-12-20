using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Wolf.HardSpec
{
    class CPU
    {
        public List<string> CPUInfo = new List<string>();
        public List<string> CPUInfo2 = new List<string>();

        public int intCPULength = 0;

        public CPU()
        {
        }

        public CPU(ManagementObject procInfo)
        {
            //Included a second function as the CPU class got more complex
            //and left the old code in.
            setCPUInfo2(procInfo);
        }

        public void setCPUInfo1()
        {
            CPUInfo.Clear();

            SelectQuery QueryCPU = new SelectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryCPU);

            foreach (ManagementObject item in searcher.Get())
            {
                if (item["Caption"] != null)
                {
                    CPUInfo.Add(item["Caption"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Name"] != null)
                {
                    CPUInfo.Add(funcConvertName(item["Name"].ToString()));
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Family"] != null)
                {
                    CPUInfo.Add(item["Family"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["CpuStatus"] != null)
                {
                    CPUInfo.Add(funcConvertStatus(item["CpuStatus"].ToString()));
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Architecture"] != null)
                {
                    CPUInfo.Add(funcConvertArch(item["Architecture"].ToString()));
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["AddressWidth"] != null)
                {
                    CPUInfo.Add(item["AddressWidth"].ToString() + "-bit");
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["MaxClockSpeed"] != null)
                {
                    CPUInfo.Add(item["MaxClockSpeed"].ToString() + " MHz");
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Manufacturer"] != null)
                {
                    CPUInfo.Add(item["Manufacturer"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["CurrentClockSpeed"] != null)
                {
                    CPUInfo.Add(item["CurrentClockSpeed"].ToString() + " MHz");
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["CurrentVoltage"] != null)
                {
                    CPUInfo.Add(item["CurrentVoltage"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["L2CacheSize"] != null)
                {
                    CPUInfo.Add(item["L2CacheSize"].ToString() + " KB");
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["L3CacheSize"] != null)
                {
                    CPUInfo.Add(item["L3CacheSize"].ToString() + " KB");
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["NumberOfCores"] != null)
                {
                    CPUInfo.Add(item["NumberOfCores"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["NumberOfLogicalProcessors"] != null)
                {
                    CPUInfo.Add(item["NumberOfLogicalProcessors"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Revision"] != null)
                {
                    CPUInfo.Add(item["Revision"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["Role"] != null)
                {
                    CPUInfo.Add(item["Role"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                if (item["SocketDesignation"] != null)
                {
                    CPUInfo.Add(item["SocketDesignation"].ToString());
                }
                else
                {
                    CPUInfo.Add("No Data. (xNull)");
                }

                intCPULength = 17;
            }
        }

        public void setCPUInfo2(ManagementObject procInfo)
        {
            CPUInfo2.Clear();
            intCPULength = 0;

            if (procInfo["DeviceID"] != null)
            {
                CPUInfo2.Add("Device ID: " + procInfo["DeviceID"].ToString());
            }
            else
            {
                CPUInfo2.Add("Device ID: No Data. (xNull)");
            }

            if (procInfo["Name"] != null)
            {
                CPUInfo2.Add("Name: " + funcConvertName(procInfo["Name"].ToString()));
            }
            else
            {
                CPUInfo2.Add("Name: No Data. (xNull)");
            }

            if (procInfo["Manufacturer"] != null)
            {
                CPUInfo2.Add("Manufacturer: " + procInfo["Manufacturer"].ToString());
            }
            else
            {
                CPUInfo2.Add("Manufacturer: No Data. (xNull)");
            }

            if (procInfo["CpuStatus"] != null)
            {
                CPUInfo2.Add("CPU Status: " + funcConvertStatus(procInfo["CpuStatus"].ToString()));
            }
            else
            {
                CPUInfo2.Add("CPU Status: No Data. (xNull)");
            }

            if (procInfo["Availability"] != null)
            {
                CPUInfo2.Add("Availability: " + funcConvertAvail(procInfo["Availability"].ToString()));
            }
            else
            {
                CPUInfo2.Add("Availability: No Data. (xNull)");
            }

            if (procInfo["Caption"] != null)
            {
                CPUInfo2.Add("Caption: " + procInfo["Caption"].ToString());
            }
            else
            {
                CPUInfo2.Add("Caption: No Data. (xNull)");
            }

            if (procInfo["Description"] != null)
            {
                CPUInfo2.Add("Description: " + procInfo["Description"].ToString());
            }
            else
            {
                CPUInfo2.Add("Description: No Data. (xNull)");
            }

            if (procInfo["CurrentClockSpeed"] != null)
            {
                CPUInfo2.Add("Current Clock Speed: " + procInfo["CurrentClockSpeed"].ToString() + " MHz");
            }
            else
            {
                CPUInfo2.Add("Current Clock Speed: No Data. (xNull)");
            }

            if (procInfo["MaxClockSpeed"] != null)
            {
                CPUInfo2.Add("Stock Clock Speed: " + procInfo["MaxClockSpeed"].ToString() + " MHz");
            }
            else
            {
                CPUInfo2.Add("Stock Clock Speed: No Data. (xNull)");
            }

            if (procInfo["ExtClock"] != null)
            {
                CPUInfo2.Add("External Clock Frequency: " + procInfo["ExtClock"].ToString() + " MHz");
            }
            else
            {
                CPUInfo2.Add("External Clock Frequency: No Data. (xNull)");
            }

            if (procInfo["LoadPercentage"] != null)
            {
                CPUInfo2.Add("Load Percentage: " + procInfo["LoadPercentage"].ToString() + "%");
            }
            else
            {
                CPUInfo2.Add("Load Percentage: No Data. (xNull)");
            }

            if (procInfo["NumberOfCores"] != null)
            {
                CPUInfo2.Add("Cores: " + procInfo["NumberOfCores"].ToString());
            }
            else
            {
                CPUInfo2.Add("Cores: No Data. (xNull)");
            }

            if (procInfo["NumberOfLogicalProcessors"] != null)
            {
                CPUInfo2.Add("Logical Processors: " + procInfo["NumberOfLogicalProcessors"].ToString());
            }
            else
            {
                CPUInfo2.Add("Logical Processors: No Data. (xNull)");
            }

            if (procInfo["Architecture"] != null)
            {
                CPUInfo2.Add("Architecture: " + funcConvertArch(procInfo["Architecture"].ToString()));
            }
            else
            {
                CPUInfo2.Add("Architecture: No Data. (xNull)");
            }

            if (procInfo["AddressWidth"] != null)
            {
                CPUInfo2.Add("Address Width: " + procInfo["AddressWidth"].ToString() + "-bit");
            }
            else
            {
                CPUInfo2.Add("Address Width: No Data. (xNull)");
            }

            if (procInfo["DataWidth"] != null)
            {
                CPUInfo2.Add("Data Width: " + procInfo["DataWidth"].ToString() + "-bit");
            }
            else
            {
                CPUInfo2.Add("Data Width: No Data. (xNull)");
            }

            if (procInfo["L2CacheSize"] != null)
            {
                CPUInfo2.Add("L2 Cache Size: " + procInfo["L2CacheSize"].ToString() + " KB");
            }
            else
            {
                CPUInfo2.Add("L2 Cache Size: No Data. (xNull)");
            }

            if (procInfo["L2CacheSpeed"] != null)
            {
                CPUInfo2.Add("L2 Cache Speed: " + procInfo["L2CacheSpeed"].ToString() + " MHz");
            }
            else
            {
                CPUInfo2.Add("L2 Cache Speed: No Data. (xNull)");
            }

            if (procInfo["L3CacheSize"] != null)
            {
                CPUInfo2.Add("L3 Cache Size: " + procInfo["L3CacheSize"].ToString() + " KB");
            }
            else
            {
                CPUInfo2.Add(": No Data. (xNull)");
            }

            if (procInfo["L3CacheSpeed"] != null)
            {
                CPUInfo2.Add("L3 Cache Speed: " + procInfo["L3CacheSpeed"].ToString() + " MHz");
            }
            else
            {
                CPUInfo2.Add("L3 Cache Speed: No Data. (xNull)");
            }

            if (procInfo["PowerManagementSupported"] != null)
            {
                CPUInfo2.Add("Power Management Supported: " + procInfo["PowerManagementSupported"].ToString());
            }
            else
            {
                CPUInfo2.Add("Power Management Supported: No Data. (xNull)");
            }

            if (procInfo["SocketDesignation"] != null)
            {
                CPUInfo2.Add("Socket Designation: " + procInfo["SocketDesignation"].ToString());
            }
            else
            {
                CPUInfo2.Add("Socket Designation: No Data. (xNull)");
            }

            if (procInfo["ProcessorType"] != null)
            {
                CPUInfo2.Add("Processor Type: " + funcConvertType(procInfo["ProcessorType"].ToString()));
            }
            else
            {
                CPUInfo2.Add("Processor Type: No Data. (xNull)");
            }

            if (procInfo["Role"] != null)
            {
                CPUInfo2.Add("Role: " + procInfo["Role"].ToString());
            }
            else
            {
                CPUInfo2.Add("Role: No Data. (xNull)");
            }

            if (procInfo["ProcessorId"] != null)
            {
                CPUInfo2.Add("Processor ID: " + procInfo["ProcessorId"].ToString());
            }
            else
            {
                CPUInfo2.Add("Processor ID: No Data. (xNull)");
            }

            if (procInfo["SystemName"] != null)
            {
                CPUInfo2.Add("System Name: " + procInfo["SystemName"].ToString());
            }
            else
            {
                CPUInfo2.Add("System Name: No Data. (xNull)");
            }

            if (procInfo["SystemCreationClassName"] != null)
            {
                CPUInfo2.Add("System Creation Class Name: " + procInfo["SystemCreationClassName"].ToString());
            }
            else
            {
                CPUInfo2.Add("System Creation Class Name: No Data. (xNull)");
            }

            if (procInfo["CreationClassName"] != null)
            {
                CPUInfo2.Add("Creation Class Name: " + procInfo["CreationClassName"].ToString());
            }
            else
            {
                CPUInfo2.Add("Creation Class Name: No Data. (xNull)");
            }

            if (procInfo["CurrentVoltage"] != null)
            {
                CPUInfo2.Add("Current Voltage: " + procInfo["CurrentVoltage"].ToString());
            }
            else
            {
                CPUInfo2.Add("Current Voltage: No Data. (xNull)");
            }

            if (procInfo["ConfigManagerErrorCode"] != null)
            {
                CPUInfo2.Add("Config Manager Error Code: " + procInfo["ConfigManagerErrorCode"].ToString());
            }
            else
            {
                CPUInfo2.Add("Config Manager Error Code: No Data. (xNull)");
            }

            if (procInfo["ConfigManagerUserConfig"] != null)
            {
                CPUInfo2.Add("Config Manager User Config: " + procInfo["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                CPUInfo2.Add("Config Manager User Config: No Data. (xNull)");
            }

            if (procInfo["Status"] != null)
            {
                CPUInfo2.Add("Status: " + procInfo["Status"].ToString());
            }
            else
            {
                CPUInfo2.Add("Status: No Data. (xNull)");
            }

            if (procInfo["StatusInfo"] != null)
            {
                CPUInfo2.Add("Status Info: " + funcConvertStatusInfo(procInfo["StatusInfo"].ToString()));
            }
            else
            {
                CPUInfo2.Add("Status Info: No Data. (xNull)");
            }

            if (procInfo["ErrorCleared"] != null)
            {
                CPUInfo2.Add("Error Cleared: " + procInfo["ErrorCleared"].ToString());
            }
            else
            {
                CPUInfo2.Add("Error Cleared: No Data. (xNull)");
            }

            if (procInfo["LastErrorCode"] != null)
            {
                CPUInfo2.Add("Last Error Code: " + procInfo["LastErrorCode"].ToString());
            }
            else
            {
                CPUInfo2.Add("Last Error Code: No Data. (xNull)");
            }

            if (procInfo["ErrorDescription"] != null)
            {
                CPUInfo2.Add("Error Description: " + procInfo["ErrorDescription"].ToString());
            }
            else
            {
                CPUInfo2.Add("Error Description: No Data. (xNull)");
            }

            if (procInfo["Family"] != null)
            {
                CPUInfo2.Add("Family: " + procInfo["Family"].ToString());
            }
            else
            {
                CPUInfo2.Add("Family: No Data. (xNull)");
            }

            if (procInfo["InstallDate"] != null)
            {
                CPUInfo2.Add("Install Date: " + procInfo["InstallDate"].ToString());
            }
            else
            {
                CPUInfo2.Add("Install Date: No Data. (xNull)");
            }
           

            if (procInfo["Level"] != null)
            {
                CPUInfo2.Add("Level: " + procInfo["Level"].ToString());
            }
            else
            {
                CPUInfo2.Add("Level: No Data. (xNull)");
            }

            if (procInfo["OtherFamilyDescription"] != null)
            {
                CPUInfo2.Add("Other Family Description: " + procInfo["OtherFamilyDescription"].ToString());
            }
            else
            {
                CPUInfo2.Add("Other Family Description: No Data. (xNull)");
            }

            if (procInfo["PNPDeviceID"] != null)
            {
                CPUInfo2.Add("PNP Device ID: " + procInfo["PNPDeviceID"].ToString());
            }
            else
            {
                CPUInfo2.Add("PNP Device ID: No Data. (xNull)");
            }

            if (procInfo["Revision"] != null)
            {
                CPUInfo2.Add("Revision: " + procInfo["Revision"].ToString());
            }
            else
            {
                CPUInfo2.Add("Revision: No Data. (xNull)");
            }
            
            if (procInfo["Stepping"] != null)
            {
                CPUInfo2.Add("Stepping: " + procInfo["Stepping"].ToString());
            }
            else
            {
                CPUInfo2.Add("Stepping: No Data. (xNull)");
            }

            if (procInfo["UniqueId"] != null)
            {
                CPUInfo2.Add("Unique ID: " + procInfo["UniqueId"].ToString());
            }
            else
            {
                CPUInfo2.Add("Unique ID: No Data. (xNull)");
            }

            if (procInfo["UpgradeMethod"] != null)
            {
                CPUInfo2.Add("Upgrade Method: " + procInfo["UpgradeMethod"].ToString());
            }
            else
            {
                CPUInfo2.Add("Upgrade Method: No Data. (xNull)");
            }

            if (procInfo["Version"] != null)
            {
                CPUInfo2.Add("Version: " + procInfo["Version"].ToString());
            }
            else
            {
                CPUInfo2.Add("Version: No Data. (xNull)");
            }

            if (procInfo["VoltageCaps"] != null)
            {
                CPUInfo2.Add("Voltage Caps: " + procInfo["VoltageCaps"].ToString());
            }
            else
            {
                CPUInfo2.Add("Voltage Caps: No Data. (xNull)");
            }


            intCPULength = 47;


        }

        /*
         * Index:
                1 Caption
                2 Name
                3 Family
                4 CpuStatus
                5 Architecture
                6 AddressWidth
                7 MaxClockSpeed
                8 Manufacturer
                9 CurrentClockSpeed
                10 CurrentVoltage
                11 L2CacheSize
                12 L3CacheSize
                13 NumberOfCores
                14 NumberOfLogicalProcessors
                15 Revision
                16 Role
                17 SocketDesignation

                17 Total
         */

        //Index = 0;
        public string GetCPUCaption()
        {
            return this.CPUInfo.ElementAt(0).ToString();
        }

        //Index = 1;
        public string GetCPUName()
        {
            string Name = "";

            Name = funcConvertName(this.CPUInfo.ElementAt(1).ToString());

            return Name;
        }

        //Index = 2;
        public string GetCPUFamily()
        {
            return this.CPUInfo.ElementAt(2).ToString();
        }

        //Index = 3;
        public string GetCPUStatus()
        {
            string Status = "";

            Status = funcConvertStatus(this.CPUInfo.ElementAt(3).ToString());

            return Status;
        }

        //Index = 4;
        public string GetCPUArchitecture()
        {
            string Arch = "";

            Arch = funcConvertArch(this.CPUInfo.ElementAt(4).ToString());

            return Arch;
        }

        //Index = 5;
        public string GetCPUAddressWidth()
        {
            return this.CPUInfo.ElementAt(5).ToString();
        }

        //Index = 6;
        public string GetCPUMaxFreq()
        {
            return this.CPUInfo.ElementAt(6).ToString();
        }

        //Index = 7;
        public string GetCPUMan()
        {
            return this.CPUInfo.ElementAt(7).ToString();
        }

        //Index = 8;
        public string GetCPUCurrentFreq()
        {
            return this.CPUInfo.ElementAt(8).ToString();
        }

        //Index = 9;
        public string GetCPUCurrentVolt()
        {
            return this.CPUInfo.ElementAt(9).ToString();
        }

        //Index = 10;
        public string GetCPUL2CacheSize()
        {
            return this.CPUInfo.ElementAt(10).ToString();
        }

        //Index = 11;
        public string GetCPUL3CacheSize()
        {
            return this.CPUInfo.ElementAt(11).ToString();
        }

        //Index = 12;
        public string GetCPUCores()
        {
            return this.CPUInfo.ElementAt(12).ToString();
        }

        //Index = 13;
        public string GetCPUThreads()
        {
            return this.CPUInfo.ElementAt(13).ToString();
        }

        //Index = 14;
        public string GetCPURevision()
        {
            return this.CPUInfo.ElementAt(14).ToString();
        }

        //Index = 15;
        public string GetCPURole()
        {
            return this.CPUInfo.ElementAt(15).ToString();
        }

        //Index = 16;
        public string GetCPUSocket()
        {
            return this.CPUInfo.ElementAt(16).ToString();
        }

        //Converts WMI number to meaningful String.
        private string funcConvertStatus(string Status)
        {
            if (Status == "0")
            {
                Status = "Unknown";
            }
            else if (Status == "1")
            {
                Status = "Enabled";
            }
            else if (Status == "2")
            {
                Status = "CPU Disabled in BIOS by User";
            }
            else if (Status == "3")
            {
                Status = "CPU Disabled in BIOS by POST Error";
            }
            else if (Status == "4")
            {
                Status = "CPU is Idle";
            }
            else if (Status == "5")
            {
                Status = "Reserved";
            }
            else if (Status == "6")
            {
                Status = "Reserved";
            }
            else if (Status == "7")
            {
                Status = "Other";
            }

            return Status;
        }

        //Converts WMI number to meaningful String.
        private string funcConvertArch(string Arch)
        {
            /*
            0 (0x0)
            x86
            1 (0x1)
            MIPS
            2 (0x2)
            Alpha
            3 (0x3)
            PowerPC
            5 (0x5)
            ARM
            6 (0x6)
            Itanium-based systems
            9 (0x9)
            x64
            */
            if (Arch == "0")
            {
                Arch = "x86";
            }
            else if (Arch == "1")
            {
                Arch = "MIPS";
            }
            else if (Arch == "2")
            {
                Arch = "Alpha";
            }
            else if (Arch == "3")
            {
                Arch = "PowerPC";
            }
            else if (Arch == "5")
            {
                Arch = "ARM";
            }
            else if (Arch == "6")
            {
                Arch = "Itanium";
            }
            else if (Arch == "9")
            {
                Arch = "x64";
            }

            return Arch;
        }

        //Filters useless Trademark and Register suffixes.
        private string funcConvertName(string Name)
        {
            Name = Name.Replace("(TM)", "");
            Name = Name.Replace("(R)", "");

            return Name;
        }

        //Converts WMI number to meaningful String.
        private string funcConvertAvail(string Avail)
        {
            if (Avail == "0")
            {
                Avail = "Super Unknown";
            }
            else if (Avail == "1")
            {
                Avail = "Other";
            }
            else if (Avail == "2")
            {
                Avail = "Unknown";
            }
            else if (Avail == "3")
            {
                Avail = "Running or Full Power";
            }
            else if (Avail == "4")
            {
                Avail = "Warning";
            }
            else if (Avail == "5")
            {
                Avail = "In Test";
            }
            else if (Avail == "6")
            {
                Avail = "Not Applicable";
            }
            else if (Avail == "7")
            {
                Avail = "Power Off";
            }
            else if (Avail == "8")
            {
                Avail = "Offline";
            }
            else if (Avail == "9")
            {
                Avail = "Off Duty";
            }
            else if (Avail == "10")
            {
                Avail = "Degraded";
            }
            else if (Avail == "11")
            {
                Avail = "Not Installed";
            }
            else if (Avail == "12")
            {
                Avail = "Install Error";
            }
            else if (Avail == "13")
            {
                Avail = "Power Save Mode: Unknown";
            }
            else if (Avail == "14")
            {
                Avail = "Power Save Mode: Low Power Mode";
            }
            else if (Avail == "15")
            {
                Avail = "Power Save Mode: Standy";
            }
            else if (Avail == "15")
            {
                Avail = "Power Cycling";
            }
            else if (Avail == "15")
            {
                Avail = "Power Save Mode Warning";
            }

            return Avail;
        }

        //Converts WMI number to meaningful String.
        private string funcConvertType(string Type)
        {
            if (Type == "0")
            {
                Type = "Super Unknown";
            }
            else if (Type == "1")
            {
                Type = "Other";
            }
            else if (Type == "2")
            {
                Type = "Unknown";
            }
            else if (Type == "3")
            {
                Type = "Central Processor";
            }
            else if (Type == "4")
            {
                Type = "Math Processor";
            }
            else if (Type == "5")
            {
                Type = "DSP Processor";
            }
            else if (Type == "6")
            {
                Type = "Video Processor";
            }

            return Type;
        }

        private string funcConvertStatusInfo(string Info)
        {
            if (Info == "0")
            {
                Info = "Super Unknown";
            }
            else if (Info == "1")
            {
                Info = "Other";
            }
            else if (Info == "2")
            {
                Info = "Unknown";
            }
            else if (Info == "3")
            {
                Info = "Enabled";
            }
            else if (Info == "4")
            {
                Info = "Disabled";
            }
            else if (Info == "5")
            {
                Info = "Not Applicable";
            }

            return Info;
        }
    }
}
