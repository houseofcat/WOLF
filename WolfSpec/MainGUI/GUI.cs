using System;
using System.Management;
using System.Management.Automation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using OracleSoft;
using Wolf.CI_Report;
using Wolf.SearchRename;
using Wolf.HardSpec;
using Wolf.SoftSpec;

namespace Wolf
{
    public partial class GUI : Form
    {
        private string version = "0.3587";
        private string latestversion = "";
        private string Windows3264ProductKeyLocation = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";

        private string O2010KeyLocation64_01 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\14.0\\Registration\\{90140000-0011-0000-0000-0000000FF1CE}";
        private string O2010KeyLocation64_02 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\14.0\\Registration\\{90140000-0017-0000-0000-0000000FF1CE}";
        private string O2010KeyLocation64_03 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\15.0\\Registration\\{90140000-0051-0000-1000-0000000FF1CE}";
        private string O2010KeyLocation32_01 = "SOFTWARE\\Microsoft\\Office\\14.0\\Registration\\{90140000-0011-0000-0000-0000000FF1CE}";
        private string O2010KeyLocation32_02 = "SOFTWARE\\Microsoft\\Office\\14.0\\Registration\\{90140000-0017-0000-0000-0000000FF1CE}";
        private string O2010KeyLocation32_03 = "SOFTWARE\\Microsoft\\Office\\14.0\\Registration\\{90140000-0051-0000-1000-0000000FF1CE}";
        private string O2013KeyLocation64_01 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\15.0\\Registration\\{90150000-0011-0000-0000-0000000FF1CE}";
        private string O2013KeyLocation64_02 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\15.0\\Registration\\{90150000-0017-0000-0000-0000000FF1CE}";
        private string O2013KeyLocation64_03 = "SOFTWARE\\Wow6432Node\\Microsoft\\Office\\15.0\\Registration\\{90150000-0051-0000-1000-0000000FF1CE}";
        private string O2013KeyLocation32_01 = "SOFTWARE\\Microsoft\\Office\\15.0\\Registration\\{90150000-0011-0000-0000-0000000FF1CE}";
        private string O2013KeyLocation32_02 = "SOFTWARE\\Microsoft\\Office\\15.0\\Registration\\{90150000-0017-0000-0000-0000000FF1CE}";
        private string O2013KeyLocation32_03 = "SOFTWARE\\Microsoft\\Office\\15.0\\Registration\\{90150000-0051-0000-1000-0000000FF1CE}";

        private string FFProtection32_01 = "SOFTWARE\\Microsoft\\Forefront Server Security\\Exchange Server\\Registration";
        private string FFProtection64_01 = "SOFTWARE\\Wow6432Node\\Microsoft\\Forefront Server Security\\Exchange Server\\Registration";

        private string IE32_01 = "SOFTWARE\\Microsoft\\Internet Explorer\\Registration";
        private string IE64_01 = "SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\Registration";

        //private List<string> OfficeKeys = new List<string>();
        private List<string> KeyLocations32 = new List<string>();
        private List<string> KeyLocations64 = new List<string>();
        //private String Culture = "en-US";
        private Computer comp = new Computer();
        //private Boolean ComputerLoaded = false;

        private List<string> OSEnv = new List<string>();
        private bool PERFCONSTANT = true;
        private bool NPCONSTANT = false;
        private bool OFEnabled = false;
        private short NPRATE = 3000;
        private short NPRATE2 = 0;
        private long intLogEvent = 0;
        private ulong intTotMem = 0;
        private double dblTotMem = 0;

        private string physNicName = "";
        private string physNicMac = "";
        private string physNicGuid = "";

        private string OSProductKey = "";
        private string OSEmbeddedKey = "";

        private bool ShowProductKey = false;
        private bool ShowEmbeddedKey = false;

        private int SelectedInstalledProgram = -1;
        private int NP_KillProcess = -1;
        private int NP_CurrentSelection = -1;
        private string NP_KillProcessName = "";

        private RemoteMachine remMac = new RemoteMachine();

        //RemoteMachine Feedback Boolean
        private bool[] RemChoices = new bool[8];
        private bool AccessDenied = false;
        private bool ErrorDetected = false;

        //Comp BW
        private BackgroundWorker BW0 = new BackgroundWorker();
        //ExtIP BW
        private BackgroundWorker BW1 = new BackgroundWorker();
        //CPUInfo BW
        private BackgroundWorker BW2 = new BackgroundWorker();
        //CPU2Info BW
        private BackgroundWorker BW3 = new BackgroundWorker();
        //Fill and Populating TaskManager
        private BackgroundWorker BW4 = new BackgroundWorker();

        //Get remote queried work station
        private BackgroundWorker GatherRemoteMachine = new BackgroundWorker();
        private BackgroundWorker BWQ = new BackgroundWorker();

        //PerformanceQuery BW
        private BackgroundWorker BWPQ = new BackgroundWorker();
        private BackgroundWorker BWCUPDATE = new BackgroundWorker();

        //NetPort Refresher
        private BackgroundWorker BWNP = new BackgroundWorker();

        //Performance Variables
        Stopwatch DisplayTimer = new Stopwatch();
        Stopwatch DisplayTimer1 = new Stopwatch();
        Stopwatch DisplayTimer2 = new Stopwatch();
        Stopwatch DisplayTimer3 = new Stopwatch();
        Stopwatch UPDATETIME = new Stopwatch();

        PerformanceCounter cpuUsage = new PerformanceCounter();
        PerformanceCounter ramUsage = new PerformanceCounter();

        public List<string> ListIPs = new List<string>();
        public List<string> ListChecked = new List<string>();
        public List<RemoteMachine> ListRMs = new List<RemoteMachine>();
        public List<string> ListADs = new List<string>();
        public List<string> ListEDs = new List<string>();
        public List<string> ListWEs = new List<string>();
        public List<string> ListOff = new List<string>();
        private int End = 0;
        private int Start = 0;
        private int TotalIPs = 0;

        //IP Range Feedback Booleans
        private bool[] RangeChoices = new bool[8];
        private bool AccessDenied1 = false;
        private bool ErrorDetected1 = false;
        private bool WMIError1 = false;

        public int MachinesQueried = 0;
        public int LoopIterations = 0;
        public int RangeTimeElapsed = 0;

        //Extending ToolStripPro with custom ColorTable
        //http://stackoverflow.com/questions/9260303/how-to-change-menu-hover-color-winforms
        private class DarkThemeRenderer : ToolStripProfessionalRenderer
        {
            public DarkThemeRenderer() : base(new DarkThemeColorTable()) { }

            //Extending ToolStripPro with Overrides
            //http://stackoverflow.com/questions/26605368/change-the-color-of-the-arrow-near-the-menuitem-in-c-sharp
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                var tsMenuItem = e.Item as ToolStripMenuItem;

                if ((tsMenuItem is ToolStripMenuItem) && (tsMenuItem != null))
                {
                    e.ArrowColor = Color.LightGray;
                }

                base.OnRenderArrow(e);
            }
        }

        //Examples of ProfessionalColorTable
        //http://stackoverflow.com/questions/13235627/how-to-change-the-appearance-of-a-menustrip
        private class DarkThemeColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.Black; }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.Black; }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.Black; }
            }

            public override Color MenuBorder
            {
                get { return Color.DarkGray; }
            }

            public override Color ToolStripDropDownBackground
            {
                get { return Color.DarkGray; }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.Black; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.Black; }
            }

            public override Color MenuItemPressedGradientMiddle
            {
                get { return Color.DarkGray; }
            }
        }

        public GUI()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            PS_Initialize();
            //menuStrip1.Renderer = new ToolStripRenderer();

            SetDoubleBuffered(this);
            //this.ResizeBegin += (s, e) => { this.SuspendLayout(); };
            //this.ResizeEnd += (s, e) => { this.ResumeLayout(true); };

            if (Environment.Is64BitProcess)
            {
                this.Text += " (x64)";
            }
            else
            {
                this.Text += " (x86)";
            }

            funcLOG("============= Program Start-Up ===================");

            cbxMicrosoftKeys.SelectedIndex = 0;

            funcLoadKeyLocations();

            if (!(BWCUPDATE.IsBusy))
            {
                try
                {
                    BWCUPDATE.RunWorkerAsync();
                }
                catch (Exception Ex)
                {
                    logEXCEPT(Ex);
                }
            }

            if (Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"] != null)
            {
                funcLOG("IGNORE_WINDEF_MESSAGE: " + Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"]);
            }

            if (Properties.Settings.Default["IGNORE_WINDOWS_FIREWALL"] != null)
            {
                funcLOG("IGNORE_WINDOWS_FIREWALL: " + Properties.Settings.Default["IGNORE_WINDOWS_FIREWALL"]);
            }

            lblVersion.Text = "v" + version;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            btnAccountExport.Enabled = false;

            ConfigureDGVStyle();
            SetDoubleBufferOnControls();
            menuStrip1.Renderer = new DarkThemeRenderer();

            if (BW0.IsBusy != true)
            {
                BW0.RunWorkerAsync();
            }

            if (BW1.IsBusy != true)
            {
                BW1.RunWorkerAsync();
            }

            if (BW2.IsBusy != true)
            {
                BW2.RunWorkerAsync();
            }

            if (BW3.IsBusy != true)
            {
                BW3.RunWorkerAsync();
            }

            if (BW4.IsBusy != true)
            {
                treeACTs.Nodes.Add("Account information is loading (i.e. not frozen, give it time)...");
                BW4.RunWorkerAsync();
            }
        }

        private void ConfigureDGVStyle()
        {
            Color color = ColorTranslator.FromHtml("#383838");
            dgvInstalledProgs.DefaultCellStyle.BackColor = color;
            dgvInstalledProgs.DefaultCellStyle.ForeColor = Color.White;

            dgvDrivers.DefaultCellStyle.BackColor = color;
            dgvDrivers.DefaultCellStyle.ForeColor = Color.White;

            dgvNetPorts.DefaultCellStyle.BackColor = color;
            dgvNetPorts.DefaultCellStyle.ForeColor = Color.White;

            dgvDrivers.DefaultCellStyle.BackColor = color;
            dgvDrivers.DefaultCellStyle.ForeColor = Color.White;
        }

        private void SetDoubleBufferOnControls()
        {
            SetDoubleBuffered(this);
            SetDoubleBuffered(tabHW);
            SetDoubleBuffered(tabSoft);
            SetDoubleBuffered(tabToolGroups);
            SetDoubleBuffered(toolsPage1);
            SetDoubleBuffered(tabMainTab);
            SetDoubleBuffered(dgvInstalledProgs);
            SetDoubleBuffered(dgvDrivers);
            SetDoubleBuffered(dgvNetPorts);
            SetDoubleBuffered(treeACTs);
            SetDoubleBuffered(treeBIOS);
            SetDoubleBuffered(treeCONs);
            SetDoubleBuffered(treeDISPS);
            SetDoubleBuffered(treeGPUs);
            SetDoubleBuffered(treeIPRange);
            SetDoubleBuffered(treeIRQ);
            SetDoubleBuffered(treeLDRVs);
            SetDoubleBuffered(treeMB);
            SetDoubleBuffered(treeMEM);
            SetDoubleBuffered(treeNICS);
            SetDoubleBuffered(treeOS);
            SetDoubleBuffered(treePARTs);
            SetDoubleBuffered(treePDRVs);
            SetDoubleBuffered(treePROCS);
            SetDoubleBuffered(treeSDs);
            SetDoubleBuffered(treeTPs);
            SetDoubleBuffered(treeVNICS);
            SetDoubleBuffered(treeWS);
            SetDoubleBuffered(tbxLog);

            foreach(Control c in groupBox6.Controls)
            {
                SetDoubleBuffered(c);
            }

            foreach (Control c in groupBox18.Controls)
            {
                SetDoubleBuffered(c);
            }

        }

        //Double Buffer Form Controls
        //http://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form
        public static void SetDoubleBuffered(Control c)
        {
            try
            {
                //Taxes: Remote Desktop Connection and painting
                //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
                if (!(SystemInformation.TerminalServerSession))
                {

                    System.Reflection.PropertyInfo aProp =
                      typeof(Control).GetProperty(
                            "DoubleBuffered",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance);

                    aProp.SetValue(c, true, null);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("Setting double buffered controls failed. \n\n" +
                                "Exception : " + EX.Message + "\n\n" +
                                "Stack: " + EX.StackTrace);
            }
        }

        private void InitializeBackgroundWorkers()
        {
            BW0.DoWork += new DoWorkEventHandler(BW0_DoWork);
            BW0.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW0_RunWorkerCompleted);

            BW1.DoWork += new DoWorkEventHandler(BW1_DoWork);
            BW1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW1_RunWorkerCompleted);

            BW2.DoWork += new DoWorkEventHandler(BW2_DoWork);
            BW2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW2_RunWorkerCompleted);

            BW3.DoWork += new DoWorkEventHandler(BW3_DoWork);
            BW3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW3_RunWorkerCompleted);

            BW4.DoWork += new DoWorkEventHandler(BW4_DoWork);
            BW4.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW4_RunWorkerCompleted);

            GatherRemoteMachine.DoWork += new DoWorkEventHandler(GRM_DoWork);
            GatherRemoteMachine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GRM_RunWorkerCompleted);

            BWQ.DoWork += new DoWorkEventHandler(BWQ_DoWork);
            BWQ.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWQ_RunWorkerCompleted);
            BWQ.ProgressChanged += new ProgressChangedEventHandler(BWQ_ProgressChanged);
            BWQ.WorkerReportsProgress = true;

            BWPQ.DoWork += new DoWorkEventHandler(BWPQ_DoWork);
            BWPQ.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWPQ_RunWorkerCompleted);
            BWPQ.ProgressChanged += new ProgressChangedEventHandler(BWPQ_ProgressChanged);
            BWPQ.WorkerReportsProgress = true;

            BWCUPDATE.DoWork += new DoWorkEventHandler(BWCUPDATE_DoWork);
            BWCUPDATE.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWCUPDATE_RunWorkerCompleted);

            BWNP.WorkerSupportsCancellation = true;
            BWNP.WorkerReportsProgress = true;
            BWNP.DoWork += new DoWorkEventHandler(BWNP_DoWork);
            BWNP.ProgressChanged += new ProgressChangedEventHandler(BWNP_ProgressChanged);
            BWNP.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWNP_RunWorkerCompleted);

            GRL.DoWork += new DoWorkEventHandler(GRL_DoWork);
            GRL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GRL_WorkCompleted);
        }

        private void funcCheckUpdate()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    WebClient wc = new WebClient();
                    string webData = wc.DownloadString("http://www.bytemedev.com/Public/lv.txt");

                    if (webData.Any())
                    {
                        this.latestversion = webData;
                    }
                }
                catch 
                {
                    this.latestversion = "";
                }
            }
        }

        private void funcLoadKeyLocations()
        {
            KeyLocations64.Add(O2010KeyLocation64_01);
            KeyLocations64.Add(O2010KeyLocation64_02);
            KeyLocations64.Add(O2010KeyLocation64_03);
            KeyLocations64.Add(O2013KeyLocation64_01);
            KeyLocations64.Add(O2013KeyLocation64_02);
            KeyLocations64.Add(O2013KeyLocation64_03);

            KeyLocations32.Add(O2010KeyLocation32_01);
            KeyLocations32.Add(O2010KeyLocation32_02);
            KeyLocations32.Add(O2010KeyLocation32_03);
            KeyLocations32.Add(O2013KeyLocation32_01);
            KeyLocations32.Add(O2013KeyLocation32_02);
            KeyLocations32.Add(O2013KeyLocation32_03);

            KeyLocations64.Add(FFProtection64_01);
            KeyLocations32.Add(FFProtection32_01);

            KeyLocations64.Add(IE64_01);
            KeyLocations32.Add(IE32_01);
        }

        private void BW0_DoWork(object sender, DoWorkEventArgs e)
        {
            comp.funcLoadComputer();
        }

        private void BW1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DisplayTimer = Stopwatch.StartNew();
                comp.os.SetExternalIP();
                DisplayTimer.Stop();
            }
            catch (Exception BUG)
            {
                MessageBox.Show("Error lost connection!", "Connection Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                BUG.ToString();
            }
        }

        private void BW2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Execute the standard CPU Info grabber.
                DisplayTimer = Stopwatch.StartNew();
                comp.cpu.setCPUInfo1();
                DisplayTimer.Stop();
            }
            catch (Exception BUG)
            {
                MessageBox.Show("Error reading from CPU!", "CPU Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                BUG.ToString();
            }

        }

        private void BW3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Execute the extended CPU Info grabber.
                DisplayTimer = Stopwatch.StartNew();
                comp.funcPopulateCPUs();
                DisplayTimer.Stop();
            }
            catch (Exception BUG)
            {
                MessageBox.Show("Error reading from CPU!", "CPU Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                BUG.ToString();
            }
        }

        private void BW4_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Wait till the background thread of the OS class has finished
                //loading the accounts.
                while ((!(comp.os.AreAccountsLoaded())) || (comp.os.AccountErrorOccurred))
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void GRM_DoWork(object sender, DoWorkEventArgs e)
        {
            AccessDenied = false;
            ErrorDetected = false;
            DisplayTimer = Stopwatch.StartNew();

            try
            {
                remMac = new RemoteMachine(tbxDomainName2.Text, tbxDomainUserName.Text, tbxDomainUserPassword.Text, tbxWSName.Text, RemChoices);
            }
            catch (System.Management.ManagementException MEX)
            {
                MessageBox.Show("ManagementException occurred. Remote machine's WMI encountered an error." +
                    Environment.NewLine + Environment.NewLine + "Exception: " + MEX.Message +
                    Environment.NewLine + "\nStack Trace: " + MEX.StackTrace);

                remMac.funcClear();
            }
            // I am trying to manipulate the treeview with an access denied concatenation so I am sending this exception
            // on up to the next level.
            catch (System.UnauthorizedAccessException UEX)
            {
                AccessDenied = true;

                //Bypasses IDE declared but never used.  Error handling for these two were moved to next stack up
                //so it could be logged.
                UEX.ToString();
            }
            catch (System.Runtime.InteropServices.COMException CEX)
            {
                ErrorDetected = true;

                //Bypasses IDE declared but never used.  Error handling for these two were moved to next stack up
                //so it could be logged.
                CEX.ToString();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("If you are seeing this message, very very very strong chance I have no idea what caused this.\n\n" +
                                "You should definitely submit this to me.\n\n" + Ex.Message + "\n\nStack Trace: " + Ex.StackTrace);

                remMac.funcClear();
            }

            DisplayTimer.Stop();
        }

        private void BW0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //First chunk
            funcLoadOSEnv();
            funcLoadNetEnv();
            funcLoadConnections();

            bool memoryLoad = false;

            //Second Chunk
            //Function for populating the MemTreeView.  Windows 8 has
            //been erroring out, even though I verified all the data
            //being received was not Null, however Windows 8 has still
            //shown the tendencey to drop an NRE.
            try
            {
                funcLoadMEM();
                memoryLoad = true;
            }
            catch (NullReferenceException NRE)
            {
                MessageBox.Show(null, "An NRE occurred. Why? Win8.", "Bullshit Windows 8 Error", MessageBoxButtons.OK, MessageBoxIcon.Question);

                logEXCEPT(NRE);

                memoryLoad = false;

            }
            catch (Exception BUG)
            {
                MessageBox.Show(null, "A real exception occurred.", "We are all going to die.", MessageBoxButtons.OK, MessageBoxIcon.Question);

                logEXCEPT(BUG);

                memoryLoad = false;
            }


            if (memoryLoad == true)
            {
                try
                {
                    tbxGenRAM.Text = comp.getTotalMemorySize();
                    tbxGenDrive.Text = comp.os.GetWindowsInstall();

                    //Third Chunk
                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadOSStatus();
                    DisplayTimer.Stop();
                    funcLOG("OS status has loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadMemConfig();
                    DisplayTimer.Stop();
                    funcLOG("Memory configuration has loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadMB();
                    DisplayTimer.Stop();
                    funcLOG("Motherboard configuration has loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadGPUs();
                    DisplayTimer.Stop();
                    funcLOG("GPUs have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadSDs();
                    DisplayTimer.Stop();
                    funcLOG("Sound devices have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadNICs();
                    DisplayTimer.Stop();
                    funcLOG("Physical network interfaces have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadVNICs();
                    DisplayTimer.Stop();
                    funcLOG("Virtual network interfaces have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadLogDrvs();
                    DisplayTimer.Stop();
                    funcLOG("Logical drives have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadDskDrvs();
                    DisplayTimer.Stop();
                    funcLOG("Physical drives have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadPartitions();
                    DisplayTimer.Stop();
                    funcLOG("Partitions have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    //Fourth Chunk
                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadBIOSes();
                    DisplayTimer.Stop();
                    funcLOG("BIOSes have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadSMBIOSes();
                    DisplayTimer.Stop();
                    funcLOG("SMBIOSes have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadIRQs();
                    DisplayTimer.Stop();
                    funcLOG("IRQs have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadTPs();
                    DisplayTimer.Stop();
                    funcLOG("Thermal probes have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadMons();
                    DisplayTimer.Stop();
                    funcLOG("Monitors have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadInstalledPrograms();
                    DisplayTimer.Stop();
                    funcLOG("Installed programs have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadInstalledDrivers();
                    DisplayTimer.Stop();
                    funcLOG("Installed drivers have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadNetworkPorts();
                    DisplayTimer.Stop();
                    funcLOG("Network Ports have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    DisplayTimer = Stopwatch.StartNew();
                    funcLoadTelemetryStatus();
                    DisplayTimer.Stop();
                    funcLOG("Telemetry status has been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

                    funcLoadProgressCounter();
                    funcLOG("Performance counters started.");

                    //ComputerLoaded = true;

                }
                catch (Exception BUG)
                {
                    logEXCEPT(BUG);
                }
            }
        }

        private void BW1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tbxExtIP.Text = comp.os.GetExtIP();

                funcLOG("External IP function has finished. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
            }
            catch (InvalidOperationException IOE)
            {
                funcLOG("Ext. IP: Invalid Operation Exception occurred.");
                logEXCEPT(IOE);
            }
        }

        private void BW2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tbxCPUAddWidth.Text = comp.cpu.GetCPUAddressWidth();
                tbxCPUArch.Text = comp.cpu.GetCPUArchitecture();
                tbxCPUCaption.Text = comp.cpu.GetCPUCaption();
                tbxCPUCores.Text = comp.cpu.GetCPUCores();
                tbxCPUFamily.Text = comp.cpu.GetCPUFamily();
                tbxCPUFreq.Text = comp.cpu.GetCPUCurrentFreq();
                tbxCPUL2.Text = comp.cpu.GetCPUL2CacheSize();
                tbxCPUL3.Text = comp.cpu.GetCPUL3CacheSize();
                tbxCPULP.Text = comp.cpu.GetCPUThreads();
                tbxCPUManufacturer.Text = comp.cpu.GetCPUMan();
                tbxCPUName.Text = comp.cpu.GetCPUName();
                tbxCPUStatus.Text = comp.cpu.GetCPUStatus();
                tbxCPURevision.Text = comp.cpu.GetCPURevision();
                tbxCPURole.Text = comp.cpu.GetCPURole();
                tbxCPUSocket.Text = comp.cpu.GetCPUSocket();

                tbxGenCPU.Text = comp.cpu.GetCPUName();
            }
            catch (Exception EX)
            {
                logEXCEPT(EX);
            }

            funcLOG("Main processor information has been determined. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
        }

        private void BW3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            funcLoadCPUs();
        }

        private void BW4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            treeACTs.Nodes.Clear();

            //Load Accounts into GUI.
            if (comp.os.AccountErrorOccurred != true)
            {
                DisplayTimer = Stopwatch.StartNew();
                funcLoadAccounts();
                DisplayTimer.Stop();
                btnAccountExport.Enabled = true;
                funcLOG("Accounts have been loaded. Displayed in: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
            }
            else
            {
                funcLOG("An error occurred retrieving accounts information. Possibly AccessDenied, Null, or StackOverFlow was reached.");
            }
        }

        private void GRM_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (AccessDenied)
            {
                //MessageBox.Show("Unauthorized Access: " + UAE.Message + "\n\nStack: " + UAE.StackTrace);
                TreeNode[] foundNodes = treeWS.Nodes.Find(tbxWSName.Text, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = tbxWSName.Text + " access denied. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms";
                }

                funcLOG("Remote Query: " + tbxWSName.Text + " was access denied. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
                remMac.funcClear();

                tbxWSName.Enabled = true;
                btnWSQuery.Enabled = true;
                btnRCLEAR.Enabled = true;
            }
            else if (ErrorDetected)
            {
                //MessageBox.Show("Remote machine query failed.\n\nException: " + CEX.Message + "\n\nPossible Causes:\n" +
                //"Firewall or Anti-Virus\n" +
                //"Remote WMI / RPC Settings are preventing contact.\n" +
                //"Machine is off or does not exist.");

                TreeNode[] foundNodes = treeWS.Nodes.Find(tbxWSName.Text, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = tbxWSName.Text + " has an error, firewall, or offline. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms";
                }

                funcLOG("Remote Query: " + tbxWSName.Text + " has an error, firewall, or offline. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
                remMac.funcClear();

                tbxWSName.Enabled = true;
                btnWSQuery.Enabled = true;
                btnRCLEAR.Enabled = true;
            }
            else
            {
                funcDisplayRemoteInfo();

                tbxWSName.Enabled = true;
                btnWSQuery.Enabled = true;
                btnRCLEAR.Enabled = true;
            }
        }

        private void BWQ_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string IP in ListIPs)
            {
                LoopIterations++;
                AccessDenied1 = false;
                ErrorDetected1 = false;
                WMIError1 = false;

                DisplayTimer1 = Stopwatch.StartNew();

                Ping ping = new Ping();
                PingReply pingReply = ping.Send(IP, 120);

                if (pingReply.Status == IPStatus.Success)
                {
                    RemoteMachine remMac1 = new RemoteMachine();

                    try
                    {
                        remMac1 = new RemoteMachine(tbxDomainName2.Text, tbxDomainUserName.Text, tbxDomainUserPassword.Text, IP, RangeChoices);
                    }
                    catch (System.Management.ManagementException MEX)
                    {
                        WMIError1 = true;

                        MEX.ToString();
                        continue;
                    }
                    // I am trying to manipulate the treeview with an access denied concatenation so I am sending this exception
                    // on up to the next level.
                    catch (System.UnauthorizedAccessException UEX)
                    {
                        AccessDenied1 = true;

                        //Bypasses IDE declared but never used.  Error handling for these two were moved to next stack up
                        //so it could be logged.
                        UEX.ToString();
                        continue;
                    }
                    catch (System.Runtime.InteropServices.COMException CEX)
                    {
                        ErrorDetected1 = true;

                        //Bypasses IDE declared but never used.  Error handling for these two were moved to next stack up
                        //so it could be logged.
                        CEX.ToString();
                        continue;
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("If you are seeing this message, very very very strong chance I have no idea what caused this.\n\n" +
                                        "You should definitely submit this to me.\n\n" + Ex.Message + "\n\nStack Trace: " + Ex.StackTrace);
                        continue;
                    }
                    finally
                    {
                        if (AccessDenied1)
                        {
                            ListADs.Add(IP);
                        }
                        else if (ErrorDetected1)
                        {
                            ListEDs.Add(IP);
                        }
                        else if (WMIError1)
                        {
                            ListWEs.Add(IP);
                        }

                        DisplayTimer1.Stop();

                        RangeTimeElapsed += (int)DisplayTimer1.ElapsedMilliseconds;
                        BWQ.ReportProgress((int)(((double)LoopIterations / ListIPs.Count) * 100));
                    }

                    DisplayTimer1.Stop();

                    remMac1.ElapsedTime = DisplayTimer1.ElapsedMilliseconds;
                    RangeTimeElapsed += (int)DisplayTimer1.ElapsedMilliseconds;

                    ListRMs.Add(remMac1);
                    BWQ.ReportProgress((int)(((double)LoopIterations / ListIPs.Count) * 100));
                }
                else
                {
                    ListOff.Add(IP);

                    DisplayTimer1.Stop();

                    RangeTimeElapsed += (int)DisplayTimer1.ElapsedMilliseconds;
                    BWQ.ReportProgress((int)(((double)LoopIterations / ListIPs.Count) * 100));
                }
            }
        }

        private void BWQ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*MessageBox.Show("Success.\n\nMachines Queried: " + MachinesQueried + "\n" +
                            "Machines Errored: " + ListEDs.Count + "\n" +
                            "Access Denied: " + ListADs.Count + "\n" + 
                            "WMI Errors: " + ListWEs.Count + "\n" +
                            "Remote Machines Found: " + ListRMs.Count + "\n" +
                            "Loop Iterations: " + LoopIterations + "\n" +
                            "Sleep Iterations: " + SleepIterations);*/

            treeIPRange.BeginUpdate();
            funcLoadRemoteMachines();
            treeIPRange.EndUpdate();

            lbRangePercent.Text = "Percent Complete: 100%";
            double calc = RangeTimeElapsed / 1000.0;
            lbRangeTotalTime.Text = "Total Time Elapsed: " + calc.ToString() + "s";

            btnRangeQuery.Enabled = true;
            btnRangeExport.Enabled = true;
            btnRangeClear.Enabled = true;

            tbxIPStartOne.Enabled = true;
            tbxIPStartTwo.Enabled = true;
            tbxIPStartThree.Enabled = true;
            tbxIPStartFour.Enabled = true;
            tbxIPEndFour.Enabled = true;
        }

        private void BWQ_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lbRangePercent.Text = "Percent Complete: " + (e.ProgressPercentage.ToString() + "%");
        }

        private void BWPQ_DoWork(object sender, DoWorkEventArgs e)
        {
            short temp = 0;

            cpuUsage = new PerformanceCounter();
            cpuUsage.CategoryName = "Processor";
            cpuUsage.CounterName = "% Processor Time";
            cpuUsage.InstanceName = "_Total";

            ramUsage = new PerformanceCounter("Memory", "Available MBytes", "");

            while (PERFCONSTANT)
            {
                Thread.Sleep(500);

                //MessageBox.Show(cpuUsage.NextValue().ToString());
                temp = (short)(cpuUsage.NextValue());
                //temp = (Int16)(cpuUsage.NextValue());
                //temp = (Int16)(cpuUsage.NextValue() * 100);

                BWPQ.ReportProgress(temp);
            }
        }

        private void BWPQ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cpuUsage.Close();
        }

        private void BWPQ_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double temp = temp = ramUsage.NextValue() / 1000.0;
            temp = (100 - ((temp / dblTotMem) * 100));

            this.lbCPUUsage.Text = "CPU: " + (e.ProgressPercentage + "%");
            this.lbRamUsage.Text = "RAM: " + temp.ToString("0.##") + "%";
        }

        private void BWCUPDATE_DoWork(object sender, DoWorkEventArgs e)
        {
            UPDATETIME = Stopwatch.StartNew();

            funcCheckUpdate();

            UPDATETIME.Stop();
        }

        private void BWCUPDATE_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            funcLOG("Latest Version check conducted. Elapsed Time: " + UPDATETIME.ElapsedMilliseconds + " ms");

            if (version != latestversion)
            { lblLatestVersion.Text = "Update Available!"; }
            else { lblLatestVersion.Text = ""; }

            tbxLog.SelectionStart = tbxLog.Text.Length;
            tbxLog.ScrollToCaret();
        }

        private void BWNP_DoWork(object sender, DoWorkEventArgs e)
        {
            while (NPCONSTANT)
            {
                Thread.Sleep(NPRATE);

                comp.os.funcPopulateNetPorts();

                NPRATE2 += 1000;
                BWNP.ReportProgress(0);
            }
        }

        private void BWNP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BWNP.CancellationPending != true)
            {
                funcLOG("NetPort refresh has succesfully cancelled.");
            }
            else
            {
                funcLOG("NetPort refresh has succesfully finished.");
            }

            cbxNPAutoRefresh.Enabled = true;
        }

        private void BWNP_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (NPRATE2 >= 3000)
            {
                cbxNPAutoRefresh.Enabled = true;
            }

            List<int> AllColumnWidths = new List<int>();
            int ScrollVert = 0;
            int ScrollHoriz = 0;
            int TotalColumns = 0;
            int TotalRows = 0;

            int selRow = -1;
            int sortCol = -1;
            ListSortDirection LSD = new ListSortDirection();


            ScrollVert = dgvNetPorts.VerticalScrollingOffset;
            ScrollHoriz = dgvNetPorts.HorizontalScrollingOffset;
            TotalColumns = dgvNetPorts.Columns.Count;
            TotalRows = dgvNetPorts.Rows.Count;

            //MessageBox.Show("Col: " + ScrollColIndex + "  Row: " + ScrollRowIndex);

            if (dgvNetPorts.SortedColumn != null)
            {
                sortCol = dgvNetPorts.SortedColumn.Index;
            }

            foreach (DataGridViewRow row in dgvNetPorts.Rows)
            {
                if (row.Selected)
                {
                    selRow = row.Index;
                }
            }

            foreach (DataGridViewColumn col in dgvNetPorts.Columns)
            {
                AllColumnWidths.Add(col.Width);
            }

            if (sortCol != -1)
            {
                if (dgvNetPorts.SortOrder.Equals(SortOrder.Ascending))
                {
                    LSD = ListSortDirection.Ascending;
                }
                else
                {
                    LSD = ListSortDirection.Descending;
                }
            }

            if (BWNP.CancellationPending != true)
            {
                this.SuspendLayout();

                funcLoadNetworkPorts();

                if (sortCol > -1)
                {
                    dgvNetPorts.Sort(dgvNetPorts.Columns[sortCol], LSD);
                    dgvNetPorts.ClearSelection();
                }

                if (selRow != -1)
                {
                    //Handling a sudden OutOfRange Exception.
                    try
                    {
                        dgvNetPorts.ClearSelection();
                        dgvNetPorts.Rows[selRow].Selected = true;
                    }
                    catch
                    {
                        dgvNetPorts.ClearSelection();
                        dgvNetPorts.Rows[dgvNetPorts.Rows.Count - 1].Selected = true;
                    }

                }

                dgvNetPorts.ScrollBars = ScrollBars.Both;
                dgvNetPorts.HorizontalScrollingOffset = ScrollHoriz;

                PropertyInfo verticalOffset = dgvNetPorts.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                verticalOffset.SetValue(this.dgvNetPorts, ScrollVert, null);

                this.ResumeLayout(true);
            }
        }

        private void funcStartStopNPAutoRefresh()
        {
            NPRATE2 = 0;
            cbxNPAutoRefresh.Enabled = false;

            if (NPCONSTANT)
            {
                if (BWNP.IsBusy)
                {
                    BWNP.CancelAsync();
                    NPCONSTANT = false;
                    btnNetRefresh.Enabled = true;
                    cbxNPAutoRefresh.Enabled = true;
                }
                else
                {
                    NPCONSTANT = false;
                    btnNetRefresh.Enabled = true;
                    cbxNPAutoRefresh.Enabled = true;
                }
            }
            else
            {
                try
                {
                    if (!(BWNP.IsBusy))
                    {
                        NPCONSTANT = true;
                        BWNP.RunWorkerAsync();
                        btnNetRefresh.Enabled = false;
                    }
                    else
                    {
                        //Tries to handle a pending cancellation by waiting for succesful cancellation
                        //and restarting the BW.  Wait loops stops when BWNP is not busy or when threshold
                        //reaches approx. ~1000 ms.
                        int threshold = 0;

                        while ((BWNP.IsBusy) && (threshold <= 1000))
                        {
                            Thread.Sleep(20);
                            threshold += 20;
                        }

                        NPCONSTANT = true;
                        BWNP.RunWorkerAsync();

                        btnNetRefresh.Enabled = false;
                    }
                }
                catch (InvalidOperationException IOE)
                {
                    logEXCEPT(IOE);
                }
            }
        }

        private void NetPortAutoRefresh(object sender, EventArgs e)
        {
            if (cbxNPAutoRefresh.Checked)
            {
                funcStartStopNPAutoRefresh();
            }
            else
            {
                funcStartStopNPAutoRefresh();
            }
        }

        private void ScrollLogDown(object sender, EventArgs e)
        {
            tbxLog.SelectionStart = tbxLog.Text.Length;
            tbxLog.ScrollToCaret();
        }

        public static void funcSetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, control, new object[] { true });
        }

        private void funcLoadProgressCounter()
        {
            BWPQ.RunWorkerAsync();
        }

        //Loads Main tab's OSENV info.
        private void funcLoadOSEnv()
        {
            /*
                0: ComputerName
                1: UserName
                2: OSVersion
                3: DomainName
                4: NETVersion
                5: OSName
                6: CompanyName
                7: InstallDate
                8: LastBootUp
            */
            funcCopyOSENV();

            if (OSEnv.Any())
            {
                tbxCompName.Text = OSEnv.ElementAt(0);
                tbxUserName.Text = OSEnv.ElementAt(1);
                tbxDomainUserName.Text = OSEnv.ElementAt(1);
                tbxOSName.Text = OSEnv.ElementAt(5);
                tbxOSVersion.Text = OSEnv.ElementAt(2);
                tbxDomainName.Text = OSEnv.ElementAt(3);


                if (OSEnv.ElementAt(3) != "No Domain")
                {
                    tbxDomainName2.Text = OSEnv.ElementAt(3);
                }
                else
                {
                    tbxDomainName2.Text = "WORKGROUP";
                }

                tbxNetVersion.Text = OSEnv.ElementAt(4);
                tbxCompanyName.Text = OSEnv.ElementAt(6);
                tbxOSInstallDate.Text = OSEnv.ElementAt(7);
                tbxLastBootUp.Text = OSEnv.ElementAt(8);

                
                if (Tools.IsHPETEnabled())
                {
                    tbxHPETStatus.Text = "True";
                }
                else
                {
                    tbxHPETStatus.Text = "False";
                }

                if (Tools.IsDEPEnabled())
                {
                    tbxDEPStatus.Text = "True";
                }
                else
                {
                    tbxDEPStatus.Text = "False";
                }
            }

            //Expanded Information
            tbxLocalization.Text = comp.os.GetLocale();
            funcLOG("System localization has been established.");

            //These two items have to be loaded at start as opposed to being
            //placed in the Designer.CS file. Visual Studio keeps changing these
            //values from multi-computer ambiguous to hard-code.  Which is 
            //terrible.
            drvBox.DataSource = Environment.GetLogicalDrives();
            fsBox.SelectedIndex = 1;

            funcLOG("The OS information in Main has been loaded.");

            try
            {
                byte[] encryptedKey = Tools.funcGetProductKey("Microsoft", Windows3264ProductKeyLocation);
                funcLOG("The OS product key has been looked for.");

                OSProductKey = Tools.funcDecodeProductKey(encryptedKey);

                if (OSProductKey != "")
                {
                    funcLOG("The OS product key has been found & decoded.");
                }
                else
                {
                    funcLOG("A OS product key has not been found & decoded.");
                }
            }
            catch (Exception EX)
            {
                logEXCEPT(EX);
            }

            try
            {
                int count = 0;


                if (Environment.Is64BitOperatingSystem)
                {
                    foreach (string keylocation in KeyLocations64)
                    {
                        string temp = "";
                        byte[] encryptedKey = Tools.funcGetProductKey("Microsoft", keylocation);
                        funcLOG("A Microsoft product key is being looked for.");

                        temp = Tools.funcDecodeProductKey(encryptedKey);

                        if (temp != "")
                        {
                            funcLOG("A Microsoft product key has been found & decoded.");
                        }
                        else
                        {
                            funcLOG("A Microsoft product key has not been found & decoded.");
                        }

                        if (temp != "")
                        {
                            if (keylocation.Contains("14.0"))
                            {
                                cbxMicrosoftKeys.Items.Add("Office 2010 (loc. " + count.ToString() + "): " + temp);
                            }
                            else if (keylocation.Contains("15.0"))
                            {
                                cbxMicrosoftKeys.Items.Add("Office 2013 (loc. " + count.ToString() + "): " + temp);
                            }
                            else if (keylocation.Contains("Internet Explorer"))
                            {
                                cbxMicrosoftKeys.Items.Add("Internet Explorer (loc. " + count.ToString() + "): " + temp);
                            }
                            else if (keylocation.Contains("Forefront"))
                            {
                                cbxMicrosoftKeys.Items.Add("Forefront Protection (loc. " + count.ToString() + "): " + temp);
                            }
                        }

                        count++;
                    }

                    count = 0;
                }

                foreach (string keylocation in KeyLocations32)
                {
                    string temp = "";
                    byte[] encryptedKey = Tools.funcGetProductKey("Microsoft", keylocation);
                    funcLOG("A Microsoft product key is being looked for.");

                    temp = Tools.funcDecodeProductKey(encryptedKey);

                    if (temp != "")
                    {
                        funcLOG("A Microsoft product key has been found & decoded.");
                    }
                    else
                    {
                        funcLOG("A Microsoft product key has not been found & decoded.");
                    }

                    if (temp != "")
                    {
                        if (keylocation.Contains("14.0"))
                        {
                            cbxMicrosoftKeys.Items.Add("Office 2010 (loc. " + count.ToString() + "): " + temp);
                        }
                        else if (keylocation.Contains("15.0"))
                        {
                            cbxMicrosoftKeys.Items.Add("Office 2013 (loc. " + count.ToString() + "): " + temp);
                        }
                        else if (keylocation.Contains("Internet Explorer"))
                        {
                            cbxMicrosoftKeys.Items.Add("Internet Explorer (loc. " + count.ToString() + "): " + temp);
                        }
                        else if (keylocation.Contains("Forefront"))
                        {
                            cbxMicrosoftKeys.Items.Add("Forefront Protection (loc. " + count.ToString() + "): " + temp);
                        }
                    }

                    count++;
                }
            }
            catch (Exception EX)
            {
                logEXCEPT(EX);
            }
            

            funcLoadAllOSInfo();

            if (comp.listOS.Any())
            {
                tbxOpersName.Text = comp.listOS.ElementAt(0).GetOpersName();
                tbxOpersVersion.Text = comp.listOS.ElementAt(0).GetOpersVersion();
                tbxOpersSP.Text = comp.listOS.ElementAt(0).GetOpersSP();
                tbxOpersInstall.Text = comp.listOS.ElementAt(0).GetOpersInstall();
                tbxOpersDir.Text = comp.listOS.ElementAt(0).GetOpersDir();
                tbxOpersRegistered.Text = comp.listOS.ElementAt(0).GetOpersRegistered();
            }

            funcLOG("The first OS detected information has been loaded.");
        }

        //Loads the rest of the OS information.
        private void funcLoadAllOSInfo()
        {
            funcLOG("All OS information has been gathered.");

            int j = 1;

            foreach (OS temp in comp.listOS)
            {
                treeOS.Nodes.Add("OS #: " + j.ToString());

                if (temp.AllOSInfo.Any())
                {
                    for (int i = 0; i < temp.allOSLength; i++)
                    {
                        treeOS.Nodes[(j - 1)].Nodes.Add(temp.AllOSInfo.ElementAt(i));
                    }
                }

                j++;
            }

            funcLOG("All OSes have finished loading into tree view.");
        }

        //List copier.
        private void funcCopyOSENV()
        {
            foreach (string temp in comp.os.ToList())
            {
                OSEnv.Add(temp);
            }
        }

        //Calls the Computer's Net Connection class to get info.
        private void funcLoadNetEnv()
        {
            tbxIPv4.Text = comp.os.GetIPv4();
            tbxIPv6.Text = comp.os.GetIPv6();
            tbxExtIP.Text = comp.os.GetExtIP();

            funcLOG("System IPs have been gathered.");
        }

        //Complex function for loading Active Connections to Main tab's
        //tree view.
        private void funcLoadConnections()
        {
            funcLoadActCons();
        }

        //Created this function if I one day learned how to handle the
        //inactive functions and wanted them separate.
        private void funcLoadActCons()
        {
            funcLOG("All active connection information has been gathered.");

            int j = 1;

            foreach (NetCon con in comp.ActCons)
            {
                treeCONs.Nodes.Add("Active Connection #: " + j.ToString() + " - " + con.getConnName());

                if (con.CONInfo.Any())
                {
                    for (int i = 0; i < con.intCONlength; i++)
                    {
                        treeCONs.Nodes[(j - 1)].Nodes.Add(con.CONInfo.ElementAt(i));
                    }
                }

                j++;
            }

            funcLOG("All active connection have finished loading into tree view.");
        }

        //Loading specific settings from either registry or from K32.
        private void funcLoadOSStatus()
        {
            try
            {
                if (Tools.funcCheckCoreParking())
                {
                    tbxCoreParking.Text = "Enabled";
                }
                else
                {
                    tbxCoreParking.Text = "Disabled";
                }

                funcLOG("Core park check completed.");

                if (Tools.funcCheckUAC())
                {
                    tbxUAC.Text = "Enabled";
                }
                else
                {
                    tbxUAC.Text = "Disabled";
                }

                funcLOG("Local UAC check completed.");

                if (Tools.funcCheckUACRemoteStatus())
                {
                    tbxUACRemoteStatus.Text = "Enabled";
                }
                else
                {
                    tbxUACRemoteStatus.Text = "Disabled";
                }

                funcLOG("UAC Remote Restrictions check completed.");

                if (!(Properties.Settings.Default.IGNORE_WINDOWS_FIREWALL))
                {
                    if (Tools.funcCheckFirewallStatus())
                    {
                        tbxFirewallEnabled.Text = "Enabled";
                    }
                    else
                    {
                        tbxFirewallEnabled.Text = "Disabled";
                    }

                    funcLOG("Firewall status check completed.");
                }
                else
                {
                    tbxFirewallEnabled.Text = "Ignored.";
                    funcLOG("Firewall status check disabled.");
                }

                if (!(Properties.Settings.Default.IGNORE_WINDEF_MESSAGE))
                {
                    if (Tools.funcCheckWindowsDefenderStatus())
                    {
                        tbxDefenderStatus.Text = "Enabled";
                    }
                    else
                    {
                        tbxDefenderStatus.Text = "Disabled";
                    }

                    funcLOG("Windows Defender status check completed.");
                }
                else
                {
                    tbxDefenderStatus.Text = "Ignored.";
                    funcLOG("Windows Defender status check disabled.");
                }



                tbxHiberSize.Text = Tools.funcCheckHiberFileSize();
                funcLOG("Hibernate file size/check completed.");
            }
            catch (Exception EX)
            {
                MessageBox.Show("OS Status Exception: " + EX.Message + "\n\n" +
                                "Stack Trace: " + EX.StackTrace);
            }
        }

        //Populates the RAW MEM info to TreeView.
        private void funcLoadMEM()
        {
            funcLOG("All memory information has been gathered.");

            int j = 1;
            this.intTotMem = 0;

            //MessageBox.Show("There are a total of " + comp.intMemModules.ToString() + " recognized DIMMS");

            foreach (MEM mem in comp.listMEM)
            {

                treeMEM.Nodes.Add("RAM Module #: " + j.ToString());

                for (int i = 0; i < mem.intMEMLength; i++)
                {
                    treeMEM.Nodes[(j - 1)].Nodes.Add(mem.MEMInfo.ElementAt(i));
                }

                if (j == 1)
                {
                    tbxMEMManu.Text = mem.getMEMManufacturer();
                    tbxMEMManu.Text = tbxMEMManu.Text.Replace("Manufacturer: ", "");

                    tbxMEMSpeed.Text = mem.getMEMSpeed();
                    tbxMEMSpeed.Text = tbxMEMSpeed.Text.Replace("Speed: ", "");

                    funcLOG("Specifics of the primary DIMM have been set.");
                }

                intTotMem += mem.getMemSize();
                //tbxLog.Text += "Int Mem Size #" + j + " is " + intTotMem;
                //tbxLog.Text += System.Environment.NewLine;

                funcLOG("DIMM" + (j - 1).ToString() + " has been loaded.");
                j++;
            }

            dblTotMem = Convert.ToDouble(this.intTotMem);

            //tbxLog.Text += "Double Converted Value: " + dblTotMem;
            //tbxLog.Text += System.Environment.NewLine;

            dblTotMem = (dblTotMem / 1073741824);
            //dblTotMem = (dblTotMem / 1050702048);

            //tbxLog.Text += "Double Bytes to Gigabytes: " + dblTotMem;
            //tbxLog.Text += System.Environment.NewLine;

            dblTotMem = Math.Round(dblTotMem, 2);

            //tbxLog.Text += "Double value rounded: " + dblTotMem;
            //tbxLog.Text += System.Environment.NewLine;

            tbxMEMTotal.Text = dblTotMem.ToString("0.##") + " GB";

            funcLOG("All memory has finished loading into tree view.");
        }

        //Populates the RAW CPU info to TreeView.
        private void funcLoadCPUs()
        {
            funcLOG("All processor information has been gathered. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

            int j = 1;

            foreach (CPU cpu in comp.listCPU)
            {
                treePROCS.Nodes.Add("Processor #: " + j.ToString());

                for (int i = 0; i < cpu.intCPULength; i++)
                {
                    treePROCS.Nodes[(j - 1)].Nodes.Add(cpu.CPUInfo2.ElementAt(i));
                }

                j++;
            }

            funcLOG("All processors have finished loading into tree view.");
        }

        //Populates the RAW Partitions info to TreeView.
        private void funcLoadPartitions()
        {
            funcLOG("All partition info has been gathered. Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");

            int j = 1;

            foreach (DiskPartition dp in comp.os.DPs)
            {
                treePARTs.Nodes.Add("Partition #: " + j.ToString());

                for (int i = 0; i < dp.intPRTLength; i++)
                {
                    treePARTs.Nodes[(j - 1)].Nodes.Add(dp.PartitionInfo.ElementAt(i));
                }

                j++;
            }

            funcLOG("All partitions have finished loading into tree view.");
        }

        //Populates the RAW BIOS info to TreeView.
        private void funcLoadBIOSes()
        {
            funcLOG("All BIOS information has been gathered.");

            int j = 1;

            foreach (BIOS bios in comp.listBIOS)
            {
                treeBIOS.Nodes.Add("BIOS #: " + j.ToString());

                for (int i = 0; i < bios.intBIOSLength; i++)
                {
                    treeBIOS.Nodes[(j - 1)].Nodes.Add(bios.BIOSInfo.ElementAt(i));
                }

                j++;
            }

            funcLOG("All BIOSes have finished loading into tree view.");

            funcLoadFirstBIOS();
        }

        //Populates the RAW Motherboard info to TreeView.
        private void funcLoadMB()
        {
            funcLOG("All Motherboard/Chispet information has been gathered.");
            treeMB.Nodes.Clear();

            treeMB.Nodes.Add("Motherboard / Chipset #0");

            foreach (string temp in comp.mb.MBInfo)
            {
                treeMB.Nodes[0].Nodes.Add(temp);
            }

            tbxMBName.Text = comp.mb.getName();
            tbxMBManu.Text = comp.mb.getManu();
            tbxMBVersion.Text = comp.mb.getVersion();
            tbxMBSerial.Text = comp.mb.getSerial();
            tbxMBPID.Text = comp.mb.getPID();

            funcLOG("All Motherboard/Chispets have finished loading into tree view.");
        }

        //Populates the RAW SMBIOS info to TreeView.
        private void funcLoadSMBIOSes()
        {
            funcLOG("All SMBIOS Memory Array information has been gathered.");

            int j = treeBIOS.Nodes.Count + 1;

            foreach (SMBIOS smbios in comp.listSMBIOS)
            {
                treeBIOS.Nodes.Add("SMBIOS" + (j - 1).ToString(), "SMBIOS Memory Array #: " + (j - 1).ToString());

                for (int i = 0; i < smbios.intSMLength; i++)
                {
                    treeBIOS.Nodes[(j - 1)].Nodes.Add(smbios.SMInfo.ElementAt(i));
                }

                j++;
            }

            funcLOG("All SMBIOS Memory Arrays have finished loading into tree view.");
        }

        //Populates the BIOS textbox fields.
        private void funcLoadFirstBIOS()
        {
            string[] ProductKey = new string[2];

            if (comp.listBIOS.Any())
            {
                tbxBIOSDate.Text = comp.listBIOS.ElementAt(0).getBIOSDate();
                tbxBIOSManufacturer.Text = comp.listBIOS.ElementAt(0).getBIOSManufacturer();
                tbxBIOSName.Text = comp.listBIOS.ElementAt(0).getBIOSName();
                tbxBIOSSerial.Text = comp.listBIOS.ElementAt(0).getBIOSSerial();
                tbxBIOSSMV.Text = comp.listBIOS.ElementAt(0).getBIOSVersion();
                tbxPBIOS.Text = comp.listBIOS.ElementAt(0).getBIOSPrimary();
            }

            ProductKey = Tools.funcGetEmbeddedProductKey();
            OSEmbeddedKey = ProductKey[0];
            tbxOemId.Text = ProductKey[1];

            funcLOG("Embedded BIOS key retrieval was successful and finished.");
        }

        #region Account Functions
        //Populates the Accounts info into Treeview.
        private void funcLoadAccounts()
        {
            funcLOG("All Accounts information has been gathered. Elapsed time: " + comp.os.AccountElapsedTime + " ms");

            funcPreloadAccountSections();

            funcLoadUsers();
            funcLoadGroups();
            funcLoadDomains();
            funcLoadAlias();
            funcLoadWKGs();
            funcLoadDeleted();
            funcLoadInvalid();
            funcLoadComputer();

            funcLOG("All Accounts have finished loading into tree view.");
        }

        //Populates the individual groups under funcLoadAccounts();
        /*
         * Users
         * Groups
         * Domains
         * Alias
         * WKGs
         * Deleted
         * Invalid
         * Unknown
         * Computer
        */
        private void funcPreloadAccountSections()
        {
            treeACTs.Nodes.Clear();

            treeACTs.Nodes.Add("Users", "Users - " + comp.os.UserAccounts.Count + " found.");
            treeACTs.Nodes.Add("Groups", "Groups - " + comp.os.GroupAccounts.Count + " found.");
            treeACTs.Nodes.Add("Domain", "Domain - " + comp.os.DomainAccounts.Count + " found.");
            treeACTs.Nodes.Add("Alias", "Alias - " + comp.os.AliasAccounts.Count + " found.");
            treeACTs.Nodes.Add("WellKnownGroups", "Well Known Groups - " + comp.os.WKGAccounts.Count + " found.");
            treeACTs.Nodes.Add("DeletedAccounts", "Deleted Accounts - " + comp.os.DeletedAccounts.Count + " found.");
            treeACTs.Nodes.Add("Invalid", "Invalid - " + comp.os.InvalidAccounts.Count + " found.");
            treeACTs.Nodes.Add("Unknown", "Unknown - " + comp.os.UnknownAccounts.Count + " found.");
            treeACTs.Nodes.Add("Computer", "Computer - " + comp.os.ComputerAccounts.Count + " found.");

            //User, Local & Domain
            treeACTs.Nodes[0].Nodes.Add("Local", "Local");
            treeACTs.Nodes[0].Nodes.Add("Domain", "Domain");

            //User, Local, Active & Disabled
            treeACTs.Nodes[0].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[0].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //User, Domain, Active & Disabled
            treeACTs.Nodes[0].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[0].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Group, Local & Domain
            treeACTs.Nodes[1].Nodes.Add("Local", "Local");
            treeACTs.Nodes[1].Nodes.Add("Domain", "Domain");

            //Group, Local, Active & Disabled
            treeACTs.Nodes[1].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[1].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Group, Domain, Active & Disabled
            treeACTs.Nodes[1].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[1].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Domain, Local & Domain
            treeACTs.Nodes[2].Nodes.Add("Local", "Local");
            treeACTs.Nodes[2].Nodes.Add("Domain", "Domain");

            //Domain, Local, Active & Disabled
            treeACTs.Nodes[2].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[2].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Domain, Domain, Active & Disabled
            treeACTs.Nodes[2].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[2].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Alias, Local & Domain
            treeACTs.Nodes[3].Nodes.Add("Local", "Local");
            treeACTs.Nodes[3].Nodes.Add("Domain", "Domain");

            //Alias, Local, Active & Disabled
            treeACTs.Nodes[3].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[3].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Alias, Domain, Active & Disabled
            treeACTs.Nodes[3].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[3].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //WellKnownGroups, Local & Domain
            treeACTs.Nodes[4].Nodes.Add("Local", "Local");
            treeACTs.Nodes[4].Nodes.Add("Domain", "Domain");

            //WellKnownGroups, Local, Active & Disabled
            treeACTs.Nodes[4].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[4].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //WellKnownGroups, Domain, Active & Disabled
            treeACTs.Nodes[4].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[4].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Deleted Accounts, Local & Domain
            treeACTs.Nodes[5].Nodes.Add("Local", "Local");
            treeACTs.Nodes[5].Nodes.Add("Domain", "Domain");

            //Deleted Accounts, Local, Active & Disabled
            treeACTs.Nodes[5].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[5].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Deleted Accounts, Domain, Active & Disabled
            treeACTs.Nodes[5].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[5].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Invalid, Local & Domain
            treeACTs.Nodes[6].Nodes.Add("Local", "Local");
            treeACTs.Nodes[6].Nodes.Add("Domain", "Domain");

            //Invalid, Local, Active & Disabled
            treeACTs.Nodes[6].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[6].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Invalid, Domain, Active & Disabled
            treeACTs.Nodes[6].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[6].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Unknown, Local & Domain
            treeACTs.Nodes[7].Nodes.Add("Local", "Local");
            treeACTs.Nodes[7].Nodes.Add("Domain", "Domain");

            //Unknown, Local, Active & Disabled
            treeACTs.Nodes[7].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[7].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Unknown, Domain, Active & Disabled
            treeACTs.Nodes[7].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[7].Nodes[1].Nodes.Add("Disabled", "Disabled");

            //Computer, Local & Domain
            treeACTs.Nodes[8].Nodes.Add("Local", "Local");
            treeACTs.Nodes[8].Nodes.Add("Domain", "Domain");

            //Computer, Local, Active & Disabled
            treeACTs.Nodes[8].Nodes[0].Nodes.Add("Active", "Active");
            treeACTs.Nodes[8].Nodes[0].Nodes.Add("Disabled", "Disabled");

            //Computer, Domain, Active & Disabled
            treeACTs.Nodes[8].Nodes[1].Nodes.Add("Active", "Active");
            treeACTs.Nodes[8].Nodes[1].Nodes.Add("Disabled", "Disabled");

        }

        private void funcLoadUsers()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.UserAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[0].Nodes[0].Nodes[0].Nodes.Add("User #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[0].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[0].Nodes[0].Nodes[1].Nodes.Add("User #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[0].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[0].Nodes[1].Nodes[0].Nodes.Add("User #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[0].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[0].Nodes[1].Nodes[1].Nodes.Add("User #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[0].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadGroups()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.GroupAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[1].Nodes[0].Nodes[0].Nodes.Add("Group #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[1].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[1].Nodes[0].Nodes[1].Nodes.Add("Group #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[1].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[1].Nodes[1].Nodes[0].Nodes.Add("Group #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[1].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[1].Nodes[1].Nodes[1].Nodes.Add("Group #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[1].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadDomains()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.DomainAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[2].Nodes[0].Nodes[0].Nodes.Add("Domain #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[2].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[2].Nodes[0].Nodes[1].Nodes.Add("Domain #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[2].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[2].Nodes[1].Nodes[0].Nodes.Add("Domain #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[2].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[2].Nodes[1].Nodes[1].Nodes.Add("Domain #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[2].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadAlias()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.AliasAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[3].Nodes[0].Nodes[0].Nodes.Add("Alias #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[3].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[3].Nodes[0].Nodes[1].Nodes.Add("Alias #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[3].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[3].Nodes[1].Nodes[0].Nodes.Add("Alias #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[3].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[3].Nodes[1].Nodes[1].Nodes.Add("Alias #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[3].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadWKGs()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.WKGAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[4].Nodes[0].Nodes[0].Nodes.Add("WellKnownGroups #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[4].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[4].Nodes[0].Nodes[1].Nodes.Add("WellKnownGroups #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[4].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[4].Nodes[1].Nodes[0].Nodes.Add("WellKnownGroups #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[4].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[4].Nodes[1].Nodes[1].Nodes.Add("WellKnownGroups #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[4].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadDeleted()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.DeletedAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[5].Nodes[0].Nodes[0].Nodes.Add("Deleted #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[5].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[5].Nodes[0].Nodes[1].Nodes.Add("Deleted #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[5].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[5].Nodes[1].Nodes[0].Nodes.Add("Deleted #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[5].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[5].Nodes[1].Nodes[1].Nodes.Add("Deleted #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[5].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadInvalid()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.InvalidAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[6].Nodes[0].Nodes[0].Nodes.Add("Invalid #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[6].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[6].Nodes[0].Nodes[1].Nodes.Add("Invalid #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[6].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[6].Nodes[1].Nodes[0].Nodes.Add("Invalid #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[6].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[6].Nodes[1].Nodes[1].Nodes.Add("Invalid #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[6].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadUnknown()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.UnknownAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[7].Nodes[0].Nodes[0].Nodes.Add("Unknown #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[7].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[7].Nodes[0].Nodes[1].Nodes.Add("Unknown #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[7].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[7].Nodes[1].Nodes[0].Nodes.Add("Unknown #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[7].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[7].Nodes[1].Nodes[1].Nodes.Add("Unknown #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[7].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcLoadComputer()
        {
            int j = 1;
            int k = 1;
            int l = 1;
            int m = 1;

            foreach (Account acct in comp.os.ComputerAccounts)
            {
                if ((acct.isLocal()) && (acct.isActive()))
                {
                    treeACTs.Nodes[8].Nodes[0].Nodes[0].Nodes.Add("Computer #: " + j.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[8].Nodes[0].Nodes[0].Nodes[(j - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    j++;
                }
                else if (acct.isLocal())
                {
                    treeACTs.Nodes[8].Nodes[0].Nodes[1].Nodes.Add("Computer #: " + k.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[8].Nodes[0].Nodes[1].Nodes[(k - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    k++;
                }
                else if (acct.isActive())
                {
                    treeACTs.Nodes[8].Nodes[1].Nodes[0].Nodes.Add("Computer #: " + l.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[8].Nodes[1].Nodes[0].Nodes[(l - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    l++;
                }
                else
                {
                    treeACTs.Nodes[8].Nodes[1].Nodes[1].Nodes.Add("Computer #: " + m.ToString() + " - " + acct.getName());

                    for (int i = 0; i < acct.intAcctLength; i++)
                    {
                        treeACTs.Nodes[8].Nodes[1].Nodes[1].Nodes[(m - 1)].Nodes.Add(acct.ACCTInfo.ElementAt(i));
                    }

                    m++;
                }
            }
        }

        private void funcExportAccounts()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter newStream = new StreamWriter(sfd.FileName);

                    newStream.WriteLine("Account Name, Account Type, SID, Local?, Enabled?");

                    foreach (Account temp in comp.os.UserAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2;
                                newLine += ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.DomainAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2;
                                newLine += ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.GroupAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2;
                                newLine += ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.AliasAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2;
                                newLine += ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.WKGAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2 + ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.DeletedAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2 + ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.InvalidAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2 + ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.UnknownAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2 + ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    foreach (Account temp in comp.os.ComputerAccounts)
                    {
                        string newLine = "";

                        if (temp.getName() != "")
                        {
                            foreach (string temp2 in temp.ExportData())
                            {
                                newLine += temp2 + ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    newStream.Close();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error ocurred writing a general CSV." + Environment.NewLine +
                                Environment.NewLine + "Exception Message: " + Ex.Message +
                                Environment.NewLine + "Message Stack: " + Ex.StackTrace);
                }
            }
        }
        #endregion

        //Populates the IRQs info into TreeView.
        private void funcLoadIRQs()
        {
            funcLOG("All IRQ information has been gathered. Elapsed time: " + comp.os.IRQElapsedTime + " ms");

            int j = 1;

            foreach (IRQ irq in comp.os.IRQs)
            {
                treeIRQ.Nodes.Add(irq.getIRQName());

                for (int i = 0; i < irq.intIRQLength; i++)
                {
                    treeIRQ.Nodes[(j - 1)].Nodes.Add(irq.IRQInfo.ElementAt(i));
                }

                //A lot of data will be sent to the log that may be unnecessary.
                //funcLOG("IRQ Detected: " + irq.getIRQNumber() + "  IRQ#: " + irq.getIRQNumber());

                j++;
            }

            funcLOG("All IRQs have finished loading into tree view.");
        }

        //Populates the NIC TreeView.
        private void funcLoadNICs()
        {
            funcLOG("All network interface information has been gathered.");

            int j = 1;

            foreach (NIC nic in comp.listNIC)
            {
                if (nic.IsPhysical)
                {
                    treeNICS.Nodes.Add("Network Adapter #: " + j.ToString() + " - " + nic.getName());

                    if (j == 1)
                    {
                        physNicName = nic.getName();
                        physNicMac = nic.getMAC();
                        physNicGuid = nic.getGUID();
                    }

                    for (int i = 0; i < nic.intNICLength; i++)
                    {
                        treeNICS.Nodes[(j - 1)].Nodes.Add(nic.NICInfo.ElementAt(i));
                    }

                    funcLOG("NIC" + j + ": " + nic.getName() + " has loaded.");
                    j++;
                }
            }

            funcLOG("All Physical NICs have finished loading in tree view.");
        }

        //Populates the VNICs tree with non-physical adapter.
        private void funcLoadVNICs()
        {
            int j = 1;

            foreach (NIC nic in comp.listNIC)
            {
                if (!(nic.IsPhysical))
                {
                    treeVNICS.Nodes.Add("VNI #: " + j.ToString() + " - " + nic.getName());

                    for (int i = 0; i < nic.intNICLength; i++)
                    {
                        treeVNICS.Nodes[(j - 1)].Nodes.Add(nic.NICInfo.ElementAt(i));
                    }

                    funcLOG("VNI" + j + ": " + nic.getName() + " has loaded.");
                    j++;
                }
            }

            funcLOG("All Virtual Network Interfaces have finished loading in tree view.");
        }

        //Populates the logical drives.
        private void funcLoadLogDrvs()
        {
            funcLOG("All logical drive information has been gathered.");

            int j = 1;

            foreach (LOGDRV drv in comp.listLOGDRV)
            {
                treeLDRVs.Nodes.Add("Drive #: " + j.ToString() + " - (" + drv.getName() + "\\)");

                for (int i = 0; i < drv.intDRVLength; i++)
                {
                    treeLDRVs.Nodes[(j - 1)].Nodes.Add(drv.DRVInfo.ElementAt(i));
                }

                funcLOG("Drive" + j + ": " + drv.getName() + " has loaded.");

                j++;
            }

            funcLOG("All logical drives have finished loading in tree view.");
        }

        //Populates the disks.
        private void funcLoadDskDrvs()
        {
            funcLOG("All physical drive information has been gathered.");

            int j = 1;

            foreach (DSKDRV drv in comp.listDSKDRV)
            {
                treePDRVs.Nodes.Add("Drive #: " + j.ToString() + " - (" + drv.getName() + "\\)");

                for (int i = 0; i < drv.intDRVLength; i++)
                {
                    treePDRVs.Nodes[(j - 1)].Nodes.Add(drv.DRVInfo.ElementAt(i));
                }

                funcLOG("Drive" + j + ": " + drv.getName() + " has loaded.");

                j++;
            }

            funcLOG("All physical drives have finished loading in tree view.");

        }

        //Populates the GPUs.
        private void funcLoadGPUs()
        {
            funcLOG("All GPU information has been gathered.");

            if (comp.listGPU.Any())
            {
                int j = 1;
                tbxGVEND.Text = comp.listGPU[0].getVendor();

                if (tbxGVEND.Text == "nVidia")
                {
                    tabHW_GPU.BackColor = Color.FromArgb(118, 185, 0);
                }
                else if (tbxGVEND.Text == "AMD")
                {
                    tabHW_GPU.BackColor = Color.FromArgb(202, 2, 31);
                    tabHW_GPU.ForeColor = Color.Silver;
                    groupBox10.ForeColor = Color.Silver;
                }

                tbxGVRAM.Text = comp.listGPU[0].getVRAM();
                tbxGMODEL.Text = comp.listGPU[0].getModel();
                tbxGDRIVER.Text = comp.listGPU[0].getDriverVersion();

                foreach (string temp in comp.listGPU[0].getDriverFiles())
                {
                    tbxGDRIVERFILES.Text += temp + " ";
                }

                tbxGINF.Text = comp.listGPU[0].getINF();
                tbxGINFSEC.Text = comp.listGPU[0].getINFSEC();
                tbxGOUT.Text = comp.listGPU[0].getOut();

                foreach (GPU gpu in comp.listGPU)
                {
                    treeGPUs.Nodes.Add("GPU #: " + j.ToString());

                    for (int i = 0; i < gpu.intGPULength; i++)
                    {
                        treeGPUs.Nodes[(j - 1)].Nodes.Add(gpu.GPUInfo.ElementAt(i));
                    }

                    j++;
                }
            }

            funcLOG("All GPUs have finished loading into tree view.");
        }

        //Populates the thermal probes.
        private void funcLoadTPs()
        {
            funcLOG("All Thermal Probe information has been gathered.");

            int j = 1;

            foreach (ThermalProbe tp in comp.listTP)
            {
                treeTPs.Nodes.Add("TP #: " + j.ToString() + " - " + tp.getName());

                for (int i = 0; i < tp.intTPLength; i++)
                {
                    treeTPs.Nodes[(j - 1)].Nodes.Add(tp.TPInfo.ElementAt(i));
                }

                j++;
            }

            funcLOG("All Thermal Probes have finished loading into tree view.");
        }

        private void funcLoadSDs()
        {
            funcLOG("All Sound Devices information has been gathered.");

            int j = 1;
            int g = treeGPUs.Nodes.Count;

            foreach (SoundDevice sd in comp.listSD)
            {
                if (sd.IsOnGPU() == false)
                {
                    treeSDs.Nodes.Add("Sound Device #: " + j.ToString() + " - " + sd.getName());

                    for (int i = 0; i < sd.intSDLength; i++)
                    {
                        treeSDs.Nodes[(j - 1)].Nodes.Add(sd.SDInfo.ElementAt(i));
                    }

                    if (j == 1)
                    {
                        tbxSVEN.Text = sd.getName();
                        tbxSPROC.Text = sd.getModel();
                        tbxSMANU.Text = sd.getManufacturer();
                    }

                    j++;
                }
                else if (sd.IsOnGPU() == true)
                {
                    treeGPUs.Nodes.Add("Sound Device (on GPUs) #: " + g.ToString() + " - " + sd.getName());

                    for (int i = 0; i < sd.intSDLength; i++)
                    {
                        treeGPUs.Nodes[g].Nodes.Add(sd.SDInfo.ElementAt(i));
                    }

                    g++;
                }
            }

            funcLOG("All Sound Devices have finished loading into tree view.");
        }

        //Populates the displays.
        private void funcLoadMons()
        {
            funcLOG("All Monitor information has been gathered.");

            int j = 1;

            if (comp.listMons.Any())
            {
                tbxDMON.Text = comp.listMons[0].getName();
                tbxDMAN.Text = comp.listMons[0].getManu();
                tbxDTYPE.Text = comp.listMons[0].getType();
                tbxDID.Text = comp.listMons[0].getDeviceID();
                tbxDSTATS.Text = comp.listMons[0].getStatus();
                tbxDPNP.Text = comp.listMons[0].getPNPID();
            }
            else
            {
                tbxDMON.Text = "No Data. (xNull)";
                tbxDMAN.Text = "No Data. (xNull)";
                tbxDTYPE.Text = "No Data. (xNull)";
                tbxDID.Text = "No Data. (xNull)";
                tbxDSTATS.Text = "No Data. (xNull)";
                tbxDPNP.Text = "No Data. (xNull)";
            }

            treeDISPS.Nodes.Add("VirtDesks", "Virtual Desktops");
            treeDISPS.Nodes.Add("PNPDisplays", "PNP Monitor Data");

            foreach (Screen screen in Screen.AllScreens)
            {
                // For each screen, add the screen properties to a list box.
                treeDISPS.Nodes[0].Nodes.Add("Device Name: " + screen.DeviceName);
                treeDISPS.Nodes[0].Nodes[j - 1].Nodes.Add("Type: " + screen.GetType().ToString());
                treeDISPS.Nodes[0].Nodes[j - 1].Nodes.Add("Primary: " + screen.Primary.ToString());
                treeDISPS.Nodes[0].Nodes[j - 1].Nodes.Add("Bits Per Pixel: " + screen.BitsPerPixel.ToString());
                treeDISPS.Nodes[0].Nodes[j - 1].Nodes.Add("Bounds: " + screen.Bounds.ToString());
                treeDISPS.Nodes[0].Nodes[j - 1].Nodes.Add("Working Area: " + screen.WorkingArea.ToString());
                j++;
            }

            j = 1;

            foreach (PNPMonitor mon in comp.listMons)
            {
                treeDISPS.Nodes[1].Nodes.Add("PNP Display #" + j.ToString() + " - " + mon.getName());

                for (int i = 0; i < mon.intMonLength; i++)
                {
                    treeDISPS.Nodes[1].Nodes[(j - 1)].Nodes.Add(mon.MONInfo.ElementAt(i));
                }

                j++;
            }

            funcLOG("All Monitors have finished loading into tree view.");
        }

        //Function load memory status of OS.
        private void funcLoadMemConfig()
        {
            tbxMEMAvail.Text = this.comp.os.getFreePhysicalMem();
            tbxVIRTAvail.Text = this.comp.os.getFreeVirtualMem();
            tbxVIRTTotal.Text = this.comp.os.getTotalVirtualMem();
            tbxPageFileFree.Text = this.comp.os.getPageFileFree();
            tbxPageFileSize.Text = this.comp.os.getPageFileSize();
            tbxMaxProcessSize.Text = this.comp.os.getMaxProcessSize();
            tbxMEMAvailPerc.Text = this.comp.os.getMEMFreePerc() + "%";
            tbxVIRTAvailPerc.Text = this.comp.os.getVIRTFreePerc() + "%";
            tbxPAGEAvailPerc.Text = this.comp.os.getPAGEFreePerc() + "%";
        }

        //Function load network ports into UI.
        private void funcLoadNetworkPorts()
        {
            funcLOG("All Network Port information has been gathered. Elapsed time: " + comp.os.NetPortElapsedTime + " ms");

            if (comp.os.NetPortsLoaded)
            {
                DataTable dt = CreateNetPortDataTable();
                dgvNetPorts.DataSource = null;

                int index = 0;

                foreach (NetObj temp in comp.os.NetPortList.NetworkProcList)
                {
                    DataRow tempRow = dt.NewRow();

                    tempRow[0] = index;
                    tempRow[1] = temp.getProtocol();
                    tempRow[2] = temp.getLocalAddress();
                    tempRow[3] = temp.getLocalPort();
                    tempRow[4] = temp.getRemoteAddress();
                    tempRow[5] = temp.getRemotePort();
                    tempRow[6] = temp.getState();
                    tempRow[7] = temp.getProcessName();
                    tempRow[8] = temp.getPID();

                    dt.Rows.Add(tempRow);
                    index++;
                }

                dgvNetPorts.DataSource = dt;

                funcLOG("All Network Port information has been loaded into the DataGridView.");
            }
            else
            {
                funcLOG("Strangely enough, OS did not successfully see any network ports open.");
            }
        }

        private DataTable CreateNetPortDataTable()
        {
            DataTable DT = new DataTable();

            DT.Columns.Add("Index", typeof(int));
            DT.Columns.Add("Protocol", typeof(string));
            DT.Columns.Add("Local Address", typeof(string));
            DT.Columns.Add("Local Port", typeof(int));
            DT.Columns.Add("Remote Address", typeof(string));
            DT.Columns.Add("Remote Port", typeof(int));
            DT.Columns.Add("State", typeof(string));
            DT.Columns.Add("Process Name", typeof(string));
            DT.Columns.Add("Process Id", typeof(int));

            return DT;
        }

        //Function of loading the Task Manager one time.
        private void funcLoadTM()
        {
            foreach (Process proc in comp.os.tm.getAllProcesses())
            {
                ListViewItem lvi = new ListViewItem();

                string[] arrString = { "", "", "", "", "" };


                arrString[0] = Tools.getProcessOwner(proc.Id);
                arrString[1] = proc.ProcessName.ToString();
                arrString[2] = proc.Id.ToString();
                //arrString[3] = proc.BasePriority.ToString();
                try
                {
                    //arrString[4] = proc.UserProcessorTime.ToString();
                }
                catch (Win32Exception DER)
                {
                    MessageBox.Show(null, "Process: " + proc.ProcessName + " won't let you view time properties. " +
                                          "I will find a work around.",
                                          "Permissions Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbxLog.Text += proc.ProcessName.ToString() + " has casued an Exception.";
                    tbxLog.Text += Environment.NewLine;
                    tbxLog.Text += "Exception: " + DER.ToString();
                    tbxLog.Text += Environment.NewLine;

                    arrString[4] = "Acess Denied! WTF?!";
                }

                lvi = new ListViewItem(arrString);

                //listProcess.Items.Add(lvi);
            }
        }

        //Needs to be added to CEH.
        private void btnGrabExternal_Click(object sender, EventArgs e)
        {
            //BackgroundWorker / Async + How To: Make Thread-Safe Calls
            //Source: http://msdn.microsoft.com/en-us/library/ms171728(v=vs.100).aspx

            funcLOG("User attempts External IP resolution.");

            if (BW1.IsBusy != true)
            {
                tbxExtIP.Text = "Loading...";
                // Start the asynchronous operation.
                BW1.RunWorkerAsync();

                funcLOG("External IP resolution succeeded.");
            }
            else
            {
                tbxExtIP.Text = "Try again in a second.";
                funcLOG("External IP resolution failed. Possibly too many user attempts were made.");
            }
        }

        //Needs to be redone.
        private void funcClearAll()
        {
            tbxCompName.Text = "";
            tbxUserName.Text = "";
            tbxLastBootUp.Text = "";
            tbxOSInstallDate.Text = "";
            tbxCompanyName.Text = "";
            tbxOSName.Text = "";
            tbxOSVersion.Text = "";
            tbxNetVersion.Text = "";
            tbxDomainName.Text = "";
            tbxIPv4.Text = "";
            tbxIPv6.Text = "";
            tbxExtIP.Text = "";

            //CPU Tab
            tbxCPUAddWidth.Text = "";
            tbxCPUArch.Text = "";
            tbxCPUCaption.Text = "";
            tbxCPUCores.Text = "";
            tbxCPUFamily.Text = "";
            tbxCPUFreq.Text = "";
            tbxCPUL2.Text = "";
            tbxCPUL3.Text = "";
            tbxCPULP.Text = "";
            tbxCPUManufacturer.Text = "";
            tbxCPUName.Text = "";
            tbxCPUStatus.Text = "";
        }

        //Todo: Log should be separated to it's own object with it's own functions.
        public void funcLOG(string input)
        {
            tbxLog.AppendText("  Log Event " + intLogEvent.ToString() + ":  " + input + Environment.NewLine);

            tbxLog.SelectionStart = tbxLog.Text.Length;
            tbxLog.ScrollToCaret();

            intLogEvent += 1;
        }

        //Eventhandler for clicking buttons and shortcuts.
        private void ClickEventHandler(object sender, EventArgs e)
        {
            if ((sender.Equals(btnCMDAdmin)) || (sender.Equals(launchCMDAsAdminToolStripMenuItem)))
            {
                Tools.startCMDasAdmin();
            }
            else if (sender.Equals(btnSysFileCheck) || (sender.Equals(sfcToolStrip)))
            {
                Tools.runSFC();
            }
            else if (sender.Equals(btnSFCLog) || (sender.Equals(openSFCLogToolStripMenuItem)))
            {
                Tools.runSFCLog();
            }
            else if (sender.Equals(btnHiberEnable))
            {
                Tools.funcEnableHibernate();

                Thread.Sleep(500);

                tbxHiberSize.Text = Tools.funcCheckHiberFileSize();
            }
            else if (sender.Equals(btnHiberDisable))
            {
                Tools.funcDisableHibernate();

                Thread.Sleep(500);

                tbxHiberSize.Text = Tools.funcCheckHiberFileSize();
            }
            /*
            else if (sender.Equals(btnRefresh))
            {
                funcRefreshAll();
            }
            else if (sender.Equals(btnClose))
            {
                Close();
            }
             * */
            else if (sender.Equals(exitToolStripMenuItem))
            {
                Close();
            }
            else if (sender.Equals(preferencesToolStripMenuItem))
            {
                MessageBox.Show("Does nothing yet ;)");
            }
            else if (sender.Equals(aboutToolStripMenuItem))
            {
                MessageBox.Show("You made this?.....  I made this.");
            }
            else if ((sender.Equals(btnIPREN)) || (sender.Equals(releaseRenewIPToolStripMenuItem)))
            {
                Tools.runIPALL();
            }
            else if (sender.Equals(controlPanelToolStripMenuItem))
            {
                Tools.runCP();
            }
            else if (sender.Equals(windowsUpdateToolStripMenuItem))
            {
                Tools.runWU();
            }
            else if (sender.Equals(windowsAdminToolsToolStripMenuItem))
            {
                Tools.runAT();
            }
            else if (sender.Equals(windowsDefenderToolStripMenuItem))
            {
                Tools.runWD();
            }
            else if (sender.Equals(windowsUserAccountsToolStripMenuItem))
            {
                Tools.runUA();
            }
            else if (sender.Equals(windowsFirewallToolStripMenuItem))
            {
                Tools.runFW();
            }
            else if (sender.Equals(computerManagementToolStripMenuItem))
            {
                Tools.runCM();
            }
            else if (sender.Equals(advFirewallToolStripMenuItem))
            {
                Tools.runAF();
            }
            else if (sender.Equals(deviceManagerToolStripMenuItem))
            {
                Tools.runEV();
            }
            else if (sender.Equals(eventViewerToolStripMenuItem))
            {
                Tools.runEV();
            }
            else if (sender.Equals(localSecurityPolicyToolStripMenuItem))
            {
                Tools.runLSP();
            }
            else if (sender.Equals(servicesToolStripMenuItem))
            {
                Tools.runSV();
            }
            else if (sender.Equals(systemConfigurationToolStripMenuItem))
            {
                Tools.runMSCONFIG();
            }
            else if (sender.Equals(taskSchedulerToolStripMenuItem))
            {
                Tools.runTS();
            }
            else if (sender.Equals(displayToolStripMenuItem))
            {
                Tools.runDI();
            }
            else if (sender.Equals(devicesPrintersToolStripMenuItem))
            {
                Tools.runDP();
            }
            else if (sender.Equals(keyboardToolStripMenuItem))
            {
                Tools.runKEYBOARD();
            }
            else if (sender.Equals(mouseMenu))
            {
                Tools.runMOUSE();
            }
            else if (sender.Equals(soundToolStripMenuItem))
            {
                Tools.runSD();
            }
            else if (sender.Equals(windowsPowerToolStripMenuItem))
            {
                Tools.runPWR();
            }
            else if (sender.Equals(windowsNetworkSharingToolStripMenuItem))
            {
                Tools.runNS();
            }
            else if (sender.Equals(linkToSTEAMPageToolStripMenuItem))
            {
                Tools.gotoSTEAMPAGE();
            }
            else if (sender.Equals(linkToFacebookPageToolStripMenuItem))
            {
                Tools.gotoFBPAGE();
            }
            else if (sender.Equals(btnCPE))
            {
                Tools.funcEnableCoreParking();

                if (Tools.funcCheckCoreParking())
                {
                    tbxCoreParking.Text = "Enabled";
                }
                else
                {
                    tbxCoreParking.Text = "Disabled";
                }

                funcLOG("Windows Core Parking was set to Enabled.");
            }
            else if (sender.Equals(btnCPD))
            {
                Tools.funcDisableCoreParking();

                if (Tools.funcCheckCoreParking())
                {
                    tbxCoreParking.Text = "Enabled";
                }
                else
                {
                    tbxCoreParking.Text = "Disabled";
                }

                funcLOG("Windows Core Parking was set to Disabled.");
            }
            else if (sender.Equals(btnUACE))
            {
                Tools.funcEnableLUAC();

                if (Tools.funcCheckUAC())
                {
                    tbxUAC.Text = "Enabled";
                }
                else
                {
                    tbxUAC.Text = "Disabled";
                }

                funcLOG("Windows Local UAC was set to Enabled.");
            }
            else if (sender.Equals(btnUACD))
            {
                Tools.funcDisableLUAC();

                if (Tools.funcCheckUAC())
                {
                    tbxUAC.Text = "Enabled";
                }
                else
                {
                    tbxUAC.Text = "Disabled";
                }

                funcLOG("Windows Local UAC was set to Disable.");
            }
            else if ((sender.Equals(linkToBYTEMeDevBlogToolStripMenuItem)) || (sender.Equals(llByteMeDev)))
            {
                Tools.gotoBYTEMEDEV();
            }
            else if (sender.Equals(networkAdaptersToolStripMenuItem))
            {
                Tools.runNA();
            }
            else if (sender.Equals(linkToOverclockNetThreadToolStripMenuItem))
            {
                Tools.gotoOCN();
            }
            else if ((sender.Equals(btnFlushDNS)) || (sender.Equals(flushDNSMenu)))
            {
                Tools.funcFlushDNS();
            }
            else if ((sender.Equals(btnRepairVSS)) || (sender.Equals(repairVSSMenu)))
            {
                Tools.funcRepairVSS();
            }
            else if (sender.Equals(btnRepairWMI))
            {
                Tools.funcRepairWMI();
            }
            else if (sender.Equals(btnHPETE))
            {
                Tools.funcEnableHPET();
            }
            else if (sender.Equals(btnHPETD))
            {
                Tools.funcDisableHPET();
            }
            else if ((sender.Equals(btnCheckDrive)))
            {
                string Drive = drvBox.SelectedItem.ToString().Replace("\\", " ");

                string Arguments = "";

                if ((fsBox.SelectedIndex == 0) || (fsBox.SelectedIndex == 2))
                {
                    foreach (int indexChecked in otherOptions.CheckedIndices)
                    {
                        if (indexChecked == 0)
                        {
                            Arguments += "/X ";
                        }

                        if (indexChecked == 1)
                        {
                            Arguments += "/F ";
                        }

                        if (indexChecked == 2)
                        {
                            Arguments += "/R ";
                        }

                        if (indexChecked == 3)
                        {
                            Arguments += "/V ";
                        }
                    }
                }
                else if (fsBox.SelectedIndex == 1)
                {
                    foreach (int indexChecked in ntfsOptions.CheckedIndices)
                    {
                        if (indexChecked == 0)
                        {
                            Arguments += "/X ";
                        }

                        if (indexChecked == 1)
                        {
                            Arguments += "/F ";
                        }

                        if (indexChecked == 2)
                        {
                            Arguments += "/R ";
                        }

                        if (indexChecked == 3)
                        {
                            Arguments += "/I ";
                        }

                        if (indexChecked == 4)
                        {
                            Arguments += "/C ";
                        }

                        if (indexChecked == 5)
                        {
                            Arguments += "/B ";
                        }
                    }

                }
                //Debug: Testing the output being sent to Tools class.
                //MessageBox.Show(Drive + " " + Arguments);
                Tools.funcCheckDSK(Drive, Arguments);
            }
            else if ((sender.Equals(btnDefrag)))
            {
                string Drive = drvBox.SelectedItem.ToString().Replace("\\", " ");

                string Arguments = "";

                foreach (int indexChecked in otherOptions.CheckedIndices)
                {
                    if (indexChecked == 0)
                    {
                        Arguments += "/C ";
                    }

                    if (indexChecked == 1)
                    {
                        Arguments += "/E ";
                    }

                    if (indexChecked == 2)
                    {
                        Arguments += "/A ";
                    }

                    if (indexChecked == 3)
                    {
                        Arguments += "/X ";
                    }

                    if (indexChecked == 4)
                    {
                        Arguments += "/H ";
                    }

                    if (indexChecked == 5)
                    {
                        Arguments += "/M ";
                    }

                    if (indexChecked == 6)
                    {
                        Arguments += "/U ";
                    }

                    if (indexChecked == 7)
                    {
                        Arguments += "/V ";
                    }
                }

                //Debug: Testing the output being sent to Tools class.
                //MessageBox.Show(Drive + " " + Arguments);
                Tools.funcDefragDSK(Drive, Arguments);
            }
            else if (sender.Equals(knownIssueToolStripMenuItem))
            {
                WolfSpec.MainGUI.Known issues = new WolfSpec.MainGUI.Known();

                issues.Show();
            }
            else if (sender.Equals(contributorsToolStripMenuItem))
            {
                WolfSpec.MainGUI.Contributions cont = new WolfSpec.MainGUI.Contributions();

                cont.Show();
            }
            else if (sender.Equals(btnDisableFire))
            {
                Tools.funcDisableWinFirewall();

                tbxFirewallEnabled.Text = "Disabled";
            }
            else if (sender.Equals(btnEnableFire))
            {
                Tools.funcEnableWinFirewall();

                tbxFirewallEnabled.Text = "Enabled";
            }
            else if (sender.Equals(btnUACRemoteDisable))
            {
                Tools.funcDisableRUAC();

                funcLOG("User disabled UAC Remote Restrictions.");

                tbxUACRemoteStatus.Text = "Disabled";
            }
            else if (sender.Equals(btnUACRemoteEnable))
            {
                Tools.funcEnableRUAC();

                funcLOG("User Enabled UAC Remote Restrictions.");

                tbxUACRemoteStatus.Text = "Enabled";
            }
            else if (sender.Equals(btnAdvCleanupSet))
            {
                string Drive = drvBox.SelectedItem.ToString().Replace("\\", " ");

                Tools.funcAdvCleanupSet(Drive);
            }
            else if (sender.Equals(btnAdvCleanupRun))
            {
                string Drive = drvBox.SelectedItem.ToString().Replace("\\", " ");

                Tools.funcAdvCleanupRun(Drive);
            }
            else if (sender.Equals(btnShowHideProductKey))
            {
                if (ShowProductKey)
                {
                    ShowProductKey = false;

                    tbxOSProductKey.Text = "***** - ***** - ***** - ***** - *****";
                }
                else
                {
                    ShowProductKey = true;

                    tbxOSProductKey.Text = OSProductKey;
                }
            }
            else if (sender.Equals(btnShowHideEmbeddedKey))
            {
                if (ShowEmbeddedKey)
                {
                    ShowEmbeddedKey = false;

                    tbxEmbeddedKey.Text = "***** - ***** - ***** - ***** - *****";
                }
                else
                {
                    ShowEmbeddedKey = true;

                    tbxEmbeddedKey.Text = OSEmbeddedKey;
                }
            }
            else if (sender.Equals(btnWSQuery))
            {
                btnWSQuery.Enabled = false;
                tbxWSName.Enabled = false;
                btnRCLEAR.Enabled = false;

                //Zeroes out an entire area.
                Array.Clear(RemChoices, 0, RemChoices.Length);

                RemChoices[0] = cbxROS.Checked;
                RemChoices[1] = cbxRBIOS.Checked;
                RemChoices[2] = cbxRCPU.Checked;
                RemChoices[3] = cbxRGPU.Checked;
                RemChoices[4] = cbxRMAC.Checked;
                RemChoices[5] = cbxRIP.Checked;
                RemChoices[6] = cbxRUSER.Checked;
                RemChoices[7] = cbxRAUSERS.Checked;

                if (!(GatherRemoteMachine.IsBusy))
                {
                    try
                    {
                        if ((tbxDomainName2.Text != "") && (tbxWSName.Text != "") && (tbxDomainUserName.Text != ""))
                        {
                            funcRemoteMachineClear();

                            TreeNode workStation = new TreeNode();
                            workStation.Name = tbxWSName.Text;
                            workStation.Text = tbxWSName.Text + " loading...";

                            treeWS.Nodes.Add(workStation);

                            GatherRemoteMachine.RunWorkerAsync();
                        }
                        else
                        {
                            MessageBox.Show("Remote Query: Please make sure that Domain, Username, and Workstation boxes, are all filled out.");

                            remMac.funcClear();

                            btnRCLEAR.Enabled = true;
                            btnWSQuery.Enabled = true;
                            tbxWSName.Enabled = true;
                        }
                    }
                    catch (UnauthorizedAccessException UAE)
                    {
                        //MessageBox.Show("Unauthorized Access: " + UAE.Message + "\n\nStack: " + UAE.StackTrace);
                        TreeNode[] foundNodes = treeWS.Nodes.Find(tbxWSName.Text, false);

                        if (foundNodes.Any())
                        {
                            foundNodes[0].Text = tbxWSName.Text + " access denied.";
                        }

                        logEXCEPT(UAE);
                        remMac.funcClear();
                    }
                }
                else
                {
                    funcRemoteMachineClear();
                }
            }
            else if (sender.Equals(btnRCLEAR))
            {
                treeWS.Nodes.Clear();

                remMac.funcClear();
            }
            else if (sender.Equals(btnRestartWMI))
            {
                Tools.funcRestartWMI();
            }
            else if (sender.Equals(btnStartWinMemTest))
            {
                Tools.funcLaunchWindowsMemTest();
            }
            else if (sender.Equals(btnStartFileSigVer))
            {
                Tools.funcLaunchWindowsFileSignatureVerification();
            }
            else if (sender.Equals(btnDriverQuery))
            {
                Tools.funcDriverQuery();
            }
            else if (sender.Equals(btnDriverQueryVerbose))
            {
                Tools.funcDriverQueryV();
            }
            else if (sender.Equals(btnOpenFiles))
            {
                Tools.funcLaunchOpenFile();
            }
            else if (sender.Equals(btnOpenFilesDisabled))
            {
                if (OFEnabled)
                {
                    Tools.funcDisableOpenFileQuery();
                    funcLOG("Open file queries are being disabled.");
                    OFEnabled = false;
                }
                else
                {
                    Tools.funcEnableOpenFileQuery();
                    funcLOG("Open file queries are being enabled.");
                    OFEnabled = true;
                }
            }
            else if (sender.Equals(btnResetCMDSize))
            {
                Tools.funcResetCMDSize();
            }
            else if (sender.Equals(btnRegEdit))
            {
                Tools.funcLaunchRegEdit();
            }
            else if (sender.Equals(btnDefenderDisable))
            {
                Tools.funcDisableWindowsDefender();
                tbxDefenderStatus.Text = "Disabled";
            }
            else if (sender.Equals(btnDefenderEnable))
            {
                Tools.funcEnableWindowsDefender();
                tbxDefenderStatus.Text = "Enabled";
            }
            else if (sender.Equals(btnDisableDEP))
            {
                Tools.funcDisableDEP();
            }
            else if (sender.Equals(btnEnableDEP))
            {
                Tools.funcEnableDEP();
            }
            else if (sender.Equals(btnOpenFileSig))
            {
                Tools.funcOpenSigVerificationFile();
            }
            else if (sender.Equals(btnDriverRefresh))
            {
                funcRefreshDrivers();
            }
            else if (sender.Equals(btnDxDiag))
            {
                Tools.runDxDiag();
            }
            else if (sender.Equals(btnDxCpl))
            {
                Tools.runDxCpl();
            }
            else if (sender.Equals(btnSharedFolderMan))
            {
                Tools.runSHM();
            }
            else if (sender.Equals(btnPolicyEditor))
            {
                Tools.runLP();
            }
            else if (sender.Equals(btnGPUpdate))
            {
                Tools.funcForceGPUpdate();
            }
            else if (sender.Equals(btnManageLUGs))
            {
                Tools.runMLUGS();
            }
            else if (sender.Equals(btnMSINFO32))
            {
                Tools.runMSINFO32();
            }
            else if (sender.Equals(btnODBCA))
            {
                Tools.runODBCA();
            }
            else if (sender.Equals(btnPerfMon))
            {
                Tools.runPerfMon();
            }
            else if (sender.Equals(btnPrintManager))
            {
                Tools.runPrintMan();
            }
            else if (sender.Equals(btnResMon))
            {
                Tools.runResMon();
            }
            else if (sender.Equals(btnRSOP))
            {
                Tools.runRSOP();
            }
            else if (sender.Equals(btnLocalSec))
            {
                Tools.runLocSec();
            }
            else if (sender.Equals(btnQuickConfigRM))
            {
                Tools.runQuickConfigRM();
            }
            else if (sender.Equals(btnWinRemote))
            {
                Tools.runWinRM();
            }
            else if (sender.Equals(btnWMIMgmt))
            {
                Tools.runWMIMgmt();
            }
            else if (sender.Equals(btnPrintMigration))
            {
                Tools.runPrinterMigration();
            }
            else if (sender.Equals(btnWindowsFeatures))
            {
                Tools.runWindowsFeatures();
            }
            else if ((sender.Equals(btnMSTSC)) || (sender.Equals(mSTSCToolStripMenuItem)))
            {
                Tools.runMSTSC();
            }
            else if (sender.Equals(btnRemoteAssistance))
            {
                Tools.runWRMA();
            }
            else if (sender.Equals(btnMSCONFIG))
            {
                Tools.runMSCONFIG();
            }
            else if (sender.Equals(btnWMSRT))
            {
                Tools.runWMSRT();
            }
            else if (sender.Equals(changeLogToolStripMenuItem))
            {
                ChangeLog newCL = new ChangeLog();
                newCL.Show();
            }
            else if ((sender.Equals(updatesToolStripMenuItem))||(sender.Equals(lblLatestVersion)))
            {
                Update newUpd = new Update(version);
                newUpd.Show();
            }
            else if (sender.Equals(btnOracle))
            {
                Oracle newOracle = new Oracle();
                newOracle.Show();
            }
            else if (sender.Equals(btnSNR))
            {
                SNR newSNR = new SNR();
                newSNR.Show();
            }
            else if (sender.Equals(lnkWMIDU))
            {
                Tools.gotoWMIDU();
            }
            else if (sender.Equals(lnkNFRT))
            {
                Tools.gotoNFRT();
            }
            else if (sender.Equals(btnAccountExport))
            {
                funcExportAccounts();
            }
            else if (sender.Equals(btnNetRefresh))
            {
                funcNetPortRefresh();
            }
            else if (sender.Equals(btnNetExport))
            {
                funcNetPortExport();
            }
            else if (sender.Equals(btnACI))
            {
                CI_Start NewReport = new CI_Start();

                NewReport.Show();
            }
        }

        //Switches out options for ScanDisk based on Filesystem the user has
        //picked.  Defaults with NTFS options.
        private void funcChangeChkDiskOptions()
        {
            if ((fsBox.SelectedIndex == 0) || (fsBox.SelectedIndex == 2))
            {
                otherOptions.Enabled = true;
                otherOptions.Visible = true;

                ntfsOptions.Enabled = false;
                ntfsOptions.Visible = false;
            }
            else if (fsBox.SelectedIndex == 1)
            {
                otherOptions.Enabled = false;
                otherOptions.Visible = false;

                ntfsOptions.Enabled = true;
                ntfsOptions.Visible = true;
            }
        }

        //Eventhandler for changing disk file system.
        private void chkDskOptions_SelectionChange(object sender, EventArgs e)
        {
            funcChangeChkDiskOptions();
        }

        //Function for gathering unhandled exceptions in the Log to be dealt with better
        //and for better user reporting.
        private void logEXCEPT(Exception BUG)
        {
            bool ExceptionRecorded = false;
            int ErrorTryCounter = 0;

            /*
             * The goal here is to recourd Exceptions.  The problem with that
             * is what happens when an exception occurs within the exception process
             * or the .NET FrameWork.
             * 
             * I attempt to handle this situation by handling a variety of exceptions
             * but only attempting to catch them 5 times before giving up.
             * 
             * The function also allows for it to call itself recursively.
             */
            do
            {
                try
                {
                    if (BUG != null)
                    {
                        funcLOG("-----------Exception Begin-----------------");
                        funcLOG("Source: " + BUG.Source.ToString());
                        funcLOG("Message: " + BUG.Message.ToString());
                        funcLOG("Stack Tace: " + BUG.StackTrace.ToString());
                        funcLOG("Inner Exception: " + BUG.InnerException.ToString());
                        funcLOG("Exception: " + BUG.ToString());
                        funcLOG("-----------Exception End-------------------");

                        ExceptionRecorded = true;
                    }
                }
                catch (InvalidOperationException IOE)
                {
                    Thread.Sleep(100);
                    ErrorTryCounter++;

                    funcLOG("Exception Log: Catch IOE ocurred. Count: " + ErrorTryCounter.ToString());
                    logEXCEPT(IOE);

                    continue;
                }
                catch (Exception EX)
                {
                    Thread.Sleep(100);
                    ErrorTryCounter++;

                    funcLOG("Exception Log: Catch EX ocurred. Count: " + ErrorTryCounter.ToString());
                    logEXCEPT(EX);

                    continue;
                }
            } while ((ExceptionRecorded == false) && (ErrorTryCounter < 5));
        }

        private void cleanShutdown(object sender, EventArgs e)
        {
            PERFCONSTANT = false;
            NPCONSTANT = false;
            Environment.Exit(0);
        }

        /*private void controlPlacementOnResize(object sender, EventArgs e)
        {
            Point logPoint = new Point(treeLDRVs.Location.X, treeLDRVs.Location.Y);
            Point physPoint = new Point(treePDRVs.Location.X, treePDRVs.Location.Y);
            Point partPoint = new Point(treePARTs.Location.X, treePARTs.Location.Y);

            lblLog.Location = logPoint;
            lblPhys.Location = physPoint;
            lblPart.Location = partPoint;

            //        FormWindowState LastWindowState = FormWindowState.Minimized;
        }*/

        //Installed Programs Section
        private void iplUninstall_Click(object sender, EventArgs e)
        {
            comp.os.allPrograms.sortedList.ElementAt(SelectedInstalledProgram).funcUninstall();

            SelectedInstalledProgram = -1;

            funcLOG("User initiated a program uninstall.");
        }

        private void iplRefresh_Click(object sender, EventArgs e)
        {
            comp.os.allPrograms = new IPL();

            funcLoadInstalledPrograms();

            funcLOG("User initiated a Installed Program List refresh.");
        }

        private void dgvIPL_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgvInstalledProgs.ClearSelection();

                iplUninstall.Enabled = true;

                DataGridView.HitTestInfo hit = dgvInstalledProgs.HitTest(e.X, e.Y);

                if (hit.RowIndex >= 0)
                {
                    dgvInstalledProgs.Rows[hit.RowIndex].Selected = true;

                    SelectedInstalledProgram = hit.RowIndex;

                    if (!(comp.os.allPrograms.sortedList.ElementAt(SelectedInstalledProgram).IsProgramUninstallable()))
                    {
                        iplUninstall.Enabled = false;
                    }

                    menu_IPL.Show(dgvInstalledProgs, new Point(e.X, e.Y));
                }
            }
        }

        //Network Port Section
        private void npKillProcess_Click(object sender, EventArgs e)
        {
            if (NP_KillProcess != -1)
            {
                comp.os.NetPortList.KillProcess(NP_KillProcess);

                if (!(cbxNPAutoRefresh.Checked))
                {
                    funcNetPortRefresh();
                }

                string temp = "{null}";

                try { temp = comp.os.NetPortList.NetworkProcList.ElementAt(NP_KillProcess).getProcessName(); }
                catch { /*Handling the missing process scenario.*/ }

                funcLOG("User initiated a kill process on " + temp);

                NP_KillProcess = -1;
            }

            npKillProcess.Text = "Kill Process";
        }

        private void npKillProcessAndChildren_Click(object sender, EventArgs e)
        {
            if (NP_KillProcess != -1)
            {
                comp.os.NetPortList.KillProcessAndChildren(NP_KillProcess);

                if (!(cbxNPAutoRefresh.Checked))
                {
                    funcNetPortRefresh();
                }

                string temp = "{null}";

                try { temp = comp.os.NetPortList.NetworkProcList.ElementAt(NP_KillProcess).getProcessName(); }
                catch { /*Handling the missing process scenario.*/ }

                funcLOG("User initiated a kill process on " + temp);

                NP_KillProcess = -1;
            }
        }

        private void dgvNetPort_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgvNetPorts.ClearSelection();

                npKillProcess.Text = "Kill Process";
                npKillProcess.Enabled = true;
                npKillProcessAndChildren.Enabled = true;

                DataGridView.HitTestInfo hit = dgvNetPorts.HitTest(e.X, e.Y);

                if (hit.RowIndex >= 0)
                {
                    dgvNetPorts.Rows[hit.RowIndex].Selected = true;
                    NP_KillProcess = ((int)dgvNetPorts.SelectedRows[0].Cells[0].Value);

                    try
                    {
                        NP_KillProcessName = comp.os.NetPortList.NetworkProcList.ElementAt(NP_KillProcess).getProcessName() +
                                           " (" + NP_KillProcess + ")";

                        if (comp.os.NetPortList.NetworkProcList.ElementAt(NP_KillProcess).getIsSystem())
                        {
                            npKillProcess.Enabled = false;
                            npKillProcessAndChildren.Enabled = false;
                        }
                        else
                        {
                            npKillProcess.Text += " " + NP_KillProcessName;
                        }
                    }
                    catch
                    {
                        npKillProcess.Enabled = false;
                        npKillProcessAndChildren.Enabled = false;
                    }

                    //Debug Testing
                    //MessageBox.Show("Hit Index: " + KillProcess +
                    //    "\nProcess Name at Hit: " + comp.os.NetPortList.NetworkProcList.ElementAt(KillProcess).getProcessName());

                    menu_NetPort.Show(dgvNetPorts, new Point(e.X, e.Y));
                }
            }
        }

        private void funcNetPortRefresh()
        {
            if (dgvNetPorts.Rows.Count > 0)
            {
                NP_CurrentSelection = ((int)dgvNetPorts.SelectedRows[0].Cells[0].Value);
            }

            comp.os.funcPopulateNetPorts();
            funcLoadNetworkPorts();

            if (NP_CurrentSelection != -1)
            {
                dgvNetPorts.ClearSelection();
                dgvNetPorts.Rows[NP_CurrentSelection].Selected = true;
                NP_CurrentSelection = -1;
            }
        }

        private void funcNetPortExport()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter newStream = new StreamWriter(sfd.FileName);

                    newStream.WriteLine("Index, Protocol, Local Address, Local Port, Remote Address, Local Port, State, Process Name, Process Id, ");

                    if (dgvNetPorts.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvNetPorts.Rows)
                        {
                            string newLine = "";

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null)
                                {
                                    string temp = cell.Value.ToString();

                                    if (temp != "")
                                    {
                                        newLine += temp + ", ";
                                    }
                                    else
                                    {
                                        newLine += ", ";
                                    }
                                }
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    newStream.Close();
                }
                catch(Exception bug)
                {
                    MessageBox.Show("Network Ports Export Exception:\n\n" + bug.Message);
                }
            }
        }

        //Installed Driver Section
        private void funcLoadInstalledPrograms()
        {
            this.SuspendLayout();

            dgvInstalledProgs.Columns.Clear();

            DataTable dtOut = new DataTable();

            dtOut.Columns.Add("Program Name", typeof(string));
            dtOut.Columns.Add("Version", typeof(string));
            dtOut.Columns.Add("Install Date", typeof(string));
            dtOut.Columns.Add("Uninstall Command", typeof(string));

            foreach (InstalledProgram inpro in comp.os.allPrograms.getSortedList())
            {
                dtOut.Rows.Add(inpro.getProgramName(), inpro.getProgramVersion(),
                    inpro.getProgramInstallDate(), inpro.getUninstallCommand());
            }

            dgvDrivers.DataSource = null;
            dgvInstalledProgs.DataSource = dtOut;  

            this.ResumeLayout(true);
        }

        private void funcLoadInstalledDrivers()
        {
            this.SuspendLayout();

            dgvDrivers.DataSource = null;
            dgvDrivers.DataSource = comp.os.allDrivers.getDT();

            this.ResumeLayout(true);
        }

        private void funcRefreshDrivers()
        {
            comp.os.allDrivers = new DRVL();

            dgvDrivers.Columns.Clear();
            dgvDrivers.DataSource = comp.os.allDrivers.getDT();
            dgvDrivers.Refresh();

            funcLOG("User initiated a Drivers List refresh.");
        }

        //Remote Machine Query Section
        private void funcDisplayRemoteInfo()
        {
            funcRemoteMachineClear();

            TreeNode workStation = new TreeNode();
            workStation.Name = tbxWSName.Text;
            workStation.Text = tbxWSName.Text;

            treeWS.Nodes.Add(workStation);

            TreeNode[] foundNode = treeWS.Nodes.Find(tbxWSName.Text, false);

            foreach (string item in remMac.RemoteInfo)
            {
                foundNode[0].Nodes.Add(item);
            }

            if (foundNode.Any())
            {
                funcLOG("Remote Query: Query sucessfully detected " + tbxWSName.Text + ". Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
            }

            funcLOG("Remote Query: Query has finished for " + tbxWSName.Text + ". Elapsed time: " + DisplayTimer.ElapsedMilliseconds.ToString() + " ms");
        }

        private void funcRemoteMachineClear()
        {
            TreeNode[] foundNode = treeWS.Nodes.Find(tbxWSName.Text, true);

            if (foundNode.Any())
            {
                foreach (TreeNode tn in foundNode)
                {
                    tn.Remove();
                }
            }

            foundNode = treeWS.Nodes.Find(tbxWSName.Text + " loading...", true);

            if (foundNode.Any())
            {
                foreach (TreeNode tn in foundNode)
                {
                    tn.Remove();
                }
            }

            foundNode = treeWS.Nodes.Find(" ", true);

            if (foundNode.Any())
            {
                foreach (TreeNode tn in foundNode)
                {
                    tn.Remove();
                }
            }
        }

        #region IP Range Section
        private void IPR_KeyDown(object sender, KeyEventArgs e)
        {
            //Keys.Decimal is the Decimal value on the Numpad and Keys.OemPeriod
            //act as the period key on most keyboards.
            if ((e.KeyCode == Keys.Decimal) || (e.KeyCode == Keys.OemPeriod))
            {
                if ((sender.Equals(tbxIPStartOne)) || (sender.Equals(tbxIPStartTwo)) || (sender.Equals(tbxIPStartThree)))
                {
                    this.GetNextControl((Control)sender, true).Focus();
                }

                /*Prevent selection after Focus();
                if (sender is TextBox)
                {
                    TextBox temp = (TextBox)sender;

                    temp.SelectionLength = 0;
                }*/

                //Force selection after Focus();
                if (sender is TextBox)
                {
                    TextBox temp = (TextBox)sender;

                    temp.SelectAll();
                }

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void IPR_KeyUp(object sender, KeyEventArgs e)
        {
            bool NumberKeyPressed = false;
            bool IgnoreKey = false;

            if ((!(e.KeyCode < Keys.D0)) && (!(e.KeyCode > Keys.D9)))
            {
                NumberKeyPressed = true;
            }
            else if ((!(e.KeyCode < Keys.NumPad0)) && (!(e.KeyCode > Keys.NumPad9)))
            {
                NumberKeyPressed = true;
            }
            else if ((e.KeyCode == Keys.Back) || (e.KeyCode == Keys.Tab) || (e.KeyCode == Keys.Left) ||
                        (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
            {
                IgnoreKey = true;
            }

            if ((!(NumberKeyPressed)) && (!(IgnoreKey)))
            {
                funcClearInput(sender);
            }
            else
            {
                funcCorrectValuesOnly(sender);

                if (sender.Equals(tbxIPStartOne))
                {
                    tbxIPEndOne.Text = tbxIPStartOne.Text;
                }
                else if (sender.Equals(tbxIPStartTwo))
                {
                    tbxIPEndTwo.Text = tbxIPStartTwo.Text;
                }
                else if (sender.Equals(tbxIPStartThree))
                {
                    tbxIPEndThree.Text = tbxIPStartThree.Text;
                }
            }
        }

        private void funcClearInput(object sender)
        {

            if (sender.Equals(tbxIPStartOne))
            {
                tbxIPStartOne.Text = "";
            }
            else if (sender.Equals(tbxIPStartTwo))
            {
                tbxIPStartTwo.Text = "";
            }
            else if (sender.Equals(tbxIPStartThree))
            {
                tbxIPStartThree.Text = "";
            }
            else if (sender.Equals(tbxIPStartFour))
            {
                tbxIPStartFour.Text = "";
            }
            else if (sender.Equals(tbxIPEndFour))
            {
                tbxIPEndFour.Text = "";
            }
        }

        private void funcCorrectValuesOnly(object sender)
        {
            if (sender.Equals(tbxIPStartOne))
            {
                short temp = 0;

                if (short.TryParse(tbxIPStartOne.Text, out temp))
                {
                    if (temp > 255)
                    {
                        tbxIPStartOne.Text = "255";
                    }
                    else if (temp < 0)
                    {
                        tbxIPStartOne.Text = "0";
                    }
                }
                else
                {
                    tbxIPStartOne.Text = "0";
                }

            }
            else if (sender.Equals(tbxIPStartTwo))
            {
                short temp = 0;

                if (short.TryParse(tbxIPStartTwo.Text, out temp))
                {
                    if (temp > 255)
                    {
                        tbxIPStartTwo.Text = "255";
                    }
                    else if (temp < 0)
                    {
                        tbxIPStartTwo.Text = "0";
                    }
                }
                else
                {
                    tbxIPStartTwo.Text = "0";
                }
            }
            else if (sender.Equals(tbxIPStartThree))
            {
                short temp = 0;

                if (short.TryParse(tbxIPStartThree.Text, out temp))
                {
                    if (temp > 255)
                    {
                        tbxIPStartThree.Text = "255";
                    }
                    else if (temp < 0)
                    {
                        tbxIPStartThree.Text = "0";
                    }
                }
                else
                {
                    tbxIPStartThree.Text = "0";
                }
            }
            else if (sender.Equals(tbxIPStartFour))
            {
                short temp = 0;
                short temp2 = 0;

                if (short.TryParse(tbxIPStartFour.Text, out temp))
                {
                    if (short.TryParse(tbxIPEndFour.Text, out temp2))
                    {
                        if (temp > 254)
                        {
                            tbxIPStartFour.Text = "254";
                        }
                        else if (temp < 0)
                        {
                            tbxIPStartFour.Text = "0";
                        }
                        else if ((temp > temp2) && (temp2 != 0))
                        {
                            tbxIPStartFour.Text = (temp2 - 1).ToString();
                        }
                    }
                    else
                    {
                        tbxIPEndFour.Text = "0";
                    }
                }
                else
                {
                    tbxIPStartFour.Text = "0";
                }
            }
            else if (sender.Equals(tbxIPEndFour))
            {
                short temp = 0;
                short temp2 = 0;

                if (short.TryParse(tbxIPEndFour.Text, out temp))
                {
                    if (short.TryParse(tbxIPStartFour.Text, out temp2))
                    {
                        if (temp > 255)
                        {
                            tbxIPEndFour.Text = "255";
                        }
                        else if (temp < 0)
                        {
                            tbxIPEndFour.Text = "0";
                        }
                        else if (temp2 > temp)
                        {
                            tbxIPEndFour.Text = (temp2 + 1).ToString();
                        }
                    }
                    else
                    {
                        tbxIPStartFour.Text = "0";
                    }
                }
                else
                {
                    tbxIPEndFour.Text = "0";
                }
            }
        }

        private void IPRangeEventHandler(object sender, EventArgs e)
        {
            if (sender.Equals(btnRangeQuery))
            {
                funcIPRangeReset();

                //Testing to make sure inputs are not blank.  Password excluded.
                //Unauthorized has to be handled in code.
                if ((tbxDomainName2.Text != "") && (tbxDomainUserName.Text != "") &&
                     (tbxIPStartFour.Text != "") && (tbxIPEndFour.Text != "") &&
                     (tbxIPStartThree.Text != "") && (tbxIPEndThree.Text != "") &&
                     (tbxIPStartTwo.Text != "") && (tbxIPEndTwo.Text != "") &&
                     (tbxIPStartOne.Text != "") && (tbxIPEndOne.Text != ""))
                {
                    short[] StartRange = new short[4];
                    short[] EndRange = new short[4];

                    short.TryParse(tbxIPStartOne.Text, out StartRange[0]);
                    short.TryParse(tbxIPStartTwo.Text, out StartRange[1]);
                    short.TryParse(tbxIPStartThree.Text, out StartRange[2]);
                    short.TryParse(tbxIPStartFour.Text, out StartRange[3]);

                    short.TryParse(tbxIPEndOne.Text, out EndRange[0]);
                    short.TryParse(tbxIPEndTwo.Text, out EndRange[1]);
                    short.TryParse(tbxIPEndThree.Text, out EndRange[2]);
                    short.TryParse(tbxIPEndFour.Text, out EndRange[3]);

                    Array.Clear(RangeChoices, 0, RangeChoices.Length);

                    RangeChoices[0] = cbxRangeOS.Checked;
                    RangeChoices[1] = cbxRangeBIOS.Checked;
                    RangeChoices[2] = cbxRangeCPU.Checked;
                    RangeChoices[3] = cbxRangeGPU.Checked;
                    RangeChoices[4] = cbxRangeMAC.Checked;
                    RangeChoices[5] = cbxRangeIP.Checked;
                    RangeChoices[6] = cbxRangeUSER.Checked;
                    RangeChoices[7] = cbxRangeUSERS.Checked;

                    funcGenerateIPs(StartRange, EndRange);

                    foreach (string temp in ListIPs)
                    {
                        TreeNode[] foundNodes = treeIPRange.Nodes.Find(temp, false);

                        if (foundNodes.Any())
                        {
                            foundNodes[0].Nodes.Clear();
                        }
                        else
                        {
                            treeIPRange.Nodes.Add(temp, temp + " loading...");
                        }
                    }

                    if (!(BWQ.IsBusy))
                    {
                        tbxIPStartOne.Enabled = false;
                        tbxIPStartTwo.Enabled = false;
                        tbxIPStartThree.Enabled = false;
                        tbxIPStartFour.Enabled = false;
                        tbxIPEndFour.Enabled = false;
                        btnRangeQuery.Enabled = false;
                        btnRangeClear.Enabled = false;
                        btnRangeExport.Enabled = false;

                        BWQ.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show("IP Range Query: Please make sure that Domain, Username, and Workstation boxes, are all filled out.");
                }
            }
            else if (sender.Equals(btnRangeClear))
            {
                treeIPRange.Nodes.Clear();
                funcIPRangeReset();
                btnRangeExport.Enabled = false;
            }
            else if (sender.Equals(btnRangeExport))
            {
                funcRangeWriteToFile();
            }
        }

        private void funcRangeWriteToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter newStream = new StreamWriter(sfd.FileName);

                    //MessageBox.Show("There are " + ListRMs.Count + " Remote Machines.");

                    foreach (RemoteMachine temp in ListRMs)
                    {
                        string newLine = "";

                        //MessageBox.Show("This Remote Machine has " + temp.RemoteInfo.Count + " items.");

                        if (temp.Export.Any())
                        {
                            foreach (string temp2 in temp.Export)
                            {
                                newLine += temp2;
                                newLine += ", ";
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    newStream.Close();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error ocurred writing a general CSV." + Environment.NewLine +
                                Environment.NewLine + "Exception Message: " + Ex.Message +
                                Environment.NewLine + "Message Stack: " + Ex.StackTrace);
                }
            }
        }

        public void funcGenerateIPs(short[] startRange, short[] endRange)
        {
            ListIPs.Clear();

            End = endRange[3];
            Start = startRange[3];
            TotalIPs = (End - Start) + 1;

            /*MessageBox.Show("Start Range: " + startRange[3].ToString() + "\n" +
                            "End Range: " + endRange[3].ToString() + "\n" +
                           "Total IPs: " + TotalIPs);*/


            string Subnet = startRange[0].ToString() + "." + startRange[1].ToString() +
                            "." + startRange[2].ToString() + ".";

            for (int i = 0; i < TotalIPs; i++)
            {
                ListIPs.Add(Subnet + (Start + i).ToString());
            }
        }

        private void funcLoadRemoteMachines()
        {
            TreeNode[] foundNodes;

            foreach (RemoteMachine temp in ListRMs)
            {
                /*MessageBox.Show(temp.computerName + " has " + temp.RemoteInfo.Count + " properties. \n" +
                                temp.computerIP + " has " + temp.RemoteInfo.Count + " properties. \n" +
                                temp.WorkStation + " has " + temp.RemoteInfo.Count + " properties.");*/

                foundNodes = treeIPRange.Nodes.Find(temp.WorkStation, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = temp.WorkStation + " - Found. Elapsed time: " + temp.ElapsedTime + " ms";

                    foreach (string temp2 in temp.RemoteInfo)
                    {
                        foundNodes[0].Nodes.Add(temp2);
                    }
                }
            }

            foreach (string temp in ListADs)
            {
                //MessageBox.Show("Access Denied: " + temp);
                foundNodes = treeIPRange.Nodes.Find(temp, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = temp + " - Returned access denied!";
                }
            }

            foreach (string temp in ListEDs)
            {
                //MessageBox.Show("Error Detected: " + temp);
                foundNodes = treeIPRange.Nodes.Find(temp, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = temp + " - Returned an error/blocked by firewall/non-Windows device!";
                }
            }

            foreach (string temp in ListWEs)
            {
                //MessageBox.Show("WMI Error Detected: " + temp);
                foundNodes = treeIPRange.Nodes.Find(temp, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = temp + " - Returned a WMI error!";
                }
            }

            foreach (string temp in ListOff)
            {
                //MessageBox.Show("WMI Error Detected: " + temp);
                foundNodes = treeIPRange.Nodes.Find(temp, false);

                if (foundNodes.Any())
                {
                    foundNodes[0].Text = temp + " - Appears offline!";
                }
            }
        }

        private void funcIPRangeReset()
        {
            tbxIPStartOne.Enabled = true;
            tbxIPStartTwo.Enabled = true;
            tbxIPStartThree.Enabled = true;
            tbxIPStartFour.Enabled = true;
            tbxIPEndFour.Enabled = true;
            btnRangeClear.Enabled = true;
            btnRangeQuery.Enabled = true;
            btnRangeExport.Enabled = true;

            ListADs.Clear();
            ListEDs.Clear();
            ListWEs.Clear();
            ListIPs.Clear();

            ListRMs.Clear();
            treeIPRange.Nodes.Clear();

            MachinesQueried = 0;
            LoopIterations = 0;
            RangeTimeElapsed = 0;

            AccessDenied1 = false;
            ErrorDetected1 = false;
            WMIError1 = false;
            lbRangePercent.Text = "Percent Complete: ";
            lbRangeTotalTime.Text = "Total Time Elapsed: ";
        }
        #endregion

        //Handling copying information.
        private void NodeClicks(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);

            if (((TreeView)sender).SelectedNode != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (((TreeView)sender).SelectedNode.Nodes.Count == 0)
                    {
                        Clipboard.SetText(((TreeView)sender).SelectedNode.Text);
                    }
                    else
                    {
                        string temp = "";

                        foreach (TreeNode node in ((TreeView)sender).SelectedNode.Nodes)
                        {
                            temp += node.Text + "\n";
                        }

                        Clipboard.SetText(temp);
                    }
                }
            }

        }

        private void CopyProductKeys(object sender, EventArgs e)
        {
            Clipboard.SetText(cbxMicrosoftKeys.SelectedItem.ToString());
        }

        #region PowerShell Terminal Section
        PowerShell ps;

        BackgroundWorker BW_POSH1 = new BackgroundWorker();
        private void PS_Initialize()
        {
            rtbPOSH_ConsoleDisplay.Text = Environment.NewLine + Environment.NewLine +
                                          "  ██╗    ██╗ ██████╗ ██╗     ███████╗ " + Environment.NewLine +
                                          "  ██║    ██║██╔═══██╗██║     ██╔════╝ " + Environment.NewLine +
                                          "  ██║ █╗ ██║██║   ██║██║     █████╗   " + Environment.NewLine +
                                          "  ██║███╗██║██║   ██║██║     ██╔══╝   " + Environment.NewLine +
                                          "  ╚███╔███╔╝╚██████╔╝███████╗██║      " + Environment.NewLine +
                                          "   ╚══╝╚══╝  ╚═════╝ ╚══════╝╚═╝      " + Environment.NewLine +
                                           Environment.NewLine + "Semi-Functional Embedded PowerShell Console (v0.001)";
        }

        private void PS_TryCommand(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ps = PowerShell.Create();
                string Input = rtbPOSH_ConsoleInput.Text;
                rtbPOSH_ConsoleDisplay.Text = "Executing: " + Input;
                rtbPOSH_ConsoleDisplay.AppendText(Environment.NewLine + Environment.NewLine);

                Input = PS_FilterShortcuts(Input);
                ps.AddScript(Input);

                if ((Input.ToUpper().Contains("FT") || (Input.ToUpper().Contains("FL"))))
                {
                    if (!(Input.ToUpper().Contains("OUT-STRING")))
                    {
                        ps.AddScript(Input + " | Out-String");
                    }

                    Collection<PSObject> PSOutput = ps.Invoke();

                    if (PSOutput.Any())
                    {
                        foreach (PSObject obj in PSOutput)
                        {
                            rtbPOSH_ConsoleDisplay.AppendText(obj.ToString());
                        }
                    }
                    else
                    {
                        rtbPOSH_ConsoleDisplay.AppendText("No results.");
                    }
                }
                else
                {
                    Collection<PSObject> PSOutput = ps.Invoke();

                    if (PSOutput.Any())
                    {
                       PS_DisplayOutput(PSOutput, Input);
                    }
                    else
                    {
                        rtbPOSH_ConsoleDisplay.AppendText("No results.");
                    }
                }

                this.SuspendLayout();
                this.ResumeLayout(true);
                e.SuppressKeyPress = true;
            }
        }

        private void PS_DisplayOutput(Collection<PSObject> PSOutput, string Command)
        {
            try
            {
                foreach (PSObject result in PSOutput)
                {
                    if (result != null)
                    {

                            foreach (PSProperty prop in result.Properties)
                            {
                                string PropertyName = "";
                                string PropertyValue = "";

                                try { PropertyName = prop.Name.ToString() + ":\t"; }
                                catch { PropertyName = "Exception:\t"; }

                                if (PropertyName.Length <= 7)
                                {
                                    PropertyName += "\t\t";
                                }
                                else if (PropertyName.Length <= 14)
                                {
                                    PropertyName += "\t";
                                }

                                try { PropertyValue = prop.Value.ToString() + Environment.NewLine; }
                                catch { PropertyValue = "Exception" + Environment.NewLine; }

                                rtbPOSH_ConsoleDisplay.AppendText(PropertyName + PropertyValue);
                            }
                        }
                    }
            }
            catch(InvalidCastException ICE)
            {
                rtbPOSH_ConsoleDisplay.AppendText("Invalid Cast Exception: " + ICE.Message +
                                                 Environment.NewLine + Environment.NewLine +
                                                 "Try using | FT or | FL with your command." +
                                                 Environment.NewLine + "For example: " +
                                                 Command + " | FT");
            }
        }

        private String PS_FilterShortcuts(String Input)
        {
            string Conversion = Input;

            Conversion = Conversion.Replace("?", "Where-Object");

            return Conversion;
        }

        private void PS_LaunchPSSession(object sender, EventArgs e)
        {
            if (sender.Equals(btnLaunchVanillaPOSH))
            {
                try { Process.Start("C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe"); }
                catch { MessageBox.Show("PowerShell executable was not found."); }
            }
            else if (sender.Equals(btnLaunchWindowsPOSH))
            {
                ProcessStartInfo PSI = new ProcessStartInfo();

                string command1 = "$console = $host.ui.RawUI; $buffer = $console.BufferSize; $buffer.width = 120; " +
                                  "$console.ForegroundColor = 'White'; $console.BackgroundColor = 'Blue'; " +
                                  "$buffer.height = 2000; $console.BufferSize = $buffer; $size = $console.WindowSize; " +
                                  "$size.width = 120; $size.height = 50; $console.WindowSize = $size; " +
                                  "$host.ui.RawUI.WindowTitle = 'WOLF - PowerShell (v3.0) - WindowsStyle'; " +
                                  "Set-Location 'C:\\'; cls;";

                PSI.FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
                PSI.Arguments = "-noprofile -noexit -command " + command1;

                Process.Start(PSI);
            }
            else if (sender.Equals(btnLaunchRC1POSH))
            {
                ProcessStartInfo PSI = new ProcessStartInfo();

                string command1 = "$console = $host.ui.RawUI; $buffer = $console.BufferSize; $buffer.width = 120; " +
                                  "$console.ForegroundColor = 'Black'; $console.BackgroundColor = 'DarkGray'; " +
                                  "$a = (Get-Host).PrivateData; $a.ErrorBackgroundColor = 'darkred'; $a.ErrorForegroundColor = 'white'; " +
                                  "$buffer.height = 2000; $console.BufferSize = $buffer; $size = $console.WindowSize; " +
                                  "$size.width = 120; $size.height = 50; $console.WindowSize = $size; " +
                                  "$host.ui.RawUI.WindowTitle = 'WOLF - PowerShell (v3.0) - RagingCain Special #1'; " +
                                  "Set-Location 'C:\\'; cls;";

                PSI.FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
                PSI.Arguments = "-noprofile -noexit -command " + command1;

                Process.Start(PSI);
            }
            else if (sender.Equals(btnLaunchRC2POSH))
            {
                ProcessStartInfo PSI = new ProcessStartInfo();

                string command1 = "$console = $host.ui.RawUI; $buffer = $console.BufferSize; $buffer.width = 170; " +
                                  "$console.ForegroundColor = 'Green'; $console.BackgroundColor = 'Black'; " +
                                  "$a = (Get-Host).PrivateData; $a.ErrorBackgroundColor = 'darkred'; $a.ErrorForegroundColor = 'yellow'; " +
                                  "$buffer.height = 9999; $console.BufferSize = $buffer; $size = $console.WindowSize; " +
                                  "$size.width = 170; $size.height = 75; $console.WindowSize = $size; " +
                                  "$host.ui.RawUI.WindowTitle = 'WOLF - PowerShell (v3.0) - RagingCain Special #2'; " +
                                  "Set-Location 'C:\\'; cls;";

                PSI.FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
                PSI.Arguments = "-noprofile -noexit -command " + command1;

                Process.Start(PSI);
            }
            else if (sender.Equals(btnRemoteSigned))
            {
                ProcessStartInfo PSI = new ProcessStartInfo();

                string command1 = "$console = $host.ui.RawUI; $buffer = $console.BufferSize; $buffer.width = 120; " +
                                  "$console.ForegroundColor = 'Black'; $console.BackgroundColor = 'DarkGray'; " +
                                  "$a = (Get-Host).PrivateData; $a.ErrorBackgroundColor = 'darkred'; $a.ErrorForegroundColor = 'white'; " +
                                  "$buffer.height = 2000; $console.BufferSize = $buffer; $size = $console.WindowSize; " +
                                  "$size.width = 120; $size.height = 50; $console.WindowSize = $size; " +
                                  "$host.ui.RawUI.WindowTitle = 'WOLF - PowerShell (v3.0) - RagingCain Special 1'; " +
                                  "Set-Location 'C:\\'; cls; Set-ExecutionPolicy 'RemoteSigned' -Force; Get-ExecutionPolicy;";

                PSI.FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
                PSI.Arguments = "-noprofile -noexit -command " + command1;

                Process.Start(PSI);
            }
        }
        #endregion

        #region Licensing
        private BackgroundWorker GRL = new BackgroundWorker();

        private LicenseQuery NewLQ = new LicenseQuery();
        private List<LicenseQuery> LQS = new List<LicenseQuery>();
        private String L_Domain = "";
        private String L_Username = "";
        private String L_Password = "";
        private String L_MachineName = "";

        private void Licensing_Click(object sender, EventArgs e)
        {
            dgvLicenseQueries.DataSource = null;
            if ((tbxDomainName2.Text != "") && (tbxDomainUserName.Text !=""))
            {
                if (tbxLMachineName.Text != "")
                {
                    Licensing_TryGrab();
                }
                else
                {
                    MessageBox.Show("You have to enter a machine name.");
                }
            }
            else
            {
                MessageBox.Show("You have to enter a domain and username.");
            }
        }

        private void Licensing_TryGrab()
        {
            L_Domain = tbxDomainName2.Text;
            L_Username = tbxDomainUserName.Text;
            L_Password = tbxDomainUserPassword.Text;
            L_MachineName = tbxLMachineName.Text;

            Impersonation imp = new Impersonation(L_Domain, L_Username, L_Password);

            if (imp.ImpersonationSucceeded)
            {
                if (!(GRL.IsBusy))
                {
                    GRL.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Still working on previous request.");
                }
            }
            else
            {
                MessageBox.Show("Your credentials were not accepted.");
            }
        }

        private void GRL_DoWork(object sender, DoWorkEventArgs e)
        {
            NewLQ = new LicenseQuery();
            NewLQ.SetMachineName(L_MachineName);

            GetRemoteOSVersion();
            GetRemoteOSKey();
            GetRemoteOfficeKeys();
        }

        private void GetRemoteOSVersion()
        {
            try
            {
                ConnectionOptions newCon = new ConnectionOptions();
                newCon.Username = L_Username;
                newCon.Password = L_Password;
                newCon.Impersonation = ImpersonationLevel.Impersonate;
                newCon.Timeout = new TimeSpan(0, 0, 0, 2);
                newCon.Authority = "ntlmdomain:" + L_Domain;

                ManagementScope newMS = new ManagementScope();
                ManagementObjectSearcher newMOS = new ManagementObjectSearcher();

                newMS = new ManagementScope("\\\\" + L_MachineName.Trim() + "\\root\\CIMV2", newCon);

                ObjectQuery osQuery = new ObjectQuery("SELECT Caption FROM Win32_OperatingSystem");
                newMOS = new ManagementObjectSearcher(newMS, osQuery);

                foreach (ManagementObject item in newMOS.Get())
                {
                    NewLQ.SetOSVersion(item["Caption"].ToString().Replace("Microsoft ", ""));
                }
            }
            catch(Exception EX)
            {
                NewLQ.SetOSVersion("Error");

                MessageBox.Show("Exception Occurred: " + EX.Message + "\n\n" +
                "Stack Trace: " + EX.StackTrace);
            }
        }

        private void GetRemoteOSKey()
        {
            try
            {
                byte[] encryptedKey = Tools.funcGetRemoteProductKey("Microsoft", L_MachineName, Windows3264ProductKeyLocation);

                NewLQ.SetOSKey(Tools.funcDecodeProductKey(encryptedKey));
            }
            catch (Exception EX)
            {
                NewLQ.SetOSKey("Error");
                MessageBox.Show("Exception Occurred: " + EX.Message + "\n\n" +
                                "Stack Trace: " + EX.StackTrace);
            }
        }

        private void GetRemoteOfficeKeys()
        {
            try
            {
                string temp = "";

                foreach (string keylocation in KeyLocations64)
                {
                    if (temp == "")
                    {
                        byte[] encryptedKey = Tools.funcGetRemoteProductKey("Microsoft", L_MachineName, keylocation);

                        temp = Tools.funcDecodeProductKey(encryptedKey);

                        if (temp != "")
                        {
                            if (keylocation.Contains("12.0"))
                            {
                                NewLQ.SetOfficeVersion(temp, "Office 2007");
                            }
                            else if (keylocation.Contains("14.0"))
                            {
                                NewLQ.SetOfficeVersion(temp, "Office 2010");
                            }
                            else if (keylocation.Contains("15.0"))
                            {
                                NewLQ.SetOfficeVersion(temp, "Office 2013");
                            }
                        }
                    }
                }

                if (temp == "")
                {
                    foreach (string keylocation in KeyLocations32)
                    {
                        if (temp == "")
                        {
                            byte[] encryptedKey = Tools.funcGetRemoteProductKey("Microsoft", L_MachineName, keylocation);

                            temp = Tools.funcDecodeProductKey(encryptedKey);

                            if (temp != "")
                            {
                                if (keylocation.Contains("12.0"))
                                {
                                    NewLQ.SetOfficeVersion(temp, "Office 2007");
                                }
                                else if (keylocation.Contains("14.0"))
                                {
                                    NewLQ.SetOfficeVersion(temp, "Office 2010");
                                }
                                else if (keylocation.Contains("15.0"))
                                {
                                    NewLQ.SetOfficeVersion(temp, "Office 2013");
                                }
                            }
                        }
                    }
                }

                LQS.Add(NewLQ);
            }
            catch (Exception EX)
            {
                MessageBox.Show("Exception Occurred: " + EX.Message + "\n\n" +
                                "Stack Trace: " + EX.StackTrace);
            }
        }

        private void GRL_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SuspendLayout();

            dgvLicenseQueries.Columns.Clear();

            DataTable dtOut = new DataTable();

            dtOut.Columns.Add("Machine Name", typeof(string));
            dtOut.Columns.Add("OS", typeof(string));
            dtOut.Columns.Add("OS Key", typeof(string));
            dtOut.Columns.Add("Office", typeof(string));
            dtOut.Columns.Add("Office Key", typeof(string));
           
            foreach (LicenseQuery LQ in LQS)
            {
                string OfficeVersionTemp = "";
                foreach(string Vers in LQ.L_OfficeVersion)
                {
                    if (LQ.L_OfficeVersion.Count > 1)
                    { OfficeVersionTemp += Vers + ", "; }
                    else { OfficeVersionTemp = Vers; }

                }

                string OfficeKeyTemp = "";
                foreach(string Key in LQ.L_OfficeKey)
                {
                    if (LQ.L_OfficeKey.Count > 1)
                    { OfficeKeyTemp += Key + ", "; }
                    else { OfficeKeyTemp = Key; }
                }

                dtOut.Rows.Add(LQ.GetMachineName(), LQ.GetOSVersion(), LQ.GetOSKey(), OfficeVersionTemp, OfficeKeyTemp);
            }

            dgvLicenseQueries.DataSource = null;
            dgvLicenseQueries.DataSource = dtOut;

            this.ResumeLayout(true);
        }

        private void LicenseClearAll(object sender, EventArgs e)
        {
            dgvLicenseQueries.DataSource = null;
            dgvLicenseQueries.Refresh();
            LQS.Clear();
            NewLQ = new LicenseQuery();
        }

        private void LicenseExport(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "csv files (*.csv)|*.csv";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter newStream = new StreamWriter(sfd.FileName);

                    newStream.WriteLine("Machine Name, OS, OS Key, Office, Office Key,");

                    if (dgvLicenseQueries.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvLicenseQueries.Rows)
                        {
                            string newLine = "";

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null)
                                {
                                    string temp = cell.Value.ToString();

                                    if (temp != "")
                                    {
                                        newLine += temp + ", ";
                                    }
                                    else
                                    {
                                        newLine += ", ";
                                    }
                                }
                            }

                            newStream.WriteLine(newLine);
                        }
                    }

                    newStream.Close();
                }
                catch(Exception bug)
                {
                    MessageBox.Show("License Queries Export Exception:\n\n" + bug.Message);
                }
            }
        }
        #endregion

        #region SMTP Testing
        BackgroundWorker BW = new BackgroundWorker();
        BackgroundWorker BWADD = new BackgroundWorker();
        String ADEMAIL = "";

        bool EmailCreated = false;
        MailMessage EMAIL = null;
        SmtpClient SESSION = null;
        string SuccessMessage = "SMTP connection accepted; Email Sent!";
        string ErrorMessage = "";

        string Machine = "";
        string Relay = "";

        public void InitializeSMTPTesting()
        {
            InitializeSMTPBackgroundWorkers();

            //Convenience; grab current users email address from AD.
            if (tbxFrom.Text == "")
            {
                if (!(BWADD.IsBusy))
                {
                    BWADD.RunWorkerAsync();
                }
            }
        }

        #region BackgroundWorker Configuration
        private void InitializeSMTPBackgroundWorkers()
        {
            BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_WorkCompleted);

            BWADD.DoWork += new DoWorkEventHandler(BWADD_DoWork);
            BWADD.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWADD_WorkCompleted);
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            long RoundTrip = -1;
            RoundTrip = Pinger(Machine);

            if (RoundTrip > -1)
            {
                try
                {
                    SESSION.Send(EMAIL);
                }
                catch (SmtpFailedRecipientException EXC)
                {
                    ErrorMessage = "Possibly not added to the correct relays (ie. internal/external)." +
                        Environment.NewLine + Environment.NewLine + "Exception: " + EXC.Message;
                }
                catch (SmtpException EXC)
                {
                    if (EXC.Message.Contains("requires a secure"))
                    {
                        ErrorMessage = "Not white-listed on the relay, or bad credentials were given." +
                            Environment.NewLine + Environment.NewLine + "Exception: " + EXC.Message;
                    }
                    else if (EXC.Message.Contains("Client does not have permissions"))
                    {
                        ErrorMessage = "User authenticated, but does not have permission to Send As the \"From\" " +
                            "address." + Environment.NewLine + Environment.NewLine + "Exception: " + EXC.Message;
                    }
                    else
                    {
                        ErrorMessage = "Possible network/configuration issue elsewhere." + Environment.NewLine +
                            Environment.NewLine + "Exception: " + EXC.Message;
                    }
                }
                catch (Exception EXC)
                {
                    ErrorMessage = "Unpredicted Exception Occurred." +
                        Environment.NewLine + Environment.NewLine + "Exception: " + EXC.Message +
                        Environment.NewLine + Environment.NewLine + "Stack Trace: " + EXC.StackTrace;
                }
            }
            else
            {
                ErrorMessage = "Relay appears to be down.";
            }
        }

        private void BW_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ErrorMessage == "")
            {
                tbxResponse.Text = SuccessMessage;
            }
            else
            {
                tbxResponse.Text = ErrorMessage;
            }

            btnSendTest.Enabled = true;
        }

        private void BWADD_DoWork(object sender, DoWorkEventArgs e)
        {
            ADEMAIL = GetEmailAddress();
        }

        private void BWADD_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tbxFrom.Text = ADEMAIL;
        }

        private string GetEmailAddress()
        {
            string Email = "";

            try
            {
                Email = UserPrincipal.Current.EmailAddress;
            }
            catch { Email = "Error"; }

            return Email;
        }
        #endregion

        #region Email & Test Functions
        private void SendEmail()
        {
            btnSendTest.Enabled = false;
            ErrorMessage = "";
            tbxResponse.Text = "Attempting to send...";

            Relay = tbxRelay.Text;
            CreateSMTPSession();
            CreateEmail();

            if (EmailCreated)
            {
                if (!(BW.IsBusy))
                {
                    BW.RunWorkerAsync();
                }
            }
        }

        private void CreateSMTPSession()
        {
            if (Relay != "")
            {
                SESSION = new SmtpClient(Relay);

                if (checkCredentials.Checked)
                {
                    SESSION.Credentials = new System.Net.NetworkCredential(tbxUserName.Text,
                                                                           tbxSPassword.Text);
                }
                else if (checkBypass.Checked)
                {
                    SESSION.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                else if (checkExplicit.Checked)
                {
                    SESSION.UseDefaultCredentials = false;
                    SESSION.Credentials = null;
                }
            }
            else
            {
                MessageBox.Show("Can't relay a message without an SMTP host or email server.");
            }

        }

        private void CreateEmail()
        {
            string strTo = tbxTo.Text;
            string strFrom = tbxFrom.Text;
            string strSubject = tbxSubject.Text;
            string strBody = tbxBody.Text;

            try
            {
                EMAIL = new MailMessage(strFrom, strTo, strSubject, strBody);
                EmailCreated = true;
            }
            catch (FormatException FEX)
            {
                tbxResponse.Text = "Failed creating an email, invalid format detected." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Exception: " + FEX.Message;

                btnSendTest.Enabled = true;
                EmailCreated = false;
            }
            catch (ArgumentException AEX)
            {
                tbxResponse.Text = "Input missing detected." + Environment.NewLine +
                                    Environment.NewLine + "Exception: " + AEX.Message;

                btnSendTest.Enabled = true;
                EmailCreated = false;
            }
            catch (Exception EXC)
            {
                ErrorMessage = "Unpredicted Exception Occurred." +
                    Environment.NewLine + Environment.NewLine + "Exception: " + EXC.Message +
                    Environment.NewLine + Environment.NewLine + "Stack Trace: " + EXC.StackTrace;

                btnSendTest.Enabled = true;
                EmailCreated = false;
            }
        }

        private long Pinger(string MachineName)
        {
            long returnTime = -1;

            if (MachineName != "")
            {
                Ping pingSender = new Ping();
                IPAddress address = IPAddress.Loopback;
                PingReply reply;

                try { reply = pingSender.Send(MachineName); }
                catch { reply = null; }

                if (reply != null)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        returnTime = reply.RoundtripTime;
                    }
                    else
                    {
                        returnTime = -1;
                    }
                }
            }

            return returnTime;
        }
        #endregion

        private void BypassClicked(object sender, EventArgs e)
        {
            if (checkBypass.Checked)
            {
                checkExplicit.Enabled = false;
                checkCredentials.Enabled = false;
            }
            else
            {
                checkExplicit.Enabled = true;
                checkCredentials.Enabled = true;
            }
        }

        private void ExplicitlyClicked(object sender, EventArgs e)
        {
            if (checkExplicit.Checked)
            {
                checkBypass.Enabled = false;
                checkCredentials.Enabled = false;
            }
            else
            {
                checkBypass.Enabled = true;
                checkCredentials.Enabled = true;
            }
        }

        private void CredentialsClicked(object sender, EventArgs e)
        {
            if (checkCredentials.Checked)
            {
                tbxUserName.Enabled = true;
                tbxSPassword.Enabled = true;

                checkBypass.Enabled = false;
                checkExplicit.Enabled = false;
            }
            else
            {
                tbxUserName.Enabled = false;
                tbxSPassword.Enabled = false;

                checkBypass.Enabled = true;
                checkExplicit.Enabled = true;
            }
        }

        private void Send(object sender, EventArgs e)
        {
            SendEmail();
        }
        #endregion

        #region Windows Telemetry
        bool IsDiagTrackStopped = false;
        bool IsDiagTrackDisabled = false;
        bool IsKeyloggingStopped = false;
        bool IsKeyloggingDisabled = false;
        bool IsDataCollectionDisabled = false;
        bool IsGlobalSensorDisabled = false;
        bool IsSensorLocationDisabled = false;

        private void funcLoadTelemetryStatus()
        {
            funcLoadTelemetryOSStatus();
            funcLoadTrackingStatus();
            funcLoadKeyloggingStatus();
            funcLoadLogStatus();
            funcLoadSensorStatus();
            funcLoadHostStatus();
        }

        private void funcLoadTelemetryOSStatus()
        {
            IsDataCollectionDisabled = Tools.isDataCollectionDisabled();

            if (IsDataCollectionDisabled)
            {
                tbxTelemetryOSStatus.Text = "Disabled";
                tbxTelemetryOSStatus.BackColor = Color.LightGreen;
                btnDisableTelemetry.Text = "Enable Telemetry";
            }
            else
            {
                tbxTelemetryOSStatus.Text = "Allowed";
                tbxTelemetryOSStatus.BackColor = Color.Red;
                btnDisableTelemetry.Text = "Disable Telemetry";
            }
        }

        private void funcLoadTrackingStatus()
        {
            if (Tools.isServiceRunning("DiagTrack"))
            {
                tbxDiagnosticTrackingStatus.Text = "Running";
                IsDiagTrackStopped = false; tbxDiagnosticTrackingStatus.BackColor = Color.Red;
                btnStopDiagnosticTracking.Text = "Stop Telemetry Service";
            }
            else
            {
                tbxDiagnosticTrackingStatus.Text = "Not Running";
                IsDiagTrackStopped = true; tbxDiagnosticTrackingStatus.BackColor = Color.LightGreen;
                btnStopDiagnosticTracking.Text = "Start Telemetry Service";
            }

            string DiagTrackStartupStatus = Tools.getServiceStartupStatus("DiagTrack");
            tbxDiagnosticTrackingStartup.Text = DiagTrackStartupStatus;

            if (DiagTrackStartupStatus == "Disabled")
            {
                IsDiagTrackDisabled = true; tbxDiagnosticTrackingStartup.BackColor = Color.LightGreen;
                btnDisableDiagnosticTracking.Text = "Enable Telemetry Service Startup";
            }
            else
            {
                IsDiagTrackDisabled = false; tbxDiagnosticTrackingStartup.BackColor = Color.Red;
                btnDisableDiagnosticTracking.Text = "Disable Telemetry Service Startup";
            }

        }

        private void funcLoadKeyloggingStatus()
        {
            if (Tools.isServiceRunning("dmwappushservice"))
            {
                tbxKeylogServiceStatus.Text = "Running";
                IsKeyloggingStopped = false; tbxKeylogServiceStatus.BackColor = Color.Red;
                btnStopKeylogging.Text = "Stop User Input Logging Service";
            }
            else
            {
                tbxKeylogServiceStatus.Text = "Not Running";
                IsKeyloggingStopped = true; tbxKeylogServiceStatus.BackColor = Color.LightGreen;
                btnStopKeylogging.Text = "Start User Input Logging Service";
            }

            string Status = Tools.getServiceStartupStatus("dmwappushservice");
            tbxKeylogServiceStartup.Text = Status;

            if (Status == "Disabled")
            {
                IsKeyloggingDisabled = true; tbxKeylogServiceStartup.BackColor = Color.LightGreen;
                btnDisableKeylogging.Text = "Enable User Input Logging Service Startup";
            }
            else
            {
                IsKeyloggingDisabled = false; tbxKeylogServiceStartup.BackColor = Color.Red;
                btnDisableKeylogging.Text = "Disable User Input Logging Service Startup";
            }
        }

        private void funcLoadSensorStatus()
        {
            IsSensorLocationDisabled = Tools.isSensorDisabled();
            IsGlobalSensorDisabled = Tools.isGlobalDisabled();

            if(IsSensorLocationDisabled)
            {
                tbxLocationSensor.Text = "Disabled";
                tbxLocationSensor.BackColor = Color.LightGreen;
                btnDisableLocationSensor.Text = "Enable Location Sensor (Current User)";
            }
            else
            {
                tbxLocationSensor.Text = "Enabled";
                tbxLocationSensor.BackColor = Color.Red;
                btnDisableLocationSensor.Text = "Disable Location Sensor (Current User)";
            }

            if (IsGlobalSensorDisabled)
            {
                tbxLocationUsage.Text = "Disabled";
                tbxLocationUsage.BackColor = Color.LightGreen;
                btnDisableLocationUsage.Text = "Enable Location Usage (Current User)";
            }
            else
            {
                tbxLocationUsage.Text = "Enabled";
                tbxLocationUsage.BackColor = Color.Red;
                btnDisableLocationUsage.Text = "Disable Location Usage (Current User)";
            }

        }

        private void funcLoadHostStatus()
        {
            bool IsModified = Tools.isHostsModified();
            bool IsOwner = Tools.isOwnerOfHosts();

            if (IsModified)
            {
                tbxHostStatus.Text = "Modified";
                tbxHostStatus.BackColor = Color.LightGreen;
                btnModifyHosts.Enabled = false;
            }
            else
            {
                tbxHostStatus.Text = "Unmodified";
                tbxHostStatus.BackColor = Color.Red;
                btnModifyHosts.Enabled = true;
            }

            if (IsOwner)
            {
                tbxHostsOwner.Text = "Owner";
                btnOwnsHosts.Enabled = false;
            }
            else
            {
                tbxHostsOwner.Text = "Not Owner";
                string HostsPath = Environment.GetEnvironmentVariable("systemroot");
                HostsPath += "\\System32\\drivers\\etc\\hosts";

                if (File.Exists(HostsPath))
                { btnOwnsHosts.Enabled = true; }
                else { btnOwnsHosts.Enabled = false; }
            }
        }

        private void funcLoadLogStatus()
        {
            string IsOwner = "Not Owner";
            bool IsGone = false;

            try
            {
                if (Tools.isTrackerLogGone())
                { IsGone = true; }
                else { IsGone = false; }
            }
            catch { IsGone = false; }

            try
            {
                if (Tools.isOwnerOfLogs()) { IsOwner = "Owner"; }
                else { IsOwner = "Not Owner"; }
            }
            catch { IsOwner = "Unreadable"; }

            if (IsGone)
            {
                tbxTrackingLogStatus.Text = "Deleted";
                tbxTrackingLogStatus.BackColor = Color.LightGreen;
                btnDeleteTrackingLog.Enabled = false;
            }
            else
            {
                tbxTrackingLogStatus.Text = "Exists";
                tbxTrackingLogStatus.BackColor = Color.Red;
                btnDeleteTrackingLog.Enabled = true;
            }

            if (IsOwner == "Owner")
            {
                tbxTrackingOwner.Text = "Owner";
                btnOwnsLog.Enabled = false;
            }
            else
            {
                tbxTrackingOwner.Text = IsOwner;
                string FilePath = "C:\\ProgramData\\Microsoft\\Diagnosis\\ETLLogs\\AutoLogger\\AutoLogger-Diagtrack-Listener.etl";

                if (File.Exists(FilePath))
                { btnOwnsLog.Enabled = true; }
                else { { btnOwnsLog.Enabled = false; } }
            }
        }

        private void evt_WindowsTelemetry(object sender, EventArgs e)
        {
            if (sender.Equals(btnStopDiagnosticTracking))
            {
                if (IsDiagTrackStopped)
                { Tools.startService("DiagTrack", 60000); }
                else { Tools.stopService("DiagTrack", 60000); }
            }
            else if (sender.Equals(btnDisableDiagnosticTracking))
            {
                if (IsDiagTrackDisabled)
                { Tools.enableService("DiagTrack"); }
                else { Tools.disableService("DiagTrack"); }
            }
            else if (sender.Equals(btnStopKeylogging))
            {
                if (IsKeyloggingStopped)
                { Tools.startService("dmwappushservice", 60000); }
                else { Tools.stopService("dmwappushservice", 60000); }
            }
            else if (sender.Equals(btnDisableKeylogging))
            {
                if (IsKeyloggingDisabled)
                { Tools.enableService("dmwappushservice"); }
                else
                { Tools.disableService("dmwappushservice"); }
            }
            else if (sender.Equals(btnDeleteTrackingLog))
            { Tools.deleteAutoTrackerLog(); }
            else if (sender.Equals(btnDisableTelemetry))
            {
                if (IsDataCollectionDisabled)
                { Tools.enableDataCollection(); }
                else { Tools.disableDataCollection(); };
            }
            else if (sender.Equals(btnDisableLocationUsage))
            {
                if (IsGlobalSensorDisabled)
                { Tools.enableGlobalSensorCU(); }
                else
                { Tools.disableGlobalSensorCU(); }
            }
            else if (sender.Equals(btnDisableLocationSensor))
            {
                if (IsSensorLocationDisabled)
                { Tools.enableSensorPermissionCU(); }
                else
                { Tools.disableSensorPermissionCU(); }
            }
            else if (sender.Equals(btnOwnsHosts))
            {
                Tools.SetOwnershipOfHosts();
            }
            else if (sender.Equals(btnOwnsLog))
            {
                Tools.SetOwnershipOfTrackerLog();
            }
            else if (sender.Equals(btnBackupHosts))
            { Tools.backupHostsFile(); }
            else if (sender.Equals(btnRestoreHosts))
            { Tools.restoreHostsFile(); }
            else if (sender.Equals(btnModifyHosts))
            { Tools.appendHostsFile(); }
            
            funcLoadTelemetryStatus();
        }
        #endregion

    }
}
