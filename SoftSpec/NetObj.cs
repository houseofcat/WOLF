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
            this.LocalAddress = LocAddress;
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
            this.LEPAdd = Input;
            this.LEPIsIP4MappedToIP6 = IP4toIP6;
            this.LEPIsIP6LinkLocal = IP6LL;
            this.LEPIsIP6Multi = IP6MC;
            this.LEPIsIP6SiteLocal = IP6SL;
            this.LEPIsIP6Toredo = IP6Tore;
            this.LEPScopeID = Scope;
            this.LEPAddFam = AddFam;
            this.LEPPort = Port;
        }

        public void setREP(string Input, bool IP4toIP6, bool IP6LL, bool IP6MC, bool IP6SL, bool IP6Tore,
                           string Scope, string AddFam, string Port)
        {
            this.REPAdd = Input;
            this.REPIsIP4MappedToIP6 = IP4toIP6;
            this.REPIsIP6LinkLocal = IP6LL;
            this.REPIsIP6Multi = IP6MC;
            this.REPIsIP6SiteLocal = IP6SL;
            this.REPIsIP6Toredo = IP6Tore;
            this.REPScopeID = Scope;
            this.REPAddFam = AddFam;
            this.REPPort = Port;
        }

        public void setState(string Input)
        {
            this.State = Input;
        }

        public string LEP_getAddress()
        {
            return this.LEPAdd;
        }

        public bool LEP_IsIP4MappedToIP6()
        {
            return this.LEPIsIP4MappedToIP6;
        }

        public bool LEP_IsIP6LinkLocal()
        {
            return this.LEPIsIP6LinkLocal;
        }

        public bool LEP_IsIP6Multicast()
        {
            return this.LEPIsIP6Multi;
        }

        public bool LEP_IsIP6SiteLocal()
        {
            return this.LEPIsIP6SiteLocal;
        }

        public bool LEP_IsIP6Toredo()
        {
            return this.LEPIsIP6Toredo;
        }

        public string LEP_getPort()
        {
            return this.LEPPort;
        }

        public string LEP_getScopeID()
        {
            return this.LEPScopeID;
        }

        public string REP_getAddress()
        {
            return this.REPAdd;
        }

        public bool REP_IsIP4MappedToIP6()
        {
            return this.REPIsIP4MappedToIP6;
        }

        public bool REP_IsIP6LinkLocal()
        {
            return this.REPIsIP6LinkLocal;
        }

        public bool REP_IsIP6Multicast()
        {
            return this.REPIsIP6Multi;
        }

        public bool REP_IsIP6SiteLocal()
        {
            return this.REPIsIP6SiteLocal;
        }

        public bool REP_IsIP6Toredo()
        {
            return this.REPIsIP6Toredo;
        }

        public string REP_getPort()
        {
            return this.REPPort;
        }

        public string REP_getScopeID()
        {
            return this.REPScopeID;
        }

        public string getState()
        {
            return this.State;
        }

        public string getProtocol()
        {
            return this.Protocol;
        }

        public string getLocalAddress()
        {
            return this.LocalAddress;
        }

        public int getLocalPort()
        {
            int PORT = 0;

            try
            {
                string[] temparray = this.LocalAddress.Split(':');

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
                string[] temparray = this.RemoteAddress.Split(':');

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
            return this.RemoteAddress;
        }

        public string getProcessName()
        {
            return this.ProcessName;
        }

        public int getPID()
        {
            int PID = 0;

            int.TryParse(this.PID, out PID);

            return PID;
        }

        public bool getIsSystem()
        {
            return this.IsSystem;
        }
    }
}
