using System.Collections.Generic;
using System.Management;

namespace Wolf.SoftSpec
{
    class DiskPartition
    {
        public List<string> PartitionInfo = new List<string>();
        public int intPRTLength = 0;
        private string prtName = "";
        private string prtSize = "";

        public DiskPartition(ManagementObject Input)
        {
            setPartitionInfo(Input);
        }

        private void setPartitionInfo(ManagementObject PRT)
        {
            PartitionInfo.Clear();
            intPRTLength = 0;

            if (PRT["DeviceID"] != null)
            {
                PartitionInfo.Add("Device ID: " + PRT["DeviceID"].ToString());
            }
            else
            {
                PartitionInfo.Add("Device ID: No Data. (xNull)");
            }

            if (PRT["Name"] != null)
            {
                prtName = PRT["Name"].ToString();
                PartitionInfo.Add("Name: " + prtName);
            }
            else
            {
                PartitionInfo.Add("Name: No Data. (xNull)");
            }


            if (PRT["Type"] != null)
            {
                PartitionInfo.Add("Type: " + funcConvertInfo(PRT["Type"].ToString()));
            }
            else
            {
                PartitionInfo.Add("Type: No Data. (xNull)");
            }

            if (PRT["Purpose"] != null)
            {
                PartitionInfo.Add("Purpose: " + prtName);
            }
            else
            {
                PartitionInfo.Add("Name: No Data. (xNull)");
            }

            if (PRT["Bootable"] != null)
            {
                PartitionInfo.Add("Bootable: " + PRT["Bootable"].ToString());
            }
            else
            {
                PartitionInfo.Add("Bootable: No Data. (xNull)");
            }

            if (PRT["BootPartition"] != null)
            {
                PartitionInfo.Add("Boot Partition: " + PRT["BootPartition"].ToString());
            }
            else
            {
                PartitionInfo.Add("Boot Partition: No Data. (xNull)");
            }

            if (PRT["Caption"] != null)
            {
                PartitionInfo.Add("Caption: " + PRT["Caption"].ToString());
            }
            else
            {
                PartitionInfo.Add("Caption: No Data. (xNull)");
            }

            if (PRT["Index"] != null)
            {
                PartitionInfo.Add("Index: " + PRT["Index"].ToString());
            }
            else
            {
                PartitionInfo.Add("Index: No Data. (xNull)");
            }

            if (PRT["Status"] != null)
            {
                PartitionInfo.Add("Status: " + PRT["Status"].ToString());
            }
            else
            {
                PartitionInfo.Add("Status: No Data. (xNull)");
            }

            if (PRT["StatusInfo"] != null)
            {
                PartitionInfo.Add("Status Info: " + funcConvertInfo(PRT["StatusInfo"].ToString()));
            }
            else
            {
                PartitionInfo.Add("Status Info: No Data. (xNull)");
            }

            if (PRT["Size"] != null)
            {
                prtSize = funcConvertSize(PRT["Size"].ToString());
                PartitionInfo.Add("Size: " + prtSize);
            }
            else
            {
                PartitionInfo.Add("Size: No Data. (xNull)");
            }

            if (PRT["NumberOfBlocks"] != null)
            {
                PartitionInfo.Add("Number Of Blocks: " + PRT["NumberOfBlocks"].ToString());
            }
            else
            {
                PartitionInfo.Add("Number Of Blocks: No Data. (xNull)");
            }

            if (PRT["HiddenSectors"] != null)
            {
                PartitionInfo.Add("Hidden Sectors: " + PRT["HiddenSectors"].ToString());
            }
            else
            {
                PartitionInfo.Add("Hidden Sectors: No Data. (xNull)");
            }

            if (PRT["PrimaryPartition"] != null)
            {
                PartitionInfo.Add("Primary Partition: " + PRT["PrimaryPartition"].ToString());
            }
            else
            {
                PartitionInfo.Add("Primary Partition: No Data. (xNull)");
            }

            if (PRT["Description"] != null)
            {
                PartitionInfo.Add("Description: " + PRT["Description"].ToString());
            }
            else
            {
                PartitionInfo.Add("Description: No Data. (xNull)");
            }

            if (PRT["ConfigManagerErrorCode"] != null)
            {
                PartitionInfo.Add("Config Manager Error Code: " + PRT["ConfigManagerErrorCode"].ToString());
            }
            else
            {
                PartitionInfo.Add("Config Manager Error Code: No Data. (xNull)");
            }

            if (PRT["ConfigManagerUserConfig"] != null)
            {
                PartitionInfo.Add("Config Manager User Config: " + PRT["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                PartitionInfo.Add("Config Manager User Config: No Data. (xNull)");
            }

            if (PRT["SystemName"] != null)
            {
                PartitionInfo.Add("System Name: " + PRT["SystemName"].ToString());
            }
            else
            {
                PartitionInfo.Add("System Name: No Data. (xNull)");
            }

            if (PRT["SystemCreationClassName"] != null)
            {
                PartitionInfo.Add("System Creation Class Name: " + PRT["SystemCreationClassName"].ToString());
            }
            else
            {
                PartitionInfo.Add("System Creation Class Name: No Data. (xNull)");
            }

            if (PRT["CreationClassName"] != null)
            {
                PartitionInfo.Add("Creation Class Name: " + PRT["CreationClassName"].ToString());
            }
            else
            {
                PartitionInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (PRT["Availability"] != null)
            {
                PartitionInfo.Add("Availability: " + funcConvertAvail(PRT["Availability"].ToString()));
            }
            else
            {
                PartitionInfo.Add("Availability: No Data. (xNull)");
            }

            if (PRT["ErrorCleared"] != null)
            {
                PartitionInfo.Add("Error Cleared: " + PRT["ErrorCleared"].ToString());
            }
            else
            {
                PartitionInfo.Add("Error Cleared: No Data. (xNull)");
            }

            if (PRT["LastErrorCode"] != null)
            {
                PartitionInfo.Add("Last Error Code: " + PRT["LastErrorCode"].ToString());
            }
            else
            {
                PartitionInfo.Add("Last Error Code: No Data. (xNull)");
            }

            if (PRT["ErrorDescription"] != null)
            {
                PartitionInfo.Add("Error Description: " + PRT["ErrorDescription"].ToString());
            }
            else
            {
                PartitionInfo.Add("Error Description: No Data. (xNull)");
            }

            if (PRT["ErrorMethodology"] != null)
            {
                PartitionInfo.Add("Error Methodology: " + PRT["ErrorMethodology"].ToString());
            }
            else
            {
                PartitionInfo.Add("Error Methodology: No Data. (xNull)");
            }


            if (PRT["InstallDate"] != null)
            {
                PartitionInfo.Add("Install Date: " + PRT["InstallDate"].ToString());
            }
            else
            {
                PartitionInfo.Add("Install Date: No Data. (xNull)");
            }

            if (PRT["PNPDeviceID"] != null)
            {
                PartitionInfo.Add("PNP Device ID: " + PRT["PNPDeviceID"].ToString());
            }
            else
            {
                PartitionInfo.Add("PNP Device ID: No Data. (xNull)");
            }

            if (PRT["PowerManagementSupported"] != null)
            {
                PartitionInfo.Add("Power Management Supported: " + PRT["PowerManagementSupported"].ToString());
            }
            else
            {
                PartitionInfo.Add("Power Management Supported: No Data. (xNull)");
            }

            intPRTLength = 27;
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
            return this.prtName;
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
