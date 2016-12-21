using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolf.SoftSpec
{
    class NetObj
    {
        private string LEPAdd = "";
        private bool LEPIsIP4MappedToIP6 = false;
        private bool LEPIsIP6LinkLocal = false;
        private bool LEPIsIP6Multi = false;
        private bool LEPIsIP6SiteLocal = false;
        private bool LEPIsIP6Toredo = false;
        private string LEPScopeID = "";
        private string LEPAddFam = "";
        private string LEPPort = "";

        private string REPAdd = "";
        private bool REPIsIP4MappedToIP6 = false;
        private bool REPIsIP6LinkLocal = false;
        private bool REPIsIP6Multi = false;
        private bool REPIsIP6SiteLocal = false;
        private bool REPIsIP6Toredo = false;
        private string REPScopeID = "";
        private string REPAddFam = "";
        private string REPPort = "";

        private string State = "";
        private string Protocol = "";
        private string LocalAddress = "";
        private string RemoteAddress = "";
        private string ProcessName = "";
        private string PID = "";

        private bool IsSystem = true;

        public NetObj()
        {
            
        }

        public NetObj(string Protocol, string LocAddress, string RemoteAddress, string State, string ProcessName, string PID)
        {
            this.Protocol = Protocol;
            LocalAddress = LocAddress;
            this.RemoteAddress = RemoteAddress;
            this.State = State;
            this.ProcessName = ProcessName;
            this.PID = PID;

            if (getPID() > 4)
            {
                IsSystem = false;
            }
        }

        public void setLEP(string Input, bool IP4toIP6, bool IP6LL, bool IP6MC, bool IP6SL, bool IP6Tore,
                           string Scope, string AddFam, string Port)
        {
            LEPAdd = Input;
            LEPIsIP4MappedToIP6 = IP4toIP6;
            LEPIsIP6LinkLocal = IP6LL;
            LEPIsIP6Multi = IP6MC;
            LEPIsIP6SiteLocal = IP6SL;
            LEPIsIP6Toredo = IP6Tore;
            LEPScopeID = Scope;
            LEPAddFam = AddFam;
            LEPPort = Port;
        }

        public void setREP(string Input, bool IP4toIP6, bool IP6LL, bool IP6MC, bool IP6SL, bool IP6Tore,
                           string Scope, string AddFam, string Port)
        {
            REPAdd = Input;
            REPIsIP4MappedToIP6 = IP4toIP6;
            REPIsIP6LinkLocal = IP6LL;
            REPIsIP6Multi = IP6MC;
            REPIsIP6SiteLocal = IP6SL;
            REPIsIP6Toredo = IP6Tore;
            REPScopeID = Scope;
            REPAddFam = AddFam;
            REPPort = Port;
        }

        public void setState(string Input)
        {
            State = Input;
        }

        public string LEP_getAddress()
        {
            return LEPAdd;
        }

        public bool LEP_IsIP4MappedToIP6()
        {
            return LEPIsIP4MappedToIP6;
        }

        public bool LEP_IsIP6LinkLocal()
        {
            return LEPIsIP6LinkLocal;
        }

        public bool LEP_IsIP6Multicast()
        {
            return LEPIsIP6Multi;
        }

        public bool LEP_IsIP6SiteLocal()
        {
            return LEPIsIP6SiteLocal;
        }

        public bool LEP_IsIP6Toredo()
        {
            return LEPIsIP6Toredo;
        }

        public string LEP_getPort()
        {
            return LEPPort;
        }

        public string LEP_getScopeID()
        {
            return LEPScopeID;
        }

        public string REP_getAddress()
        {
            return REPAdd;
        }

        public bool REP_IsIP4MappedToIP6()
        {
            return REPIsIP4MappedToIP6;
        }

        public bool REP_IsIP6LinkLocal()
        {
            return REPIsIP6LinkLocal;
        }

        public bool REP_IsIP6Multicast()
        {
            return REPIsIP6Multi;
        }

        public bool REP_IsIP6SiteLocal()
        {
            return REPIsIP6SiteLocal;
        }

        public bool REP_IsIP6Toredo()
        {
            return REPIsIP6Toredo;
        }

        public string REP_getPort()
        {
            return REPPort;
        }

        public string REP_getScopeID()
        {
            return REPScopeID;
        }

        public string getState()
        {
            return State;
        }

        public string getProtocol()
        {
            return Protocol;
        }

        public string getLocalAddress()
        {
            return LocalAddress;
        }

        public int getLocalPort()
        {
            int PORT = 0;

            try
            {
                string[] temparray = LocalAddress.Split(':');

                if (temparray.Length > 1)
                {
                    int.TryParse(temparray.Last(), out PORT);
                }
            }
            catch { PORT = -1; }

            return PORT;
        }

        public int getRemotePort()
        {
            int PORT = 0;

            try
            {
                string[] temparray = RemoteAddress.Split(':');

                if (temparray.Length > 1)
                {
                    int.TryParse(temparray.Last(), out PORT);
                }
            }
            catch { PORT = -1; }

            return PORT;
        }

        public string getRemoteAddress()
        {
            return RemoteAddress;
        }

        public string getProcessName()
        {
            return ProcessName;
        }

        public int getPID()
        {
            int PID = 0;

            int.TryParse(this.PID, out PID);

            return PID;
        }

        public bool getIsSystem()
        {
            return IsSystem;
        }
    }
}
