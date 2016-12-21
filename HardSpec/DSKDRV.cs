using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class DSKDRV
    {
        public List<string> DRVInfo = new List<string>();
        public int intDRVLength = 0;
        private string drvName = "";
        private string drvSize = "";

        public DSKDRV(ManagementObject DRV)
        {
            setDRVInfo(DRV);
        }

        private void setDRVInfo(ManagementObject DRV)
        {
            DRVInfo.Clear();
            intDRVLength = 0;

            if (DRV["DeviceID"] != null)
            {
                DRVInfo.Add("Device ID: " + DRV["DeviceID"].ToString());
            }
            else
            {
                DRVInfo.Add("Device ID: No Data. (xNull)");
            }

            if (DRV["Name"] != null)
            {
                drvName = DRV["Name"].ToString();
                DRVInfo.Add("Name: " + drvName);
            }
            else
            {
                DRVInfo.Add("Name: No Data. (xNull)");
            }

            if (DRV["Model"] != null)
            {
                DRVInfo.Add("Model: " + DRV["Model"].ToString());
            }
            else
            {
                DRVInfo.Add("Model: No Data. (xNull)");
            }

            if (DRV["SerialNumber"] != null)
            {
                DRVInfo.Add("Serial: " + DRV["SerialNumber"].ToString());
            }
            else
            {
                DRVInfo.Add("Serial: No Data. (xNull)");
            }

            if (DRV["Caption"] != null)
            {
                DRVInfo.Add("Caption: " + DRV["Caption"].ToString());
            }
            else
            {
                DRVInfo.Add("Caption: No Data. (xNull)");
            }

            if (DRV["Index"] != null)
            {
                DRVInfo.Add("Index: " + DRV["Index"].ToString());
            }
            else
            {
                DRVInfo.Add("Index: No Data. (xNull)");
            }

            if (DRV["Status"] != null)
            {
                DRVInfo.Add("Status: " + DRV["Status"].ToString());
            }
            else
            {
                DRVInfo.Add("Status: No Data. (xNull)");
            }

            if (DRV["Size"] != null)
            {
                drvSize = funcConvertSize(DRV["Size"].ToString());
                DRVInfo.Add("Size: " + drvSize);
            }
            else
            {
                DRVInfo.Add("Size: No Data. (xNull)");
            }

            if (DRV["Partitions"] != null)
            {
                DRVInfo.Add("Partitions: " + DRV["Partitions"].ToString());
            }
            else
            {
                DRVInfo.Add("Partitions: No Data. (xNull)");
            }

            if (DRV["TotalHeads"] != null)
            {
                DRVInfo.Add("Total Heads: " + DRV["TotalHeads"].ToString());
            }
            else
            {
                DRVInfo.Add("Total Heads: No Data. (xNull)");
            }

            if (DRV["TotalCylinders"] != null)
            {
                DRVInfo.Add("Total Cylinders: " + DRV["TotalCylinders"].ToString());
            }
            else
            {
                DRVInfo.Add("Total Cylinders: No Data. (xNull)");
            }

            if (DRV["TracksPerCylinder"] != null)
            {
                DRVInfo.Add("Tracks Per Cylinder: " + DRV["TracksPerCylinder"].ToString());
            }
            else
            {
                DRVInfo.Add("Tracks Per Cylinder: No Data. (xNull)");
            }

            if (DRV["TotalTracks"] != null)
            {
                DRVInfo.Add("Total Tracks: " + DRV["TotalTracks"].ToString());
            }
            else
            {
                DRVInfo.Add("Total Tracks: No Data. (xNull)");
            }

            if (DRV["SectorsPerTrack"] != null)
            {
                DRVInfo.Add("Sectors Per Track: " + DRV["SectorsPerTrack"].ToString());
            }
            else
            {
                DRVInfo.Add("Sectors Per Track: No Data. (xNull)");
            }

            if (DRV["TotalSectors"] != null)
            {
                DRVInfo.Add("Total Sectors: " + DRV["TotalSectors"].ToString());
            }
            else
            {
                DRVInfo.Add("Total Sectors: No Data. (xNull)");
            }

            if (DRV["BytesPerSector"] != null)
            {
                DRVInfo.Add("Bytes Per Sector: " + DRV["BytesPerSector"].ToString());
            }
            else
            {
                DRVInfo.Add("Bytes Per Sector: No Data. (xNull)");
            }

            if (DRV["Description"] != null)
            {
                DRVInfo.Add("Description: " + DRV["Description"].ToString());
            }
            else
            {
                DRVInfo.Add("Description: No Data. (xNull)");
            }

            if (DRV["CompressionMethod"] != null)
            {
                DRVInfo.Add("Compression Method: " + DRV["CompressionMethod"].ToString());
            }
            else
            {
                DRVInfo.Add("Compression Method: No Data. (xNull)");
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
                DRVInfo.Add("Config Manager User Config: " + DRV["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                DRVInfo.Add("Config Manager User Config: No Data. (xNull)");
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

            if (DRV["Manufacturer"] != null)
            {
                DRVInfo.Add("Manufacturer: " + DRV["Manufacturer"].ToString());
            }
            else
            {
                DRVInfo.Add("Manufacturer: No Data. (xNull)");
            }

            if (DRV["FirmwareRevision"] != null)
            {
                DRVInfo.Add("Firmware Revision: " + DRV["FirmwareRevision"].ToString());
            }
            else
            {
                DRVInfo.Add("Firmware Revision: No Data. (xNull)");
            }

            if (DRV["Signature"] != null)
            {
                DRVInfo.Add("Signature: " + DRV["Signature"].ToString());
            }
            else
            {
                DRVInfo.Add("Signature: No Data. (xNull)");
            }

            if (DRV["DefaultBlockSize"] != null)
            {
                DRVInfo.Add("Default Block Size: " + DRV["DefaultBlockSize"].ToString());
            }
            else
            {
                DRVInfo.Add("Default Block Size: No Data. (xNull)");
            }

            if (DRV["MinBlockSize"] != null)
            {
                DRVInfo.Add("Min Block Size: " + DRV["MinBlockSize"].ToString());
            }
            else
            {
                DRVInfo.Add("Min Block Size: No Data. (xNull)");
            }

            if (DRV["MaxBlockSize"] != null)
            {
                DRVInfo.Add("Max Block Size: " + DRV["MaxBlockSize"].ToString());
            }
            else
            {
                DRVInfo.Add("Max Block Size: No Data. (xNull)");
            }

            if (DRV["MediaType"] != null)
            {
                DRVInfo.Add("Media Type: " + DRV["MediaType"].ToString());
            }
            else
            {
                DRVInfo.Add("Media Type: No Data. (xNull)");
            }

            if (DRV["MediaLoaded"] != null)
            {
                DRVInfo.Add("Media Loaded: " + DRV["MediaLoaded"].ToString());
            }
            else
            {
                DRVInfo.Add("Media Loaded: No Data. (xNull)");
            }

            if (DRV["MaxMediaSize"] != null)
            {
                DRVInfo.Add("Max Media Size: " + DRV["MaxMediaSize"].ToString());
            }
            else
            {
                DRVInfo.Add("Max Media Size: No Data. (xNull)");
            }

            if (DRV["Availability"] != null)
            {
                DRVInfo.Add("Availability: " + funcConvertAvail(DRV["Availability"].ToString()));
            }
            else
            {
                DRVInfo.Add("Availability: No Data. (xNull)");
            }

            if (DRV["ErrorCleared"] != null)
            {
                DRVInfo.Add("Error Cleared: " + DRV["ErrorCleared"].ToString());
            }
            else
            {
                DRVInfo.Add("Error Cleared: No Data. (xNull)");
            }

            if (DRV["LastErrorCode"] != null)
            {
                DRVInfo.Add("Last Error Code: " + DRV["LastErrorCode"].ToString());
            }
            else
            {
                DRVInfo.Add("Last Error Code: No Data. (xNull)");
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

            if (DRV["InterfaceType"] != null)
            {
                DRVInfo.Add("Interface Type: " + DRV["InterfaceType"].ToString());
            }
            else
            {
                DRVInfo.Add("Interface Type: No Data. (xNull)");
            }

            if (DRV["NeedsCleaning"] != null)
            {
                DRVInfo.Add("Needs Cleaning: " + DRV["NeedsCleaning"].ToString());
            }
            else
            {
                DRVInfo.Add("Needs Cleaning: No Data. (xNull)");
            }

            if (DRV["NumberOfMediaSupported"] != null)
            {
                DRVInfo.Add("Number Of Media Supported: " + DRV["NumberOfMediaSupported"].ToString());
            }
            else
            {
                DRVInfo.Add("Number Of Media Supported: No Data. (xNull)");
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

            if (DRV["SCSIBus"] != null)
            {
                DRVInfo.Add("SCSI Bus: " + DRV["SCSIBus"].ToString());
            }
            else
            {
                DRVInfo.Add("SCSI Bus: No Data. (xNull)");
            }

            if (DRV["SCSILogicalUnit"] != null)
            {
                DRVInfo.Add("SCSI Logical Unit: " + DRV["SCSILogicalUnit"].ToString());
            }
            else
            {
                DRVInfo.Add("SCSI Logical Unit: No Data. (xNull)");
            }

            if (DRV["SCSIPort"] != null)
            {
                DRVInfo.Add("SCSI Port: " + DRV["SCSIPort"].ToString());
            }
            else
            {
                DRVInfo.Add("SCSI Port: No Data. (xNull)");
            }

            if (DRV["SCSITargetId"] != null)
            {
                DRVInfo.Add("SCSI Target ID: " + DRV["SCSITargetId"].ToString());
            }
            else
            {
                DRVInfo.Add("SCSI Target ID: No Data. (xNull)");
            }

            if (DRV["StatusInfo"] != null)
            {
                DRVInfo.Add("Status Info: " + funcConvertInfo(DRV["StatusInfo"].ToString()));
            }
            else
            {
                DRVInfo.Add("Status Info: No Data. (xNull)");
            }

            intDRVLength = 48;
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

        public string getName()
        {
            return drvName;
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
    }
}
