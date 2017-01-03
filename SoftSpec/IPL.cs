using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace Wolf.SoftSpec
{
    internal class IPL
    {
        private readonly List<InstalledProgram> allInstalled = new List<InstalledProgram>();
        public List<InstalledProgram> sortedList = new List<InstalledProgram>();

        public IPL()
        {
            allInstalled = getInstalledPrograms();

            if (allInstalled.Any())
                funcSortByName();

            sortedList = new List<InstalledProgram>(allInstalled);
        }

        //http://stackoverflow.com/questions/15524161/c-how-to-get-installing-programs-exactly-like-in-control-panel-programs-and-fe
        private List<InstalledProgram> getInstalledPrograms()
        {
            var newList = new List<InstalledProgram>();
            var UninstallKey32 =
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);

            Parallel.ForEach (UninstallKey32.GetSubKeyNames(), ip =>
            {
                var subkey = UninstallKey32.OpenSubKey(ip);

                if (IsProgramVisible(subkey))
                {
                    var name = subkey?.GetValue("DisplayName")?.ToString() ?? "";
                    var version = subkey?.GetValue("DisplayVersion")?.ToString() ?? "";
                    var installDate = subkey?.GetValue("InstallDate")?.ToString() ?? "";
                    var uninstallCommand = subkey?.GetValue("UninstallString")?.ToString() ?? "";

                    newList.Add(new InstalledProgram(name, version, installDate, uninstallCommand));
                }
            });

            if (Environment.Is64BitOperatingSystem)
            {
                var UninstallKey64 =
                    Registry.LocalMachine.OpenSubKey(
                        "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);
                Parallel.ForEach(UninstallKey64.GetSubKeyNames(), ip =>
                {
                    var subkey = UninstallKey64.OpenSubKey(ip);

                    if (IsProgramVisible(subkey))
                    {
                        var name = "";
                        var version = "";
                        var installDate = "";
                        var uninstallCommand = "";

                        if (subkey.GetValue("DisplayName") != null)
                            name = subkey.GetValue("DisplayName").ToString();
                        else
                            name = "Prog. (xNull.)";

                        if (subkey.GetValue("DisplayVersion") != null)
                            version = subkey.GetValue("DisplayVersion").ToString();
                        else
                            version = "Vers. (xNull)";

                        if (subkey.GetValue("InstallDate") != null)
                            installDate = subkey.GetValue("InstallDate").ToString();
                        else
                            installDate = "";

                        if (subkey.GetValue("UninstallString") != null)
                            uninstallCommand = subkey.GetValue("UninstallString").ToString();
                        else
                            uninstallCommand = "";

                        newList.Add(new InstalledProgram(name, version, installDate, uninstallCommand));
                    }
                });
            }

            return newList;
        }

        private bool IsProgramVisible(RegistryKey subkey)
        {
            var IsVisible = false;
            var name = "";

            if (subkey.GetValue("DisplayName") != null)
                name = subkey.GetValue("DisplayName").ToString();

            if (name != null && name != "")
                IsVisible = true;

            return IsVisible;
        }

        //Uses Delta function to sort.
        private void funcSortByName()
        {
            allInstalled.Sort((x, y) => string.Compare(x.getProgramName(), y.getProgramName()));
        }

        public List<InstalledProgram> getSortedList()
        {
            return sortedList;
        }

        public List<InstalledProgram> getUnsortedList()
        {
            return allInstalled;
        }
    }
}