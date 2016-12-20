using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;

namespace Wolf.HardSpec
{
    public class RemoteMachine
    {
        public List<string> RemoteInfo = new List<string>();
        public List<string> Export = new List<string>();
        public List<string> UserList = new List<string>();
        public List<string> CPUList = new List<string>();

        public string WorkStation = "";

        public string lastBootup = "";
        public string installDate = "";
        public string computerName = "";

        public string computerArch = "";
        public string computerOS = "";
        public string computerOSVersion = "";
        public string computerMAC = "";
        public string computerIP = "";
        public string computerRAM = "";

        public bool userChecked = false;
        public bool local = false;
        public string localUser = "";
        public string computerUser = "";

        public string GPU = "";
        public string GPUDriver = "";

        public string OEM = "";
        public string BIOS_VERSION = "";
        public string BIOS_SERIAL = "";

        public bool RemoteMachineFullyLoaded = false;
        public long ElapsedTime = 0;

        //Future Code Hopefully
        //String EmbeddedKey = "";
        //String OSKey = "";

        public RemoteMachine()
        {
             RemoteMachineFullyLoaded = false;
        }

        public RemoteMachine(string Domain, string Username, string Userpass, string Workstation, bool[] RemChoices)
        {
            RemoteMachineFullyLoaded = false;

            this.WorkStation = Workstation;
            //MessageBox.Show("Inside RM1 - IP:" + Workstation + ":");

            getRemoteMachineOSInfo(Domain, Username, Userpass, Workstation, RemChoices);

            funcSetRemoteData();

            if (RemoteInfo.Any())
            {
                RemoteMachineFullyLoaded = true;
            }
        }

        private void getRemoteMachineOSInfo(string Domain, string Username, string Userpass, string Workstation, bool[] RemChoices)
        {
            List<string> RemoteInfo = new List<string>();

            string format = "G";
            DateTime tempDate = new DateTime(1900, 1, 1);
            ManagementScope newMS = new ManagementScope();
            ManagementObjectSearcher newMOS = new ManagementObjectSearcher();

            if (!(Environment.MachineName.Equals(Workstation, StringComparison.OrdinalIgnoreCase)))
            {
                ConnectionOptions newCon = new ConnectionOptions();
                newCon.Username = Username;
                newCon.Password = Userpass;
                newCon.Impersonation = ImpersonationLevel.Impersonate;
                newCon.Timeout = new TimeSpan(0, 0, 0, 2);

                if (Domain == "No Domain")
                {
                    Domain = "WORKGROUP";
                }
                else
                {
                    newCon.Authority = "ntlmdomain:" + Domain;
                }

                newMS = new ManagementScope("\\\\" + Workstation.Trim() + "\\root\\CIMV2", newCon);
            }
            else
            {
                newMS = new ManagementScope("\\\\.\\root\\CIMV2");
                local = true;
                localUser = Username;
            }

            //RemChoices Option Array
            //[0] - OS Info
            //[1] - BIOS Info
            //[2] - CPU Info
            //[3] - GPU Info
            //[4] - IP Address
            //[5] - MAC Address
            //[6] - User Logged On
            //[7] - All Activer User Accounts

            newMS.Connect();

            if (newMS.IsConnected)
            {
                //OS Section
                if (RemChoices[0])
                {
                    ObjectQuery osQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                    newMOS = new ManagementObjectSearcher(newMS, osQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            tempDate = ManagementDateTimeConverter.ToDateTime(item["InstallDate"].ToString());
                            this.installDate = tempDate.ToString(format);
                        }
                        catch (ManagementException ex)
                        {
                            this.installDate = ex.Message;
                            continue;
                        }

                        try
                        {
                            tempDate = ManagementDateTimeConverter.ToDateTime(item["LastBootUpTime"].ToString());
                            this.lastBootup = tempDate.ToString(format);
                        }
                        catch (ManagementException ex)
                        {
                            this.lastBootup = ex.Message;
                            continue;
                        }

                        try
                        {
                            this.computerName = item["CSName"].ToString();
                        }
                        catch (ManagementException ex)
                        {
                            this.computerName = ex.Message;
                            continue;
                        }

                        try
                        {
                            this.computerArch = item["OSArchitecture"].ToString();
                        }
                        catch (ManagementException ex)
                        {
                            this.computerArch = ex.Message;
                            continue;
                        }

                        try
                        {
                            this.computerOS = item["Caption"].ToString().Replace("Microsoft ", "");
                        }
                        catch (ManagementException ex)
                        {
                            this.computerOS = ex.Message;
                            continue;
                        }

                        try
                        {
                            this.computerOSVersion = item["Version"].ToString();
                        }
                        catch (ManagementException ex)
                        {
                            this.computerOSVersion = ex.Message;
                            continue;
                        }

                        try
                        {
                            this.computerRAM = Tools.convertToGBFromKB(item["TotalVisibleMemorySize"].ToString());
                        }
                        catch (ManagementException ex)
                        {
                            this.computerRAM = ex.Message;
                            continue;
                        }
                    }
                }

                //BIOS Section
                if (RemChoices[1])
                {
                    ObjectQuery biosQuery = new ObjectQuery("SELECT * FROM Win32_BIOS");
                    newMOS = new ManagementObjectSearcher(newMS, biosQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            if (item["SerialNumber"] != null)
                            {
                                this.BIOS_SERIAL = item["SerialNumber"].ToString();
                            }
                        }
                        catch (ManagementException ex)
                        {
                            this.BIOS_SERIAL = ex.Message;
                            continue;
                        }

                        try
                        {
                            if (item["Manufacturer"] != null)
                            {
                                this.OEM = item["Manufacturer"].ToString();
                            }
                        }
                        catch (ManagementException ex)
                        {
                            this.OEM = ex.Message;
                            continue;
                        }

                        try
                        {
                            if (item["SMBIOSBIOSVersion"] != null)
                            {
                                this.BIOS_VERSION = item["SMBIOSBIOSVersion"].ToString();
                            }
                        }
                        catch (ManagementException ex)
                        {
                            this.BIOS_VERSION = ex.Message;
                            continue;
                        }
                    }
                }

                //CPU Section
                if (RemChoices[2])
                {
                    ObjectQuery cpuQuery = new ObjectQuery("SELECT * FROM Win32_Processor");
                    newMOS = new ManagementObjectSearcher(newMS, cpuQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            string temp = "";
                            temp = item["Name"].ToString().Replace("(R)", "");
                            temp = temp.Replace("(TM)", "");
                            temp = temp.Replace("CPU", "");
                            temp = temp.Replace("    ", "");
                            temp = temp.Replace("   ", " ");
                            temp = temp.Replace("  ", " ");

                            CPUList.Add(temp);
                        }
                        catch (ManagementException ex)
                        {
                            CPUList.Add(ex.Message);
                            continue;
                        }
                    }
                }

                //GPU Section
                if (RemChoices[3])
                {
                    ObjectQuery gpuQuery = new ObjectQuery("SELECT * FROM Win32_VideoController");
                    newMOS = new ManagementObjectSearcher(newMS, gpuQuery);
                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            if (item["VideoProcessor"] != null)
                            {
                                GPU = item["VideoProcessor"].ToString();
                            }
                            else
                            {
                                GPU = "Unknown/Unreadable.";
                            }

                            if (item["DriverVersion"] != null)
                            {
                                GPUDriver = item["DriverVersion"].ToString();
                            }
                        }
                        catch (ManagementException ex)
                        {
                            GPU = ex.Message;
                            continue;
                        }
                    }
                }

                //MAC Section
                if (RemChoices[4])
                {
                    ObjectQuery netQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
                    newMOS = new ManagementObjectSearcher(newMS, netQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            if (item["MACAddress"] != null)
                            {
                                this.computerMAC = item["MACAddress"].ToString();
                            }
                        }
                        catch (ManagementException ex)
                        {
                            this.computerMAC = ex.Message;
                            continue;
                        }
                    }
                }

                //IP Address
                if (RemChoices[5])
                {
                    ObjectQuery netConQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='True'");
                    newMOS = new ManagementObjectSearcher(newMS, netConQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            Object obj = item["IPAddress"];

                            string[] strArray = ((System.Collections.IEnumerable)obj).Cast<object>().Select(x => x.ToString()).ToArray();

                            this.computerIP = strArray[0];
                        }
                        catch (ManagementException ex)
                        {
                            this.computerIP = ex.Message;
                            continue;
                        }
                    }
                }

                //User Section
                if (RemChoices[6])
                {
                    ObjectQuery userQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                    newMOS = new ManagementObjectSearcher(newMS, userQuery);

                    foreach (ManagementObject item in newMOS.Get())
                    {
                        try
                        {
                            if (item["UserName"] != null)
                            {
                                userChecked = true;

                                if (local)
                                {
                                    this.computerUser = localUser;
                                }
                                else
                                {
                                    this.computerUser = item["UserName"].ToString();
                                }
                            }
                        }
                        catch (ManagementException ex)
                        {
                            this.computerUser = ex.Message;
                            continue;
                        }
                    }
                }

                /*
                 * This code segment handles ONLY local users logged in.
                ObjectQuery user2Query = new ObjectQuery("SELECT * FROM Win32_LogonSession WHERE LogonType=2");
                newMOS = new ManagementObjectSearcher(newMS, user2Query);

                foreach (ManagementObject WmiObject in newMOS.Get())
                {
                    //Console.WriteLine("{0,-35} {1,-40}", "LogonId", WmiObject["LogonId"]);// String
                    ObjectQuery LQuery = new ObjectQuery("Associators of {Win32_LogonSession.LogonId=" + WmiObject["LogonId"] + "} Where AssocClass=Win32_LoggedOnUser Role=Dependent");
                    ManagementObjectSearcher LSearcher = new ManagementObjectSearcher(newMS, LQuery);

                    foreach (ManagementObject LWmiObject in LSearcher.Get())
                    {
                        try
                        {
                            if (!(UserList.Contains(LWmiObject["Name"].ToString())))
                            {
                                UserList.Add(LWmiObject["Name"].ToString());
                            }
                        }
                        catch (ManagementException ex)
                        {
                            UserList.Add(ex.Message);
                            continue;
                        }
                    }
                }*/


                //All Active User Accounts Sections
                // This alternative code segments seeks all user acounts in use, service/built-ins included.
                if (RemChoices[7])
                {
                    ObjectQuery user2Query = new ObjectQuery("SELECT * FROM Win32_LogonSession WHERE LogonType=2 OR LogonType=3 OR LogonType=5 OR LogonType=10");
                    newMOS = new ManagementObjectSearcher(newMS, user2Query);

                    foreach (ManagementObject WmiObject in newMOS.Get())
                    {
                        //Console.WriteLine("{0,-35} {1,-40}", "LogonId", WmiObject["LogonId"]);// String
                        ObjectQuery LQuery = new ObjectQuery("Associators of {Win32_LogonSession.LogonId=" + WmiObject["LogonId"] + "} Where AssocClass=Win32_LoggedOnUser Role=Dependent");
                        ManagementObjectSearcher LSearcher = new ManagementObjectSearcher(newMS, LQuery);

                        foreach (ManagementObject LWmiObject in LSearcher.Get())
                        {
                            try
                            {
                                if (!(UserList.Contains(LWmiObject["Name"].ToString())))
                                {
                                    UserList.Add(LWmiObject["Name"].ToString());
                                }
                            }
                            catch (ManagementException ex)
                            {
                                UserList.Add(ex.Message);
                                continue;
                            }
                        }
                    }
                }
            }
        }

        //This function should only attach data that was collected, if the strings are
        //empty they simply are not added to the list.  This will also reduce the size
        //in GUI if options aren't checked.
        private void funcSetRemoteData()
        {
            if (computerName != "")
            {
                RemoteInfo.Add("Machine Name: " + computerName);
                Export.Add(computerName);
            }

            if (computerUser != "")
            {
                RemoteInfo.Add("User Name: " + computerUser);
                Export.Add(computerUser);
            }
            else if (userChecked)
            {
                RemoteInfo.Add("User Name: No Standard Users.");
                Export.Add("No Standard Users.");
            }

            //Handles multiple local user accounts, including service/built-in accounts.
            if (UserList.Any())
            {
                foreach (string name in UserList)
                {
                    RemoteInfo.Add("Active Account: " + name);
                    Export.Add(name);
                }
            }

            if (computerIP != "")
            {
                RemoteInfo.Add("IP Address: " + computerIP);
                Export.Add(computerIP);
            }

            if (computerMAC != "")
            {
                RemoteInfo.Add("MAC: " + computerMAC);
                Export.Add(computerMAC);
            }

            int cpuCounter = 0;

            //Handles multiple CPUs.
            if (CPUList.Any())
            {
                foreach (string cpu in CPUList)
                {
                    RemoteInfo.Add("CPU" + cpuCounter + ": " + cpu);
                    Export.Add(cpuCounter + ": " + cpu);
                    cpuCounter++;
                }
            }

            if (GPU != "")
            {
                RemoteInfo.Add("GPU: " + GPU);
                Export.Add(GPU);
            }

            if (GPUDriver != "")
            {
                RemoteInfo.Add("GPU Driver: " + GPUDriver);
                Export.Add(GPUDriver);
            }

            if (computerRAM != "")
            {
                RemoteInfo.Add("RAM: " + computerRAM);
                Export.Add(computerRAM);
            }

            if (computerOS != "")
            {
                RemoteInfo.Add("OS: " + computerOS + " " + computerArch);
                Export.Add(computerOS + " " + computerArch);
            }

            if (computerOSVersion != "")
            {
                RemoteInfo.Add("OS Version: " + computerOSVersion);
                Export.Add(computerOSVersion);
            }

            if (installDate != "")
            {
                RemoteInfo.Add("Windows Installed: " + installDate);
                Export.Add(installDate);
            }

            if (lastBootup != "")
            {
                RemoteInfo.Add("Last Reboot: " + lastBootup);
                Export.Add(lastBootup);
            }

            if (OEM != "")
            {
                RemoteInfo.Add("OEM: " + OEM);
                Export.Add(OEM);
            }

            if (BIOS_SERIAL != "")
            {
                RemoteInfo.Add("Serial: " + BIOS_SERIAL);
                Export.Add(BIOS_SERIAL);
            }

            if (BIOS_VERSION != "")
            {
                RemoteInfo.Add("BIOS Version: " + BIOS_VERSION);
                Export.Add(BIOS_VERSION);
            }
        }

        public void funcClear()
        {
            RemoteInfo.Clear();
            UserList.Clear();
            CPUList.Clear();

            computerName = "";
            computerOS = "";
            computerArch = "";
            computerOSVersion = "";
            installDate = "";
            lastBootup = "";
            computerMAC = "";
            computerIP = "";

            computerUser = "";
            localUser = "";
            userChecked = false;
            local = false;

            GPU = "";
            GPUDriver = "";

            OEM = "";
            BIOS_VERSION = "";
            BIOS_SERIAL = "";
        }
    }
}
