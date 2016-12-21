using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Wolf.CI_Report
{
    public partial class CI_Report : Form
    {
        List<string> Win32ClassList = new List<string>();
        bool NoNulls = false;
        bool ClipboardOnly = false;

        List<List<string>> DataLists = new List<List<string>>();
        BindingSource Data = new BindingSource();
        DataTable dtAllComputerInfo = new DataTable();
        BackgroundWorker BW = new BackgroundWorker();

        public CI_Report(List<string> Win32ClassList, bool NoNulls, bool ClipboardOnly)
        {
            InitializeComponent();

            if (ClipboardOnly)
            {
                Hide();

                CI_MiniReport newMiniReport = new CI_MiniReport(Win32ClassList, NoNulls, ClipboardOnly);

                newMiniReport.Show();
            }
            else
            {
                this.Win32ClassList = Win32ClassList;
                this.NoNulls = NoNulls;
                this.ClipboardOnly = ClipboardOnly;

                ConfigureDGVStyle();
                dgvCompInfo.DataSource = Data;

                BW.DoWork += new DoWorkEventHandler(BW_DoWork);
                BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_RunWorkerCompleted);
                BW.RunWorkerAsync();

                Text += " loading...";
                SetDoubleBuffered(this);
                SetDoubleBuffered(dgvCompInfo);
            }
        }

        private void ConfigureDGVStyle()
        {
            Color color = ColorTranslator.FromHtml("#383838");
            dgvCompInfo.DefaultCellStyle.BackColor = color;
            dgvCompInfo.DefaultCellStyle.ForeColor = Color.White;
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateLists(Win32ClassList, NoNulls);
            dtAllComputerInfo = ConvertListsToDataTable(DataLists);
        }

        private void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SuspendLayout();

            if (ClipboardOnly)
            {
                MessageBox.Show("Data copied!");
                CopyDataToClipboard();

                Close();
            }
            else if (dtAllComputerInfo.Columns.Count > 500)
            {
                //Prevent COM non-communication / timeouts.
                MessageBox.Show("This report can't be generated to screen.\n\nThe contents have already been copied to your Clipboard!");

                CopyDataToClipboard();

                Close();
            }

            else if (dtAllComputerInfo.Columns.Count > 350)
            {
                //Handle fillweight issues.
                dgvCompInfo.AutoGenerateColumns = false;
                dgvCompInfo.DataSource = null;
                dgvCompInfo.DataSource = dtAllComputerInfo;

                dgvCompInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                for (int i = 0; i < dtAllComputerInfo.Columns.Count && i < int.MaxValue; i++)
                {

                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                    column.Name = dtAllComputerInfo.Columns[i].ColumnName;
                    column.DataPropertyName = dtAllComputerInfo.Columns[i].ColumnName;
                    column.HeaderText = dtAllComputerInfo.Columns[i].ColumnName;
                    column.FillWeight = 50;
                    dgvCompInfo.Columns.Add(column);
                }
            }
            else
            {
                //Data is a bindingsource.
                Data.DataSource = dtAllComputerInfo;
            }

            ResumeLayout();

            Text = "Computer Info Report (v0.001)";
        }

        private void CreateLists(List<string> Win32ClassList, bool NoNulls)
        {
            DataLists.Clear();

            if (NoNulls)
            {
                foreach (string Win32Class in Win32ClassList)
                {
                    GetComputerInfo_NoNulls(Win32Class);
                }
            }
            else
            {
                foreach (string Win32Class in Win32ClassList)
                {
                    GetComputerInfo(Win32Class);
                }
            }
        }

        private DataTable ConvertListsToDataTable(List<List<string>> DataLists)
        {
            DataTable table = CreateNewTable(DataLists);
            int Max_Rows = GetMaxRows(DataLists);

            for (int i = 0; i < Max_Rows && i < int.MaxValue; i++ )
            {
                DataRow row = table.NewRow();
                table.Rows.InsertAt(row, i);
            }

            int columncounter = 0;
            foreach(List<string> list in DataLists)
            {
                int rowcounter = 0;
                foreach(string temp in list)
                {
                    if (rowcounter > 0)
                    {
                        table.Rows[rowcounter - 1][columncounter] = temp;
                    }

                    rowcounter++;
                }
                columncounter++;
            }

            return table;
        }

        private DataTable CreateNewTable(List<List<string>> DataLists)
        {
            DataTable NewTable = new DataTable();
            int count = 0;

            foreach(List<string> list in DataLists)
            {
                if (list.Any())
                {
                    NewTable.Columns.Add(list.ElementAt(0) + " (" + count + ")", typeof(string));
                    count++;
                }
            }

            return NewTable;
        }

        private int GetMaxRows(List<List<string>> DataLists)
        {
            int Max_Rows = -1;

            foreach(List<string> list in DataLists)
            {
                if (Max_Rows < (list.Count()))
                {
                    Max_Rows = (list.Count());
                }
            }

            return Max_Rows;
        }

        private void GetComputerInfo( string WMIClass )
        {
            ManagementClass mc = new ManagementClass(WMIClass);
            ManagementObjectCollection MOC = mc.GetInstances();

            if (MOC.Count > 0)
            {
                foreach (ManagementObject MO in MOC)
                {
                    List<string> ComputerInfo = new List<string>();
                    ComputerInfo.Add(WMIClass);

                    PropertyDataCollection pdlist = MO.Properties;

                    foreach (PropertyData pd in pdlist)
                    {
                        string temp = "";

                        if (pd.Value is string[])
                        {
                            string[] strArray = ((string[])pd.Value);

                            temp = pd.Name + ": ";

                            for (int i = 0; i < strArray.Length && i < int.MaxValue; i++)
                            {
                                ComputerInfo.Add(temp + strArray[i]);
                            }
                        }
                        else
                        {
                            if (pd.Name.Contains("VariableValue"))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    string[] splitString = pd.Value.ToString().Split(';');

                                    foreach (string split in splitString)
                                    {
                                        ComputerInfo.Add(temp + split);
                                    }
                                }
                                else
                                {
                                    temp += "(NULL)";
                                    ComputerInfo.Add(temp);
                                }
                            }
                            else if ((pd.Name.Contains("GroupComponent") || (pd.Name.Contains("PartComponent"))))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    ComputerInfo.Add(temp + pd.Value);
                                    string[] splitString = pd.Value.ToString().Split(',');

                                    if (splitString.Any())
                                    {
                                        for (int i = 0; i < splitString.Length && i < int.MaxValue; i++)
                                        {
                                            if (i != 0)
                                            {
                                                ComputerInfo.Add(temp + splitString[i]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        splitString = pd.Value.ToString().Split(':');

                                        for (int i = 0; i < splitString.Length || i < int.MaxValue; i++)
                                        {
                                            if (i != 0)
                                            {
                                                ComputerInfo.Add("Win32 Relationship: " + splitString[i]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    temp += "(NULL)";
                                    ComputerInfo.Add(temp);
                                }
                            }
                            else if ((pd.Name.Contains("SameElement")) || (pd.Name.Contains("SystemElement")))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    ComputerInfo.Add(temp + pd.Value);
                                    string[] splitString = pd.Value.ToString().Split(':');

                                    ComputerInfo.Add("Root: " + splitString[0]);

                                    splitString = pd.Value.ToString().Split(':');

                                    for (int i = 0; i < splitString.Length || i < int.MaxValue; i++)
                                    {
                                        if (i != 0)
                                        {
                                            ComputerInfo.Add("Win32 Relationship: " + splitString[i]);
                                            string deviceid = (splitString[i].Split('.'))[1];
                                            deviceid = deviceid.Replace("DeviceID=", "");
                                            ComputerInfo.Add("Device: " +deviceid);
                                        }
                                    }
                                }
                                else
                                {
                                    temp += "(NULL)";
                                    ComputerInfo.Add(temp);
                                }
                            }
                            else
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    temp += pd.Value.ToString();
                                }
                                else
                                {
                                    temp += "(NULL)";
                                }

                                ComputerInfo.Add(temp);
                            }
                        }
                    }

                    DataLists.Add(ComputerInfo);
                }
            }
        }

        private void GetComputerInfo_NoNulls( string WMIClass )
        {
            ManagementClass mc = new ManagementClass(WMIClass);
            ManagementObjectCollection MOC = mc.GetInstances();
            int count = 0;
            
            //MOC has a chance of success in assignment but failure on Count call.
            try { count = MOC.Count; }
            catch { count = -1; }

            if (count > 0)
            {
                foreach (ManagementObject MO in MOC)
                {
                    List<string> ComputerInfo = new List<string>();
                    ComputerInfo.Add(WMIClass);

                    PropertyDataCollection pdlist = MO.Properties;

                    foreach (PropertyData pd in pdlist)
                    {
                        string temp = "";

                        if (pd.Value is string[])
                        {
                            string[] strArray = ((string[])pd.Value);

                            temp = pd.Name + ": ";

                            for (int i = 0; i < strArray.Length && i < int.MaxValue; i++)
                            {
                                ComputerInfo.Add(temp + strArray[i]);
                            }
                        }
                        else
                        {
                            if (pd.Name.Contains("VariableValue"))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    string[] splitString = pd.Value.ToString().Split(';');

                                    foreach (string split in splitString)
                                    {
                                        ComputerInfo.Add(temp + split);
                                    }
                                }
                            }
                            else if ((pd.Name.Contains("GroupComponent") || (pd.Name.Contains("PartComponent"))))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    ComputerInfo.Add(temp + pd.Value);
                                    string[] splitString = pd.Value.ToString().Split(',');

                                    if (splitString.Any())
                                    {
                                        for (int i = 0; i < splitString.Length && i < int.MaxValue; i++)
                                        {
                                            if (i != 0)
                                            {
                                                ComputerInfo.Add(temp + splitString[i]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        splitString = pd.Value.ToString().Split(':');

                                        for (int i = 0; i < splitString.Length && i < int.MaxValue; i++)
                                        {
                                            if (i != 0)
                                            {
                                                ComputerInfo.Add("Win32 Relationship: " + splitString[i]);
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((pd.Name.Contains("SameElement")) || (pd.Name.Contains("SystemElement")))
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    ComputerInfo.Add(temp + pd.Value);
                                    string[] splitString = pd.Value.ToString().Split(':');

                                    ComputerInfo.Add("Root: " + splitString[0]);

                                    splitString = pd.Value.ToString().Split(':');

                                    for (int i = 0; i < splitString.Length && i < int.MaxValue; i++)
                                    {
                                        if (i != 0)
                                        {
                                            ComputerInfo.Add("Win32 Relationship: " + splitString[i]);
                                            string deviceid = (splitString[i].Split('.'))[1];
                                            deviceid = deviceid.Replace("DeviceID=", "");
                                            ComputerInfo.Add("Device: " + deviceid);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                temp = pd.Name + ": ";

                                if (pd.Value != null)
                                {
                                    temp += pd.Value.ToString();
                                    ComputerInfo.Add(temp);
                                }
                            }
                        }
                    }

                    DataLists.Add(ComputerInfo);
                }
            }
        }

        private void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, control, new object[] { true });
        }

        private void CopyDataToClipboard()
        {
            StringBuilder Output = new StringBuilder();
 
            //The first "line" will be the Headers.
            for (int i = 0; i < dtAllComputerInfo.Columns.Count && i < int.MaxValue; i++)
            {
                Output.Append(dtAllComputerInfo.Columns[i].ColumnName + "\t");
            }
 
            Output.Append("\n");
 
            //Generate Cell Value Data
            foreach (DataRow Row in dtAllComputerInfo.Rows)
            {
                //Don't generate a new line at all if Row is not visible
                for (int i = 0; i < Row.ItemArray.Length && i < int.MaxValue; i++)
                {
                    //Handling the last cell of the line.
                    if (i == (Row.ItemArray.Length - 1))
                    {

                        Output.Append(Row.ItemArray[i].ToString() + "\n");
                    }
                    else
                    {

                        Output.Append(Row.ItemArray[i].ToString() + "\t");
                    }
                }
            }
 
            Clipboard.SetText(Output.ToString());
        }
    }
}
