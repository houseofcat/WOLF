﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Wolf.SoftSpec;
using Wolf.WolfSpec;
using System.Threading.Tasks;

namespace Wolf
{
    class OS : IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BWACCOUNTS.Dispose();
                BWIRQS.Dispose();
                BWNET.Dispose();

                allDrivers.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TaskManager tm = new TaskManager();
        public IPL allPrograms = new IPL();
        public DRVL allDrivers = new DRVL();
        public NetList NetPortList = new NetList();
        public List<Account> UserAccounts = new List<Account>();
        public List<Account> GroupAccounts = new List<Account>();
        public List<Account> DomainAccounts = new List<Account>();
        public List<Account> AliasAccounts = new List<Account>();
        public List<Account> WKGAccounts = new List<Account>();
        public List<Account> DeletedAccounts = new List<Account>();
        public List<Account> InvalidAccounts = new List<Account>();
        public List<Account> UnknownAccounts = new List<Account>();
        public List<Account> ComputerAccounts = new List<Account>();
        public List<IRQ> IRQs = new List<IRQ>();
        public List<DiskPartition> DPs = new List<DiskPartition>();

        public Stopwatch AccountTimer = new Stopwatch();
        public Stopwatch NPTimer = new Stopwatch();
        public Stopwatch IRQTimer = new Stopwatch();
        public long AccountElapsedTime = 0;
        public long NetPortElapsedTime = 0;
        public long IRQElapsedTime = 0;

        private BackgroundWorker BWACCOUNTS = new BackgroundWorker();
        private bool AccountsLoaded = false;
        public bool AccountErrorOccurred = false;
        public string AccountErrorMessage = "";

        private BackgroundWorker BWIRQS = new BackgroundWorker();
        private bool IRQsLoaded = false;
        public bool IRQErrorOccured = false;
        public string IRQErrorMessage = "";

        public BackgroundWorker BWNET = new BackgroundWorker();
        public bool NetPortsLoaded = false;
        public bool NetPortsErrorOccurred = false;
        public string NetPortsErrorMessage = "";

        //Class variables.
        string ComputerName = "";
        string UserName = "";
        string OSName = "";
        string OSVersion = "";
        string NETVersion = "";
        string DomainName = "";
        string WindowsInstall = "";
        string Localization = "";

        string OpersName = "";
        string OpersVersion = "";
        string OpersDir = "";
        string OpersRegistered = "";
        string OpersSP = "";
        string OpersInstall = "";

        private string strIPv4 = "";
        private string strIPv6 = "N/A";
        private string strExtIP = "Click Button";
        public UInt32 allOSLength = 0;

        List<string> OSInfo = new List<string>();
        List<string> MemConfig = new List<string>();
   
        public List<string> AllOSInfo = new List<string>();

        int intMemConfigLength = 0;

        public OS()
        {
            InitializeBackgroundWorkers();
            SetOSInfo();
            SetOSInfo2();
            SetMemoryConfig();
            SetPartitions();

            if (!(BWACCOUNTS.IsBusy))
            {
                BWACCOUNTS.RunWorkerAsync();
            }

            if (!(BWIRQS.IsBusy))
            {
                BWIRQS.RunWorkerAsync();
            }

            if (!(BWNET.IsBusy))
            {
                BWNET.RunWorkerAsync();
            }
        }

        public OS(ManagementObject input)
        {
            SetAllOSInfo(input);
        }

        private void InitializeBackgroundWorkers()
        {
            BWACCOUNTS.DoWork += new DoWorkEventHandler(BWACCOUNTS_DoWork);
            BWACCOUNTS.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWACCOUNTS_RunWorkerCompleted);
            BWIRQS.DoWork += new DoWorkEventHandler(BWIRQS_DoWork);
            BWIRQS.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWIRQS_RunWorkerCompleted);
            BWNET.DoWork += new DoWorkEventHandler(BWNET_DoWork);
            BWNET.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWNET_RunWorkerCompleted);
        }

        private void BWACCOUNTS_DoWork(object sender, DoWorkEventArgs e)
        {
            AccountsLoaded = false;
            AccountErrorMessage = "";

            try
            {
                funcPopulateAccounts();
            }
            catch(Exception ex)
            {
                AccountErrorMessage = "Exception: " + ex.Message + "\n\nStack: " + ex.StackTrace;
                AccountErrorOccurred = true;
            }
        }

        private void BWACCOUNTS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (AccountErrorOccurred != true)
            {
                AccountsLoaded = true;
            }
        }

        private void BWIRQS_DoWork(object sender, DoWorkEventArgs e)
        {
            IRQsLoaded = false;
            IRQErrorMessage = "";

            try
            {
                funcPopulateIRQs();
            }
            catch(Exception ex)
            {
                IRQErrorMessage = "Exception: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace;
                IRQErrorOccured = true;
            }
        }

        private void BWIRQS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IRQErrorOccured != true)
            {
                IRQsLoaded = true;
            }
        }

        private void BWNET_DoWork(object sender, DoWorkEventArgs e)
        {
            NetPortsLoaded = false;
            NetPortsErrorMessage = "";

            try
            {
                funcPopulateNetPorts();
            }
            catch (Exception ex)
            {
                NetPortsErrorMessage = "Exception: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace;
                NetPortsErrorOccurred = true;
            }
        }

        private void BWNET_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (NetPortsErrorOccurred != true)
            {
                NetPortsLoaded = true;
            }
            else
            {
                MessageBox.Show(NetPortsErrorMessage);
            }
        }

        public bool AreAccountsLoaded()
        {
            return AccountsLoaded;
        }

        public bool AreIRQsLoaded()
        {
            return IRQsLoaded;
        }

        private void SetOSInfo()
        {
            ComputerName = Environment.MachineName;
            UserName = Environment.UserName;

            //Filter OS Version to remove redundancy.
            OSVersion = Environment.OSVersion.ToString();
            OSVersion = OSVersion.Replace("Microsoft Windows ", "");

            NETVersion = Environment.Version.ToString();
            DomainName = Environment.UserDomainName;

            if (DomainName.Contains(ComputerName))
            {
                DomainName = "No Domain";
            }

            SetLocalIP();

            WindowsInstall = Environment.SystemDirectory;
            Localization = CultureInfo.CurrentCulture.ToString();
        }

        //Source StackOverflow, username: George Duckett
        //Site: http://stackoverflow.com/questions/6331826/get-os-version-friendly-name-in-c-sharp
        //Modified to my own style/usage/best practices.
        //Win32_OperatingSystem Class Site: http://msdn.microsoft.com/en-us/library/aa394239(v=vs.85).aspx
        private void SetOSInfo2()
        {
            string format = "G";
            DateTime tempDate = new DateTime(1900, 1, 1);

            SelectQuery QueryOS = new SelectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryOS);

            foreach (ManagementObject item in searcher.Get())
            {
                if (item["Caption"] != null)
                {
                    OSName = item["Caption"].ToString();

                    //Filter Microsoft, we know who makes Windows.  Save UI Space.
                    OSName = OSName.Replace("Microsoft ", "");

                    //Attach x86/x64 Suffix to OS Name.
                    if (Environment.Is64BitOperatingSystem)
                    {
                        OSName += "(x64)";
                    }
                    else
                    {
                        OSName += "(x86)";
                    }

                    OSInfo.Add(OSName);
                }

                if (item["Organization"] != null)
                {
                    OSInfo.Add(item["Organization"].ToString());
                }
                else
                {
                    OSInfo.Add("N/A");
                }

                if (item["InstallDate"] != null)
                {
                    tempDate = ManagementDateTimeConverter.ToDateTime(item["InstallDate"].ToString());
                    OSInfo.Add(tempDate.ToString(format));
                }

                if (item["LastBootUpTime"] != null)
                {
                    tempDate = ManagementDateTimeConverter.ToDateTime(item["LastBootUpTime"].ToString());
                    OSInfo.Add(tempDate.ToString(format));
                }

                break;
            }
        }

        private void SetAllOSInfo(ManagementObject input)
        {
            if (input["Primary"] != null)
            {
                AllOSInfo.Add("Primary: " + input["Primary"].ToString());
            }
            else
            {
                AllOSInfo.Add("Primary:  No Data. (xNull)");
            }

            if (input["Caption"] != null)
            {
                OpersName = input["Caption"].ToString();
                AllOSInfo.Add("Caption: " + OpersName);
            }
            else
            {
                AllOSInfo.Add("Caption:  No Data. (xNull)");
            }

            if (input["OSArchitecture"] != null)
            {
                AllOSInfo.Add("OS Architecture: " + input["OSArchitecture"].ToString());
            }
            else
            {
                AllOSInfo.Add("OS Architecture:  No Data. (xNull)");
            }

            if (input["BuildNumber"] != null)
            {
                AllOSInfo.Add("Build Number: " + input["BuildNumber"].ToString());
            }
            else
            {
                AllOSInfo.Add("Build Number:  No Data. (xNull)");
            }

            if (input["Version"] != null)
            {
                OpersVersion = input["Version"].ToString();
                AllOSInfo.Add("Version: " + OpersVersion);
            }
            else
            {
                AllOSInfo.Add("Version:  No Data. (xNull)");
            }

            if (input["ForegroundApplicationBoost"] != null)
            {
                AllOSInfo.Add("Foreground Application Boost: " + funcConvertForeground(input["ForegroundApplicationBoost"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Foreground Application Boost:  No Data. (xNull)");
            }

            if (input["FreePhysicalMemory"] != null)
            {
                AllOSInfo.Add("Free Physical Memory: " + Tools.convertToGBFromKB(input["FreePhysicalMemory"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Free Physical Memory:  No Data. (xNull)");
            }

            if (input["TotalVisibleMemorySize"] != null)
            {
                AllOSInfo.Add("Total Visible Memory Size: " + Tools.convertToGBFromKB(input["TotalVisibleMemorySize"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Total Visible Memory Size:  No Data. (xNull)");
            }

            if (input["FreeVirtualMemory"] != null)
            {
                AllOSInfo.Add("Free Virtual Memory: " + Tools.convertToGBFromKB(input["FreeVirtualMemory"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Free Virtual Memory:  No Data. (xNull)");
            }

            if (input["TotalVirtualMemorySize"] != null)
            {
                AllOSInfo.Add("Total Virtual Memory Size: " + Tools.convertToGBFromKB(input["TotalVirtualMemorySize"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Total Virtual Memory Size:  No Data. (xNull)");
            }

            if (input["SizeStoredInPagingFiles"] != null)
            {
                AllOSInfo.Add("Size Stored In Paging Files: " + Tools.convertToGBFromKB(input["SizeStoredInPagingFiles"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Size Stored In Paging Files:  No Data. (xNull)");
            }

            if (input["FreeSpaceInPagingFiles"] != null)
            {
                AllOSInfo.Add("Free Space In Page File: " + Tools.convertToGBFromKB(input["FreeSpaceInPagingFiles"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Free Space In Page File:  No Data. (xNull)");
            }

            if (input["TotalSwapSpaceSize"] != null)
            {
                AllOSInfo.Add("Total Swap Space Size: " + Tools.convertToGBFromKB(input["TotalSwapSpaceSize"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Total Swap Space Size:  No Data. (xNull)");
            }

            if (input["Name"] != null)
            {
                AllOSInfo.Add("Name: " + input["Name"].ToString());
            }
            else
            {
                AllOSInfo.Add("Name:  No Data. (xNull)");
            }

            if (input["BootDevice"] != null)
            {
                AllOSInfo.Add("Boot Device: " + input["BootDevice"].ToString());
            }
            else
            {
                AllOSInfo.Add("Boot Device:  No Data. (xNull)");
            }

            if (input["BuildType"] != null)
            {
                AllOSInfo.Add("Build Type: " + input["BuildType"].ToString());
            }
            else
            {
                AllOSInfo.Add("Build Type:  No Data. (xNull)");
            }

            if (input["CodeSet"] != null)
            {
                AllOSInfo.Add("Code Set: " + input["CodeSet"].ToString());
            }
            else
            {
                AllOSInfo.Add("Code Set:  No Data. (xNull)");
            }

            if (input["CountryCode"] != null)
            {
                AllOSInfo.Add("Country Code: " + input["CountryCode"].ToString());
            }
            else
            {
                AllOSInfo.Add("CountryCode:  No Data. (xNull)");
            }

            if (input["CreationClassName"] != null)
            {
                AllOSInfo.Add("Creation Class Name: " + input["CreationClassName"].ToString());
            }
            else
            {
                AllOSInfo.Add("Creation Class name:  No Data. (xNull)");
            }

            if (input["CSCreationClassName"] != null)
            {
                AllOSInfo.Add("CS Creation Class Name: " + input["CSCreationClassName"].ToString());
            }
            else
            {
                AllOSInfo.Add("CS Creation Class Name:  No Data. (xNull)");
            }

            if (input["CSDVersion"] != null)
            {
                AllOSInfo.Add("CSD Version: " + input["CSDVersion"].ToString());
            }
            else
            {
                AllOSInfo.Add("CSD Version:  No Data. (xNull)");
            }

            if (input["CSName"] != null)
            {
                AllOSInfo.Add("CS Name: " + input["CSName"].ToString());
            }
            else
            {
                AllOSInfo.Add("CS Name:  No Data. (xNull)");
            }

            if (input["CurrentTimeZone"] != null)
            {
                AllOSInfo.Add("Current Time Zone: " + input["CurrentTimeZone"].ToString());
            }
            else
            {
                AllOSInfo.Add("Current Time Zone:  No Data. (xNull)");
            }

            if (input["DataExecutionPrevention_Available"] != null)
            {
                AllOSInfo.Add("DEP Available: " + input["DataExecutionPrevention_Available"].ToString());
            }
            else
            {
                AllOSInfo.Add("DEP Available:  No Data. (xNull)");
            }

            if (input["DataExecutionPrevention_32BitApplications"] != null)
            {
                AllOSInfo.Add("DEP for 32-bit Applications: " + input["DataExecutionPrevention_32BitApplications"].ToString());
            }
            else
            {
                AllOSInfo.Add("DEP for 32-bit Applications:  No Data. (xNull)");
            }

            if (input["DataExecutionPrevention_Drivers"] != null)
            {
                AllOSInfo.Add("DEP for Drivers: " + input["DataExecutionPrevention_Drivers"].ToString());
            }
            else
            {
                AllOSInfo.Add("DEP for Drivers:  No Data. (xNull)");
            }

            if (input["DataExecutionPrevention_SupportPolicy"] != null)
            {
                AllOSInfo.Add("DEP Support Policy: " + funcConvertDEPPolicy(input["DataExecutionPrevention_SupportPolicy"].ToString()));
            }
            else
            {
                AllOSInfo.Add("DEP Support Policy:  No Data. (xNull)");
            }

            if (input["Debug"] != null)
            {
                AllOSInfo.Add("Debug: " + input["Debug"].ToString());
            }
            else
            {
                AllOSInfo.Add("Debug:  No Data. (xNull)");
            }

            if (input["Description"] != null)
            {
                AllOSInfo.Add("Description: " + input["Description"].ToString());
            }
            else
            {
                AllOSInfo.Add("Description:  No Data. (xNull)");
            }

            if (input["Distributed"] != null)
            {
                AllOSInfo.Add("Distributed: " + input["Distributed"].ToString());
            }
            else
            {
                AllOSInfo.Add("Distributed:  No Data. (xNull)");
            }

            if (input["EncryptionLevel"] != null)
            {
                AllOSInfo.Add("Encryption Level: " + input["EncryptionLevel"].ToString());
            }
            else
            {
                AllOSInfo.Add("Encryption Level:  No Data. (xNull)");
            }

            if (input["InstallDate"] != null)
            {
                string Date = "";
                Date = ManagementDateTimeConverter.ToDateTime(input["InstallDate"].ToString()).ToString();

                OpersInstall = Date.ToString();
                AllOSInfo.Add("Install Date: " + Date);
            }
            else
            {
                AllOSInfo.Add("Install Date:  No Data. (xNull)");
            }

            if (input["LargeSystemCache"] != null)
            {
                AllOSInfo.Add("Large System Cache: " + input["LargeSystemCache"].ToString());
            }
            else
            {
                AllOSInfo.Add("Large System Cache:  No Data. (xNull)");
            }

            if (input["LastBootUpTime"] != null)
            {
                string Date = "";
                Date = ManagementDateTimeConverter.ToDateTime(input["LastBootUpTime"].ToString()).ToString();

                AllOSInfo.Add("Last Boot Up: " + Date);
            }
            else
            {
                AllOSInfo.Add("Last Boot Up:  No Data. (xNull)");
            }

            if (input["LocalDateTime"] != null)
            {
                string Date = "";
                Date = ManagementDateTimeConverter.ToDateTime(input["LocalDateTime"].ToString()).ToString();

                AllOSInfo.Add("Local Date Time: " + Date);
            }
            else
            {
                AllOSInfo.Add("Local Date Time:  No Data. (xNull)");
            }

            if (input["Locale"] != null)
            {
                AllOSInfo.Add("Locale: " + input["Locale"].ToString());
            }
            else
            {
                AllOSInfo.Add("Locale:  No Data. (xNull)");
            }

            if (input["Manufacturer"] != null)
            {
                AllOSInfo.Add("Manufacturer: " + input["Manufacturer"].ToString());
            }
            else
            {
                AllOSInfo.Add("Manufacturer:  No Data. (xNull)");
            }

            if (input["MaxNumberOfProcesses"] != null)
            {
                AllOSInfo.Add("Max Number of Processes: " + input["MaxNumberOfProcesses"].ToString());
            }
            else
            {
                AllOSInfo.Add("Max Number of Porcesses:  No Data. (xNull)");
            }

            if (input["MaxProcessMemorySize"] != null)
            {
                AllOSInfo.Add("Max Process Size: " + Tools.convertToGBFromKB(input["MaxProcessMemorySize"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Max Process Size:  No Data. (xNull)");
            }

            if (input["NumberOfLicensedUsers"] != null)
            {
                AllOSInfo.Add("Number Of Licensed Users: " + input["NumberOfLicensedUsers"].ToString());
            }
            else
            {
                AllOSInfo.Add("Number Of Licenseded Users:  No Data. (xNull)");
            }

            if (input["NumberOfProcesses"] != null)
            {
                AllOSInfo.Add("Number Of Processes: " + input["NumberOfProcesses"].ToString());
            }
            else
            {
                AllOSInfo.Add("Number Of Processes:  No Data. (xNull)");
            }

            if (input["NumberOfUsers"] != null)
            {
                AllOSInfo.Add("Number Of Users: " + input["NumberOfUsers"].ToString());
            }
            else
            {
                AllOSInfo.Add("Number Of Users:  No Data. (xNull)");
            }

            if (input["OperatingSystemSKU"] != null)
            {
                AllOSInfo.Add("OS SKU: " + input["OperatingSystemSKU"].ToString());
            }
            else
            {
                AllOSInfo.Add("OS SKU:  No Data. (xNull)");
            }

            if (input["Organization"] != null)
            {
                AllOSInfo.Add("Organization: " + input["Organization"].ToString());
            }
            else
            {
                AllOSInfo.Add("Organization:  No Data. (xNull)");
            }

            if (input["OSLanguage"] != null)
            {
                AllOSInfo.Add("OS Language: " + input["OSLanguage"].ToString());
            }
            else
            {
                AllOSInfo.Add("OS Language:  No Data. (xNull)");
            }

            if (input["OSProductSuite"] != null)
            {
                AllOSInfo.Add("OS Product Suite: " + input["OSProductSuite"].ToString());
            }
            else
            {
                AllOSInfo.Add("OS Product Suite:  No Data. (xNull)");
            }

            if (input["OSType"] != null)
            {
                AllOSInfo.Add("OS Type: " + funcConvertOSType(input["OSType"].ToString()));
            }
            else
            {
                AllOSInfo.Add("OS Type:  No Data. (xNull)");
            }

            if (input["OtherTypeDescription"] != null)
            {
                AllOSInfo.Add("Other Type Description: " + input["OtherTypeDescription"].ToString());
            }
            else
            {
                AllOSInfo.Add("Other Type Description:  No Data. (xNull)");
            }

            if (input["PAEEnabled"] != null)
            {
                AllOSInfo.Add("PAE Enabled: " + input["PAEEnabled"].ToString());
            }
            else
            {
                AllOSInfo.Add("PAE Enabled:  No Data. (xNull)");
            }

            if (input["PlusProductID"] != null)
            {
                AllOSInfo.Add("Plus Product ID: " + input["PlusProductID"].ToString());
            }
            else
            {
                AllOSInfo.Add("Plus Product ID:  No Data. (xNull)");
            }

            if (input["PlusVersionNumber"] != null)
            {
                AllOSInfo.Add("Plus Version Number: " + input["PlusVersionNumber"].ToString());
            }
            else
            {
                AllOSInfo.Add("Plus Version Number:  No Data. (xNull)");
            }

            //if (input["PortableOperatingSystem"] != null)
            //{
            //    AllOSInfo.Add("Portable Operating System: " + input["PortableOperatingSystem"].ToString());
            //}
            //else
            {
                AllOSInfo.Add("Portable Operating System:  No Data. (xNull)");
            }

            if (input["ProductType"] != null)
            {
                AllOSInfo.Add("ProductType: " + funcConvertProductType(input["ProductType"].ToString()));
            }
            else
            {
                AllOSInfo.Add("ProductType:  No Data. (xNull)");
            }

            if (input["RegisteredUser"] != null)
            {
                OpersRegistered = input["RegisteredUser"].ToString();
                AllOSInfo.Add("Registered User: " + OpersRegistered);
            }
            else
            {
                AllOSInfo.Add("Registered User:  No Data. (xNull)");
            }

            if (input["SerialNumber"] != null)
            {
                AllOSInfo.Add("Serial Number: " + input["SerialNumber"].ToString());
            }
            else
            {
                AllOSInfo.Add("Serial Number:  No Data. (xNull)");
            }

            if (input["ServicePackMajorVersion"] != null)
            {
                OpersSP = input["ServicePackMajorVersion"].ToString();
                AllOSInfo.Add("Service Pack Major Version: " + OpersSP);
            }
            else
            {
                AllOSInfo.Add("Service Pack Major Version:  No Data. (xNull)");
            }

            if (input["ServicePackMinorVersion"] != null)
            {
                OpersSP += "." + input["ServicePackMinorVersion"].ToString();
                AllOSInfo.Add("Service Pack Minor Version: " + input["ServicePackMinorVersion"].ToString());
            }
            else
            {
                AllOSInfo.Add("Service Pack Minor Version:  No Data. (xNull)");
            }

            if (input["Status"] != null)
            {
                AllOSInfo.Add("Status: " + input["Status"].ToString());
            }
            else
            {
                AllOSInfo.Add("Status:  No Data. (xNull)");
            }

            if (input["SuiteMask"] != null)
            {
                AllOSInfo.Add("Suite Mask: " + funcConvertSuiteMask(input["SuiteMask"].ToString()));
            }
            else
            {
                AllOSInfo.Add("Suite Mask:  No Data. (xNull)");
            }

            if (input["SystemDevice"] != null)
            {
                AllOSInfo.Add("System Device: " + input["SystemDevice"].ToString());
            }
            else
            {
                AllOSInfo.Add("System Device:  No Data. (xNull)");
            }

            if (input["WindowsDirectory"] != null)
            {
                OpersDir = input["WindowsDirectory"].ToString();
                AllOSInfo.Add("Windows Directory: " + OpersDir);
            }
            else
            {
                AllOSInfo.Add("Windows Directory:  No Data. (xNull)");
            }

            allOSLength = 61;
        }

        public string GetLocale()
        { return Localization; }

        public string GetCompName()
        { return ComputerName; }

        public string GetUserName()
        { return UserName; }

        public string GetOSVersion()
        { return OSVersion; }

        public string GetNETVersion()
        { return NETVersion; }

        public string GetDomainName()
        { return DomainName; }

        public string GetOSName()
        { return OSInfo.ElementAt(0); }

        public string GetOpersName()
        { return OpersName; }

        public string GetOpersVersion()
        { return OpersVersion; }

        public string GetOpersDir()
        { return OpersDir; }

        public string GetOpersRegistered()
        { return OpersRegistered; }

        public string GetOpersSP()
        { return OpersSP; }

        public string GetOpersInstall()
        { return OpersInstall; }

        public string GetCompanyName()
        { return OSInfo.ElementAt(1); }

        public string GetInstallDate()
        { return OSInfo.ElementAt(2); }

        public string GetLastBootUpTime()
        { return OSInfo.ElementAt(3); }

        public string GetWindowsInstall()
        { return WindowsInstall; }

        public List<string> ToList()
        {
            List<string> info = new List<string>();

            info.Add( ComputerName );
            info.Add( UserName );
            info.Add( OSVersion );
            info.Add( DomainName );
            info.Add( NETVersion );

            foreach (string temp in OSInfo)
            { info.Add(temp); }

            return info;
        }

        //Source: StackOverflow, Users: Mohammed Sakher Sawan, Mrchief, Habib
        //Site: http://stackoverflow.com/questions/6803073/get-local-ip-address-c-sharp
        //Site: http://stackoverflow.com/questions/11411486/how-to-get-ipv4-and-ipv6-address-of-local-machine
        //System.Net Site: http://msdn.microsoft.com/en-us/library/system.net(v=vs.110).aspx
        //Obviously modified and combined, with reformating.
        private void SetLocalIP()
        {
            IPHostEntry LocalHost = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in LocalHost.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                { strIPv4 = ip.ToString(); }
            }

            IPAddress[] ipAddress = LocalHost.AddressList;

            if (ipAddress[0].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            { strIPv6 = ipAddress[0].ToString(); }
        }

        //Source: CodeProject, User: Huseyin Atasoy
        //Site: http://www.codeproject.com/Tips/452024/Getting-the-External-IP-Address
        //Modified for style/personal readability.
        public void SetExternalIP()
        {
            strExtIP = "";

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    strExtIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                    strExtIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(strExtIP)[0].ToString();
                }
                catch
                { strExtIP = "Error"; }
            }
            else
            { strExtIP = "No internet connection."; }
        }

        public void SetPartitions()
        {
            ObjectQuery PartitionQuery = new ObjectQuery("SELECT * FROM Win32_DiskPartition");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(PartitionQuery);
            ManagementObjectCollection moc = searcher.Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                DiskPartition newPart = new DiskPartition(mo);

                DPs.Add(newPart);
            });
        }

        public string GetIPv4()
        { return strIPv4; }

        public string GetIPv6()
        { return strIPv6; }

        public string GetExtIP()
        { return strExtIP; }

        public void SetMemoryConfig()
        {
            ObjectQuery MemConfigQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(MemConfigQuery);
            ManagementObjectCollection moc = searcher.Get();
            intMemConfigLength = 0;

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                MemConfig.Add(Tools.convertToGBFromKB(mo["FreePhysicalMemory"]?.ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["FreeVirtualMemory"].ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["TotalVirtualMemorySize"].ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["TotalVisibleMemorySize"].ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["FreeSpaceInPagingFiles"].ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["SizeStoredInPagingFiles"].ToString()));
                MemConfig.Add(Tools.convertToGBFromKB(mo["MaxProcessMemorySize"].ToString()));

                intMemConfigLength = 7;
            });
        }

        public List<string> getMemoryConfig()
        { return MemConfig; }

        public string getFreePhysicalMem()
        { return MemConfig.ElementAt(0); }

        public string getFreeVirtualMem()
        { return MemConfig.ElementAt(1); }

        public string getTotalVirtualMem()
        { return MemConfig.ElementAt(2); }

        public string getTotalVisibleMem()
        { return MemConfig.ElementAt(3); }

        public string getPageFileFree()
        { return MemConfig.ElementAt(4); }

        public string getPageFileSize()
        { return MemConfig.ElementAt(5); }

        public string getMaxProcessSize()
        { return MemConfig.ElementAt(6); }

        public int getMemConfigLength()
        { return intMemConfigLength; }

        public string getMEMFreePerc()
        {
            return Tools.getPercentage(Tools.convertGBtoDouble(getFreePhysicalMem()),
                                       Tools.convertGBtoDouble(getTotalVisibleMem()));
        }

        public string getVIRTFreePerc()
        {
            return Tools.getPercentage(Tools.convertGBtoDouble(getFreeVirtualMem()),
                                       Tools.convertGBtoDouble(getTotalVirtualMem()));
        }

        public string getPAGEFreePerc()
        {
            return Tools.getPercentage(Tools.convertGBtoDouble(getPageFileFree()),
                                       Tools.convertGBtoDouble(getPageFileSize()));
        }

        private string funcConvertForeground(string input)
        {
            if (input == "0")
            {
                input = "None";
            }
            else if (input == "1")
            {
                input = "Minimum";
            }
            else if (input == "2")
            {
                input = "Maximum";
            }

            return input;
        }

        private string funcConvertDEPPolicy(string input)
        {
            if (input == "0")
            {
                input = "Always Off";
            }
            else if (input == "1")
            {
                input = "Always On";
            }
            else if (input == "2")
            {
                input = "Opt In";
            }
            else if (input == "3")
            {
                input = "Opt Out";
            }

            return input;
        }

        private string funcConvertSuiteMask(string input)
        {
            int intType = -1;
            if (int.TryParse(input, out intType))
            {
                switch (intType)
                {
                    case 1: input = "Small Business"; break;
                    case 2: input = "Enterprise"; break;
                    case 4: input = "Back Office"; break;
                    case 8: input = "Communications"; break;
                    case 16: input = "Terminal"; break;
                    case 32: input = "Small Business Restricted"; break;
                    case 64: input = "Embedded NT"; break;
                    case 128: input = "Data Center"; break;
                    case 256: input = "Single User"; break;
                    case 512: input = "Personal"; break;
                    case 1024: input = "Blade"; break;
                    default: break;
                }
            }

            return input;
        }

        private string funcConvertProductType(string input)
        {
            if (input == "1")
            {
                input = "Work Station";
            }
            else if (input == "2")
            {
                input = "Domain Controller";
            }
            else if (input == "3")
            {
                input = "Server";
            }

            return input;
        }

        private string funcConvertOSType(string input)
        {
            int intType = -1;
            if (int.TryParse(input, out intType))
            {
                switch (intType)
                {
                    case 0: input = "Unknown"; break;
                    case 1: input = "Other"; break;
                    case 2: input = "MACROS"; break;
                    case 3: input = "ATTUNIX"; break;
                    case 4: input = "DGUX"; break;
                    case 5: input = "DECNT"; break;
                    case 6: input = "Digital Unix"; break;
                    case 7: input = "OpenVMS"; break;
                    case 8: input = "HPUX"; break;
                    case 9: input = "AIX"; break;
                    case 10: input = "MVS"; break;
                    case 11: input = "OS400"; break;
                    case 12: input = "OS/2"; break;
                    case 13: input = "JavaVM"; break;
                    case 14: input = "MSDOS"; break;
                    case 15: input = "WIN3X"; break;
                    case 16: input = "WIN95"; break;
                    case 17: input = "WIN98"; break;
                    case 18: input = "WINNT"; break;
                    case 19: input = "WINCE"; break;
                    case 20: input = "NCR3000"; break;
                    case 21: input = "NetWare"; break;
                    case 22: input = "OSF"; break;
                    case 23: input = "DC/OS"; break;
                    case 24: input = "Reliant UNIX"; break;
                    case 25: input = "SCO UnixWare"; break;
                    case 26: input = "SCO OpenServer"; break;
                    case 27: input = "Sequent"; break;
                    case 28: input = "IRIX"; break;
                    case 29: input = "Solaris"; break;
                    case 30: input = "SunOS"; break;
                    case 31: input = "U6000"; break;
                    case 32: input = "ASERIES"; break;
                    case 33: input = "TandemNSK"; break;
                    case 34: input = "TandemNT"; break;
                    case 35: input = "BS2000"; break;
                    case 36: input = "LINUX"; break;
                    case 37: input = "Lynx"; break;
                    case 38: input = "XENIX"; break;
                    case 39: input = "VM/ESA"; break;
                    case 40: input = "Interactive Unix"; break;
                    case 41: input = "BSDUNIX"; break;
                    case 42: input = "FreeBSD"; break;
                    case 43: input = "NetBSD"; break;
                    case 44: input = "GNU Hurd"; break;
                    case 45: input = "OS9"; break;
                    case 46: input = "MACH Kernel"; break;
                    case 47: input = "Inferno"; break;
                    case 48: input = "QNX"; break;
                    case 49: input = "EPOC"; break;
                    case 50: input = "IxWorks"; break;
                    case 51: input = "VxWorks"; break;
                    case 52: input = "MiNT"; break;
                    case 53: input = "BeOS"; break;
                    case 54: input = "HP MPE"; break;
                    case 55: input = "NextStep"; break;
                    case 56: input = "PalmPilot"; break;
                    case 57: input = "Rhapsody"; break;
                    default: break;
                }
            }

            return input;
        }

        private void funcPopulateAccounts()
        {
            AccountTimer = Stopwatch.StartNew();
            SelectQuery QueryACs = new SelectQuery("SELECT * FROM Win32_Account");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryACs);
            ManagementObjectCollection moc = searcher.Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                Account Account = new Account(mo);

                switch (Account.AccountType)
                {
                    case "User":            UserAccounts.Add(Account);      break;
                    case "Group":           GroupAccounts.Add(Account);     break;
                    case "Domain":          DomainAccounts.Add(Account);    break;
                    case "Alias":           AliasAccounts.Add(Account);     break;
                    case "WellKnownGroup":  WKGAccounts.Add(Account);       break;
                    case "DeletedAccount":  DeletedAccounts.Add(Account);   break;
                    case "Invalid":         InvalidAccounts.Add(Account);   break;
                    case "Unknown":         UnknownAccounts.Add(Account);   break;
                    case "Computer":        ComputerAccounts.Add(Account);  break;
                    default: break;
                }
            });

            AccountTimer.Stop();
            AccountElapsedTime = AccountTimer.ElapsedMilliseconds;
        }

        private void funcPopulateIRQs()
        {
            IRQTimer = Stopwatch.StartNew();

            SelectQuery QueryIRQs = new SelectQuery("SELECT * FROM Win32_IRQResource");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryIRQs);

            foreach (ManagementObject irqInfo in searcher.Get())
            {
                IRQ temp = new IRQ(irqInfo);

                IRQs.Add(temp);
            }

            IRQTimer.Stop();
            IRQElapsedTime = IRQTimer.ElapsedMilliseconds;
        }

        public void funcPopulateNetPorts()
        {
            NPTimer = Stopwatch.StartNew();

            NetPortList.NetworkProcList.Clear();
            NetPortList.funcGetPorts();

            NPTimer.Stop();
            NetPortElapsedTime = NPTimer.ElapsedMilliseconds;
        }
    }
}
