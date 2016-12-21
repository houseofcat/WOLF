using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class LOGDRV
    {
        public List<string> DRVInfo = new List<string>();
        public int intDRVLength = 0;
        public string drvSize = "";
        public string drvFreeSpace = "";
        public string fileSystem = "";
        public string drvName = "";
        public string drvLabel = "";
        public string drvSerial = "";

        public LOGDRV(ManagementObject DRV)
        {
            setDRVInfo(DRV);
        }

        private void setDRVInfo(ManagementObject DRV)
        {
            DRVInfo.Clear();
            intDRVLength = 0;

            if (DRV["Name"] != null)
            {
                drvName = DRV["Name"].ToString();
                DRVInfo.Add("Letter: " + drvName );
            }
            else
            {
                DRVInfo.Add("Letter: No Data. (xNull)");
            }

            if (DRV["VolumeName"] != null)
            {
                drvLabel = DRV["VolumeName"].ToString();
                DRVInfo.Add("Label: " + drvLabel );
            }
            else
            {
                DRVInfo.Add("Label: No Data. (xNull)");
            }

            if (DRV["VolumeSerialNumber"] != null)
            {
                drvSerial = DRV["VolumeSerialNumber"].ToString();
                DRVInfo.Add("Serial: " + drvSerial );
            }
            else
            {
                DRVInfo.Add("Serial: No Data. (xNull)");
            }

            if (DRV["FileSystem"] != null)
            {
                fileSystem = DRV["FileSystem"].ToString();
                DRVInfo.Add("File System: " + fileSystem );
            }
            else
            {
                DRVInfo.Add("File System: No Data. (xNull)");
            }

            if (DRV["Size"] != null)
            {
                drvSize = funcConvertSize(DRV["Size"].ToString());

                DRVInfo.Add("Size: " + drvSize );
            }
            else
            {
                DRVInfo.Add("Size: No Data. (xNull)");
            }

            if (DRV["FreeSpace"] != null)
            {
                drvFreeSpace = funcConvertSize(DRV["FreeSpace"].ToString());

                DRVInfo.Add("Free Space: " + drvFreeSpace );
            }
            else
            {
                DRVInfo.Add("Free Space: No Data. (xNull)");
            }

            if (DRV["DriveType"] != null)
            {
                DRVInfo.Add("Drive Type: " + funcConvertType(DRV["DriveType"].ToString()));
            }
            else
            {
                DRVInfo.Add("Drive Type: No Data. (xNull)");
            }

            if (DRV["VolumeDirty"] != null)
            {
                DRVInfo.Add("Volume Dirty: " + DRV["VolumeDirty"].ToString());
            }
            else
            {
                DRVInfo.Add("Volume Dirty: No Data. (xNull)");
            }

            if (DRV["Access"] != null)
            {
                DRVInfo.Add("Access: " + DRV["Access"].ToString());
            }
            else
            {
                DRVInfo.Add("Access: No Data. (xNull)");
            }

            if (DRV["DeviceID"] != null)
            {
                DRVInfo.Add("Device ID: " + DRV["DeviceID"].ToString());
            }
            else
            {
                DRVInfo.Add("Device ID: No Data. (xNull)");
            }

            if (DRV["Caption"] != null)
            {
                DRVInfo.Add("Caption: " + DRV["Caption"].ToString());
            }
            else
            {
                DRVInfo.Add("Caption: No Data. (xNull)");
            }

            if (DRV["SupportsFileBasedCompression"] != null)
            {
                DRVInfo.Add("Supports File Based Compression: " + DRV["SupportsFileBasedCompression"].ToString());
            }
            else
            {
                DRVInfo.Add("Supports File Based Compression: No Data. (xNull)");
            }

            if (DRV["Compressed"] != null)
            {
                DRVInfo.Add("Compressed: " + DRV["Compressed"].ToString());
            }
            else
            {
                DRVInfo.Add("Compressed: No Data. (xNull)");
            }

            if (DRV["MaximumComponentLength"] != null)
            {
                DRVInfo.Add("Maximum Filename Length: " + DRV["MaximumComponentLength"].ToString());
            }
            else
            {
                DRVInfo.Add("Maximum Filename Length: No Data. (xNull)");
            }

            if (DRV["SupportsDiskQuotas"] != null)
            {
                DRVInfo.Add("Supports Disk Quotas: " + DRV["SupportsDiskQuotas"].ToString());
            }
            else
            {
                DRVInfo.Add("Supports Disk Quotas: No Data. (xNull)");
            }

            if (DRV["QuotasDisabled"] != null)
            {
                DRVInfo.Add("Quotas Disabled: " + DRV["QuotasDisabled"].ToString());
            }
            else
            {
                DRVInfo.Add("Quotas Disabled: No Data. (xNull)");
            }

            if (DRV["QuotasIncomplete"] != null)
            {
                DRVInfo.Add("Quotas Incomplete: " + DRV["QuotasIncomplete"].ToString());
            }
            else
            {
                DRVInfo.Add("Quotas Incomplete: No Data. (xNull)");
            }

            if (DRV["QuotasRebuilding"] != null)
            {
                DRVInfo.Add("Quotas Rebuilding: " + DRV["QuotasRebuilding"].ToString());
            }
            else
            {
                DRVInfo.Add("Quotas Rebuilding: No Data. (xNull)");
            }

            if (DRV["SystemName"] != null)
            {
                DRVInfo.Add("System Name: " + DRV["SystemName"].ToString());
            }
            else
            {
                DRVInfo.Add("System Name: No Data. (xNull)");
            }

            if (DRV["SystemCreationClassName"] != null)
            {
                DRVInfo.Add("System Creation Class Name: " + DRV["SystemCreationClassName"].ToString());
            }
            else
            {
                DRVInfo.Add("System Creation Class Name: No Data. (xNull)");
            }

            if (DRV["CreationClassName"] != null)
            {
                DRVInfo.Add("Creation Class Name: " + DRV["CreationClassName"].ToString());
            }
            else
            {
                DRVInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (DRV["Availability"] != null)
            {
                DRVInfo.Add("Availability: " + funcConvertAvail(DRV["Availability"].ToString()));
            }
            else
            {
                DRVInfo.Add("Availability: No Data. (xNull)");
            }

            if (DRV["BlockSize"] != null)
            {
                DRVInfo.Add("Block Size: " + DRV["BlockSize"].ToString());
            }
            else
            {
                DRVInfo.Add("Block Size: No Data. (xNull)");
            }

            if (DRV["ConfigManagerErrorCode"] != null)
            {
                DRVInfo.Add("Config Manager Error Code: " + DRV["ConfigManagerErrorCode"].ToString());
            }
            else
            {
                DRVInfo.Add("Config Manager Error Code: No Data. (xNull)");
            }

            if (DRV["ConfigManagerUserConfig"] != null)
            {
                DRVInfo.Add("Config Manager User Code: " + DRV["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                DRVInfo.Add("Config Manager User Code: No Data. (xNull)");
            }

            if (DRV["Description"] != null)
            {
                DRVInfo.Add("Description: " + DRV["Description"].ToString());
            }
            else
            {
                DRVInfo.Add("Description: No Data. (xNull)");
            }

            if (DRV["ErrorCleared"] != null)
            {
                DRVInfo.Add("Error Cleared: " + DRV["ErrorCleared"].ToString());
            }
            else
            {
                DRVInfo.Add("Error Cleared: No Data. (xNull)");
            }

            if (DRV["ErrorDescription"] != null)
            {
                DRVInfo.Add("Error Description: " + DRV["ErrorDescription"].ToString());
            }
            else
            {
                DRVInfo.Add("Error Description: No Data. (xNull)");
            }

            if (DRV["ErrorMethodology"] != null)
            {
                DRVInfo.Add("Error Methodology: " + DRV["ErrorMethodology"].ToString());
            }
            else
            {
                DRVInfo.Add("Error Methodology: No Data. (xNull)");
            }

            if (DRV["InstallDate"] != null)
            {
                DRVInfo.Add("Install Date: " + DRV["InstallDate"].ToString());
            }
            else
            {
                DRVInfo.Add("Install Date: No Data. (xNull)");
            }

            if (DRV["LastErrorCode"] != null)
            {
                DRVInfo.Add("Last Error Code: " + DRV["LastErrorCode"].ToString());
            }
            else
            {
                DRVInfo.Add("Last Error Code: No Data. (xNull)");
            }

            if (DRV["MediaType"] != null)
            {
                DRVInfo.Add("Media Type: " + DRV["MediaType"].ToString());
            }
            else
            {
                DRVInfo.Add("Media Type: No Data. (xNull)");
            }

            if (DRV["NumberOfBlocks"] != null)
            {
                DRVInfo.Add("Number Of Blocks: " + DRV["NumberOfBlocks"].ToString());
            }
            else
            {
                DRVInfo.Add("Number Of Blocks: No Data. (xNull)");
            }

            if (DRV["PNPDeviceID"] != null)
            {
                DRVInfo.Add("PNP Device ID: " + DRV["PNPDeviceID"].ToString());
            }
            else
            {
                DRVInfo.Add("PNP Device ID: No Data. (xNull)");
            }

            if (DRV["PowerManagementSupported"] != null)
            {
                DRVInfo.Add("Power Management Supported: " + DRV["PowerManagementSupported"].ToString());
            }
            else
            {
                DRVInfo.Add("Power Management Supported: No Data. (xNull)");
            }

            if (DRV["ProviderName"] != null)
            {
                DRVInfo.Add("Provider Name: " + DRV["ProviderName"].ToString());
            }
            else
            {
                DRVInfo.Add("Provider Name: No Data. (xNull)");
            }

            if (DRV["Purpose"] != null)
            {
                DRVInfo.Add("Purpose: " + DRV["Purpose"].ToString());
            }
            else
            {
                DRVInfo.Add("Purpose: No Data. (xNull)");
            }

            if (DRV["Status"] != null)
            {
                DRVInfo.Add("Status: " + DRV["Status"].ToString());
            }
            else
            {
                DRVInfo.Add("Status: No Data. (xNull)");
            }

            if (DRV["StatusInfo"] != null)
            {
                DRVInfo.Add("Status Info: " + funcConvertInfo(DRV["StatusInfo"].ToString()));
            }
            else
            {
                DRVInfo.Add("Status Info: No Data. (xNull)");
            }

            intDRVLength = 39;
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

        private string funcConvertType(string Type)
        {
            if (Type == "0")
            {
                Type = "Unknown";
            }
            else if (Type == "1")
            {
                Type = "No Root Directory";
            }
            else if (Type == "2")
            {
                Type = "Removable Disk";
            }
            else if (Type == "3")
            {
                Type = "Logical Disk";
            }
            else if (Type == "4")
            {
                Type = "Network Drive";
            }
            else if (Type == "5")
            {
                Type = "Compact Disc";
            }
            else if (Type == "6")
            {
                Type = "RAM Disk";
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

        //Converts a capacity string from bytes to GB.
        private string funcConvertSize(string Size)
        {
            ulong temp = 0;

            ulong.TryParse(Size, out temp);

            temp = temp / 1000000000;

            Size = temp.ToString() + " GB";

            return Size;
        }

        public string getName()
        {
            return drvName;
        }

        public string getSerial()
        {
            return drvSerial;
        }

        public string getLabel()
        {
            return drvLabel;
        }

        public string getSize()
        {
            return drvSize;
        }

        public string getFreeSpace()
        {
            return drvFreeSpace;
        }
    }

}
