using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class NIC
    {
        public List<string> NICInfo = new List<string>();
        public int intNICLength = 0;
        public bool IsPhysical = false;
        public string MAC = "";
        public string GUID = "";
        public string Speed = "";
        public string Name = "";

        public NIC(ManagementObject nicInfo)
        {
            NICInfo.Clear();

            intNICLength = 0;

            if (nicInfo["Name"] != null)
            {
                this.Name = nicInfo["Name"].ToString();
                NICInfo.Add("Name: " + this.Name);
            }
            else
            {
                NICInfo.Add("Name: No Data. (xNull)");
            }

            if (nicInfo["Index"] != null)
            {
                NICInfo.Add("Device Index: " + nicInfo["Index"].ToString());
            }
            else
            {
                NICInfo.Add("Device Index: No Data. (xNull)");
            }

            if (nicInfo["Availability"] != null)
            {
                NICInfo.Add("Availability: " + funcConvertAvail(nicInfo["Availability"].ToString()));
            }
            else
            {
                NICInfo.Add("Availability: No Data. (xNull)");
            }

            if (nicInfo["Speed"] != null)
            {
                this.Speed = funcConvertSpeed(nicInfo["Speed"].ToString());
                NICInfo.Add("Speed: " + this.Speed);
            }
            else
            {
                NICInfo.Add("Speed: No Data. (xNull)");
            }

            if (nicInfo["AdapterType"] != null)
            {
                NICInfo.Add("Adapter Type: " + nicInfo["AdapterType"].ToString());
            }
            else
            {
                NICInfo.Add("Adapter Type: No Data. (xNull)");
            }

            if (nicInfo["MACAddress"] != null)
            {
                this.MAC = nicInfo["MACAddress"].ToString();
                NICInfo.Add("MAC Address: " + this.MAC);
            }
            else
            {
                NICInfo.Add("MAC Address: No Data. (xNull)");
            }

            if (nicInfo["GUID"] != null)
            {
                this.GUID = nicInfo["GUID"].ToString();
                NICInfo.Add("GUID: " + this.GUID);
            }
            else
            {
                NICInfo.Add("GUID: No Data. (xNull)");
            }

            if (nicInfo["Description"] != null)
            {
                NICInfo.Add("Description: " + nicInfo["Description"].ToString());
            }
            else
            {
                NICInfo.Add("Description: No Data. (xNull)");
            }

            if (nicInfo["DeviceID"] != null)
            {
                NICInfo.Add("Device ID: " + nicInfo["DeviceID"].ToString());
            }
            else
            {
                NICInfo.Add("Device ID: No Data. (xNull)");
            }

            if (nicInfo["InterfaceIndex"] != null)
            {
                NICInfo.Add("Interface Index: " + nicInfo["InterfaceIndex"].ToString());
            }
            else
            {
                NICInfo.Add("Interface Index: No Data. (xNull)");
            }

            if (nicInfo["AdapterTypeID"] != null)
            {
                NICInfo.Add("Adapter Type ID: " + funcConvertType(nicInfo["AdapterTypeID"].ToString()));
            }
            else
            {
                NICInfo.Add("Adapter Type ID: No Data. (xNull)");
            }

            if (nicInfo["ProductName"] != null)
            {
                NICInfo.Add("Product Name: " + nicInfo["ProductName"].ToString());
            }
            else
            {
                NICInfo.Add("Product Name: No Data. (xNull)");
            }

            if (nicInfo["ServiceName"] != null)
            {
                NICInfo.Add("Service Name: " + nicInfo["ServiceName"].ToString());
            }
            else
            {
                NICInfo.Add("ServiceName: No Data. (xNull)");
            }

            if (nicInfo["Manufacturer"] != null)
            {
                NICInfo.Add("Manufacturer: " + nicInfo["Manufacturer"].ToString());
            }
            else
            {
                NICInfo.Add("Manufacturer: No Data. (xNull)");
            }

            if (nicInfo["PhysicalAdapter"] != null)
            {
                NICInfo.Add("Physical Adapter: " + nicInfo["PhysicalAdapter"].ToString());

                if ((nicInfo["PhysicalAdapter"].ToString() == "True") || (nicInfo["PhysicalAdapter"].ToString() == "true"))
                {
                    this.IsPhysical = true;
                }
            }
            else
            {
                NICInfo.Add("Physical Adapter: No Data. (xNull)");
            }

            if (nicInfo["NetEnabled"] != null)
            {
                NICInfo.Add("Net Enabled: " + nicInfo["NetEnabled"].ToString());
            }
            else
            {
                NICInfo.Add("Net Enabled: No Data. (xNull)");
            }

            if (nicInfo["NetConnectionStatus"] != null)
            {
                NICInfo.Add("Net Connection Status: " + funcConvertStatus(nicInfo["NetConnectionStatus"].ToString()));
            }
            else
            {
                NICInfo.Add("Net Connection Status: No Data. (xNull)");
            }

            if (nicInfo["NetConnectionID"] != null)
            {
                NICInfo.Add("Net Connection ID: " + nicInfo["NetConnectionID"].ToString());
            }
            else
            {
                NICInfo.Add("Net Connection ID: No Data. (xNull)");
            }

            if (nicInfo["Caption"] != null)
            {
                NICInfo.Add("Caption: " + nicInfo["Caption"].ToString());
            }
            else
            {
                NICInfo.Add("Caption: No Data. (xNull)");
            }

            if (nicInfo["ConfigManagerErrorCode"] != null)
            {
                NICInfo.Add("Config Manager Error Code: " + nicInfo["ConfigManagerErrorCode"].ToString());
            }
            else
            {
                NICInfo.Add("Config Manager Error Code: No Data. (xNull)");
            }

            if (nicInfo["ConfigManagerUserConfig"] != null)
            {
                NICInfo.Add("Config Manager User Config: " + nicInfo["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                NICInfo.Add("Config Managaer User Config: No Data. (xNull)");
            }

            if (nicInfo["SystemName"] != null)
            {
                NICInfo.Add("System Name: " + nicInfo["SystemName"].ToString());
            }
            else
            {
                NICInfo.Add("System Name: No Data. (xNull)");
            }

            if (nicInfo["SystemCreationClassName"] != null)
            {
                NICInfo.Add("System Creation Class Name: " + nicInfo["SystemCreationClassName"].ToString());
            }
            else
            {
                NICInfo.Add("System Creation Class Name: No Data. (xNull)");
            }

            if (nicInfo["CreationClassName"] != null)
            {
                NICInfo.Add("Creation Class Name: " + nicInfo["CreationClassName"].ToString());
            }
            else
            {
                NICInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (nicInfo["Status"] != null)
            {
                NICInfo.Add("Status: " + funcConvertStatus(nicInfo["Status"].ToString()));
            }
            else
            {
                NICInfo.Add("Status: No Data. (xNull)");
            }

            if (nicInfo["StatusInfo"] != null)
            {
                NICInfo.Add("Status Info: " + funcConvertInfo(nicInfo["StatusInfo"].ToString()));
            }
            else
            {
                NICInfo.Add("Status Info: No Data. (xNull)");
            }

            if (nicInfo["AutoSense"] != null)
            {
                NICInfo.Add("AutoSense: " + nicInfo["AutoSense"].ToString());
            }
            else
            {
                NICInfo.Add("AutoSense: No Data. (xNull)");
            }

            if (nicInfo["ErrorCleared"] != null)
            {
                NICInfo.Add("Error Cleared: " + nicInfo["ErrorCleared"].ToString());
            }
            else
            {
                NICInfo.Add("Error Cleared: No Data. (xNull)");
            }

            if (nicInfo["ErrorDescription"] != null)
            {
                NICInfo.Add("Error Description: " + nicInfo["ErrorDescription"].ToString());
            }
            else
            {
                NICInfo.Add("Error Description: No Data. (xNull)");
            }

            if (nicInfo["InstallDate"] != null)
            {
                NICInfo.Add("Install Date: " + nicInfo["InstallDate"].ToString());
            }
            else
            {
                NICInfo.Add("Install Date: No Data. (xNull)");
            }

            if (nicInfo["LastErrorCode"] != null)
            {
                NICInfo.Add("Last Error Code: " + nicInfo["LastErrorCode"].ToString());
            }
            else
            {
                NICInfo.Add("Last Error Code: No Data. (xNull)");
            }

            if (nicInfo["MaxNumberControlled"] != null)
            {
                NICInfo.Add("Max Controlled Ports: " + nicInfo["MaxNumberControlled"].ToString());
            }
            else
            {
                NICInfo.Add("Max Controlled Ports: No Data. (xNull)");
            }

            if (nicInfo["MaxSpeed"] != null)
            {
                NICInfo.Add("Max Speed: " + nicInfo["MaxSpeed"].ToString());
            }
            else
            {
                NICInfo.Add("Max Speed: No Data. (xNull)");
            }

            if (nicInfo["PermanentAddress"] != null)
            {
                NICInfo.Add("Permanent Address: " + nicInfo["Permanent Address"].ToString());
            }
            else
            {
                NICInfo.Add("Permanent Address: No Data. (xNull)");
            }

            if (nicInfo["PNPDeviceID"] != null)
            {
                NICInfo.Add("PNP Device ID: " + nicInfo["PNPDeviceID"].ToString());
            }
            else
            {
                NICInfo.Add("PNP Device ID: No Data. (xNull)");
            }

            if (nicInfo["PowerManagementSupported"] != null)
            {
                NICInfo.Add("Power Management Supported: " + nicInfo["PowerManagementSupported"].ToString());
            }
            else
            {
                NICInfo.Add("Power Management Supported: No Data. (xNull)");
            }

            if (nicInfo["TimeOfLastReset"] != null)
            {
                NICInfo.Add("Time Of Last Reset: " + nicInfo["TimeOfLastReset"].ToString());
            }
            else
            {
                NICInfo.Add("Time Of Last Reset: No Data. (xNull)");
            }

            intNICLength = 37;
        }

        //Converts Speed string from Bits / second, to
        //Megabits / second & Megabytes / second.
        private string funcConvertSpeed(string Speed)
        {
            ulong temp = 0;

            ulong.TryParse(Speed, out temp);

            
            temp = temp / 1000000;

            Speed = temp.ToString();

            temp = temp / 8;

            Speed += " Mb/s" + " (" + temp.ToString() + " MB/s)";

            return Speed;
        }

        private string funcConvertType(string Type)
        {
            if (Type == "0")
            {
                Type = "Ethernet 802.3";
            }
            else if (Type == "1")
            {
                Type = "Token Ring 802.5";
            }
            else if (Type == "2")
            {
                Type = "Fiber Distributed Data Interface (FDDI)";
            }
            else if (Type == "3")
            {
                Type = "Wide Area Network (WAN)";
            }
            else if (Type == "4")
            {
                Type = "LocalTalk";
            }
            else if (Type == "5")
            {
                Type = "Ethernet using DIX header format.";
            }
            else if (Type == "6")
            {
                Type = "ARCNET";
            }
            else if (Type == "7")
            {
                Type = "ARCNET (878.2)";
            }
            else if (Type == "8")
            {
                Type = "ATM";
            }
            else if (Type == "9")
            {
                Type = "Wireless";
            }
            else if (Type == "10")
            {
                Type = "Infrared Wireless";
            }
            else if (Type == "11")
            {
                Type = "Bpc";
            }
            else if (Type == "12")
            {
                Type = "CoWan";
            }
            else if (Type == "13")
            {
                Type = "1394";
            }

            return Type;
        }

        private string funcConvertAvail(string Avail)
        {
            if (Avail == "0")
            {
                Avail = "OMG WHAT THE HELL HAPPENED?!";
            }
            else if (Avail == "1")
            {
                Avail = "Other";
            }
            else if (Avail == "2")
            {
                Avail = "Unknown";
            }
            else if (Avail == "3")
            {
                Avail = "Running \\ Full Power";
            }
            else if (Avail == "4")
            {
                Avail = "Warning";
            }
            else if (Avail == "5")
            {
                Avail = "In Test";
            }
            else if (Avail == "6")
            {
                Avail = "Not Applicable";
            }
            else if (Avail == "7")
            {
                Avail = "Power Off";
            }
            else if (Avail == "8")
            {
                Avail = "Off Line";
            }
            else if (Avail == "9")
            {
                Avail = "Off Duty";
            }
            else if (Avail == "10")
            {
                Avail = "Degraded";
            }
            else if (Avail == "11")
            {
                Avail = "Not Installed";
            }
            else if (Avail == "12")
            {
                Avail = "Install Error";
            }
            else if (Avail == "13")
            {
                Avail = "Power Save (Unknown Mode)";
            }
            else if (Avail == "14")
            {
                Avail = "Power Save (Low Power Mode)";
            }
            else if (Avail == "15")
            {
                Avail = "Power Save (Standby)";
            }
            else if (Avail == "16")
            {
                Avail = "Power Cycle";
            }
            else if (Avail == "1")
            {
                Avail = "Power Save (Warning)";
            }

            return Avail;
        }

        private string funcConvertStatus(string Status)
        {
            if (Status == "0")
            {
                Status = "Disconnected";
            }
            else if (Status == "1")
            {
                Status = "Connecting";
            }
            else if (Status == "2")
            {
                Status = "Connected";
            }
            else if (Status == "3")
            {
                Status = "Disconnecting";
            }
            else if (Status == "4")
            {
                Status = "Hardware Not Present";
            }
            else if (Status == "5")
            {
                Status = "Hardware";
            }
            else if (Status == "6")
            {
                Status = "Hardware Malfunction";
            }
            else if (Status == "7")
            {
                Status = "Media Disconnected";
            }
            else if (Status == "8")
            {
                Status = "Authenticating";
            }
            else if (Status == "9")
            {
                Status = "Authentication Succeeded";
            }
            else if (Status == "10")
            {
                Status = "Authetnication Failed";
            }
            else if (Status == "11")
            {
                Status = "Invalid Address";
            }
            else if (Status == "12")
            {
                Status = "Credentials Required";
            }

            return Status;
        }

        private string funcConvertInfo(string Info)
        {
            if (Info == "0")
            {
                Info = "ERROR";
            }
            else if (Info == "1")
            {
                Info = "Other";
            }
            else if (Info == "2")
            {
                Info = "Unknown";
            }
            else if (Info == "3")
            {
                Info = "Disabled";
            }
            else if (Info == "5")
            {
                Info = "Not Applicable";
            }

            return Info;
        }

        public string getSpeed()
        {
            return this.Speed;
        }

        public string getMAC()
        {
            return this.MAC;
        }

        public string getGUID()
        {
            return this.GUID;
        }

        public string getName()
        {
            return this.Name;
        }
    }
}
