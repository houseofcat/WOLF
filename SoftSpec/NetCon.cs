using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Wolf
{
    class NetCon
    {
        public List<string> CONInfo = new List<string>();

        private string Nic = "";
        private string Conn = "";
        private string ConnType = "";
        private string Speed = "";
        private string Mac = "";

        public int intCONlength = 5;

        public bool IsUp = false;

        public NetCon(NetworkInterface con)
        {
            setCon(con);
        }

        //Idea Source: StackOveflow, User: MainMa, Blak3r
        //Site: http://stackoverflow.com/questions/850650/reliable-method-to-get-machines-mac-address-in-c-sharp
        //Modified for matching formatting and readability, storing multiple addresses in List, then later array.
        //C# List Site: http://msdn.microsoft.com/en-us/library/6sh2ey19(v=vs.110).aspx
        //NetworkInterface Site: http://msdn.microsoft.com/en-us/library/system.net.networkinformation.networkinterface(v=vs.110).aspx
        private void setCon(NetworkInterface con)
        {
            if ((con.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) ||
                (con.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
            {
                if (con.OperationalStatus == OperationalStatus.Up)
                {
                    IsUp = true;
                }

                Conn = con.Name.ToString();
                CONInfo.Add("Connection Name: " + Conn);
                Nic = con.Description.ToString();
                CONInfo.Add("NIC: " + Nic);
                Speed = (con.Speed / 1000000).ToString() + "Mb/s (" + (con.Speed / 8000000).ToString() + " MB/s)"; 
                CONInfo.Add("Speed: " + Speed);
                ConnType = con.NetworkInterfaceType.ToString();
                CONInfo.Add("Type: " + ConnType);

                Mac = Tools.funcConvertMAC1(con.GetPhysicalAddress().ToString(),
                                      con.GetPhysicalAddress().ToString().Length);
                CONInfo.Add("Mac Address: " + Mac);
            }
        }

        public string getName()
        {
            return this.Nic;
        }

        public string getConnName()
        {
            return this.Conn;
        }

        public string getConnType()
        {
            return this.ConnType;
        }

        public string getSpeed()
        {
            return this.Speed;
        }

        public string getMAC()
        {
            return this.Mac;
        }

        public bool IsNICUp()
        {
            return this.IsUp;
        }
    }
}
