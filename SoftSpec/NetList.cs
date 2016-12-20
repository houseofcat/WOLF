using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
using System.Collections;

namespace Wolf.SoftSpec
{
    class NetList
    {
        public List<NetObj> ActiveTCPPorts = new List<NetObj>();
        public List<NetObj> ActiveListenTCPPorts = new List<NetObj>();

        public List<NetObj> ActiveUDPPorts = new List<NetObj>();
        public List<NetObj> ActiveListenUDPPorts = new List<NetObj>();

        public List<NetObj> NetworkProcList = new List<NetObj>();

        public NetList()
        {

        }

        public void funcGetPorts()
        {
            if ((Environment.Is64BitOperatingSystem) && (!(Environment.Is64BitProcess)))
            { GAP_86(); }
            else
            { GAP_64(); }
        }

        private void funcGetTCPPorts()
        {
            TcpConnectionInformation[] tcpconact = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
            IPEndPoint[] tcpconlist = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

            //Console.WriteLine("Test");
            foreach(TcpConnectionInformation obj in tcpconact)
            {
                NetObj temp = new NetObj();

                temp.setLEP(obj.LocalEndPoint.Address.ToString(),
                            obj.LocalEndPoint.Address.IsIPv4MappedToIPv6,
                            obj.LocalEndPoint.Address.IsIPv6LinkLocal,
                            obj.LocalEndPoint.Address.IsIPv6Multicast,
                            obj.LocalEndPoint.Address.IsIPv6SiteLocal,
                            obj.LocalEndPoint.Address.IsIPv6Teredo,
                            obj.LocalEndPoint.Address.ScopeId.ToString(),
                            obj.LocalEndPoint.AddressFamily.ToString(),
                            obj.LocalEndPoint.Port.ToString());

                temp.setREP(obj.RemoteEndPoint.Address.ToString(),
                            obj.RemoteEndPoint.Address.IsIPv4MappedToIPv6,
                            obj.RemoteEndPoint.Address.IsIPv6LinkLocal,
                            obj.RemoteEndPoint.Address.IsIPv6Multicast,
                            obj.RemoteEndPoint.Address.IsIPv6SiteLocal,
                            obj.RemoteEndPoint.Address.IsIPv6Teredo,
                            obj.RemoteEndPoint.Address.ScopeId.ToString(),
                            obj.RemoteEndPoint.AddressFamily.ToString(),
                            obj.RemoteEndPoint.Port.ToString());

                temp.setState(obj.State.ToString());

                ActiveTCPPorts.Add(temp);
            }
        }

        private void GAP_86()
        {
            IntPtr ptr = new IntPtr();
            NativeMethods.Wow64DisableWow64FsRedirection(ref ptr);

            Process p = new Process();

            ProcessStartInfo ps = new ProcessStartInfo();
            ps.Arguments = "-a -n -o";
            ps.FileName = "netstat.exe";
            ps.UseShellExecute = false;
            ps.CreateNoWindow = true;
            ps.RedirectStandardInput = true;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = true;

            p.StartInfo = ps;
            p.Start();

            NativeMethods.Wow64RevertWow64FsRedirection(ref ptr);

            StreamReader stdOutput = p.StandardOutput;
            StreamReader stdError = p.StandardError;

            string content = stdOutput.ReadToEnd();
            string exitStatus = p.ExitCode.ToString();

            if (exitStatus != "0")
            {
                MessageBox.Show("An error occurred in gathering information from NetStat.exe.\n\n" + stdError.ToString());
            }

            string[] rows = Regex.Split(content, "\r\n");
            StringArrayToNetObjList(rows);

            stdError.Close();
            stdOutput.Close();
        }

        private void GAP_64()
        {
            Process p = new Process();
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.Arguments = "-a -n -o";
            ps.FileName = "netstat.exe";
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
                MessageBox.Show("An error occurred in gathering information from NetStat.exe.\n\n" + stdError.ToString());
            }

            //Console.WriteLine(content);
            string[] rows = Regex.Split(content, "\r\n");
            StringArrayToNetObjList(rows);

            stdError.Close();
            stdOutput.Close();
        }

        private void StringArrayToNetObjList(string[] rows)
        {
            foreach (string row in rows)
            {
                //Eliminates the default NetStat Rows
                if ((row != null) && (row != "") && (!(row.Contains("Active"))) && (!(row.Contains("Proto"))))
                {
                    string[] temparray = row.Split(' ');

                    if (temparray.Any())
                    {
                        int count = 0;
                        string Protocol = "";
                        string LocalAddress = "";
                        string RemoteAddress = "";
                        string State = "";
                        string ProcessName = "";
                        string PID = "";

                        foreach (string temp in temparray)
                        {
                            //So many blank strings are in the array. This regex filters out any useless strings.
                            //netstat -ano will return 4 good strings per line, therefore every 4 elements from
                            //parsedContent equate to a row.
                            if (Regex.Matches(temp, @"[a-zA-Z0-9:*]").Count > 0)
                            {
                                if (count == 0)
                                {
                                    Protocol = temp;
                                }
                                else if (count == 1)
                                {
                                    LocalAddress = temp;
                                }
                                else if (count == 2)
                                {
                                    RemoteAddress = temp;
                                }
                                //Every fourth entry (ElementAt[3]) should be the TCP State or PID.
                                else if ((count == 3) && (!(temp.Contains("LIS"))) && (!(temp.Contains("EST"))) &&
                                    (!(temp.Contains("CLO"))) && (!(temp.Contains("TIME"))) &&
                                    (!(temp.Contains("SYN"))) && (!(temp.Contains("LAS"))) &&
                                    (!(temp.Contains("FIN"))))
                                {
                                    PID = temp;
                                    ProcessName = GetProcessName(temp);
                                }
                                else if (count == 3)
                                {
                                    State = temp;
                                }
                                //Every fifth entry should be a PID.
                                else if ((count == 4) && (!(temp.Contains("LIS"))) && (!(temp.Contains("EST"))) &&
                                        (!(temp.Contains("CLO"))) && (!(temp.Contains("TIME"))) &&
                                        (!(temp.Contains("SYN"))) && (!(temp.Contains("LAS"))) &&
                                        (!(temp.Contains("FIN"))))
                                {
                                    PID = temp;
                                    ProcessName = GetProcessName(temp);
                                }

                                count++;
                            }
                        }

                        NetObj nob = new NetObj(Protocol, LocalAddress, RemoteAddress, State, ProcessName, PID);
                        NetworkProcList.Add(nob);
                    }
                }
            }
        }

        public string GetProcessName(String ProcessID)
        {
            string PName = "";
            int PID = 0;

            if (int.TryParse(ProcessID, out PID))
            {
                try
                {
                    if( Process.GetProcesses().Any(P => P.Id == PID))
                    {
                        PName = Process.GetProcessById( PID ).ProcessName ?? " { Unknown - Unreadable } :( ";
                    }
                }
                catch 
                {
                    //MessageBox.Show("Process Name Exception \n\n" + bug.Message.ToString());
                    PName = " { Unknown - Unreadable } :( ";
                }
            }

            return PName;
        }
        
        public void KillProcess(int Index)
        {
            try
            {
                int test = NetworkProcList.Count;

                if (test > Index)
                {
                    int pid = NetworkProcList.ElementAt(Index).getPID();
                    NetworkProcList.RemoveAt(Index);

                    Process.GetProcessById(pid).Kill();
                }
            }
            catch { /*Handles missing processes.*/ }
        }

        public void KillProcessAndChildren(int Index)
        {
            int test = NetworkProcList.Count;

            if (test > Index)
            {
                int pid = NetworkProcList.ElementAt(Index).getPID();
                NetworkProcList.RemoveAt(Index);

                ManagementObjectSearcher searcher = new ManagementObjectSearcher
                  ("Select * From Win32_Process Where ParentProcessID=" + pid);

                ManagementObjectCollection procList = searcher.Get();

                foreach (ManagementObject proc in procList)
                {
                    KillProcessAndChildren(Convert.ToInt32(proc["ProcessID"]));
                }

                try
                {
                    Process proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    // Process has already exited.
                }
            }
        }
    }
}
