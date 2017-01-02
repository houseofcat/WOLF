using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using NetFwTypeLib;

namespace Wolf.WolfSpec
{
    #region UnmanagedCode
    class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint EnumSystemFirmwareTables(uint FirmwareTableProviderSignature, IntPtr pFirmwareTableBuffer, uint BufferSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetSystemFirmwareTable(uint FirmwareTableProviderSignature, uint FirmwareTableID, IntPtr pFirmwareTableBuffer, uint BufferSize);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(ref IntPtr ptr);
    }
    #endregion

    public class Tools
    {

        public Tools()
        {
        }

        public static void startCMDasAdmin()
        {
            //Launch CMD Prompt with elevated credentials.
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WorkingDirectory = @"C:\";

            Process proc = new Process();

            proc.StartInfo = procStartInfo;
            proc.Start();
        }

        public static bool IsHPETEnabled()
        {
            bool IsEnabled = false;

            Process p = new Process();
            ProcessStartInfo ps = new ProcessStartInfo();

            string cmdFullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                                       Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess
                                           ? @"Sysnative\cmd.exe"
                                           : @"System32\cmd.exe");

            ps.FileName = cmdFullFileName;
            ps.Arguments = @"/c bcdedit /enum";

            ps.UseShellExecute = false;
            ps.CreateNoWindow = true;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = true;

            p.StartInfo = ps;
            p.Start();

            StreamReader stdOutput = p.StandardOutput;

            string content = stdOutput.ReadToEnd();
            string[] rows = Regex.Split(content, "\n");

            foreach (string row in rows)
            {
                //Eliminates the default BCEDIT /enumeration Rows
                if (((row.Contains("useplatformclock")) && (row.Contains("Yes"))))
                {
                    //Console.WriteLine(rows);
                    IsEnabled = true;
                }
            }

            return IsEnabled;
        }

        public static bool IsDEPEnabled()
        {
            bool IsEnabled = false;

            Process p = new Process();
            ProcessStartInfo ps = new ProcessStartInfo();

            //Purpose: For dealing with stupid Folder Redirection.
            //Source: http://stackoverflow.com/questions/14023051/bcdedit-not-recognized-when-running-via-c-sharp, user2126375
            string cmdFullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                                       ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                                           ? @"Sysnative\cmd.exe" : @"System32\cmd.exe");

            ps.FileName = cmdFullFileName;
            ps.Arguments = @"/c bcdedit /enum";

            ps.UseShellExecute = false;
            ps.CreateNoWindow = true;
            ps.RedirectStandardInput = true;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = true;

            p.StartInfo = ps;
            p.Start();

            StreamReader stdOutput = p.StandardOutput;
            StreamReader stdError = p.StandardError;

            string content = stdOutput.ReadToEnd();
            string exitStatus = p.ExitCode.ToString();

            if (exitStatus != "0")
            {
                MessageBox.Show("An error occurred in gathering information from BCDEDIT.exe.\n\n" + stdError.ToString());
            }

            string[] rows = Regex.Split(content, "\n");

            foreach (string row in rows)
            {
                //Eliminates the default NetStat Rows
                if ((row.Contains("nx")) && (row.Contains("OptIn")))
                {
                    IsEnabled = true;
                }
            }

            return IsEnabled;
        }

        public static void funcEnableHPET()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\system32";
                procStartInfo.Arguments = @"/k bcdedit /set useplatformclock true";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\system32";
                procStartInfo.Arguments = @"/k bcdedit /set useplatformclock true";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }

        }

        public static void funcDisableHPET()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set useplatformclock false & " +
                                          @"bcdedit /deletevalue useplatformclock";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set useplatformclock false & " +
                                          @"bcdedit /deletevalue useplatformclock";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
            }
        }

        public static void funcEnableDEP()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set {current} nx AlwaysOn";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                proc.WaitForExit();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set {current} nx AlwaysOn";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                proc.WaitForExit();
            }

        }

        public static void funcDisableDEP()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set {current} nx AlwaysOff";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                proc.WaitForExit();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k bcdedit /set {current} nx AlwaysOff";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                proc.WaitForExit();
            }

        }

        public static void funcFlushDNS()
        {
            Process.Start("ipconfig", "/flushDNS");
        }

        public static void funcRepairVSS()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

            procStartInfo.WorkingDirectory = @"C:\";
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.Arguments = @"/K net stop ""System Event Notification Service"" & " +
                                      @"net stop ""Background Intelligent Transfer Service"" & " +
                                      @"net stop ""COM+ Event System"" & " +
                                      @"net stop ""Microsoft Software Shadow Copy Provider"" & " +
                                      @"net stop vss & net stop swprv & " +
                                      @"regsvr32 /s ATL.DLL & regsvr32 /s comsvcs.DLL & regsvr32 /s credui.DLL & " +
                                      @"regsvr32 /s CRYPTNET.DLL & regsvr32 /s CRYPTUI.DLL & regsvr32 /s dhcpqec.DLL & " +
                                      @"regsvr32 /s dssenh.DLL & regsvr32 /s eapqec.DLL & regsvr32 /s esscli.DLL & " +
                                      @"regsvr32 /s esscli.DLL & regsvr32 /s FastProx.DLL & regsvr32 /s FirewallAPI.DLL & " +
                                      @"regsvr32 /s kmsvc.DLL & regsvr32 /s lsmproxy.DLL & regsvr32 /s MSCTF.DLL & " +
                                      @"regsvr32 /s msi.DLL & regsvr32 /s msxml3.DLL & regsvr32 /s ncprov.DLL & " +
                                      @"regsvr32 /s ole32.DLL & regsvr32 /s OLEACC.DLL & regsvr32 /s OLEAUT32.DLL & " +
                                      @"regsvr32 /s PROPSYS.DLL & regsvr32 /s QAgent.DLL & regsvr32 /s qagentrt.DLL & " +
                                      @"regsvr32 /s QUtil.DLL & regsvr32 /s raschap.DLL & regsvr32 /s RASQEC.DLL & " +
                                      @"regsvr32 /s rastls.DLL & regsvr32 /s repdrvfs.DLL & regsvr32 /s RPCRT4.DLL & " +
                                      @"regsvr32 /s rsaenh.DLL & regsvr32 /s SHELL32.DLL & regsvr32 /s shsvcs.DLL & " +
                                      @"regsvr32 /s swprv.DLL & regsvr32 /s tschannel.DLL & regsvr32 /s USERENV.DLL & " +
                                      @"regsvr32 /s vss_ps.DLL & regsvr32 /s wbemcons.DLL & regsvr32 /s wbemcore.DLL & " +
                                      @"regsvr32 /s wbemess.DLL & regsvr32 /s wbemsvc.DLL & regsvr32 /s WINHTTP.DLL & " +
                                      @"regsvr32 /s WINTRUST.DLL & regsvr32 /s wmiprvsd.DLL & regsvr32 /s wmisvc.DLL & " +
                                      @"regsvr32 /s wmiutils.DLL & regsvr32 /s wuaueng.DLL & " +
                                      @"sfc /SCANFILE=C:\Windows\System32\catsrv.DLL &" +
                                      @"sfc /SCANFILE=C:\Windows\System32\catsrvut.DLL &" +
                                      @"sfc /SCANFILE=C:\Windows\System32\CLBCatQ.DLL & " +
                                      @"net start ""COM+ Event System"" & " +
                                      @"net start ""System Event Notification Service"" & " +
                                      @"net start ""Background Intelligent Transfer Service"" & " +
                                      @"net start ""COM+ Event System"" & " +
                                      @"net start ""Microsoft Software Shadow Copy Provider"" & " +
                                      @"net start ""Volume Shadow Copy""";

            Process proc = Process.Start(procStartInfo);
            proc.WaitForExit();

            /*
            sw.WriteLine("");
            sw.WriteLine(");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");

            sw.Flush();*/
        }

        public static void funcRepairWMI()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K winmgmt /verifyrepository &" +
                                          @"winmgmt /salvagerepository";

                Process proc = Process.Start(procStartInfo);
                proc.WaitForExit();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K winmgmt /verifyrepository &" +
                                          @"winmgmt /salvagerepository";

                Process proc = Process.Start(procStartInfo);
                proc.WaitForExit();
            }

        }

        public static void funcRestartWMI()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K net stop ""wscsvc"" & " +
                                          @"net stop ""iphlpsvc"" & " +
                                          @"net stop ""winmgmt"" & " +
                                          @"net start ""winmgmt"" & " +
                                          @"net start ""iphlpsvc"" & " +
                                          @"net start ""wscsvc""";

                Process proc = Process.Start(procStartInfo);
                proc.WaitForExit();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K net stop ""wscsvc"" & " +
                                            @"net stop ""iphlpsvc"" & " +
                                            @"net stop ""winmgmt"" & " +
                                            @"net start ""winmgmt"" & " +
                                            @"net start ""iphlpsvc"" & " +
                                            @"net start ""wscsvc""";

                Process proc = Process.Start(procStartInfo);
                proc.WaitForExit();
            }

            /*ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

            procStartInfo.WorkingDirectory = @"C:\";
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = false;

            procStartInfo.Arguments = @"/K net stop ""wscsvc"" & " +
                                      @"net stop ""iphlpsvc"" & " +
                                      @"net stop ""winmgmt"" & " + 
                                      @"net start ""winmgmt"" & " +
                                      @"net start ""iphlpsvc"" & " +
                                      @"net start ""wscsvc""";

            Process proc = Process.Start(procStartInfo);*/
        }

        public static void funcForceGPUpdate()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K gpupdate /force";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K gpupdate /force";

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcLaunchWindowsMemTest()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("mdsched.exe");
                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("mdsched.exe");
                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcLaunchRegEdit()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("regedit.exe");
                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("regedit.exe");

                Process proc = Process.Start(procStartInfo);
            }
        }

        public static void funcLaunchWindowsFileSignatureVerification()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("sigverif.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("sigverif.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
        }

        public static void funcOpenSigVerificationFile()
        {
            if (File.Exists("C:\\Users\\Public\\Documents\\SIGVERIF.TXT"))
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("C:\\Users\\Public\\Documents\\SIGVERIF.TXT");

                try
                {
                    Process proc = Process.Start(procStartInfo);
                }
                catch (FileNotFoundException fnfe)
                {
                    MessageBox.Show(null, "SIGVERIF log was not found.", "SIGVERIF log not found.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fnfe.ToString();
                }
            }
            else
            {
                MessageBox.Show("Signature File Verification (SIGVERIF.TXT) was not found in C:\\Users\\Public\\Documents\\");
            }
        }

        public static void funcDriverQuery()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K driverquery";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K driverquery";

                Process proc = Process.Start(procStartInfo);
            }
        }

        public static void funcDriverQueryV()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K mode con cols=225 lines=3000 & " +
                                          @"driverquery /v";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K mode con cols=225 lines=3000 & " +
                                          @"driverquery /v";

                Process proc = Process.Start(procStartInfo);
            }

        }

        //Launches the Windows Remote Management CLI
        public static void runWinRM()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K winrm.cmd";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K winrm.cmd";

                Process proc = Process.Start(procStartInfo);
            }
        }

        //Windows RM CLI Quick Configuration
        public static void runQuickConfigRM()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K winrm quickconfig";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/K winrm quickconfig";

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcResetCMDSize()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k mode con cols=80 lines=2000";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k mode con cols=80 lines=2000";

                Process proc = Process.Start(procStartInfo);
            }
        }

        public static void funcLaunchOpenFile()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles";

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcEnableOpenFileQuery()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles /local on";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles /local on";

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcDisableOpenFileQuery()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles /local off";

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                procStartInfo.Arguments = @"/k openfiles /local off";

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void funcCheckDSK(string Drive, string Arguments)
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K chkdsk " + Drive + Arguments;

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K chkdsk " + Drive + Arguments;

                Process proc = Process.Start(procStartInfo);
            }


        }

        public static void funcDefragDSK(string Drive, string Arguments)
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K defrag " + Drive + Arguments;

                Process proc = Process.Start(procStartInfo);

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.Arguments = @"/K defrag " + Drive + Arguments;

                Process proc = Process.Start(procStartInfo);
            }


        }

        public static void funcAdvCleanupRun(string Drive)
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("cleanmgr", Drive + " /sagerun:1");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("cleanmgr", Drive + " /sagerun:1");
            }


        }

        public static void funcAdvCleanupSet(string Drive)
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("cleanmgr", Drive + " /tuneup:1");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                Process.Start("cleanmgr", Drive + " /tuneup:1");

                Process proc = Process.Start(procStartInfo);
            }

        }

        public static void runIPREL()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                //Run IPCONFIG with /RELEASE command.
                ProcessStartInfo procStartInfo = new ProcessStartInfo("ipconfig.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\System32\";
                procStartInfo.Arguments = @"/release *";

                Process proc = new Process();

                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                //Run IPCONFIG with /RELEASE command.
                ProcessStartInfo procStartInfo = new ProcessStartInfo("ipconfig.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\System32\";
                procStartInfo.Arguments = @"/release *";

                Process proc = new Process();

                proc.StartInfo = procStartInfo;
                proc.Start();
            }

        }

        public static void runIPREN()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                //Run IPCONFIG with /RELEASE command.
                ProcessStartInfo procStartInfo = new ProcessStartInfo("ipconfig.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\System32\";
                procStartInfo.Arguments = @"/renew";

                Process proc = new Process();

                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                //Run IPCONFIG with /RELEASE command.
                ProcessStartInfo procStartInfo = new ProcessStartInfo("ipconfig.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\Windows\System32\";
                procStartInfo.Arguments = @"/renew";

                Process proc = new Process();

                proc.StartInfo = procStartInfo;
                proc.Start();
            }
        }

        public static void runIPALL()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\ipconfig.exe";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("ipconfig", "/renew");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("ipconfig", "/renew");
                }
            }
            else { MessageBox.Show("IPConfig was missing from System32.", "File Not Found"); }
        }

        public static bool funcCheckCoreParking()
        {
            bool IsEnabled = true;

            try
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    string RegLocation = "SYSTEM\\ControlSet001\\Control\\Power\\PowerSettings\\54533251-82be-4824-96c1-47b60b740d00\\0cc5b647-c1df-4637-891a-dec35c318583";
                    RegistryKey corekey = (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                           RegistryView.Registry64)).OpenSubKey(RegLocation, false);

                    if (corekey != null)
                    {
                        if (corekey.GetValue("ValueMax").ToString() == "0")
                        {
                            IsEnabled = false;
                        }
                        else if (corekey.GetValue("ValueMax").ToString() == "64")
                        {
                            IsEnabled = true;
                        }

                        corekey.Close();
                    }
                }
                else
                {
                    RegistryKey corekey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Power\\PowerSettings\\54533251-82be-4824-96c1-47b60b740d00\\0cc5b647-c1df-4637-891a-dec35c318583", false);

                    if (corekey != null)
                    {
                        if (corekey.GetValue("ValueMax").ToString() == "0")
                        {
                            IsEnabled = false;
                        }
                        else if (corekey.GetValue("ValueMax").ToString() == "64")
                        {
                            IsEnabled = true;
                        }

                        corekey.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking Core Parking Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return IsEnabled;
        }

        public static bool funcCheckUAC()
        {
            bool IsEnabled = true;

            try
            {
                RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow632Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", false);

                if (uacKey != null)
                {
                    if (uacKey.GetValue("EnableLUA").ToString() == "1")
                    {
                        IsEnabled = true;
                    }
                    else if (uacKey.GetValue("EnableLUA").ToString() == "0")
                    {
                        IsEnabled = false;
                    }

                    uacKey.Close();
                }
                else
                {
                    uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", false);

                    if (uacKey != null)
                    {
                        if (uacKey.GetValue("EnableLUA").ToString() == "1")
                        {
                            IsEnabled = true;
                        }
                        else if (uacKey.GetValue("EnableLUA").ToString() == "0")
                        {
                            IsEnabled = false;
                        }

                        uacKey.Close();
                    }
                    else
                    {
                        MessageBox.Show("UAC status can't be read.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking UAC Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return IsEnabled;
        }

        public static bool funcCheckUACRemoteStatus()
        {
            bool IsEnabled = true;

            try
            {
                string LATFP = "";

                RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

                if (uacKey != null)
                {
                    if (uacKey.GetValue("LocalAccountTokenFilterPolicy") != null)
                    {
                        LATFP = uacKey.GetValue("LocalAccountTokenFilterPolicy").ToString();

                        if (LATFP != null)
                        {
                            if (LATFP == "1")
                            {
                                IsEnabled = false;
                            }
                            else if (LATFP == "0")
                            {
                                IsEnabled = true;
                            }
                        }

                        uacKey.Close();
                    }
                }
                else
                {
                    uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

                    if (uacKey.GetValue("LocalAccountTokenFilterPolicy") != null)
                    {
                        LATFP = uacKey.GetValue("LocalAccountTokenFilterPolicy").ToString();

                        if (LATFP != null)
                        {
                            if (LATFP == "1")
                            {
                                IsEnabled = false;
                            }
                            else if (LATFP == "0")
                            {
                                IsEnabled = true;
                            }
                        }

                        uacKey.Close();
                    }
                    else
                    {
                        MessageBox.Show("UAC Remote Restriction status can't be read.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking UAC Remote Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return IsEnabled;
        }

        public static bool funcCheckWindowsDefenderStatus()
        {
            bool IsEnabled = true;
            string DAS = "";

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows Defender", false);

                    if (wdKey != null)
                    {
                        if (wdKey.GetValue("DisableAntiSpyware") != null)
                        {
                            DAS = wdKey.GetValue("DisableAntiSpyware").ToString();

                            if (DAS == "1")
                            {
                                IsEnabled = false;
                            }
                            else if (DAS == "0")
                            {
                                IsEnabled = true;
                            }
                        }

                        wdKey.Close();
                    }
                    else
                    {
                        DialogResult temp = MessageBox.Show(null, "Windows Defender status can't be read.\n\nWould you like to disable all future notices?",
                                                             "Windows Defender Status", MessageBoxButtons.YesNo);

                        if (temp == DialogResult.Yes)
                        {
                            Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"] = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"] = false;
                            Properties.Settings.Default.Save();
                        }
                    }
                }
                else
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender", false);

                    if (wdKey != null)
                    {
                        if (wdKey.GetValue("DisableAntiSpyware") != null)
                        {
                            DAS = wdKey.GetValue("DisableAntiSpyware").ToString();

                            if (DAS == "1")
                            {
                                IsEnabled = false;
                            }
                            else if (DAS == "0")
                            {
                                IsEnabled = true;
                            }
                        }

                        wdKey.Close();
                    }
                    else
                    {
                        DialogResult temp = MessageBox.Show(null, "Windows Defender status can't be read.\n\nWould you like to disable all future notices?",
                                                             "Windows Defender Status", MessageBoxButtons.YesNo);

                        if (temp == DialogResult.Yes)
                        {
                            Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"] = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default["IGNORE_WINDEF_MESSAGE"] = false;
                            Properties.Settings.Default.Save();
                        }
                    }
                }

                //wdKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking Windows Defender Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return IsEnabled;
        }

        public static void funcDisableWindowsDefender()
        {
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432node\\Microsoft\\Windows Defender", true);

                    if (wdKey != null)
                    {
                        wdKey.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                        wdKey.Close();

                        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                        procStartInfo.UseShellExecute = false;
                        procStartInfo.CreateNoWindow = true;
                        procStartInfo.WorkingDirectory = @"C:\";
                        procStartInfo.Arguments = @"/c net start windefend";

                        Process proc = new Process();
                        proc.StartInfo = procStartInfo;

                        proc.Start();
                    }
                }
                else
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender", true);

                    if (wdKey != null)
                    {
                        wdKey.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                        wdKey.Close();

                        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                        procStartInfo.UseShellExecute = false;
                        procStartInfo.CreateNoWindow = true;
                        procStartInfo.WorkingDirectory = @"C:\";
                        procStartInfo.Arguments = @"/c net start windefend";

                        Process proc = new Process();
                        proc.StartInfo = procStartInfo;

                        proc.Start();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Microsoft is not allowing you (the Admin) to make changes. Ownership of regkey is now required to give permissions.\n\n" +
                                "Registry Location (64-bit): HKLM\\Software\\Wow6432Node\\Microsoft\\Windows Defender\n" +
                                "Registry Location (32-bit): HKLM\\Software\\Microsoft\\Windows Defender");
            }
        }

        public static void funcEnableWindowsDefender()
        {
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432node\\Microsoft\\Windows Defender", true);

                    if (wdKey != null)
                    {
                        wdKey.SetValue("DisableAntiSpyware", 0, RegistryValueKind.DWord);
                        wdKey.Close();

                        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                        procStartInfo.UseShellExecute = false;
                        procStartInfo.CreateNoWindow = true;
                        procStartInfo.WorkingDirectory = @"C:\";
                        procStartInfo.Arguments = @"/c net start windefend";

                        Process proc = new Process();
                        proc.StartInfo = procStartInfo;

                        proc.Start();
                    }
                }
                else
                {
                    RegistryKey wdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender", true);

                    if (wdKey != null)
                    {
                        wdKey.SetValue("DisableAntiSpyware", 0, RegistryValueKind.DWord);
                        wdKey.Close();

                        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                        procStartInfo.UseShellExecute = false;
                        procStartInfo.CreateNoWindow = true;
                        procStartInfo.WorkingDirectory = @"C:\";
                        procStartInfo.Arguments = @"/c net start windefend";

                        Process proc = new Process();
                        proc.StartInfo = procStartInfo;

                        proc.Start();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Microsoft is not allowing you (the Admin) to make changes. Ownership of regkey is now required to give permissions.\n\n" +
                                "Registry Location (64-bit): HKLM\\Software\\Wow6432Node\\Microsoft\\Windows Defender\n" +
                                "Registry Location (32-bit): HKLM\\Software\\Microsoft\\Windows Defender");
            }
        }

        public static void funcDisableCoreParking()
        {
            RegistryKey corekey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Power\\PowerSettings\\54533251-82be-4824-96c1-47b60b740d00\\0cc5b647-c1df-4637-891a-dec35c318583", true);

            if (corekey != null)
            {
                corekey.SetValue("ValueMax", 0, RegistryValueKind.DWord);
                corekey.Close();
            }
        }

        public static void funcDisableWinFirewall()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WorkingDirectory = @"C:\";
            procStartInfo.Arguments = @"/k netsh advfirewall set allprofiles state off";

            Process proc = new Process();
            proc.StartInfo = procStartInfo;

            proc.Start();
        }

        public static void funcEnableWinFirewall()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WorkingDirectory = @"C:\";
            procStartInfo.Arguments = @"/k netsh advfirewall set allprofiles state on";

            Process proc = new Process();
            proc.StartInfo = procStartInfo;

            proc.Start();
        }

        public static bool funcCheckFirewallStatus()
        {
            bool IsUp = true;

            try
            {
                ServiceController sc = new ServiceController("MpsSvc");
                string status = "";

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        status = "Running";
                        break;
                    case ServiceControllerStatus.Stopped:
                        status = "Stopped";
                        break;
                    case ServiceControllerStatus.Paused:
                        status = "Paused";
                        break;
                    case ServiceControllerStatus.StopPending:
                        status = "Stopping";
                        break;
                    case ServiceControllerStatus.StartPending:
                        status = "Starting";
                        break;
                    default:
                        status = "Status Changing";
                        break;
                }

                if (status == "Running")
                {
                    //Source: http://blogs.msdn.com/b/securitytools/archive/2009/08/21/automating-windows-firewall-settings-with-c.aspx
                    //Correct way for interfacing with Firewall.  More tips at site.
                    Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
                    INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);

                    IsUp = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;
                }
                else
                {
                    DialogResult dr;
                    dr = MessageBox.Show(null, "I noticed Windows Firewall service (MpsSvc) is disabled or not running. \n\n" +
                                    "Even with Windows Firewall functionality disabled, I recommend leaving the service running. \n\n" +
                                    "It has almost no overhead with Firewalls disabled and certain Windows " +
                                    "operations/functions do not work correctly with it disabled.\n\nDisable this message?", "Windows Firewall Notice", MessageBoxButtons.YesNo);

                    if (dr == DialogResult.Yes)
                    {
                        Properties.Settings.Default["IGNORE_WINDOWS_FIREWALL"] = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default["IGNORE_WINDOWS_FIREWALL"] = false;
                        Properties.Settings.Default.Save();
                    }

                    IsUp = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking Firewall Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return IsUp;
        }

        public static void funcDisableHibernate()
        {
            Process.Start("powercfg", "/h off");
        }

        public static void funcEnableHibernate()
        {
            Process.Start("powercfg", "/h on");
        }

        public static string funcCheckHiberFileSize()
        {
            string temp = "";
            double size = 0.0;

            try
            {
                if (System.IO.File.Exists("C:\\hiberfil.sys"))
                {
                    temp = convertToGBFromBytes(new System.IO.FileInfo("C:\\hiberfil.sys").Length.ToString());
                }

                temp = size.ToString() + " GB";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error Occured.\n\nLocation: Checking Hibernate Status.\n\nException: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }

            return temp;
        }

        public static void funcEnableCoreParking()
        {
            try
            {
                RegistryKey corekey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Control\\Power\\PowerSettings\\54533251-82be-4824-96c1-47b60b740d00\\0cc5b647-c1df-4637-891a-dec35c318583", true);

                if (corekey != null)
                {
                    corekey.SetValue("ValueMax", 64, RegistryValueKind.DWord);
                }

                corekey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enable Core Parking Exception: " + ex.Message + "\n\nStack: " + ex.StackTrace);
            }
        }

        public static void funcEnableLUAC()
        {
            RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            if (uacKey != null)
            {
                uacKey.SetValue("EnableLUA", 1, RegistryValueKind.DWord);
            }

            uacKey.Close();
        }

        public static void funcDisableLUAC()
        {
            RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            if (uacKey != null)
            {
                uacKey.SetValue("EnableLUA", 0, RegistryValueKind.DWord);
            }

            uacKey.Close();
        }

        public static void funcEnableRUAC()
        {
            RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            if (uacKey != null)
            {
                uacKey.SetValue("LocalAccountTokenFilterPolicy", 0, RegistryValueKind.DWord);
            }

            uacKey.Close();
        }

        public static void funcDisableRUAC()
        {
            RegistryKey uacKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);

            if (uacKey != null)
            {
                uacKey.SetValue("LocalAccountTokenFilterPolicy", 1, RegistryValueKind.DWord);
            }

            uacKey.Close();
        }

        #region Windows-Tools
        //Control Panel
        public static void runCP()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\control.exe";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("control.exe");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("control.exe");
                }
            }
            else { MessageBox.Show("Control.exe was not found in System32", "File Not Found"); }

        }

        //Execute System File Check / SFC /scannow
        public static void runSFC()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k sfc /scannow";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
                //dd
                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.WorkingDirectory = @"C:\";
                procStartInfo.Arguments = @"/k sfc /scannow";

                Process proc = new Process();
                proc.StartInfo = procStartInfo;

                proc.Start();
            }

            //Process.Start("sfc", "/scannow");
        }

        //Open SFC Log
        public static void runSFCLog()
        {
            if (File.Exists("C:\\Windows\\logs\\cbs\\cbs.log"))
            {
                try
                {
                    Process.Start("C:\\Windows\\logs\\cbs\\cbs.log");
                }
                catch
                {
                    MessageBox.Show(null, "CBS Log not found.", "CBS log not found.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("SFC (cbs.log) was not found in C:\\Windows\\Logs");
            }
        }

        //Windows Update
        public static void runWU()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.WindowsUpdate");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.WindowsUpdate");
            }

        }

        //Windows Firewall(Basic)
        public static void runFW()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.WindowsFirewall");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.WindowsFirewall");
            }

        }

        //Windows Defender
        public static void runWD()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.WindowsDefender");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.WindowsDefender");
            }

        }

        //User Accounts
        public static void runUA()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.UserAccounts");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.UserAccounts");
            }

        }

        //Admin Tools
        public static void runAT()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.AdministrativeTools");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.AdministrativeTools");
            }
        }

        //Device Manager
        public static void runDM()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.DeviceManager");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.DeviceManager");
            }
        }

        //Devices and Printers
        public static void runDP()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.DevicesAndPrinters");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.DevicesAndPrinters");
            }
        }

        //Display
        public static void runDI()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.Display");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.Display");
            }
        }

        //Network Sharing
        public static void runNS()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.NetworkAndSharingCenter");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.NetworkAndSharingCenter");
            }
        }

        //Advanced Firewall
        public static void runAF()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("WF.msc");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("WF.msc");
            }
        }

        //Computer Management
        public static void runCM()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("compmgmt.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("compmgmt.msc", "/s");
            }
        }

        //Event Viewer
        public static void runEV()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("eventvwr.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("eventvwr.msc", "/s");
            }
        }

        //Local Security Policies
        public static void runLSP()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("secpol.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("secpol.msc", "/s");
            }
        }

        //Services
        public static void runSV()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("services.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("services.msc", "/s");
            }

        }

        //Task Scheduler
        public static void runTS()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("taskschd.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("taskschd.msc", "/s");
            }
        }

        //Windows Memory Diagnostics
        public static void runWMD()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("MdSched.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("MdSched.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
        }

        //Sound
        public static void runSD()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.Sound");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.Sound");
            }

        }

        //Power
        public static void runPWR()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.Power");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.Power");
            }

        }

        //Mouse
        public static void runMOUSE()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.Mouse");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.Mouse");
            }

        }

        //Keyboard
        public static void runKEYBOARD()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("control", "/name Microsoft.Keyboard");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("control", "/name Microsoft.Keyboard");
            }
        }

        //DxDiagnostics
        public static void runDxDiag()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("dxdiag.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("dxdiag.exe");
            }
        }

        //DxControlpanel
        public static void runDxCpl()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\dxcpl.exe";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("dxcpl.exe");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("dxcpl.exe");
                }
            }
            else { MessageBox.Show("Dxcpl.exe not found.", "File Not Found"); }

        }

        //Share Folder Management
        public static void runSHM()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\fsmgmt.msc";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("fsmgmt.msc");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("fsmgmt.msc");
                }
            }
            else { MessageBox.Show("fsmgmt.msc not found.", "File Not Found"); }

        }

        //Local Policy Editor
        public static void runLP()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\gpedit.msc";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("gpedit.msc");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("gpedit.msc");
                }
            }
            else { MessageBox.Show("Gpedit.msc not found.", "File Not Found"); }

        }

        //Results Set Of Policies Applied
        public static void runRSOP()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("rsop.msc");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("rsop.msc");
            }

        }

        //Manage Local Users and Groups
        public static void runMLUGS()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("lusrmgr.msc");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("lusrmgr.msc");

                procStartInfo.UseShellExecute = true;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
        }

        //Executes MSINFO32
        public static void runMSINFO32()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("msinfo32.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("msinfo32.exe");
            }
        }

        //Run ODMC Administration Console
        public static void runODBCA()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("odbcad32.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("odbcad32.exe");
            }

        }

        //Launches the Windows Performance Monitor.
        public static void runPerfMon()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("PerfMon.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("PerfMon.exe");
            }

        }

        //Launches the Windows Resource Monitor.
        public static void runResMon()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("ResMon.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("ResMon.exe");
            }

        }

        //Launches the Windows Print Manager
        public static void runPrintMan()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("printmanagement.msc");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("printmanagement.msc");
            }

        }

        //Local Security Editor
        public static void runLocSec()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("secpol.msc", "/s");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("secpol.msc", "/s");
            }
        }

        //Launches Windows Management Instrumentation.... Management
        public static void runWMIMgmt()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("Wmimgmt.msc");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("Wmimgmt.msc");
            }

        }

        //Launches Printer Migration Window
        public static void runPrinterMigration()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("PrintBrmUi.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("PrintBrmUi.exe");
            }

        }

        //Launches the Windows Optional Features menu for enabling and disabling.
        public static void runWindowsFeatures()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("OptionalFeatures.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("OptionalFeatures.exe");
            }

        }

        //Launches the Remote Desktop Connection
        public static void runMSTSC()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("Mstsc.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("Mstsc.exe");
            }

        }

        //Launches the Windows Remote Assistance Program
        public static void runWRMA()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("msra.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("msra.exe");
            }

        }

        //Launches MSCONFIG
        public static void runMSCONFIG()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);
                ProcessStartInfo procStartInfo = new ProcessStartInfo("msconfig.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("msconfig.exe");

                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
        }

        public static void runWMSRT()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("MRT.exe");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("MRT.exe");
            }

        }
        #endregion

        //Goto STEAM Page
        public static void gotoSTEAMPAGE()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "http://steamcommunity.com/sharedfiles/filedetails/?id=202683704");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "http://steamcommunity.com/sharedfiles/filedetails/?id=202683704");
            }

        }

        //Goto FB Page
        public static void gotoFBPAGE()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "https://www.facebook.com/pages/WOLF-Windows-Operations-and-Library-of-Functions/1391598027753006");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "https://www.facebook.com/pages/WOLF-Windows-Operations-and-Library-of-Functions/1391598027753006");
            }
        }

        //Goto Website Page
        public static void gotoBYTEMEDEV()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "http://www.bytemedev.com/");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "http://www.bytemedev.com/");
            }
        }

        public static void gotoBYTEMEDEVWOLFCURRENT()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "https://houseofcat.io/Home/WOLF");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "https://houseofcat.io/Home/WOLF" );
            }
        }

        public static void gotoNFRT()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "http://www.microsoft.com/en-us/download/details.aspx?id=30135");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "http://www.microsoft.com/en-us/download/details.aspx?id=30135");
            }
        }

        public static void gotoWMIDU()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "http://www.microsoft.com/en-us/download/details.aspx?id=7684");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "http://www.microsoft.com/en-us/download/details.aspx?id=7684");
            }
        }

        public static void runNA()
        {
            string FilePath = Environment.GetEnvironmentVariable("systemroot");
            FilePath += FilePath + "\\system32\\ncpa.cpl";

            if (File.Exists(FilePath))
            {
                if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                {
                    IntPtr ptr = new IntPtr();
                    NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                    Process.Start("ncpa.cpl");

                    NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
                }
                else
                {
                    Process.Start("ncpa.cpl");
                }
            }
            else { MessageBox.Show("ncpa.cpl was missing from System32.", "File Not Found"); }

        }

        public static string convertToGBFromBytes(string input)
        {
            string result = "";
            double dblTemp = 0.0;

            dblTemp = Convert.ToDouble(input);
            dblTemp = (dblTemp / 1073741824);
            dblTemp = Math.Round(dblTemp, 2);

            result = dblTemp.ToString("0.##") + " GB";

            return result;
        }

        public static string convertToGBFromKB(object input)
        {
            var strMemoryValue = input as string;
            var result = "";

            if (strMemoryValue != null)
            {
                var dblTemp = Math.Round((Convert.ToDouble(strMemoryValue) / 1073742), 2);

                result = dblTemp.ToString("0.##") + " GB";
            }
            else { result = "No Data (xNull)."; }

            return result;
        }

        public static double convertGBtoDouble(string input)
        {
            double temp = 0.0;

            input = input.Replace(" GB", "");

            temp = Convert.ToDouble(input);

            return temp;
        }

        public static string getPercentage(double numerator, double denominator)
        {
            string temp = "";
            double perc = 0.0;

            perc = numerator / denominator;
            perc *= 100;

            temp = perc.ToString("0.##");

            return temp;
        }

        public static string getProcessOwner(int processID)
        {
            string[] temp = new string[2];
            string owner = "Unkown";

            int rVal = 0;

            string query = "Select * From Win32_Process Where ProcessID = " + processID;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject item in processList)
            {
                rVal = Convert.ToInt32(item.InvokeMethod("GetOwner", temp));

                if (rVal == 0)
                {
                    owner = temp[1] + "\\" + temp[0];
                }
            }

            return owner;
        }

        public static void gotoOCN()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            {
                IntPtr ptr = new IntPtr();
                NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

                Process.Start("explorer.exe", "http://www.overclock.net/t/1445995");

                NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);
            }
            else
            {
                Process.Start("explorer.exe", "http://www.overclock.net/t/1445995");
            }
        }

        //Converts unsplit MAC into dash (-) split string.
        //Ex.) FFFFFFFF -> FF-FF-FF-FF
        public static string funcConvertMAC1(string input, int length)
        {
            string temp = "";

            for (int i = 0; i < length && i < int.MaxValue; i++)
            {
                temp += input.ElementAt(i);

                if (((i % 2) == 1) && (i != 11))
                {
                    temp += "-";
                }
            }

            return temp;
        }

        //Converts unsplit MAC into colon (:) split string.
        //Ex.) FFFFFFFF -> FF:FF:FF:FF
        public static string funcConvertMAC2(string input, int length)
        {
            string temp = "";

            for (int i = 0; i < length && i < int.MaxValue; i++)
            {
                temp += input.ElementAt(i);

                if (((i % 2) == 1) && (i != 11))
                {
                    temp += ":";
                }
            }

            return temp;
        }

        public static string funcUndoConvertMAC(string input)
        {
            input = input.Replace("-", "");
            input = input.Replace(":", "");

            return input;
        }

        //Source: http://forums.mydigitallife.info/threads/43788-C-C-VB-NET-Read-MSDM-license-information-from-BIOS-ACPI-tables
        //User Alphawaves: Post #4.
        private static bool funcCheckMSDM(ref byte[] buffer)
        {
            bool KeyExists = false;

            try
            {
                uint firmwareTableProvider = 'A' << 24 | 'C' << 16 | 'P' << 8 | 'I';
                uint bSize = NativeMethods.EnumSystemFirmwareTables(firmwareTableProvider, IntPtr.Zero, 0);

                IntPtr FirmwareTableBuffer = Marshal.AllocHGlobal((int)bSize);

                buffer = new byte[bSize];
                NativeMethods.EnumSystemFirmwareTables(firmwareTableProvider, FirmwareTableBuffer, bSize);

                Marshal.Copy(FirmwareTableBuffer, buffer, 0, buffer.Length);
                Marshal.FreeHGlobal(FirmwareTableBuffer);

                string str = Encoding.GetEncoding(0x4e4).GetString(buffer);
                List<string> array = new List<string>();

                for (int i = 0; i <= str.Length - 1; i += 4)
                {
                    array.Add(str.Substring(i, 4));
                }

                if (array.Contains("MSDM"))
                {
                    uint firmwareTableID = BitConverter.ToUInt32(buffer, array.IndexOf("MSDM") * 4);
                    bSize = NativeMethods.GetSystemFirmwareTable(firmwareTableProvider, firmwareTableID, IntPtr.Zero, 0);

                    buffer = new byte[bSize];

                    FirmwareTableBuffer = Marshal.AllocHGlobal((int)bSize);
                    NativeMethods.GetSystemFirmwareTable(firmwareTableProvider, firmwareTableID, FirmwareTableBuffer, bSize);

                    Marshal.Copy(FirmwareTableBuffer, buffer, 0, buffer.Length);
                    Marshal.FreeHGlobal(FirmwareTableBuffer);

                    KeyExists = true;
                }
                else
                {
                    KeyExists = false;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return KeyExists;
        }

        public static string[] funcGetEmbeddedProductKey()
        {
            byte[] buffer = null;
            string[] ProductKeyInfo = new string[2];

            if (funcCheckMSDM(ref buffer))
            {
                try
                {
                    Encoding encoding = Encoding.GetEncoding(0x4e4);

                    string oemid = encoding.GetString(buffer, 10, 6);
                    string dmkey = encoding.GetString(buffer, 56, 29);

                    ProductKeyInfo[0] = dmkey;
                    ProductKeyInfo[1] = oemid;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                ProductKeyInfo[0] = "No embedded key found.";
                ProductKeyInfo[1] = "No OEM established.";
            }

            return ProductKeyInfo;
        }

        public static string funcDecodeProductKey(byte[] digitalProductId)
        {
            //Proper intiliaztion technique.
            string DecodedKey = "";

            if (digitalProductId != null)
            {
                try
                {
                    // Offset of first byte of encoded product key in 
                    //  'DigitalProductIdxxx" REG_BINARY value. Offset = 34H.
                    const int keyStartIndex = 52;

                    // Offset of last byte of encoded product key in 
                    //  'DigitalProductIdxxx" REG_BINARY value. Offset = 43H.
                    const int keyEndIndex = keyStartIndex + 15;

                    //Constant for test for N.
                    const int numLetters = 24;

                    // Possible alpha-numeric characters in product key.
                    char[] digits = new char[]
                    {
                    'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R',
                    'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9',
                    };
                    //Check for the letter N.
                    int containsN = (digitalProductId[keyStartIndex + 14] >> 3) & 1;
                    digitalProductId[keyStartIndex + 14] = (byte)((digitalProductId[keyStartIndex + 14] & 0xF7) | ((containsN & 2) << 2));
                    // Length of decoded product key
                    const int decodeLength = 29;
                    // Length of decoded product key in byte-form.
                    // Each byte represents 2 chars.
                    const int decodeStringLength = 15;
                    // Array of containing the decoded product key.
                    char[] decodedChars = new char[decodeLength];

                    // Extract byte 52 to 67 inclusive.
                    List<byte> hexPid = new List<byte>();

                    for (int i = keyStartIndex; i <= keyEndIndex; i++)
                    {
                        hexPid.Add(digitalProductId[i]);
                    }
                    for (int i = decodeLength - 1; i >= 0; i--)
                    {
                        // Every sixth char is a separator.
                        if ((i + 1) % 6 == 0)
                        {
                            decodedChars[i] = '-';
                        }
                        else
                        {
                            // Do the actual decoding.
                            int digitMapIndex = 0;
                            for (int j = decodeStringLength - 1; j >= 0; j--)
                            {
                                int byteValue = (digitMapIndex << 8) | (byte)hexPid[j];
                                hexPid[j] = (byte)(byteValue / 24);
                                digitMapIndex = byteValue % 24;
                                decodedChars[i] = digits[digitMapIndex];
                            }
                        }
                    }

                    if (containsN != 0)
                    {
                        int firstLetterIndex = 0;

                        for (int index = 0; index < numLetters; index++)
                        {
                            if (decodedChars[0] != digits[index]) continue;
                            firstLetterIndex = index;
                            break;
                        }

                        string keyWithN = new string(decodedChars);

                        keyWithN = keyWithN.Replace("-", string.Empty).Remove(0, 1);
                        keyWithN = keyWithN.Substring(0, firstLetterIndex) + "N" +
                                        keyWithN.Remove(0, firstLetterIndex);
                        keyWithN = keyWithN.Substring(0, 5) + "-" + keyWithN.Substring(5, 5) + "-" +
                                        keyWithN.Substring(10, 5) + "-" + keyWithN.Substring(15, 5) + "-" +
                                        keyWithN.Substring(20, 5);

                        DecodedKey = keyWithN;
                    }
                    else
                    {
                        DecodedKey = new string(decodedChars);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            return DecodedKey;
        }

        public static byte[] funcGetProductKey(string vendor, string RegLocation)
        {
            byte[] digitalProductId = null;
            RegistryKey registry = null;

            switch (vendor)
            {
                // Open the Windows subkey read-only. TODO: For other products, vendor maybe different.
                case ("Microsoft"):
                    {

                        if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
                        {
                            registry = (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                        RegistryView.Registry64)).OpenSubKey(RegLocation, false);
                        }
                        else
                        {
                            registry = Registry.LocalMachine.OpenSubKey(RegLocation, false);
                        }

                        break;
                    }
            }

            if (registry != null)
            {
                digitalProductId = registry.GetValue("DigitalProductId") as byte[];

                registry.Close();
            }

            return digitalProductId;
        }

        public static byte[] funcGetRemoteProductKey(string vendor, string MachineName, string RegLocation)
        {
            byte[] digitalProductId = null;
            RegistryKey registry = null;

            switch (vendor)
            {
                // Open the Windows subkey read-only. TODO: For other products, vendor maybe different.
                case ("Microsoft"):
                    {
                        registry = (RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, MachineName,
                                    RegistryView.Registry64)).OpenSubKey(RegLocation, false);

                        break;
                    }
            }

            if (registry != null)
            {
                digitalProductId = registry.GetValue("DigitalProductId") as byte[];

                registry.Close();
            }

            return digitalProductId;
        }

        #region Windows Telemetry
        public static void stopService( string ServiceName, int Timeout)
        {
            ServiceController service = null;
            try { service = new ServiceController(ServiceName); }
            catch { service = null; }

            if (service != null)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(Timeout);

                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    }
                }
                catch (Exception exc)
                { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }
                finally { service.Dispose(); }
            }
            else { MessageBox.Show("Service: " + ServiceName + "\n\nService was not found, unable to stop.", "Service Not Found"); }
        }

        public static void startService( string ServiceName, int Timeout)
        {
            ServiceController service = null;
            try { service = new ServiceController(ServiceName); }
            catch { service = null; }

            if (service != null)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(Timeout);

                    if ((service.Status == ServiceControllerStatus.Stopped) || (service.Status == ServiceControllerStatus.StopPending))
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                }
                catch
                {
                    MessageBox.Show("This service " + ServiceName + " is unable to be started.\n\n" +
                                    "To remedy this on Windows 10, enable Automatic startup and reboot.", "Unable To Start Service");
                }
                finally { service.Dispose(); }
            }
            else { MessageBox.Show("Service: " + ServiceName + "\n\nService was not found, unable to start.", "Service Not Found"); }
        }

        public static bool isServiceRunning( string ServiceName )
        {
            ServiceController service = new ServiceController(ServiceName);
            bool IsRunning = false;

            if (service != null)
            {
                try
                {
                    if ((service.Status == ServiceControllerStatus.Running) || (service.Status == ServiceControllerStatus.StartPending))
                    { IsRunning = true; }
                }
                catch (Exception exc)
                { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); IsRunning = false; }
                finally { service.Dispose(); }
            }
            else
            { MessageBox.Show("Service: " + ServiceName + "\n\nService was not found, unable to stop.", "Service Not Found"); }

            return IsRunning;
        }

        public static void disableService( string ServiceName )
        {
            RegistryKey Key = null;

            try
            { Key = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\" + ServiceName, true); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); Key = null; }

            if (Key != null)
            { Key.SetValue("Start", 4); }
            else
            { MessageBox.Show("Service: " + ServiceName + "\n\nService registry key was not found.", "RegKey Not Found"); }
        }

        public static void enableService( string ServiceName )
        {
            RegistryKey Key = null;

            try
            { Key = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\" + ServiceName, true); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }

            if (Key != null)
            { Key.SetValue("Start", 2); }
            else
            { MessageBox.Show("Service: " + ServiceName + "\n\nService registry key was not found.", "RegKey Not Found"); }
        }

        public static void backupHostsFile()
        {
            string HostsPath = Environment.GetEnvironmentVariable("systemroot");
            HostsPath += "\\System32\\drivers\\etc\\hosts";

            if (File.Exists(HostsPath))
            {
                try { File.Copy(HostsPath, HostsPath + ".Backup", true); }
                catch (Exception exc) { MessageBox.Show("Backing up Hosts failed.\n\nException: " + exc.Message, "File Operation Error"); }
            }
            else { MessageBox.Show("Hosts file not found.", "Error"); }
        }

        public static void restoreHostsFile()
        {
            string HostsPath = Environment.GetEnvironmentVariable("systemroot");
            HostsPath += "\\System32\\drivers\\etc\\hosts";

            if (File.Exists(HostsPath + ".Backup"))
            {
                try { File.Copy(HostsPath + ".Backup", HostsPath, true); }
                catch (Exception exc) { MessageBox.Show("Restoring Hosts Failed.\n\nException: " + exc.Message, "File Operation Error"); }
            }
            else { MessageBox.Show("Hosts backup file not found.", "Error"); }
        }

        public static void appendHostsFile()
        {
            string HostsPath = Environment.GetEnvironmentVariable("systemroot");
            HostsPath += "\\System32\\drivers\\etc\\hosts";

            if (File.Exists(HostsPath))
            {
                try
                {
                    StreamWriter sw = File.AppendText(HostsPath);

                    if (sw != null)
                    {
                        sw.WriteLine("#############################################");
                        sw.WriteLine("# WOLF Windows Telemetry IP Block/Redirects #");
                        sw.WriteLine("#############################################");

                        foreach (string ip in IPBlocks)
                        {
                            sw.WriteLine(ip);
                        }

                        sw.WriteLine("#############################################");
                        sw.Close();
                        sw.Dispose();
                    }
                }
                catch (Exception exc)
                { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }
            }
            else { MessageBox.Show("Hosts file not found.", "Error"); }
        }

        public static bool isHostsModified()
        {
            bool IsModified = false;

            string HostsPath = Environment.GetEnvironmentVariable("systemroot");
            HostsPath += "\\System32\\drivers\\etc\\hosts";

            if (File.Exists(HostsPath))
            {
                try
                {
                    string[] Lines = File.ReadAllLines(HostsPath);

                    for (int i = 0; i < Lines.Length && i < int.MaxValue; i++)
                    {
                        if (Lines[i] == "# WOLF Windows Telemetry IP Block/Redirects #")
                        {
                            IsModified = true; break;
                        }
                    }
                }
                catch (Exception exc)
                { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }
            }

            return IsModified;
        }

        public static void deleteAutoTrackerLog()
        {
            string FilePath = "C:\\ProgramData\\Microsoft\\Diagnosis\\ETLLogs\\AutoLogger\\AutoLogger-Diagtrack-Listener.etl";

            if (File.Exists(FilePath))
            {
                try { File.Delete(FilePath); }
                catch (Exception exc) { MessageBox.Show("Unable to delete file.\n\nException: " + exc.Message, "File Operation Failed"); }
            }
            else { MessageBox.Show("AutoTracker file was not found.", "File Not Found"); }
        }

        public static bool isTrackerLogGone()
        {
            string FilePath = "C:\\ProgramData\\Microsoft\\Diagnosis\\ETLLogs\\AutoLogger\\AutoLogger-Diagtrack-Listener.etl";
            bool IsTrackerLogGone = true;

            if (File.Exists(FilePath))
            { IsTrackerLogGone = false; }

            return IsTrackerLogGone;
        }

        public static void disableDataCollection()
        {
            RegistryKey Key = null;

            try
            { Key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", true); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); Key = null; }

            if (Key != null)
            { Key.CreateSubKey("Allow Telemetry", true); Key.SetValue("Allow Telemetry", 0, RegistryValueKind.DWord); }
            else
            { MessageBox.Show("Unable to create and modify Allow Telemetry", "Registry Key Error"); }
        }

        public static void enableDataCollection()
        {
            RegistryKey Key = null;

            try
            { Key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", true); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); Key = null; }

            if (Key != null)
            { Key.CreateSubKey("Allow Telemetry", true); Key.SetValue("Allow Telemetry", 1, RegistryValueKind.DWord); }
            else
            { MessageBox.Show("Unable to create and modify Allow Telemetry", "Registry Key Error"); }
        }

        public static bool isDataCollectionDisabled()
        {
            bool DataCollectionDisabled = false;

            string Value = "";
            RegistryKey Key = null;

            try
            { Key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", false); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }

            if (Key != null)
            {

                if( Key.GetSubKeyNames().Any( x => x.ToString() == "Allow Telemetry" ) )
                {
                    try
                    { Value = Key.GetValue( "Allow Telemetry" ).ToString(); }
                    catch { Value = null; }
                }

                if (Value != null)
                {
                    switch (Value)
                    {
                        case "0": DataCollectionDisabled = true; break;
                        case "1": DataCollectionDisabled = false; break;
                        default: DataCollectionDisabled = false; break;
                    }
                }
            }

            return DataCollectionDisabled;
        }

        public static string getServiceStartupStatus( string ServiceName )
        {
            RegistryKey wdKey = null;
            string Value = "";
            string Status = "";

            try
            { wdKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\" + ServiceName, false); }
            catch (Exception exc)
            { MessageBox.Show("Exception: " + exc.Message + "\n\nStack: " + exc.StackTrace); }

            if (wdKey != null)
            { Value = wdKey.GetValue("Start").ToString(); }

            switch (Value)
            {
                case "0": Status = "Boot"; break;
                case "1": Status = "System"; break;
                case "2": Status = "Automatic"; break;
                case "3": Status = "Manual"; break;
                case "4": Status = "Disabled"; break;
                case "5": Status = "Auto (Delayed)"; break;
                default: Status = "Unknown"; break;
            }

            return Status;
        }

        public static void disableSensorPermissionCU()
        {
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Sensor\\" +
                             "Permissions\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, true); }
            catch
            { Key = null; }

            if (Key != null)
            {
                Key.CreateSubKey("SensorPermissionState", true);
                Key.SetValue("SensorPermissionState", 0, RegistryValueKind.DWord);
            }
            else
            { MessageBox.Show("Unable to create and modify SensorPermissionState", "Registry Key Error"); }
        }

        public static void enableSensorPermissionCU()
        {
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Sensor\\" +
                             "Permissions\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, true); }
            catch
            { Key = null; }

            if (Key != null)
            {
                Key.CreateSubKey("SensorPermissionState", true);
                Key.SetValue("SensorPermissionState", 1, RegistryValueKind.DWord);
            }
            else
            { MessageBox.Show("Unable to create and modify SensorPermissionState", "Registry Key Error"); }
        }

        public static void disableGlobalSensorCU()
        {
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\DeviceAccess\\" +
                             "Global\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, true); }
            catch
            { Key = null; }

            if (Key != null)
            {
                Key.CreateSubKey("Value", true);
                Key.SetValue("Value", "Deny", RegistryValueKind.String);
            }
            else
            { MessageBox.Show("Unable to create and modify GlobalSensor", "Registry Key Error"); }
        }

        public static void enableGlobalSensorCU()
        {
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\DeviceAccess\\" +
                             "Global\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, true); }
            catch
            { Key = null; }

            if (Key != null)
            {
                Key.CreateSubKey("Value", true);
                Key.SetValue("Value", "Allow", RegistryValueKind.String);
            }
            else
            { MessageBox.Show("Unable to create and modify GlobalSensor", "Registry Key Error"); }
        }

        public static bool isGlobalDisabled()
        {
            bool GlobalSensorUsageDisabled = false;

            string Value = "";
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\DeviceAccess\\" +
                             "Global\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, false); }
            catch
            { Key = null; }

            if (Key != null)
            {
                try { Value = Key.GetValue("Value").ToString(); }
                catch { Value = ""; }
            }

            switch (Value)
            {
                case "Allow": GlobalSensorUsageDisabled = false; break;
                case "Deny": GlobalSensorUsageDisabled = true; break;
                default: GlobalSensorUsageDisabled = false; break;
            }

            return GlobalSensorUsageDisabled;
        }

        public static bool isSensorDisabled()
        {
            bool SensorUsageDisabled = false;

            string Value = "";
            RegistryKey Key = null;
            string KeyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Sensor\\" +
                             "Permissions\\{BFA794E4-F964-4FDB-90F6-51056BFE4B44}";

            try
            { Key = Registry.CurrentUser.OpenSubKey(KeyPath, false); }
            catch
            { Key = null; }

            if (Key != null)
            {
                try { Value = Key.GetValue("SensorPermissionState").ToString(); }
                catch { Value = ""; }
            }

            switch (Value)
            {
                case "0": SensorUsageDisabled = true; break;
                case "1": SensorUsageDisabled = false; break;
                default: SensorUsageDisabled = false; break;
            }

            return SensorUsageDisabled;
        }

        public static void SetOwnershipOfHosts()
        {
            WindowsIdentity SecObj = WindowsIdentity.GetCurrent();

            string HostsPath = Environment.GetEnvironmentVariable("systemroot");

            HostsPath += "\\System32\\drivers\\etc\\hosts";

            if (File.Exists(HostsPath))
            {
                FileInfo FIFO = new FileInfo(HostsPath);
                FileSecurity FISEC = FIFO.GetAccessControl();

                if (FISEC != null)
                {
                    FISEC.SetOwner(SecObj.User);
                    FIFO.SetAccessControl(FISEC);

                    FISEC = FIFO.GetAccessControl();

                    IdentityReference NewOwner = FISEC.GetOwner(typeof(NTAccount));
                }
            }
        }

        public static bool isOwnerOfHosts()
        {
            string CurrentUser = (Environment.UserDomainName + "\\" + Environment.UserName);
            string HostsPath = Environment.GetEnvironmentVariable("systemroot");
            HostsPath += "\\System32\\drivers\\etc\\hosts";

            bool HasOwnership = false;

            if (File.Exists(HostsPath))
            {
                FileInfo FIFO = new FileInfo(HostsPath);
                FileSecurity FISEC = FIFO.GetAccessControl();

                if (FISEC != null)
                {
                    FISEC = FIFO.GetAccessControl();

                    IdentityReference NewOwner = FISEC.GetOwner(typeof(NTAccount));

                    if (NewOwner.ToString() == CurrentUser)
                    {
                        HasOwnership = true;
                    }
                }
            }   

            return HasOwnership;

        }

        public static void SetOwnershipOfTrackerLog()
        {
            WindowsIdentity SecObj = WindowsIdentity.GetCurrent();
            string FilePath = "C:\\ProgramData\\Microsoft\\Diagnosis\\ETLLogs\\AutoLogger\\AutoLogger-Diagtrack-Listener.etl";

            if (File.Exists(FilePath))
            {
                FileInfo FIFO = new FileInfo(FilePath);
                FileSecurity FISEC = FIFO.GetAccessControl();

                if (FISEC != null)
                {
                    FISEC.SetOwner(SecObj.User);
                    FIFO.SetAccessControl(FISEC);

                    FISEC = FIFO.GetAccessControl();

                    IdentityReference NewOwner = FISEC.GetOwner(typeof(NTAccount));
                }
            }
        }

        public static bool isOwnerOfLogs()
        {
            string CurrentUser = (Environment.UserDomainName + "\\" + Environment.UserName);
            string FilePath = "C:\\ProgramData\\Microsoft\\Diagnosis\\ETLLogs\\AutoLogger\\AutoLogger-Diagtrack-Listener.etl";

            bool HasOwnership = false;

            if (File.Exists(FilePath))
            {
                FileInfo FIFO = new FileInfo(FilePath);
                FileSecurity FISEC = FIFO.GetAccessControl();

                if (FISEC != null)
                {
                    FISEC = FIFO.GetAccessControl();

                    IdentityReference NewOwner = FISEC.GetOwner(typeof(NTAccount));

                    if (NewOwner.ToString() == CurrentUser)
                    {
                        HasOwnership = true;
                    }
                }
            }

            return HasOwnership;

        }
        #endregion

        #region Library Variables
        private static string[] IPBlocks = {
            "127.0.0.1  localhost",
            "127.0.0.1   localhost.localdomain",
            "255.255.255.255 broadcasthost",
            "::1     localhost",
            "127.0.0.1   local",
            "0.0.0.0 vortex.data.microsoft.com",
            "0.0.0.0 vortex-win.data.microsoft.com",
            "0.0.0.0 telecommand.telemetry.microsoft.com",
            "0.0.0.0 telecommand.telemetry.microsoft.com.nsatc.net",
            "0.0.0.0 oca.telemetry.microsoft.com",
            "0.0.0.0 oca.telemetry.microsoft.com.nsatc.net",
            "0.0.0.0 sqm.telemetry.microsoft.com",
            "0.0.0.0 sqm.telemetry.microsoft.com.nsatc.net",
            "0.0.0.0 watson.telemetry.microsoft.com",
            "0.0.0.0 watson.telemetry.microsoft.com.nsatc.net",
            "0.0.0.0 redir.metaservices.microsoft.com",
            "0.0.0.0 choice.microsoft.com",
            "0.0.0.0 choice.microsoft.com.nsatc.net",
            "0.0.0.0 df.telemetry.microsoft.com",
            "0.0.0.0 reports.wes.df.telemetry.microsoft.com",
            "0.0.0.0 wes.df.telemetry.microsoft.com",
            "0.0.0.0 services.wes.df.telemetry.microsoft.com",
            "0.0.0.0 sqm.df.telemetry.microsoft.com",
            "0.0.0.0 telemetry.microsoft.com",
            "0.0.0.0 watson.ppe.telemetry.microsoft.com",
            "0.0.0.0 telemetry.appex.bing.net",
            "0.0.0.0 telemetry.urs.microsoft.com",
            "0.0.0.0 telemetry.appex.bing.net:443",
            "0.0.0.0 settings-sandbox.data.microsoft.com",
            "0.0.0.0 vortex-sandbox.data.microsoft.com",
            "0.0.0.0 survey.watson.microsoft.com",
            "0.0.0.0 watson.live.com",
            "0.0.0.0 watson.microsoft.com",
            "0.0.0.0 statsfe2.ws.microsoft.com",
            "0.0.0.0 corpext.msitadfs.glbdns2.microsoft.com",
            "0.0.0.0 compatexchange.cloudapp.net",
            "0.0.0.0 cs1.wpc.v0cdn.net",
            "0.0.0.0 a-0001.a-msedge.net",
            "0.0.0.0 statsfe2.update.microsoft.com.akadns.net",
            "0.0.0.0 sls.update.microsoft.com.akadns.net",
            "0.0.0.0 fe2.update.microsoft.com.akadns.net",
            "0.0.0.0 65.55.108.23",
            "0.0.0.0 65.39.117.230",
            "0.0.0.0 23.218.212.69",
            "0.0.0.0 134.170.30.202",
            "0.0.0.0 137.116.81.24",
            "0.0.0.0 diagnostics.support.microsoft.com",
            "0.0.0.0 corp.sts.microsoft.com",
            "0.0.0.0 statsfe1.ws.microsoft.com",
            "0.0.0.0 pre.footprintpredict.com",
            "0.0.0.0 204.79.197.200",
            "0.0.0.0 23.218.212.69",
            "0.0.0.0 i1.services.social.microsoft.com",
            "0.0.0.0 i1.services.social.microsoft.com.nsatc.net",
            "0.0.0.0 feedback.windows.com",
            "0.0.0.0 feedback.microsoft-hohm.com",
            "0.0.0.0 feedback.search.microsoft.com" };
        #endregion

        #region Win32Processes
        //Found Here: http://stackoverflow.com/questions/860656/using-c-how-does-one-figure-out-what-process-locked-a-file
        //Git: https://gist.github.com/i-e-b/2290426
        public class Win32Processes
        {
            /// <summary>
            /// Return a list of processes that hold on the given file.
            /// </summary>
            public static List<Process> GetProcessesLockingFile(string filePath)
            {
                var procs = new List<Process>();

                var processListSnapshot = Process.GetProcesses();
                foreach (var process in processListSnapshot)
                {
                    if (process.Id <= 4) { continue; } // system processes
                    var files = GetFilesLockedBy(process);
                    if (files.Contains(filePath)) procs.Add(process);
                }
                return procs;
            }

            /// <summary>
            /// Return a list of file locks held by the process.
            /// </summary>
            public static List<string> GetFilesLockedBy(Process process)
            {
                var outp = new List<string>();

                ThreadStart ts = delegate
                {
                    try
                    {
                        outp = UnsafeGetFilesLockedBy(process);
                    }
                    catch { Ignore(); }
                };

                try
                {
                    var t = new Thread(ts);
                    t.IsBackground = true;
                    t.Start();
                    if (!t.Join(250))
                    {
                        try
                        {
                            t.Interrupt();
                            t.Abort();
                        }
                        catch { Ignore(); }
                    }
                }
                catch { Ignore(); }

                return outp;
            }


            #region Inner Workings
            private static void Ignore() { }
            private static List<string> UnsafeGetFilesLockedBy(Process process)
            {
                try
                {
                    var handles = GetHandles(process);
                    var files = new List<string>();

                    foreach (var handle in handles)
                    {
                        var file = GetFilePath(handle, process);
                        if (file != null) files.Add(file);
                    }

                    return files;
                }
                catch
                {
                    return new List<string>();
                }
            }

            const int CNST_SYSTEM_HANDLE_INFORMATION = 16;
            private static string GetFilePath(Win32API.SYSTEM_HANDLE_INFORMATION systemHandleInformation, Process process)
            {
                var ipProcessHwnd = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, false, process.Id);
                var objBasic = new Win32API.OBJECT_BASIC_INFORMATION();
                var objObjectType = new Win32API.OBJECT_TYPE_INFORMATION();
                var objObjectName = new Win32API.OBJECT_NAME_INFORMATION();
                var strObjectName = "";
                var nLength = 0;
                IntPtr ipTemp, ipHandle;

                if (!Win32API.DuplicateHandle(ipProcessHwnd, systemHandleInformation.Handle, Win32API.GetCurrentProcess(), out ipHandle, 0, false, Win32API.DUPLICATE_SAME_ACCESS))
                    return null;

                IntPtr ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
                Win32API.NtQueryObject(ipHandle, (int)Win32API.ObjectInformationClass.ObjectBasicInformation, ipBasic, Marshal.SizeOf(objBasic), ref nLength);
                objBasic = (Win32API.OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(ipBasic, objBasic.GetType());
                Marshal.FreeHGlobal(ipBasic);

                IntPtr ipObjectType = Marshal.AllocHGlobal(objBasic.TypeInformationLength);
                nLength = objBasic.TypeInformationLength;
                // this one never locks...
                while ((uint)(Win32API.NtQueryObject(ipHandle, (int)Win32API.ObjectInformationClass.ObjectTypeInformation, ipObjectType, nLength, ref nLength)) == Win32API.STATUS_INFO_LENGTH_MISMATCH)
                {
                    if (nLength == 0)
                    {
                        Console.WriteLine("nLength returned at zero! ");
                        return null;
                    }
                    Marshal.FreeHGlobal(ipObjectType);
                    ipObjectType = Marshal.AllocHGlobal(nLength);
                }

                objObjectType = (Win32API.OBJECT_TYPE_INFORMATION)Marshal.PtrToStructure(ipObjectType, objObjectType.GetType());
                if (Is64Bits())
                {
                    ipTemp = new IntPtr(Convert.ToInt64(objObjectType.Name.Buffer.ToString(), 10) >> 32);
                }
                else
                {
                    ipTemp = objObjectType.Name.Buffer;
                }

                var strObjectTypeName = Marshal.PtrToStringUni(ipTemp, objObjectType.Name.Length >> 1);
                Marshal.FreeHGlobal(ipObjectType);
                if (strObjectTypeName != "File")
                    return null;

                nLength = objBasic.NameInformationLength;

                var ipObjectName = Marshal.AllocHGlobal(nLength);

                // ...this call sometimes hangs. Is a Windows error.
                while ((uint)(Win32API.NtQueryObject(ipHandle, (int)Win32API.ObjectInformationClass.ObjectNameInformation, ipObjectName, nLength, ref nLength)) == Win32API.STATUS_INFO_LENGTH_MISMATCH)
                {
                    Marshal.FreeHGlobal(ipObjectName);
                    if (nLength == 0)
                    {
                        Console.WriteLine("nLength returned at zero! " + strObjectTypeName);
                        return null;
                    }
                    ipObjectName = Marshal.AllocHGlobal(nLength);
                }
                objObjectName = (Win32API.OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(ipObjectName, objObjectName.GetType());

                if (Is64Bits())
                {
                    ipTemp = new IntPtr(Convert.ToInt64(objObjectName.Name.Buffer.ToString(), 10) >> 32);
                }
                else
                {
                    ipTemp = objObjectName.Name.Buffer;
                }

                if (ipTemp != IntPtr.Zero)
                {

                    var baTemp = new byte[nLength];
                    try
                    {
                        Marshal.Copy(ipTemp, baTemp, 0, nLength);

                        strObjectName = Marshal.PtrToStringUni(Is64Bits() ? new IntPtr(ipTemp.ToInt64()) : new IntPtr(ipTemp.ToInt32()));
                    }
                    catch (AccessViolationException)
                    {
                        return null;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ipObjectName);
                        Win32API.CloseHandle(ipHandle);
                    }
                }

                string path = GetRegularFileNameFromDevice(strObjectName);
                try
                {
                    return path;
                }
                catch
                {
                    return null;
                }
            }

            private static string GetRegularFileNameFromDevice(string strRawName)
            {
                string strFileName = strRawName;
                foreach (string strDrivePath in Environment.GetLogicalDrives())
                {
                    var sbTargetPath = new StringBuilder(Win32API.MAX_PATH);
                    if (Win32API.QueryDosDevice(strDrivePath.Substring(0, 2), sbTargetPath, Win32API.MAX_PATH) == 0)
                    {
                        return strRawName;
                    }
                    string strTargetPath = sbTargetPath.ToString();
                    if (strFileName.StartsWith(strTargetPath))
                    {
                        strFileName = strFileName.Replace(strTargetPath, strDrivePath.Substring(0, 2));
                        break;
                    }
                }
                return strFileName;
            }

            private static IEnumerable<Win32API.SYSTEM_HANDLE_INFORMATION> GetHandles(Process process)
            {
                var nHandleInfoSize = 0x10000;
                var ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
                var nLength = 0;
                IntPtr ipHandle;

                while (Win32API.NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, ref nLength) == Win32API.STATUS_INFO_LENGTH_MISMATCH)
                {
                    nHandleInfoSize = nLength;
                    Marshal.FreeHGlobal(ipHandlePointer);
                    ipHandlePointer = Marshal.AllocHGlobal(nLength);
                }

                var baTemp = new byte[nLength];
                Marshal.Copy(ipHandlePointer, baTemp, 0, nLength);

                long lHandleCount;
                if (Is64Bits())
                {
                    lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                    ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
                }
                else
                {
                    lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                    ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
                }

                var lstHandles = new List<Win32API.SYSTEM_HANDLE_INFORMATION>();

                for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
                {
                    var shHandle = new Win32API.SYSTEM_HANDLE_INFORMATION();
                    if (Is64Bits())
                    {
                        shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                        ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                    }
                    else
                    {
                        ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                        shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    }
                    if (shHandle.ProcessID != process.Id) continue;
                    lstHandles.Add(shHandle);
                }
                return lstHandles;
            }

            private static bool Is64Bits()
            {
                return Marshal.SizeOf(typeof(IntPtr)) == 8;
            }

            internal class Win32API
            {
                [DllImport("ntdll.dll")]
                public static extern int NtQueryObject(IntPtr ObjectHandle, int
                    ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength,
                    ref int returnLength);

                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

                [DllImport("ntdll.dll")]
                public static extern uint NtQuerySystemInformation(int
                    SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength,
                    ref int returnLength);

                [DllImport("kernel32.dll")]
                public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
                [DllImport("kernel32.dll")]
                public static extern int CloseHandle(IntPtr hObject);
                [DllImport("kernel32.dll", SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
                   ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
                   uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
                [DllImport("kernel32.dll")]
                public static extern IntPtr GetCurrentProcess();

                public enum ObjectInformationClass
                {
                    ObjectBasicInformation = 0,
                    ObjectNameInformation = 1,
                    ObjectTypeInformation = 2,
                    ObjectAllTypesInformation = 3,
                    ObjectHandleInformation = 4
                }

                [Flags]
                public enum ProcessAccessFlags : uint
                {
                    All = 0x001F0FFF,
                    Terminate = 0x00000001,
                    CreateThread = 0x00000002,
                    VMOperation = 0x00000008,
                    VMRead = 0x00000010,
                    VMWrite = 0x00000020,
                    DupHandle = 0x00000040,
                    SetInformation = 0x00000200,
                    QueryInformation = 0x00000400,
                    Synchronize = 0x00100000
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct OBJECT_BASIC_INFORMATION
                { // Information Class 0
                    public int Attributes;
                    public int GrantedAccess;
                    public int HandleCount;
                    public int PointerCount;
                    public int PagedPoolUsage;
                    public int NonPagedPoolUsage;
                    public int Reserved1;
                    public int Reserved2;
                    public int Reserved3;
                    public int NameInformationLength;
                    public int TypeInformationLength;
                    public int SecurityDescriptorLength;
                    public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct OBJECT_TYPE_INFORMATION
                { // Information Class 2
                    public UNICODE_STRING Name;
                    public int ObjectCount;
                    public int HandleCount;
                    public int Reserved1;
                    public int Reserved2;
                    public int Reserved3;
                    public int Reserved4;
                    public int PeakObjectCount;
                    public int PeakHandleCount;
                    public int Reserved5;
                    public int Reserved6;
                    public int Reserved7;
                    public int Reserved8;
                    public int InvalidAttributes;
                    public GENERIC_MAPPING GenericMapping;
                    public int ValidAccess;
                    public byte Unknown;
                    public byte MaintainHandleDatabase;
                    public int PoolType;
                    public int PagedPoolUsage;
                    public int NonPagedPoolUsage;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct OBJECT_NAME_INFORMATION
                { // Information Class 1
                    public UNICODE_STRING Name;
                }

                [StructLayout(LayoutKind.Sequential, Pack = 1)]
                public struct UNICODE_STRING
                {
                    public ushort Length;
                    public ushort MaximumLength;
                    public IntPtr Buffer;
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct GENERIC_MAPPING
                {
                    public int GenericRead;
                    public int GenericWrite;
                    public int GenericExecute;
                    public int GenericAll;
                }

                [StructLayout(LayoutKind.Sequential, Pack = 1)]
                public struct SYSTEM_HANDLE_INFORMATION
                { // Information Class 16
                    public int ProcessID;
                    public byte ObjectTypeNumber;
                    public byte Flags; // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
                    public ushort Handle;
                    public int Object_Pointer;
                    public UInt32 GrantedAccess;
                }

                public const int MAX_PATH = 260;
                public const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
                public const int DUPLICATE_SAME_ACCESS = 0x2;
                public const uint FILE_SEQUENTIAL_ONLY = 0x00000004;
            }
            #endregion
        }
        #endregion
    }
}
