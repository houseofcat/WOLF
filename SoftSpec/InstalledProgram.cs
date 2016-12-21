using System.Diagnostics;

namespace Wolf
{
    class InstalledProgram
    {
        private string programName = "";
        private string programVersion = "";
        private string programInstDate = "";
        private string programUninstallCMD = "";
        private bool CanBeUninstalled = true;

        public InstalledProgram()
        {

        }

        public InstalledProgram(string name, string version, string date, string UCMD)
        {
            setProgramName(name);
            setProgramVersion(version);
            setInstallDate(date);
            setUninstallCMD(UCMD);

            if ((UCMD == "")||(UCMD == null))
            {
                CanBeUninstalled = false;
            }
        }

        private void setProgramName(string name)
        {
            programName = name;
        }

        private void setProgramVersion(string version)
        {
            programVersion = version;
        }

        private void setInstallDate(string date)
        {
            if ((date != null)&&(date != ""))
            {
                if (!(date.Contains("/")))
                {
                    date = date.Insert(4, "/");
                    date = date.Insert(7, "/");
                }
            }
            else
            {
                date = "Unknown";
            }

            programInstDate = date;
        }

        private void setUninstallCMD(string UCMD)
        {
            if (UCMD != "")
            {
                programUninstallCMD = UCMD;
            }
            else
            {
                programUninstallCMD = "Unknown";
            }
        }

        //Executes a cmd process to use the Uninstall via command line process
        //similar to Windows Control Panel.
        public void funcUninstall()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo();

            procStartInfo.FileName = "cmd.exe";
            procStartInfo.WorkingDirectory = @"C:\";
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            procStartInfo.Arguments = "/C \"" + programUninstallCMD + "\"";

            Process startProc = Process.Start(procStartInfo);

            startProc.WaitForExit();
        }

        public string getProgramName()
        {
            return programName;
        }

        public string getProgramVersion()
        {
            return programVersion;
        }

        public string getProgramInstallDate()
        {
            return programInstDate;
        }

        public string getUninstallCommand()
        {
            return programUninstallCMD;
        }

        public bool IsProgramUninstallable()
        {
            return CanBeUninstalled;
        }
    }
}
