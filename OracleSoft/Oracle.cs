using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using OracleSoft.HardwareStats;
using OracleSoft.SoftwareStats;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OracleSoft
{
    public partial class Oracle : Form
    {
        bool KEEPRUNNING = true;
        int ExecuteCPUOnce = 0;
        int ExecuteSYSOnce = 0;
        int ExecuteCACOnce = 0;
        //Int32 ExecuteLDSKOnce = 0;
        //Int32 ExecutePDSKOnce = 0;

        int RATE = 1000;
        string VERSION = "0.003";

        //String DRIVE = "";
        int CPUCOUNT = 0;
        int CORECOUNT = 0;
        int LOGPCOUNT = 0;
        int LOCALDRVCOUNT = 0;
        int NETDRVCOUNT = 0;

        //Central Processing
        DataTable TemporaryPTable = new DataTable();
        DataTable ProcessorTable = new DataTable();
        DataTable TemporarySTable = new DataTable();
        DataTable SystemTable = new DataTable();
        DataTable TemporaryCTable = new DataTable();
        DataTable CacheTable = new DataTable();

        List<DataRow> CPUDataRows = new List<DataRow>();
        DataRow SYSROW;
        DataRow CACROW;

        Processor CPU = new Processor();
        OracleSoft.SoftwareStats.System SYS = new OracleSoft.SoftwareStats.System();
        Cache CAC = new Cache();

        BackgroundWorker BW_CPU = new BackgroundWorker();
        BackgroundWorker BW_SYS = new BackgroundWorker();
        BackgroundWorker BW_CAC = new BackgroundWorker();

        BackgroundWorker BWDGV1 = new BackgroundWorker();


        //Disk
        DataTable TemporaryPDTable = new DataTable();
        DataTable PhyDiskTable = new DataTable();
        DataTable TemporaryLDTable = new DataTable();
        DataTable LogDiskTable = new DataTable();

        //DataRow LOGDROW;
        //DataRow PHYDROW;

        BackgroundWorker BW_LOGD = new BackgroundWorker();
        BackgroundWorker BW_PHYD = new BackgroundWorker();

        BackgroundWorker BWDGV2 = new BackgroundWorker();

        public Oracle()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();

            SetConstants();
        }

        private void SetConstants()
        {
            List<string> temp = SetDrvInfo();
            lblVersion.Text += VERSION;

            try
            {
                SetCPUInfo();
            }
            catch(Exception ex)
            {
                MessageBox.Show("CPU Reading Exception Occurred\n\nException: " + ex.Message +
                    "\n\nStackTrace: " + ex.StackTrace);
            }

            try
            {
                SetDrvInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Drive Reading Exception Occurred\n\nException: " + ex.Message +
                    "\n\nStackTrace: " + ex.StackTrace);
            }

            SetSysInfo();
            SetCacInfo();
        }

        private void SetCPUInfo()
        {
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select NumberOfCores FROM Win32_Processor");
            foreach (ManagementObject item in MOS.Get())
            {
                int.TryParse(item["NumberOfCores"].ToString(), out CORECOUNT);
            }

            MOS = new ManagementObjectSearcher("Select NumberOfProcessors, NumberOfLogicalProcessors FROM Win32_ComputerSystem");
            foreach (ManagementObject item in MOS.Get())
            {
                int.TryParse(item["NumberOfProcessors"].ToString(), out CPUCOUNT);
                int.TryParse(item["NumberOfLogicalProcessors"].ToString(), out LOGPCOUNT);
            }

            Debug.Write("Processor Count: " + CPUCOUNT + "\nCore Count: " + CORECOUNT +
                            "\nLog. Processor Count: " + LOGPCOUNT);

            try
            {
                ProcessorTable = SetCPUDataTable();
                TemporaryPTable = SetCPUDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("CPU DataTable Exception Occurred\n\nException: " + ex.Message +
                    "\n\nStackTrace: " + ex.StackTrace);
            }

            CPUDataRows.Clear();
            for(int i = 0; i <= LOGPCOUNT; i++)
            {
                CPUDataRows.Add(ProcessorTable.NewRow());
            }
        }

        private void SetSysInfo()
        {
            try
            {
                SystemTable = SetSysDataTable();
                TemporarySTable = SetSysDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SYS DataTable Exception Occurred\n\nException: " + ex.Message +
                    "\n\nStackTrace: " + ex.StackTrace);
            }

            SYSROW = TemporarySTable.NewRow();
        }

        private void SetCacInfo()
        {
            try
            {
                CacheTable = SetCacDataTable();
                TemporaryCTable = SetCacDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SYS DataTable Exception Occurred\n\nException: " + ex.Message +
                    "\n\nStackTrace: " + ex.StackTrace);
            }

            CACROW = TemporaryCTable.NewRow();
        }

        public static void funcSetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, control, new object[] { true });
        }

        private List<string> SetDrvInfo()
        {
            List<string> TempList = new List<string>();
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select Name, DriveType FROM Win32_LogicalDisk");
            foreach (ManagementObject item in MOS.Get())
            {
                int temp = 0;

                if (item["DriveType"] != null)
                {
                    int.TryParse(item["DriveType"].ToString(), out temp);

                    if (temp != 4)
                    {
                        if (item["Name"] != null)
                        {
                            TempList.Add(item["Name"].ToString());
                        }
                    }
                    else
                    {
                        NETDRVCOUNT++;
                    }
                }
            }

            LOCALDRVCOUNT = TempList.Count;
            Debug.Write("\nDrive Count: " + LOCALDRVCOUNT + "\n");
            foreach (string temp in TempList)
            {
                Debug.Write(temp + "\n");
            }

            Debug.Write("\nNetwork Drive Count: " + NETDRVCOUNT + "\n");

            return TempList;
        }

        private void InitializeBackgroundWorkers()
        {
            BW_CPU.WorkerReportsProgress = true;
            BW_CPU.WorkerSupportsCancellation = true;
            BW_CPU.DoWork += new DoWorkEventHandler(BW_CPU_DoWork);
            BW_CPU.ProgressChanged += new ProgressChangedEventHandler(BW_CPU_ProgressChanged);

            BW_SYS.WorkerReportsProgress = true;
            BW_SYS.WorkerSupportsCancellation = true;
            BW_SYS.DoWork += new DoWorkEventHandler(BW_SYS_DoWork);
            BW_SYS.ProgressChanged += new ProgressChangedEventHandler(BW_SYS_ProgressChanged);

            BW_CAC.WorkerReportsProgress = true;
            BW_CAC.WorkerSupportsCancellation = true;
            BW_CAC.DoWork += new DoWorkEventHandler(BW_CAC_DoWork);
            BW_CAC.ProgressChanged += new ProgressChangedEventHandler(BW_CAC_ProgressChanged);

            BWDGV1.WorkerReportsProgress = true;
            BWDGV1.WorkerSupportsCancellation = true;
            BWDGV1.DoWork += new DoWorkEventHandler(BWDGV1_DoWork);
            BWDGV1.ProgressChanged += new ProgressChangedEventHandler(BWDGV1_ProgressChanged);

        }

        private void BW_CPU_DoWork(object sender, DoWorkEventArgs e)
        {
            CPU = new Processor(CPUCOUNT, LOGPCOUNT);

            try
            {
                CPU.LoadCounters();

                foreach(Core core in CPU.Cores)
                {
                    core.LoadCore();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("EXCEPTION\n\n==== Loading Core Exception ===\n" +
                                "\nMessage: " + ex.Message +
                                "\nHResult: " + ex.HResult.ToString() +
                                "\nData: " + ex.Data +
                                "\nStackTrace: " + ex.StackTrace + "\n");
            }

            while(KEEPRUNNING)
            {
                if (CPU.Loaded)
                {
                    funcGetCPUStats();
                    BW_CPU.ReportProgress(0);
                }

                Thread.Sleep(RATE);
            }
        }

        private void BW_CPU_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BW_SYS_DoWork(object sender, DoWorkEventArgs e)
        {
            SYS = new OracleSoft.SoftwareStats.System();

            try
            {
                SYS.LoadSystem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (KEEPRUNNING)
            {
                if (SYS.Loaded)
                {
                    funcGetSysStats();
                    BW_SYS.ReportProgress(0);
                }

                Thread.Sleep(RATE);
            }
        }

        private void BW_SYS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BW_CAC_DoWork(object sender, DoWorkEventArgs e)
        {
            CAC = new Cache();

            try
            {
                CAC.LoadCache();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (KEEPRUNNING)
            {
                if (CAC.Loaded)
                {
                    funcGetCacStats();
                    BW_CAC.ReportProgress(0);
                }

                Thread.Sleep(RATE);
            }
        }

        private void BW_CAC_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void funcGetCPUStats()
        {
            short i = 1;

            CPUDataRows[0][0] = "CPU";

            foreach (double value in CPU.getNextValues())
            {
                CPUDataRows[0][i] = value;
                i++;
            }

            short j = 1;
            foreach(Core temp in CPU.Cores)
            {
                short k = 0;
                foreach (double value in temp.getNextValues())
                {
                    //MessageBox.Show(value.ToString());

                    if (k == 0)
                    {
                        CPUDataRows[j][k] = "LP" + value.ToString("0");
                    }
                    else
                    {
                        try
                        {
                            CPUDataRows[j][k] = value;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    k++;
                }

                j++;
            }
        }

        private void funcGetSysStats()
        {
            int i = 0;

            foreach (double value in SYS.getNextValues())
            {
                if (i == 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds(value);

                    string Formatted = string.Format("{0:D1}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms", 
                                                     t.Days, t.Hours, t.Minutes, t.Seconds, t.Milliseconds);

                    SYSROW[i] = Formatted;
                }
                else if (i == 1)
                {
                    SYSROW[i] = (value).ToString("0.###");
                }
                else
                {
                    SYSROW[i] = value;
                }
                i++;
            }
        }

        private void funcGetCacStats()
        {
            int i = 0;

            foreach (double value in CAC.getNextValues())
            {
                CACROW[i] = value;

                i++;
            }
        }

        private void BWDGV1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (KEEPRUNNING)
            {
                Thread.Sleep(RATE);

                funcTransferCPUData();
                funcTransferSysData();
                funcTransferCacData();
                BWDGV1.ReportProgress(0);
            }
        }

        private void BWDGV1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            funcCPURefresh();
            funcSysRefresh();
            funcCacRefresh();
        }

        private void funcTransferCPUData()
        {
            DataRow[] ROWARRAY = new DataRow[5];

            for (int i = 0; i < CPUDataRows.Count && i < int.MaxValue; i++)
            {
                ROWARRAY[i] = TemporaryPTable.NewRow();
            }

            try
            {
                int i = 0;

                foreach (DataRow temp in CPUDataRows)
                {
                    if (temp != null)
                    {
                        ROWARRAY[i].ItemArray = temp.ItemArray.Clone() as object[];
                    }
                    else
                    {
                        MessageBox.Show("CPUDataRow " + i.ToString() + "Error.");
                    }

                    i++;
                }
            }
            catch (NullReferenceException NEX)
            {
                MessageBox.Show("DataGridView NullReferenceException\n\n" + NEX.ToString());
                //continue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DataGridView DataRow Transfer Exception\n\n" + ex.ToString());
                //continue;
            }

            int j = 0;
            foreach (DataRow temp in ROWARRAY)
            {
                if (temp != null)
                {
                    TemporaryPTable.Rows.RemoveAt(j);
                    TemporaryPTable.Rows.InsertAt(ROWARRAY[j], j);
                }
                j++;

            }
        }

        private void funcTransferSysData()
        {
            DataRow TEMPROW = TemporarySTable.NewRow();
            
            try
            {
                TEMPROW.ItemArray = SYSROW.ItemArray.Clone() as object[];

                TemporarySTable.Rows.RemoveAt(0);
                TemporarySTable.Rows.InsertAt(TEMPROW, 0);
            }
            catch (NullReferenceException NEX)
            {
                MessageBox.Show("DGV System NullReferenceException\n\n" + NEX.ToString());
                //continue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DGV System Row Transfer Exception\n\n" + ex.ToString());
                //continue;
            }
        }

        private void funcTransferCacData()
        {
            DataRow TEMPROW = TemporaryCTable.NewRow();

            try
            {
                TEMPROW.ItemArray = CACROW.ItemArray.Clone() as object[];

                TemporaryCTable.Rows.RemoveAt(0);
                TemporaryCTable.Rows.InsertAt(TEMPROW, 0);
            }
            catch (NullReferenceException NEX)
            {
                MessageBox.Show("DGV System NullReferenceException\n\n" + NEX.ToString());
                //continue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DGV Cache Row Transfer Exception\n\n" + ex.ToString());
                //continue;
            }
        }

        private void funcCPURefresh()
        {
            List<int> AllColumnWidths = new List<int>();
            int ScrollVert = 0;
            int ScrollHoriz = 0;
            int TotalColumns = 0;
            int TotalRows = 0;

            int selRow = -1;
            int sortCol = -1;
            ListSortDirection LSD = new ListSortDirection();


            ScrollVert = dgvCPU.VerticalScrollingOffset;
            ScrollHoriz = dgvCPU.HorizontalScrollingOffset;
            TotalColumns = dgvCPU.Columns.Count;
            TotalRows = dgvCPU.Rows.Count;

            //MessageBox.Show("Col: " + ScrollColIndex + "  Row: " + ScrollRowIndex);

            if (dgvCPU.SortedColumn != null)
            {
                sortCol = dgvCPU.SortedColumn.Index;
            }

            foreach (DataGridViewRow row in dgvCPU.Rows)
            {
                if (row.Selected)
                {
                    selRow = row.Index;
                }
            }

            foreach (DataGridViewColumn col in dgvCPU.Columns)
            {
                AllColumnWidths.Add(col.Width);
            }

            if (sortCol != -1)
            {
                if (dgvCPU.SortOrder.Equals(SortOrder.Ascending))
                {
                    LSD = ListSortDirection.Ascending;
                }
                else
                {
                    LSD = ListSortDirection.Descending;
                }
            }

            if (BWDGV1.CancellationPending != true)
            {
                SuspendLayout();

                ProcessorTable = TemporaryPTable.Copy();
                TemporaryPTable.Clear();
                TemporaryPTable = SetCPUDataTable();

                dgvCPU.DataSource = ProcessorTable;


                if (sortCol > -1)
                {
                    dgvCPU.Sort(dgvCPU.Columns[sortCol], LSD);
                }

                if (selRow != -1)
                {
                    dgvCPU.Rows[selRow].Selected = true;
                }

                //Resets cell formats.
                funcCPUCellFormats();

                int temp = 0;
                foreach (DataGridViewColumn col in dgvCPU.Columns)
                {
                    if (AllColumnWidths.Any())
                    {
                        temp += AllColumnWidths.ElementAt(0);
                        col.Width = AllColumnWidths.ElementAt(0);
                        AllColumnWidths.RemoveAt(0);
                    }
                }

                if (temp > dgvCPU.Width)
                {
                    dgvCPU.ScrollBars = ScrollBars.Horizontal;

                    dgvCPU.HorizontalScrollingOffset = ScrollHoriz;
                }
                else
                {
                    dgvCPU.ScrollBars = ScrollBars.None;
                }

                funcSetDoubleBuffered(dgvCPU);

                dgvCPU.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                dgvCPU.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                dgvCPU.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                dgvCPU.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                if (ExecuteCPUOnce == 0)
                {
                    funcSetCPUColumnsStats();
                    ExecuteCPUOnce++;
                }

                ResumeLayout( true);
            }
        }

        private void funcSysRefresh()
        {
            List<int> AllColumnWidths = new List<int>();
            int ScrollVert = 0;
            int ScrollHoriz = 0;
            int TotalColumns = 0;
            int TotalRows = 0;

            int selRow = -1;
            int sortCol = -1;
            ListSortDirection LSD = new ListSortDirection();


            ScrollVert = dgvSys.VerticalScrollingOffset;
            ScrollHoriz = dgvSys.HorizontalScrollingOffset;
            TotalColumns = dgvSys.Columns.Count;
            TotalRows = dgvSys.Rows.Count;

            //MessageBox.Show("Col: " + ScrollColIndex + "  Row: " + ScrollRowIndex);

            if (dgvSys.SortedColumn != null)
            {
                sortCol = dgvSys.SortedColumn.Index;
            }

            foreach (DataGridViewRow row in dgvSys.Rows)
            {
                if (row.Selected)
                {
                    selRow = row.Index;
                }
            }

            foreach (DataGridViewColumn col in dgvSys.Columns)
            {
                AllColumnWidths.Add(col.Width);
            }

            if (sortCol != -1)
            {
                if (dgvSys.SortOrder.Equals(SortOrder.Ascending))
                {
                    LSD = ListSortDirection.Ascending;
                }
                else
                {
                    LSD = ListSortDirection.Descending;
                }
            }

            if (BWDGV1.CancellationPending != true)
            {
                SuspendLayout();

                SystemTable = TemporarySTable.Copy();
                TemporarySTable.Clear();
                TemporarySTable = SetSysDataTable();

                dgvSys.DataSource = SystemTable;


                if (sortCol > -1)
                {
                    dgvSys.Sort(dgvSys.Columns[sortCol], LSD);
                }

                if (selRow != -1)
                {
                    dgvSys.Rows[selRow].Selected = true;
                }

                //Resets cell formats.
                funcSysCellFormats();

                int temp = 0;
                foreach (DataGridViewColumn col in dgvSys.Columns)
                {
                    if (AllColumnWidths.Any())
                    {
                        temp += AllColumnWidths.ElementAt(0);
                        col.Width = AllColumnWidths.ElementAt(0);
                        AllColumnWidths.RemoveAt(0);
                    }
                }

                if (temp > dgvSys.Width)
                {
                    dgvSys.ScrollBars = ScrollBars.Horizontal;

                    dgvSys.HorizontalScrollingOffset = ScrollHoriz;
                }
                else
                {
                    dgvSys.ScrollBars = ScrollBars.None;
                }

                funcSetDoubleBuffered(dgvCPU);

                dgvSys.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                dgvSys.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                dgvSys.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                dgvSys.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                if (ExecuteSYSOnce == 0)
                {
                    funcSetSysColumnsStats();
                    ExecuteSYSOnce++;
                }

                ResumeLayout( true);
            }
        }

        private void funcCacRefresh()
        {
            List<int> AllColumnWidths = new List<int>();
            int ScrollVert = 0;
            int ScrollHoriz = 0;
            int TotalColumns = 0;
            int TotalRows = 0;

            int selRow = -1;
            int sortCol = -1;
            ListSortDirection LSD = new ListSortDirection();


            ScrollVert = dgvCac.VerticalScrollingOffset;
            ScrollHoriz = dgvCac.HorizontalScrollingOffset;
            TotalColumns = dgvCac.Columns.Count;
            TotalRows = dgvCac.Rows.Count;

            //MessageBox.Show("Col: " + ScrollColIndex + "  Row: " + ScrollRowIndex);

            if (dgvCac.SortedColumn != null)
            {
                sortCol = dgvCac.SortedColumn.Index;
            }

            foreach (DataGridViewRow row in dgvCac.Rows)
            {
                if (row.Selected)
                {
                    selRow = row.Index;
                }
            }

            foreach (DataGridViewColumn col in dgvCac.Columns)
            {
                AllColumnWidths.Add(col.Width);
            }

            if (sortCol != -1)
            {
                if (dgvSys.SortOrder.Equals(SortOrder.Ascending))
                {
                    LSD = ListSortDirection.Ascending;
                }
                else
                {
                    LSD = ListSortDirection.Descending;
                }
            }

            if (BWDGV1.CancellationPending != true)
            {
                SuspendLayout();

                CacheTable = TemporaryCTable.Copy();
                TemporaryCTable.Clear();
                TemporaryCTable = SetCacDataTable();

                dgvCac.DataSource = CacheTable;


                if (sortCol > -1)
                {
                    dgvCac.Sort(dgvCac.Columns[sortCol], LSD);
                }

                if (selRow != -1)
                {
                    dgvCac.Rows[selRow].Selected = true;
                }

                //Resets cell formats.
                funcCacCellFormats();

                int temp = 0;
                foreach (DataGridViewColumn col in dgvCac.Columns)
                {
                    if (AllColumnWidths.Any())
                    {
                        temp += AllColumnWidths.ElementAt(0);
                        col.Width = AllColumnWidths.ElementAt(0);
                        AllColumnWidths.RemoveAt(0);
                    }
                }

                if (temp > dgvCac.Width)
                {
                    dgvCac.ScrollBars = ScrollBars.Horizontal;

                    dgvCac.HorizontalScrollingOffset = ScrollHoriz;
                }
                else
                {
                    dgvCac.ScrollBars = ScrollBars.None;
                }

                funcSetDoubleBuffered(dgvCPU);

                dgvCac.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                dgvCac.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                dgvCac.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                dgvCac.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                if (ExecuteCACOnce == 0)
                {
                    funcSetCacColumnsStats();
                    ExecuteCACOnce++;
                }

                ResumeLayout( true);
            }
        }

        private DataTable SetCPUDataTable()
        {
            DataTable DT = new DataTable();

            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("% CPU Usage", typeof(double));
            DT.Columns.Add("Processor Frequency", typeof(double));
            DT.Columns.Add("% CPU Idle Time", typeof(double));
            DT.Columns.Add("% CPU Interrupt Time", typeof(double));
            DT.Columns.Add("% CPU Privilege Time", typeof(double));
            DT.Columns.Add("% CPU Priority Time", typeof(double));
            DT.Columns.Add("% CPU User Time", typeof(double));
            DT.Columns.Add("% Time at Max Frequency", typeof(double));
            DT.Columns.Add("Park Status", typeof(double));
            DT.Columns.Add("Processor State Flags", typeof(double));
            DT.Columns.Add("% Time in C1 State", typeof(double));
            DT.Columns.Add("% Time in C2 State", typeof(double));
            DT.Columns.Add("% Time in C3 State", typeof(double));
            DT.Columns.Add("C1 Transitions / Sec", typeof(double));
            DT.Columns.Add("C2 Transitions / Sec", typeof(double));
            DT.Columns.Add("C3 Transitions / Sec", typeof(double));
            DT.Columns.Add("Interrupts / Sec", typeof(double));
            DT.Columns.Add("% Time spent on DPC", typeof(double));
            DT.Columns.Add("DPC Rate", typeof(double));
            DT.Columns.Add("DPCs Queued / Sec", typeof(double));

            for (int i = 0; i <= LOGPCOUNT && i < int.MaxValue; i++)
            {
                DT.Rows.Add();
            }

            return DT;
        }

        private void funcCPUCellFormats()
        {
            dgvCPU.Columns[1].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[2].DefaultCellStyle.Format = "0.000\\MHz";
            dgvCPU.Columns[3].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[4].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[5].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[6].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[7].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[8].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[9].DefaultCellStyle.Format = "0";
            dgvCPU.Columns[10].DefaultCellStyle.Format = "0";
            dgvCPU.Columns[11].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[12].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[13].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[14].DefaultCellStyle.Format = "0.000\\";
            dgvCPU.Columns[15].DefaultCellStyle.Format = "0.000\\";
            dgvCPU.Columns[16].DefaultCellStyle.Format = "0.000\\";
            dgvCPU.Columns[17].DefaultCellStyle.Format = "0.000\\";
            dgvCPU.Columns[18].DefaultCellStyle.Format = "0.000\\%";
            dgvCPU.Columns[19].DefaultCellStyle.Format = "0\\";
            dgvCPU.Columns[20].DefaultCellStyle.Format = "0.000\\";
        }
        
        private DataTable SetSysDataTable()
        {
            DataTable DT = new DataTable();

            DT.Columns.Add("System Up Time", typeof(string));
            DT.Columns.Add("Tick Drift", typeof(double));
            DT.Columns.Add("System Calls / Sec", typeof(double));
            DT.Columns.Add("Processes", typeof(double));
            DT.Columns.Add("Threads", typeof(double));
            DT.Columns.Add("Processor Queue Length", typeof(double));

            DT.Columns.Add("% Registry Quota In Use", typeof(double));
            DT.Columns.Add("Floating Emulations / Sec", typeof(double));
            DT.Columns.Add("Alignment Fixups / Sec", typeof(double));
            DT.Columns.Add("Context Switches / Sec", typeof(double));
            DT.Columns.Add("Exception Dispatches / Sec", typeof(double));

            DT.Columns.Add("File Data Operations / Sec", typeof(double));
            DT.Columns.Add("File Control Bytes / Sec", typeof(double));
            DT.Columns.Add("File Control Operations / Sec", typeof(double));
            DT.Columns.Add("File Read Bytes / Sec", typeof(double));
            DT.Columns.Add("File Write Bytes / Sec", typeof(double));
            DT.Columns.Add("File Read Operations / Sec", typeof(double));
            DT.Columns.Add("File Write Operations / Sec", typeof(double));

            DT.Rows.Add();

            return DT;
        }

        private DataTable SetCacDataTable()
        {
            DataTable DT = new DataTable();

            DT.Columns.Add("Dirty Pages", typeof(double));
            DT.Columns.Add("Dirty Page Threshold", typeof(double));

            //Async
            DT.Columns.Add("Async Data Maps/sec", typeof(double));
            DT.Columns.Add("Async Copy Reads/sec", typeof(double));
            DT.Columns.Add("Async Fast Reads/sec", typeof(double));
            DT.Columns.Add("Async MDL Reads/sec", typeof(double));
            DT.Columns.Add("Async Pin Reads/sec", typeof(double));

            //Sync
            DT.Columns.Add("Sync Data Maps/sec", typeof(double));
            DT.Columns.Add("Sync Copy Reads/sec", typeof(double));
            DT.Columns.Add("Sync Fast Reads/sec", typeof(double));
            DT.Columns.Add("Sync MDL Reads/sec", typeof(double));
            DT.Columns.Add("Sync Pin Reads/sec", typeof(double));

            //Reads
            DT.Columns.Add("Read Ahead/sec", typeof(double));
            DT.Columns.Add("Copy Read Hits %", typeof(double));
            DT.Columns.Add("Copy Reads/sec", typeof(double));
            DT.Columns.Add("Fast Reads/sec", typeof(double));
            DT.Columns.Add("Fast Read Resource Misses/sec", typeof(double));
            DT.Columns.Add("Fast Read Not Possibles/sec", typeof(double));
            DT.Columns.Add("MDL Read Hits %", typeof(double));
            DT.Columns.Add("MDL Reads/sec", typeof(double));
            DT.Columns.Add("Pin Read Hits %", typeof(double));
            DT.Columns.Add("Pin Reads/sec", typeof(double));

            //Writes
            DT.Columns.Add("Lazy Write Flushes/sec", typeof(double));
            DT.Columns.Add("Lazy Write Pages/sec", typeof(double));

            //DATA
            DT.Columns.Add("Data Flushes/sec", typeof(double));
            DT.Columns.Add("Data Flush Pages/sec", typeof(double));
            DT.Columns.Add("Data Map Hits %", typeof(double));
            DT.Columns.Add("Data Map Pins/sec", typeof(double));
            DT.Columns.Add("Data Maps/sec", typeof(double));

            DT.Rows.Add();

            return DT;
        }

        private void funcSysCellFormats()
        {
            dgvSys.Columns[0].DefaultCellStyle.Format = "";
            dgvSys.Columns[1].DefaultCellStyle.Format = "0.0000\\";
            dgvSys.Columns[2].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[3].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[4].DefaultCellStyle.Format = "0";
            dgvSys.Columns[5].DefaultCellStyle.Format = "0";
            dgvSys.Columns[6].DefaultCellStyle.Format = "0.000\\%";
            dgvSys.Columns[7].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[8].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[9].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[10].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[11].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[12].DefaultCellStyle.Format = "0.000\\KB";
            dgvSys.Columns[13].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[14].DefaultCellStyle.Format = "0.000\\KB";
            dgvSys.Columns[15].DefaultCellStyle.Format = "0.000\\KB";
            dgvSys.Columns[16].DefaultCellStyle.Format = "0.000\\";
            dgvSys.Columns[17].DefaultCellStyle.Format = "0.000\\";
        }

        private void funcCacCellFormats()
        {
            dgvCac.Columns[0].DefaultCellStyle.Format = "0";
            dgvCac.Columns[1].DefaultCellStyle.Format = "0";
            dgvCac.Columns[2].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[3].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[4].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[5].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[6].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[7].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[8].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[9].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[10].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[11].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[12].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[13].DefaultCellStyle.Format = "0.00\\%";
            dgvCac.Columns[14].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[15].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[16].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[17].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[18].DefaultCellStyle.Format = "0.00\\%";
            dgvCac.Columns[19].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[20].DefaultCellStyle.Format = "0.00\\%";
            dgvCac.Columns[21].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[22].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[23].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[24].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[25].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[26].DefaultCellStyle.Format = "0.00\\%";
            dgvCac.Columns[27].DefaultCellStyle.Format = "0.000\\";
            dgvCac.Columns[28].DefaultCellStyle.Format = "0.000\\";
        }

        private void funcSetCPUColumnsStats()
        {
            int ColWidth = 50;
            int x = 5;
            int y = 10;
            int z = 15;

            dgvCPU.Columns[0].Width = ColWidth;
            dgvCPU.Columns[1].Width = ColWidth;
            dgvCPU.Columns[2].Width = ColWidth + z;
            dgvCPU.Columns[3].Width = ColWidth + z;
            dgvCPU.Columns[4].Width = ColWidth + z;
            dgvCPU.Columns[5].Width = ColWidth + z;
            dgvCPU.Columns[6].Width = ColWidth + z;
            dgvCPU.Columns[7].Width = ColWidth + z;
            dgvCPU.Columns[8].Width = ColWidth + y;
            dgvCPU.Columns[9].Width = ColWidth;
            dgvCPU.Columns[10].Width = ColWidth + y;
            dgvCPU.Columns[11].Width = ColWidth + z + x;
            dgvCPU.Columns[12].Width = ColWidth + z + x;
            dgvCPU.Columns[13].Width = ColWidth + z + x;
            dgvCPU.Columns[14].Width = ColWidth + x;
            dgvCPU.Columns[15].Width = ColWidth + x;
            dgvCPU.Columns[16].Width = ColWidth + x;
            dgvCPU.Columns[17].Width = ColWidth + x;
            dgvCPU.Columns[18].Width = ColWidth + z + x;
            dgvCPU.Columns[19].Width = ColWidth;
            dgvCPU.Columns[20].Width = ColWidth + y;

            dgvCPU.Columns[0].HeaderCell.ToolTipText = 
                "Object being monitored. LP stands for Logical Processor.";
            dgvCPU.Columns[1].HeaderCell.ToolTipText = 
                "% Processor Time is the percentage of elapsed time that the\n" +
                "processor spends to execute a non-Idle thread. It is calculated\n" +
                "by measuring the percentage of time that the processor spends\n" +
                "executing the idle thread and then subtracting that value from\n" +
                "100%. (Each processor has an idle thread to which time is\n" +
                "accumulated when no other threads are ready to run). This counter\n"
                + "is the primary indicator of processor activity, and displays the\n" +
                "average percentage of busy time observed during the sample interval.\n" +
                "It should be noted that the accounting calculation of whether the\n" +
                "processor is idle is performed at an internal sampling interval of\n" +
                "the system clock tick. On todays fast processors, % Processor Time\n" +
                "can therefore underestimate the processor utilization as the processor\n" +
                "may be spending a lot of time servicing threads between the system\n" +
                "clock sampling interval. Workload based timer applications are one\n" +
                "example of applications which are more likely to be measured\n" +
                "inaccurately as timers are signaled just after the sample is taken.";

            dgvCPU.Columns[2].HeaderCell.ToolTipText =
                "Processor Frequency is the frequency of the current processor in megahertz.";

            dgvCPU.Columns[3].HeaderCell.ToolTipText =
                "% Idle Time is the percentage of time the processor is idle during the sample interval";

            dgvCPU.Columns[4].HeaderCell.ToolTipText =
                "% Interrupt Time is the time the processor spends receiving and\n" +
                "servicing hardware interrupts during sample intervals. This value\n" +
                "is an indirect indicator of the activity of devices that generate\n" +
                "interrupts, such as the system clock, the mouse, disk drivers, data\n" +
                "communication lines, network interface cards and other peripheral\n" +
                "devices. These devices normally interrupt the processor when they have\n" +
                "completed a task or require attention. Normal thread execution is\n" +
                "suspended during interrupts. Most system clocks interrupt the processor\n" +
                "every 10 milliseconds, creating a background of interrupt activity.\n" +
                "suspends normal thread execution during interrupts. This counter displays\n" +
                "the average busy time as a percentage of the sample time.";

            dgvCPU.Columns[5].HeaderCell.ToolTipText =
                "% Privileged Time is the percentage of elapsed time that the process\n" +
                "threads spent executing code in privileged mode. When a Windows system\n" +
                "service in called, the service will often run in privileged mode to gain\n" +
                "access to system-private data. Such data is protected from access by\n" +
                "threads executing in user mode. Calls to the system can be explicit or\n" +
                "implicit, such as page faults or interrupts. Unlike some early operating\n" +
                "systems, Windows uses process boundaries for subsystem protection in\n" +
                "addition to the traditional protection of user and privileged modes. Some\n" +
                "work done by Windows on behalf of the application might appear in other\n" +
                "subsystem processes in addition to the privileged time in the process.";

            dgvCPU.Columns[6].HeaderCell.ToolTipText =
                "% Priority Time is the percentage of elapsed time that the processor spends\n" +
                "executing threads that are not low priority. It is calculated by measuring the\n" +
                "percentage of time that the processor spends executing low priority threads or\n" +
                "the idle thread and then subtracting that value from 100%. (Each processor has an\n" +
                "idle thread to which time is accumulated when no other threads are ready to run).\n" +
                "This counter displays the average percentage of busy time observed during the\n" +
                "sample interval excluding low priority background work. It should be noted that\n" +
                "the accounting calculation of whether the processor is idle is performed at an\n" +
                "internal sampling interval of the system clock tick. % Priority Time can therefore\n" +
                "underestimate the processor utilization as the processor may be spending a lot of\n" +
                "time servicing threads between the system clock sampling interval. Workload based\n" +
                "timer applications are one example  of applications  which are more likely to be\n" +
                "measured inaccurately as timers are signaled just after the sample is taken.";

            dgvCPU.Columns[7].HeaderCell.ToolTipText =
                "% User Time is the percentage of elapsed time the processor spends in the user mode.\n" +
                "User mode is a restricted processing mode designed for applications, environment\n" +
                "subsystems, and integral subsystems. The alternative, privileged mode, is designed\n" +
                "for operating system components and allows direct access to hardware and all memory.\n" +
                "The operating system switches application threads to privileged mode to access\n" +
                "operating system services. This counter displays the average busy time as a" +
                "percentage of the sample time.";

            dgvCPU.Columns[8].HeaderCell.ToolTipText = 
                "% of Maximum Frequency is the percentage of the current processor's maximum frequency.";

            dgvCPU.Columns[9].HeaderCell.ToolTipText = 
                "Parking Status represents whether a processor is parked or not.";

            dgvCPU.Columns[10].HeaderCell.ToolTipText = 
                "Processor State Flags";

            dgvCPU.Columns[11].HeaderCell.ToolTipText =
                "% C1 Time is the percentage of time the processor spends in the C1 low-power idle\n" +
                "state. % C1 Time is a subset of the total processor idle time. C1 low-power idle\n" +
                "state enables the processor to maintain its entire context and quickly return to the\n" +
                "running state. Not all systems support the % C1 state.";

            dgvCPU.Columns[12].HeaderCell.ToolTipText =
                "% C2 Time is the percentage of time the processor spends in the C2 low-power idle\n" +
                "state. % C2 Time is a subset of the total processor idle time. C2 low-power idle\n" +
                "state enables the processor to maintain the context of the system caches. The C2\n" +
                "power state is a lower power and higher exit latency state than C1. Not all\n" +
                "systems support the C2 state.";

            dgvCPU.Columns[13].HeaderCell.ToolTipText =
                "% C3 Time is the percentage of time the processor spends in the C3 low-power\n" +
                "idle state. % C3 Time is a subset of the total processor idle time. When the\n" +
                "processor is in the C3 low-power idle state it is unable to maintain the\n" +
                "coherency of its caches. The C3 power state is a lower power and higher exit\n" +
                "latency state than C2. Not all systems support the C3 state.";

            dgvCPU.Columns[14].HeaderCell.ToolTipText =
                "C1 Transitions/sec is the rate that the CPU enters the C1 low-power idle state.\n" +
                "The CPU enters the C1 state when it is sufficiently idle and exits this state\n" +
                "on any interrupt. This counter displays the difference between the values\n" +
                "observed in the last two samples, divided by the duration of the sample interval.";

            dgvCPU.Columns[15].HeaderCell.ToolTipText =
                "C2 Transitions/sec is the rate that the CPU enters the C2 low-power idle state.\n" +
                "The CPU enters the C2 state when it is sufficiently idle and exits this state\n" +
                "on any interrupt. This counter displays the difference between the values observed\n" +
                "in the last two samples, divided by the duration of the sample interval.";

            dgvCPU.Columns[16].HeaderCell.ToolTipText =
                "C3 Transitions/sec is the rate that the CPU enters the C3 low-power idle state.\n" +
                "The CPU enters the C3 state when it is sufficiently idle and exits this state on\n" +
                "any interrupt. This counter displays the difference between the values observed\n" +
                "in the last two samples, divided by the duration of the sample interval.";

            dgvCPU.Columns[17].HeaderCell.ToolTipText =
                "Interrupts/sec is the average rate, in incidents per second, at which the\n" +
                "processor received and serviced hardware interrupts. It does not include\n" +
                "deferred procedure calls (DPCs), which are counted separately. This value is an\n" +
                "indirect indicator of the activity of devices that generate interrupts, such as\n" +
                "the system clock, the mouse, disk drivers, data communication lines, network\n" +
                "interface cards, and other peripheral devices. These devices normally interrupt\n" +
                "the processor when they have completed a task or require attention. Normal thread\n" +
                "execution is suspended. The system clock typically interrupts the processor every\n" +
                "10 milliseconds, creating a background of interrupt activity. This counter displays\n" +
                "the difference between the values observed in the last two samples, divided by the\n" +
                "duration of the sample interval.";

            dgvCPU.Columns[18].HeaderCell.ToolTipText =
                "% DPC Time is the percentage of time that the processor spent receiving and\n" +
                "servicing deferred procedure calls (DPCs) during the sample interval. DPCs are\n" +
                "interrupts that run at a lower priority than standard interrupts. % DPC Time is a\n" +
                "component of % Privileged Time because DPCs are executed in privileged mode. They\n" +
                "are counted separately and are not a component of the interrupt counters. This\n" +
                "counter displays the average busy time as a percentage of the sample time.";

            dgvCPU.Columns[19].HeaderCell.ToolTipText =
                "DPC Rate is the rate at which deferred procedure calls (DPCs) were added to the\n" +
                "processors DPC queues between the timer ticks of the processor clock. DPCs are\n" +
                "interrupts that run at alower priority than standard interrupts. Each processor\n" +
                "has its own DPC queue. This counter measures the rate that DPCs were added to the\n" +
                "queue, not the number of DPCs in the queue. This counter displays the last\n" +
                "observed value only; it is not an average.";

            dgvCPU.Columns[20].HeaderCell.ToolTipText =
                "DPCs Queued/sec is the average rate, in incidents per second, at which deferred\n" +
                "procedure calls (DPCs) were added to the processor's DPC queue. DPCs are interrupts\n" +
                "that run at a lower priority than standard interrupts. Each processor has its own\n" +
                "DPC queue. This counter measures the rate that DPCs are added to the queue, not the\n" +
                "number of DPCs in the queue.  This counter displays the difference between the\n" +
                "values observed in the last two samples, divided by the duration of the sample\n" +
                "interval.";

        }

        private void funcSetSysColumnsStats()
        {
            int ColWidth = 50;
            int a = 5;
            int x = 10;
            int y = 15;
            int z = 30;

            dgvSys.Columns[0].Width = ColWidth + x + z + z;
            dgvSys.Columns[1].Width = ColWidth - a;
            dgvSys.Columns[2].Width = ColWidth + y + z;
            dgvSys.Columns[3].Width = ColWidth + x;
            dgvSys.Columns[4].Width = ColWidth + a;
            dgvSys.Columns[5].Width = ColWidth + a + y + z;
            dgvSys.Columns[6].Width = ColWidth + z + z;
            dgvSys.Columns[7].Width = ColWidth + x + z + z;
            dgvSys.Columns[8].Width = ColWidth + z + z;
            dgvSys.Columns[9].Width = ColWidth + z + z;
            dgvSys.Columns[10].Width = ColWidth + x + z + z;
            dgvSys.Columns[11].Width = ColWidth + x + z;
            dgvSys.Columns[12].Width = ColWidth + x + z;
            dgvSys.Columns[13].Width = ColWidth + y + z ;
            dgvSys.Columns[14].Width = ColWidth + z;
            dgvSys.Columns[15].Width = ColWidth + z;
            dgvSys.Columns[16].Width = ColWidth + x;
            dgvSys.Columns[17].Width = ColWidth + x;

            dgvSys.Columns[0].HeaderCell.ToolTipText =
                "System Up Time is the elapsed time (in seconds) that the computer\n" +
                "has been running since it was last started. This counter displays\n" +
                "the difference between the start time and the current time.";

            dgvSys.Columns[1].HeaderCell.ToolTipText =
                "Purely academic difference between time intervals used to measure\n" +
                "Oracle's efficiency. A value of 1.0 is perfect efficiency.";

            dgvSys.Columns[2].HeaderCell.ToolTipText =
                "System Calls/sec is the combined rate of calls to operating system\n" +
                "service routines by all processes running on the computer. These\n" +
                "routines perform all of the basic scheduling and synchronization\n" +
                "of activities on the computer, and provide access to non-graphic\n" +
                "devices, memory management, and name space management. This counter\n" +
                "displays the difference between the values observed in the last two\n" +
                "samples, divided by the duration of the sample interval.";

            dgvSys.Columns[3].HeaderCell.ToolTipText =
                "Processes is the number of processes in the computer at the time of\n" +
                "data collection. This is an instantaneous count, not an average over\n" +
                "the time interval. Each process represents the running of a program.";

            dgvSys.Columns[4].HeaderCell.ToolTipText =
                "Threads is the number of threads in the computer at the time of data\n" +
                "collection. This is an instantaneous count, not an average over the\n" +
                "time interval. A thread is the basic executable entity that can\n" +
                "execute instructions in a processor.";

            dgvSys.Columns[5].HeaderCell.ToolTipText =
                "Processor Queue Length is the number of threads in the processor\n" +
                "queue. Unlike the disk counters, this counter shows ready threads\n" +
                "only, not threads that are running. There is a single queue for\n" +
                "processor time even on computers with multiple processors.\n" +
                "Therefore, if a computer has multiple processors, you need to\n" +
                "divide this value by the number of processors servicing the\n" +
                "workload. A sustained processor queue of less than 10 threads\n" +
                "per processor is normally acceptable, dependent of the workload.";

            dgvSys.Columns[6].HeaderCell.ToolTipText =
                "% Registry Quota In Use is the percentage of the Total Registry\n" +
                "Quota Allowed that is currently being used by the system.  This\n" +
                "counter displays the current percentage value only; it is not an\n" +
                "average.";

            dgvSys.Columns[7].HeaderCell.ToolTipText =
                "Floating Emulations/sec is the rate of floating emulations performed\n" +
                "by the system.  This counter displays the difference between the values\n" +
                "observed in the last two samples, divided by the duration of the sample\n" +
                "interval.";

            dgvSys.Columns[8].HeaderCell.ToolTipText =
                "Alignment Fixups/sec is the rate, in incidents per seconds, at alignment\n" +
                "faults were fixed by the system.";

            dgvSys.Columns[9].HeaderCell.ToolTipText =
                "Context Switches/sec is the combined rate at which all processors on\n" +
                "the computer are switched from one thread to another. Context switches\n" +
                "occur when a running thread voluntarily relinquishes the processor, is\n" +
                "preempted by a higher priority ready thread, or switches between user-mode\n" +
                "and privileged (kernel) mode to use an Executive or subsystem service. It is\n" +
                "the sum of Thread\\Context Switches/sec for all threads running on all\n" +
                "processors in the computer and is measured in numbers of switches. There are\n" +
                "context switch counters on the System and Thread objects. This counter\n" +
                "displays the difference between the values observed in the last two samples,\n" +
                "divided by the duration of the sample interval.";

            dgvSys.Columns[10].HeaderCell.ToolTipText =
                "Exception Dispatches/sec is the rate, in incidents per second, at which\n" +
                "exceptions were dispatched by the system.";

            dgvSys.Columns[11].HeaderCell.ToolTipText =
                "File Data Operations/ sec is the combined rate of read and write operations\n" +
                "on all logical disks on the computer. This is the inverse of System: File\n" +
                "Control Operations/sec. This counter displays the difference between the\n" +
                "values observed in the last two samples, divided by the duration of the\n" +
                "sample interval.";

            dgvSys.Columns[12].HeaderCell.ToolTipText =
                "File Control Bytes/sec is the overall rate at which bytes are transferred\n" +
                "for all file system operations that are neither reads nor writes, including\n" +
                "file system control requests and requests for information about device\n" +
                "characteristics or status. It is measured in numbers of bytes. This counter\n" +
                "displays the difference between the values observed in the last two samples,\n" +
                "divided by the duration of the sample interval.";

            dgvSys.Columns[13].HeaderCell.ToolTipText =
                "File Control Operations/sec is the combined rate of file system operations\n" +
                "that are neither reads nor writes, such as file system control requests and\n" +
                "requests for information about device characteristics or status. This is the\n" +
                "inverse of System: File Data Operations/sec and is measured in number of\n" +
                "operations perf second. This counter displays the difference between the\n" +
                "values observed in the last two samples, divided by the duration of the\n" +
                "sample interval.";

            dgvSys.Columns[14].HeaderCell.ToolTipText =
                "File Read Bytes/sec is the overall rate at which bytes are read to satisfy\n" +
                "file system read requests to all devices on the computer, including reads\n" +
                "from the file system cache. It is measured in number of bytes per second.\n" +
                "This counter displays the difference between the values observed in the\n" +
                "last two samples, divided by the duration of the sample interval.";

            dgvSys.Columns[15].HeaderCell.ToolTipText =
                "File Write Bytes/sec is the overall rate at which bytes are written to\n" +
                "satisfy file system write requests to all devices on the computer,\n" +
                "including writes to the file system cache. It is measured in number\n" +
                "of bytes per second. This counter displays the difference between the\n" +
                "values observed in the last two samples, divided by the duration of the\n" +
                "sample interval.";

            dgvSys.Columns[16].HeaderCell.ToolTipText =
                "File Read Operations/sec is the combined rate of file system read\n" +
                "requests to all devices on the computer, including requests to read\n" +
                "from the file system cache. It is measured in numbers of reads. This\n" +
                "counter displays the difference between the values observed in the last\n" +
                "two samples, divided by the duration of the sample interval.";

            dgvSys.Columns[17].HeaderCell.ToolTipText =
                "File Write Operations/sec is the combined rate of the file system write\n" +
                "requests to all devices on the computer, including requests to write to\n" +
                "data in the file system cache. It is measured in numbers of writes. This\n" +
                "counter displays the difference between the values observed in the last\n" +
                "two samples, divided by the duration of the sample interval.";
        }

        private void funcSetCacColumnsStats()
        {
            int ColWidth = 50;
            int a = 5;
            int x = 10;
            int y = 15;
            int z = 30;

            dgvCac.Columns[0].Width = ColWidth;
            dgvCac.Columns[1].Width = ColWidth + x;
            dgvCac.Columns[2].Width = ColWidth + x;
            dgvCac.Columns[3].Width = ColWidth + y;
            dgvCac.Columns[4].Width = ColWidth + y;
            dgvCac.Columns[5].Width = ColWidth + y;
            dgvCac.Columns[6].Width = ColWidth + y;
            dgvCac.Columns[7].Width = ColWidth + y;
            dgvCac.Columns[8].Width = ColWidth + y;
            dgvCac.Columns[9].Width = ColWidth + y;
            dgvCac.Columns[10].Width = ColWidth + y;
            dgvCac.Columns[11].Width = ColWidth + y;
            dgvCac.Columns[12].Width = ColWidth + y;
            dgvCac.Columns[13].Width = ColWidth + y;
            dgvCac.Columns[14].Width = ColWidth + y;
            dgvCac.Columns[15].Width = ColWidth + y;
            dgvCac.Columns[16].Width = ColWidth + y;
            dgvCac.Columns[17].Width = ColWidth + x + y;
            dgvCac.Columns[18].Width = ColWidth + y;
            dgvCac.Columns[19].Width = ColWidth + y;
            dgvCac.Columns[20].Width = ColWidth + y;
            dgvCac.Columns[21].Width = ColWidth + y;
            dgvCac.Columns[22].Width = ColWidth + y + a;
            dgvCac.Columns[23].Width = ColWidth + y;
            dgvCac.Columns[24].Width = ColWidth + z;
            dgvCac.Columns[25].Width = ColWidth + y;
            dgvCac.Columns[26].Width = ColWidth + y;
            dgvCac.Columns[27].Width = ColWidth + y;
            dgvCac.Columns[28].Width = ColWidth + y;

            dgvCac.Columns[0].HeaderCell.ToolTipText =
                "Total number of dirty pages on the system cache";
            dgvCac.Columns[1].HeaderCell.ToolTipText =
                "Threshold for number of dirty pages on system cache";
            dgvCac.Columns[2].HeaderCell.ToolTipText =
                "Async Data Maps/sec is the frequency that an application using\n" +
                "a file system, such as NTFS, to map a page of a file into the file\n" +
                "system cache to read the page, and does not wait for the page to be\n" +
                "retrieved if it is not in main memory.";
            dgvCac.Columns[3].HeaderCell.ToolTipText =
                "Async Copy Reads/sec is the frequency of reads from pages of the\n" +
                "file system cache that involve a memory copy of the data from the\n" +
                "cache to the application's buffer. The application will regain\n" +
                "control immediately even if the disk must be accessed to retrieve\n" +
                "the page.";
            dgvCac.Columns[4].HeaderCell.ToolTipText =
                "Async Fast Reads/sec is the frequency of reads from the file\n" +
                "system cache that bypass the installed file system and retrieve\n" +
                "the data directly from the cache.  Normally, file I/O requests\n" +
                "will invoke the appropriate file system to retrieve data from a\n" +
                "file, but this path permits data to be retrieved from the cache\n" +
                "directly (without file system involvement) if the data is in the\n" +
                "cache. Even if the data is not in the cache, one invocation of\n" +
                "the file system is avoided. If the data is not in the cache,\n" +
                "the request (application program call) will not wait until the\n" +
                "data has been retrieved from disk, but will get control immediately.";
            dgvCac.Columns[5].HeaderCell.ToolTipText =
                "Async MDL Reads/sec is the frequency of reads from the file\n" +
                "system cache that use a Memory Descriptor List (MDL) to access\n" +
                "the pages. The MDL contains the physical address of each page in\n" +
                "the transfer, thus permitting Direct Memory Access (DMA) of the\n" +
                "pages. If the accessed page(s) are not in main memory, the calling\n" +
                "application program will not wait for the pages to fault in from disk.";
            dgvCac.Columns[6].HeaderCell.ToolTipText =
                "Async Pin Reads/sec is the frequency of reading data into the file\n" +
                "system cache preparatory to writing the data back to disk. Pages\n" +
                "read in this fashion are pinned in memory at the completion of the\n" +
                "read. The file system will regain control immediately even if the\n" +
                "disk must be accessed to retrieve the page. While pinned, a page's\n" +
                "physical address will not be altered.";
            dgvCac.Columns[7].HeaderCell.ToolTipText =
                "Sync Data Maps/sec counts the frequency that a file system, such as\n" +
                "NTFS, maps a page of a file into the file system cache to read the\n" +
                "page, and wishes to wait for the page to be retrieved if it is not\n" +
                "in main memory.";
            dgvCac.Columns[8].HeaderCell.ToolTipText =
                "Sync Copy Reads/sec is the frequency of reads from pages of the\n" +
                "file system cache that involve a memory copy of the data from the\n" +
                "cache to the application's buffer. The file system will not regain\n" +
                "control until the copy operation is complete, even if the disk must\n" +
                "be accessed to retrieve the page.";
            dgvCac.Columns[9].HeaderCell.ToolTipText =
                "Sync Fast Reads/sec is the frequency of reads from the file system\n" +
                "cache that bypass the installed file system and retrieve the data\n" +
                "directly from the cache. Normally, file I/O requests invoke the\n" +
                "appropriate file system to retrieve data from a file, but this path\n" +
                "permits direct retrieval of data from the cache without file system\n" +
                "involvement if the data is in the cache. Even if the data is not in\n" +
                "the cache, one invocation of the file system is avoided. If the data\n" +
                "is not in the cache, the request (application program call) will\n" +
                "wait until the data has been retrieved from disk.";
            dgvCac.Columns[10].HeaderCell.ToolTipText =
                "Sync MDL Reads/sec is the frequency of reads from the file system\n" +
                "cache that use a Memory Descriptor List (MDL) to access the pages.\n" +
                "The MDL contains the physical address of each page in the transfer, thus permitting Direct Memory Access (DMA) of the pages.  If the accessed page(s) are not in main memory, the caller will wait for the pages to fault in from the disk.";
            dgvCac.Columns[11].HeaderCell.ToolTipText =
                "Sync Pin Reads/sec is the frequency of reading data into the file\n" +
                "system cache preparatory to writing the data back to disk. Pages\n" +
                "read in this fashion are pinned in memory at the completion of\n" +
                "the read. The file system will not regain control until the page is pinned in the file system cache, in particular if the disk must be accessed to retrieve the page.  While pinned, a page's physical address in the file system cache will not be altered.";
            dgvCac.Columns[12].HeaderCell.ToolTipText =
                "Read Aheads/sec is the frequency of reads from the file system\n" +
                "cache in which the Cache detects sequential access to a file.\n" +
                "The read aheads permit the data to be transferred in larger\n" +
                "blocks than those being requested by the application, reducing\n" +
                "the overhead per access.";
            dgvCac.Columns[13].HeaderCell.ToolTipText =
                "Copy Read Hits is the percentage of cache copy read requests\n" +
                "that hit the cache, that is, they did not require a disk read in\n" +
                "order to provide access to the page in the cache. A copy read is a\n" +
                "file read operation that is satisfied by a memory copy from a page\n" +
                "in the cache to the application's buffer. The LAN Redirector uses\n" +
                "this method for retrieving information from the cache, as does\n" +
                "the LAN Server for small transfers. This is a method used by the\n" +
                "disk file systems as well.";
            dgvCac.Columns[14].HeaderCell.ToolTipText =
                "Copy Reads/sec is the frequency of reads from pages of the file\n" +
                "system cache that involve a memory copy of the data from the cache\n" +
                "to the application's buffer. The LAN Redirector uses this method\n" +
                "for retrieving information from the file system cache, as does the\n" +
                "LAN Server for small transfers. This is a method used by the disk\n" +
                "file systems as well.";
            dgvCac.Columns[15].HeaderCell.ToolTipText =
                "Fast Reads/sec is the frequency of reads from the file system cache\n" +
                "that bypass the installed file system and retrieve the data directly\n" +
                "from the cache. Normally, file I/O requests invoke the appropriate\n" +
                "file system to retrieve data from a file, but this path permits\n" +
                "direct retrieval of data from the cache without file system\n" +
                "involvement if the data is in the cache. Even if the data is not in\n" +
                "the cache, one invocation of the file system is avoided.";
            dgvCac.Columns[16].HeaderCell.ToolTipText =
                "Fast Read Resource Misses/sec is the frequency of cache misses\n" +
                "necessitated by the lack of available resources to satisfy the\n" +
                "request.";
            dgvCac.Columns[17].HeaderCell.ToolTipText =
                "Fast Read Not Possibles/sec is the frequency of attempts by an\n" +
                "Application Program Interface (API) function call to bypass the\n" +
                "file system to get to data in the file system cache that could not\n" +
                "be honored without invoking the file system.";
            dgvCac.Columns[18].HeaderCell.ToolTipText =
                "MDL Read Hits is the percentage of Memory Descriptor List (MDL)\n" +
                "Read requests to the file system cache that hit the cache, i.e.,\n" +
                "did not require disk accesses in order to provide memory access to\n" +
                "the page(s) in the cache.";
            dgvCac.Columns[19].HeaderCell.ToolTipText =
                "MDL Reads/sec is the frequency of reads from the file system cache\n" +
                "that use a Memory Descriptor List (MDL) to access the data. The MDL\n" +
                "contains the physical address of each page involved in the transfer,\n" +
                "and thus can employ a hardware Direct Memory Access (DMA) device to\n" +
                "effect the copy. The LAN Server uses this method for large transfers\n" +
                "out of the server.";
            dgvCac.Columns[20].HeaderCell.ToolTipText =
                "Pin Read Hits is the percentage of pin read requests that hit the\n" +
                "file system cache, i.e., did not require a disk read in order to provide\n" +
                "access to the page in the file system cache. While pinned, a page's physical\n" +
                "address in the file system cache will not be altered. The LAN Redirector\n" +
                "uses this method for retrieving data from the cache, as does the LAN Server\n" +
                "for small transfers. This is usually the method used by the disk file\n" +
                "systems as well.";
            dgvCac.Columns[21].HeaderCell.ToolTipText =
                "Pin Reads/sec is the frequency of reading data into the file system cache\n" +
                "preparatory to writing the data back to disk. Pages read in this fashion\n" +
                "are pinned in memory at the completion of the read. While pinned, a page's\n" +
                "physical address in the file system cache will not be altered.";
            dgvCac.Columns[22].HeaderCell.ToolTipText =
                "Lazy Write Flushes/sec is the rate at which the Lazy Writer thread has\n" +
                "written to disk. Lazy Writing is the process of updating the disk after\n" +
                "the page has been changed in memory, so that the application that changed\n" +
                "the file does not have to wait for the disk write to be complete before\n" +
                "proceeding. More than one page can be transferred by each write operation.";
            dgvCac.Columns[23].HeaderCell.ToolTipText =
                "Lazy Write Pages/sec is the rate at which the Lazy Writer thread has\n" +
                "written to disk. Lazy Writing is the process of updating the disk after\n" +
                "the page has been changed in memory, so that the application that\n" +
                "changed the file does not have to wait for the disk write to be\n" +
                "complete before proceeding. More than one page can be transferred\n" +
                "on a single disk write operation.";
            dgvCac.Columns[24].HeaderCell.ToolTipText =
                "Data Flushes/sec is the rate at which the file system cache has\n" +
                "flushed its contents to disk as the result of a request to flush or\n" +
                "to satisfy a write-through file write request. More than one page can\n" +
                "be transferred on each flush operation.";
            dgvCac.Columns[25].HeaderCell.ToolTipText =
                "Data Flush Pages/sec is the number of pages the file system cache\n" +
                "has flushed to disk as a result of a request to flush or to satisfy\n" +
                "a write-through file write request. More than one page can be\n" +
                "transferred on each flush operation.";
            dgvCac.Columns[26].HeaderCell.ToolTipText =
                "Data Map Hits is the percentage of data maps in the file system cache\n" +
                "that could be resolved without having to retrieve a page from the\n" +
                "disk, because the page was already in physical memory.";
            dgvCac.Columns[27].HeaderCell.ToolTipText =
                "Data Map Pins/sec is the frequency of data maps in the file system\n" +
                "cache that resulted in pinning a page in main memory, an action\n" +
                "usually preparatory to writing to the file on disk. While pinned, a\n" +
                "page's physical address in main memory and virtual address in the file\n" +
                "system cache will not be altered.";
            dgvCac.Columns[28].HeaderCell.ToolTipText =
                "Data Maps/sec is the frequency that a file system such as NTFS, maps\n" +
                "a page of a file into the file system cache to read the page.";
        }

        private void LinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.bytemedev.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Web Browsing Exception Occured\n\nMessage: " + ex.Message +
                                "\n\nStack Trace: " + ex.StackTrace);
            }
        }

        private void ClickEventHandler(object sender, EventArgs e)
        {
            if (sender.Equals(btnProcWatch))
            {
                if (btnProcWatch.Text == "Start Monitoring")
                {
                    btnProcWatch.Text = "Stop Monitoring";
                    funcStartCentralProcessMonitoring();
                }
                else
                {
                    btnProcWatch.Text = "Start Monitoring";
                    funcCancelProcMonitoring();
                }
            }
        }

        private void funcStartCentralProcessMonitoring()
        {
            KEEPRUNNING = true;

            if (BW_CPU.IsBusy != true)
            {
                BW_CPU.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("BW_CPU Is Busy.  Try again in a few moments");
                funcCancelProcMonitoring();
            }

            if (BW_SYS.IsBusy != true)
            {
                BW_SYS.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("BW_SYS Is Busy.  Try again in a few moments");
                funcCancelProcMonitoring();
            }

            if (BW_CAC.IsBusy != true)
            {
                BW_CAC.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("BW_CAC Is Busy.  Try again in a few moments");
                funcCancelProcMonitoring();
            }

            if (BWDGV1.IsBusy != true)
            {
                BWDGV1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("BWDGV1 Is Busy.  Try again in a few moments");
                funcCancelProcMonitoring();
            }
        }

        private void funcCancelProcMonitoring()
        {
            KEEPRUNNING = false;

            if (BW_CPU.IsBusy)
            {
                BW_CPU.CancelAsync();
            }

            if (BW_SYS.IsBusy)
            {
                BW_SYS.CancelAsync();
            }

            if (BW_CAC.IsBusy)
            {
                BW_CAC.CancelAsync();
            }

            if (BWDGV1.IsBusy)
            {
                BWDGV1.CancelAsync();
            }
        }

        private void funcCancelDriveMonitoring()
        {
            /*
            if (BW2.IsBusy == true)
            {
                BW2.CancelAsync();
            }

            cboxDrives.Enabled = true;
            btnProcWatch.Enabled = true;*/
        }
    }
}
