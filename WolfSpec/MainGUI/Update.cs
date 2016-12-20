using System;
using System.Linq;
using System.Windows.Forms;

namespace Wolf
{
    public partial class Update : Form
    {
        public Update(string Version)
        {
            InitializeComponent();
            funcCheckUpdate();
            lblCurrentVersion.Text = "Current Version: v" + Version;
        }

        private void funcCheckUpdate()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    string webData = wc.DownloadString("http://www.bytemedev.com/Public/lv.txt");

                    if (webData.Any())
                    {
                        lblLatestVersion.Text = "Latest Version: v" + webData;
                    }
                    else
                    {
                        lblLatestVersion.Text = "Latest Version: Unknown";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update Exception Occured: " + ex.Message + "\n\n" +
                                    "Stack: " + ex.StackTrace);
                }
            }
            else
            {
                lblLatestVersion.Text = "Latest Version: Offline";
            }
        }

        private void ClickEventHandler(Object sender, EventArgs e)
        {
            if (sender.Equals(llCurrent))
            {
                Tools.gotoBYTEMEDEVWOLFCURRENT();
            }
        }
    }
}
