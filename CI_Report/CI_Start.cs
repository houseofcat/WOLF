using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolf.CI_Report
{
    public partial class CI_Start : Form
    {
        string strTitle = "Computer Info Report - Configuration Tool ";
        string strVersion = "(v0.001)";
        BackgroundWorker BWHW = new BackgroundWorker();
        BackgroundWorker BWOS = new BackgroundWorker();
        BackgroundWorker BWPIQ = new BackgroundWorker();

        List<int> Win32HW_Counters = new List<int>();
        List<int> Win32OS_Counters = new List<int>();
        List<int> Win32PIQ_Counters = new List<int>();

        int PerformanceCounter = 0;

        public CI_Start()
        {
            InitializeComponent();

            this.Text = strTitle + strVersion + " - Loading...";

            BWHW.DoWork += new DoWorkEventHandler(BW_HW_DoWork);
            BWHW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_HW_RunWorkerCompleted);

            BWOS.DoWork += new DoWorkEventHandler(BW_OS_DoWork);
            BWOS.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_OS_RunWorkerCompleted);

            BWPIQ.DoWork += new DoWorkEventHandler(BWPIQ_DoWork);
            BWPIQ.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BWPIQ_RunWorkerCompleted);

            BWHW.RunWorkerAsync();
            BWOS.RunWorkerAsync();
        }

        private void BW_HW_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (Control c in gbxWin32HW.Controls)
            {
                Win32HW_Counters.Add(GetWin32InstanceCount("Win32_" + c.Text));
            }
        }

        private void BW_OS_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (Control c in gbxWin32OS.Controls)
            {
                Win32OS_Counters.Add(GetWin32InstanceCount("Win32_" + c.Text));
            }
        }

        private void BW_HW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SuspendLayout();

            int count = 0;
            foreach (Control c in gbxWin32HW.Controls)
            {
                int temp = Win32HW_Counters.ElementAt(count);

                if (temp > 0)
                {
                    c.Text += " (" + temp.ToString() + ")";
                    c.Enabled = true;
                }
                else
                {
                    c.Text += " (" + temp.ToString() + ")";
                }

                count++;
            }

            this.ResumeLayout(true);
        }

        private void BW_OS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SuspendLayout();

            int count = 0;
            foreach (Control c in gbxWin32OS.Controls)
            {
                int temp = Win32OS_Counters.ElementAt(count);

                if (temp > 0)
                {
                    c.Text += " (" + temp.ToString() + ")";
                    c.Enabled = true;
                }
                else
                {
                    c.Text += " (" + temp.ToString() + ")";
                }

                count++;
            }

            cbxNoNulls.Enabled = true;
            cbxClipboard.Enabled = true;
            btnRunReport.Enabled = true;
            btnEnablePIQs.Enabled = true;
            this.Text = strTitle + strVersion;

            this.ResumeLayout(true);
        }

        private void BWPIQ_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (Control c in gbxWin32PIQs.Controls)
            {
                Win32PIQ_Counters.Add(GetWin32InstanceCount("Win32_" + c.Text));
            }
        }

        private void BWPIQ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SuspendLayout();

            int count = 0;
            foreach (Control c in gbxWin32PIQs.Controls)
            {
                int temp = Win32PIQ_Counters.ElementAt(count);

                if (temp > 0)
                {
                    c.Text += " (" + temp.ToString() + ")";
                    c.Enabled = true;
                }
                else
                {
                    c.Text += " (" + temp.ToString() + ")";
                }

                count++;
            }

            gbxWin32PIQs.Text = "Win32 - Performance Intensive Queries";

            this.ResumeLayout(true);
        }

        private int GetWin32InstanceCount(String WMIClass)
        {
            int Count = 0;

            try
            {
                ManagementClass mc = new ManagementClass(WMIClass);
                Count = (mc.GetInstances()).Count;

                //Console.WriteLine(WMIClass + " count: " + Count);
            }
            catch
            {
                Count = -1;
            }

            return Count;
        }

        private void EnablePIQs(object sender, EventArgs e)
        {
            btnEnablePIQs.Enabled = false;

            foreach(Control c in gbxWin32PIQs.Controls)
            {
                c.Enabled = true;
            }

            btnGetInstances.Enabled = true;
        }

        private void GetInstances(object sender, EventArgs e)
        {
            btnGetInstances.Enabled = false;

            if (BWPIQ.IsBusy)
            {
                MessageBox.Show("Be patient, previous Get Instances is still working.");
            }
            else
            {
                foreach (Control c in gbxWin32PIQs.Controls)
                {
                    c.Enabled = false;
                }

                gbxWin32PIQs.Text = "Win32 - Performance Intensive Queries - Loading...";
                BWPIQ.RunWorkerAsync();
            }
        }

        private void TallyPerformance(object sender, EventArgs e)
        {
            CheckBox temp = (CheckBox)sender;
            //string[] strArray = temp.Text.Split(' ');

            if (temp.Checked)
            {
                //IncrementPerformance("Win32_" + strArray[0]);
                IncrementPerformancev2(temp.Text);

            }
            else
            {
                //DecrementPerformance("Win32_" + strArray[0]);
                DecrementPerformancev2(temp.Text);
            }

            //Console.WriteLine(temp.Text);
        }

        private void IncrementPerformance(String WMIClass)
        {
            int temp = GetWin32InstanceCount(WMIClass);

            PerformanceCounter += temp;
            tbxPerformance.Text = PerformanceCounter.ToString();

            EvaluatePerformance();
        }

        private void IncrementPerformancev2(String WMIClass)
        {
            int temp = 0;
            string[] strArray = WMIClass.Split(' ');
            string strNumber = strArray[1].Replace("(", "");
            strNumber = strNumber.Replace(")", "");

            if (Int32.TryParse(strNumber, out temp))
            {

                PerformanceCounter += temp;
                tbxPerformance.Text = PerformanceCounter.ToString();

                EvaluatePerformance();
            }
        }

        private void DecrementPerformance(String WMIClass)
        {
            int temp = GetWin32InstanceCount(WMIClass);

            //Handles delay in rendering to UI.
            if ((PerformanceCounter - temp) >= 0)
            { PerformanceCounter -= temp; }
            else
            { PerformanceCounter = 0; }

            tbxPerformance.Text = PerformanceCounter.ToString();

            EvaluatePerformance();
        }

        private void DecrementPerformancev2(String WMIClass)
        {
            int temp = 0;
            string[] strArray = WMIClass.Split(' ');
            string strNumber = strArray[1].Replace("(", "");
            strNumber = strNumber.Replace(")", "");

            if (Int32.TryParse(strNumber, out temp))
            {
                if ((PerformanceCounter - temp) >= 0)
                { PerformanceCounter -= temp; }
                else
                { PerformanceCounter = 0; }

                tbxPerformance.Text = PerformanceCounter.ToString();

                EvaluatePerformance();
            }
        }

        private void EvaluatePerformance()
        {
            if ((PerformanceCounter >= 0) && (PerformanceCounter < 20))
            {
                tbxPerformance.BackColor = Color.Green;
                tbxPerformance.ForeColor = Color.White;
            }
            else if ((PerformanceCounter >= 20) && (PerformanceCounter < 40))
            {
                tbxPerformance.BackColor = Color.Yellow;
                tbxPerformance.ForeColor = Color.Black;
            }
            else if ((PerformanceCounter >= 40) && (PerformanceCounter < 50))
            {
                tbxPerformance.BackColor = Color.Orange;
                tbxPerformance.ForeColor = Color.Black;
            }
            else if (PerformanceCounter >= 50)
            {
                tbxPerformance.BackColor = Color.Red;
                tbxPerformance.ForeColor = Color.White;
            }
        }

        private void RunReport(object sender, EventArgs e)
        {
            List<string> Win32ClassList = CompileWin32Classes();

            if (Win32ClassList.Any())
            {
                CI_Report newReport = new CI_Report(Win32ClassList, cbxNoNulls.Checked, cbxClipboard.Checked);

                if (!(cbxClipboard.Checked))
                {
                    newReport.Show();
                }
            }
        }

        private List<string> CompileWin32Classes()
        {
            List<string> Win32ClassList = new List<string>();

            foreach(Control c in gbxWin32HW.Controls)
            {
                CheckBox temp = (CheckBox)c;

                if (temp.Checked)
                {
                    string[] strArray = temp.Text.Split(' ');

                    Win32ClassList.Add("Win32_" + strArray[0]);
                }
            }

            foreach (Control c in gbxWin32OS.Controls)
            {
                CheckBox temp = (CheckBox)c;

                if (temp.Checked)
                {
                    string[] strArray = temp.Text.Split(' ');

                    Win32ClassList.Add("Win32_" + strArray[0]);
                }
            }

            foreach (Control c in gbxWin32PIQs.Controls)
            {
                CheckBox temp = (CheckBox)c;

                if (temp.Checked)
                {
                    string[] strArray = temp.Text.Split(' ');

                    Win32ClassList.Add("Win32_" + strArray[0]);
                }
            }

            return Win32ClassList;
        }
    }
}
