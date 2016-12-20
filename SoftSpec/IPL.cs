using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Wolf
{
    class IPL
    {
        private List<InstalledProgram> allInstalled = new List<InstalledProgram>();
        public List<InstalledProgram> sortedList = new List<InstalledProgram>();

        public IPL()
        {
            allInstalled = getInstalledPrograms();

            if (allInstalled.Any())
            {
                funcSortByName();
            }

            sortedList = new List<InstalledProgram>(allInstalled);
        }

        //http://stackoverflow.com/questions/15524161/c-how-to-get-installing-programs-exactly-like-in-control-panel-programs-and-fe
        private List<InstalledProgram> getInstalledPrograms()
        {
            List<InstalledProgram> newList = new List<InstalledProgram>();
            RegistryKey UninstallKey32 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);

            foreach (string temp in UninstallKey32.GetSubKeyNames())
            {
                try
                {
                    RegistryKey subkey = UninstallKey32.OpenSubKey(temp);

                    if (IsProgramVisible(subkey))
                    {
                        string name = "";
                        string version = "";
                        string installDate = "";
                        string uninstallCommand = "";

                        if (subkey.GetValue("DisplayName") != null)
                        {
                            name = subkey.GetValue("DisplayName").ToString();
                        }
                        else
                        {
                            name = "";
                        }

                        if (subkey.GetValue("DisplayVersion") != null)
                        {
                            version = subkey.GetValue("DisplayVersion").ToString();
                        }
                        else
                        {
                            version = "";
                        }

                        if (subkey.GetValue("InstallDate") != null)
                        {
                            installDate = subkey.GetValue("InstallDate").ToString();
                        }
                        else
                        {
                            installDate = "";
                        }

                        if (subkey.GetValue("UninstallString") != null)
                        {
                            uninstallCommand = subkey.GetValue("UninstallString").ToString();
                        }
                        else
                        {
                            uninstallCommand = "";
                        }

                        newList.Add(new InstalledProgram(name, version, installDate, uninstallCommand));
                    }
                }
                catch(Exception EX)
                {
                    MessageBox.Show("Exception Occurred: 32-Bit Program Reading. \n\n" +
                                    EX.Message + "\n\n" +
                                    EX.StackTrace);

                    continue;
                }
            }

            if (Environment.Is64BitOperatingSystem)
            {
                RegistryKey UninstallKey64 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);
                foreach (string temp in UninstallKey64.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey subkey = UninstallKey64.OpenSubKey(temp);

                        if (IsProgramVisible(subkey))
                        {
                            string name = "";
                            string version = "";
                            string installDate = "";
                            string uninstallCommand = "";

                            if (subkey.GetValue("DisplayName") != null)
                            {
                                name = subkey.GetValue("DisplayName").ToString();
                            }
                            else
                            {
                                name = "Prog. (xNull.)";
                            }

                            if (subkey.GetValue("DisplayVersion") != null)
                            {
                                version = subkey.GetValue("DisplayVersion").ToString();
                            }
                            else
                            {
                                version = "Vers. (xNull)";
                            }

                            if (subkey.GetValue("InstallDate") != null)
                            {
                                installDate = subkey.GetValue("InstallDate").ToString();
                            }
                            else
                            {
                                installDate = "";
                            }

                            if (subkey.GetValue("UninstallString") != null)
                            {
                                uninstallCommand = subkey.GetValue("UninstallString").ToString();
                            }
                            else
                            {
                                uninstallCommand = "";
                            }

                            newList.Add(new InstalledProgram(name, version, installDate, uninstallCommand));
                        }
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show("Exception Occurred: 64-Bit Program Reading. \n\n" +
                                        EX.Message + "\n\n" +
                                        EX.StackTrace);

                        continue;
                    }
                }
            }

            return newList;
        }

        private bool IsProgramVisible(RegistryKey subkey)
        {
            bool IsVisible = false;
            string name = "";

            if (subkey.GetValue("DisplayName") != null)
            {
                name = subkey.GetValue("DisplayName").ToString();
            }

            if ((name != null)&&(name != ""))
            {
                IsVisible = true;
            }

            return IsVisible;
        }

        //Uses Delta function to sort.
        private void funcSortByName()
        {
            allInstalled.Sort((x, y) => string.Compare(x.getProgramName(), y.getProgramName()));
        }

        public List<InstalledProgram> getSortedList()
        {
            return this.sortedList;
        }

        public List<InstalledProgram> getUnsortedList()
        {
            return this.allInstalled;
        }
    }
}
