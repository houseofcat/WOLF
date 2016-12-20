using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class SMBIOS
    {
        public List<string> SMInfo = new List<string>();
        public int intSMLength = 0;

        public SMBIOS()
        {

        }

        public SMBIOS(ManagementObject smbios)
        {
            setSMInfo(smbios);

            intSMLength = SMInfo.Count;
        }

        private void setSMInfo(ManagementObject smbios)
        {
            SMInfo.Clear();
            intSMLength = 0;

            if (smbios["Name"] != null)
            {
                SMInfo.Add("Name: " + smbios["Name"].ToString());
            }
            else
            {
                SMInfo.Add("Name: No Data (xNull).");
            }

            if (smbios["Caption"] != null)
            {
                SMInfo.Add("Caption: " + smbios["Caption"].ToString());
            }
            else
            {
                SMInfo.Add("Caption: No Data (xNull).");
            }

            if (smbios["StartingAddress"] != null)
            {
                SMInfo.Add("Starting Address: " + smbios["StartingAddress"].ToString() + " KB");
            }
            else
            {
                SMInfo.Add("Starting Address: No Data (xNull).");
            }

            if (smbios["EndingAddress"] != null)
            {
                SMInfo.Add("Ending Address: " + smbios["EndingAddress"].ToString() + " KB");
            }
            else
            {
                SMInfo.Add("Ending Address: No Data (xNull).");
            }

            if (smbios["Description"] != null)
            {
                SMInfo.Add("Description: " + smbios["Description"].ToString());
            }
            else
            {
                SMInfo.Add("Description: No Data (xNull).");
            }

            if (smbios["DeviceID"] != null)
            {
                SMInfo.Add("Device ID: " + smbios["DeviceID"].ToString());
            }
            else
            {
                SMInfo.Add("Device ID: No Data (xNull).");
            }


            if (smbios["SystemName"] != null)
            {
                SMInfo.Add("System Name: " + smbios["SystemName"].ToString());
            }
            else
            {
                SMInfo.Add("System Name: No Data (xNull).");
            }

            if (smbios["SystemCreationClassName"] != null)
            {
                SMInfo.Add("System Creation Class Name: " + smbios["SystemCreationClassName"].ToString());
            }
            else
            {
                SMInfo.Add("System Creation Class Name: No Data (xNull).");
            }

            if (smbios["CreationClassName"] != null)
            {
                SMInfo.Add("Creation Class Name: " + smbios["CreationClassName"].ToString());
            }
            else
            {
                SMInfo.Add("Creation Class Name: No Data (xNull).");
            }

            if (smbios["Access"] != null)
            {
                SMInfo.Add("Access: " + smbios["Access"].ToString());
            }
            else
            {
                SMInfo.Add("Access: No Data (xNull).");
            }

            if (smbios["Availability"] != null)
            {
                SMInfo.Add("Availability: " + smbios["Availability"].ToString());
            }
            else
            {
                SMInfo.Add("Availability: No Data (xNull).");
            }

            if (smbios["BlockSize"] != null)
            {
                SMInfo.Add("BlockSize: " + smbios["BlockSize"].ToString());
            }
            else
            {
                SMInfo.Add("BlockSize: No Data (xNull).");
            }

            if (smbios["ConfigManagerErrorCode"] != null)
            {
                SMInfo.Add("Config Manager Error Code: " + funcConvertCMErrorCode(smbios["ConfigManagerErrorCode"].ToString()));
            }
            else
            {
                SMInfo.Add("Config Manager Error Code: No Data (xNull).");
            }

            if (smbios["ConfigManagerUserConfig"] != null)
            {
                SMInfo.Add("Config Manage rUser Config: " + smbios["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                SMInfo.Add("Config Manager User Config: No Data (xNull).");
            }

            if (smbios["CorrectableError"] != null)
            {
                SMInfo.Add("Correctable Error: " + smbios["CorrectableError"].ToString());
            }
            else
            {
                SMInfo.Add("Correctable Error: No Data (xNull).");
            }

            if (smbios["ErrorAccess"] != null)
            {
                SMInfo.Add("Error Access: " + smbios["ErrorAccess"].ToString());
            }
            else
            {
                SMInfo.Add("Error Access: No Data (xNull).");
            }

            if (smbios["ErrorAddress"] != null)
            {
                SMInfo.Add("Error Address: " + smbios["ErrorAddress"].ToString());
            }
            else
            {
                SMInfo.Add("Error Address: No Data (xNull).");
            }

            if (smbios["ErrorCleared"] != null)
            {
                SMInfo.Add("Error Cleared: " + smbios["ErrorCleared"].ToString());
            }
            else
            {
                SMInfo.Add("Error Cleared: No Data (xNull).");
            }

            if (smbios["ErrorDataOrder"] != null)
            {
                SMInfo.Add("Error Data Order: " + smbios["ErrorDataOrder"].ToString());
            }
            else
            {
                SMInfo.Add("Error Data Order: No Data (xNull).");
            }

            if (smbios["ErrorDescription"] != null)
            {
                SMInfo.Add("Error Description: " + smbios["ErrorDescription"].ToString());
            }
            else
            {
                SMInfo.Add("Error Description: No Data (xNull).");
            }

            if (smbios["ErrorInfo"] != null)
            {
                SMInfo.Add("Error Info: " + smbios["ErrorInfo"].ToString());
            }
            else
            {
                SMInfo.Add("Error Info: No Data (xNull).");
            }

            if (smbios["ErrorMethodology"] != null)
            {
                SMInfo.Add("Error Methodology: " + smbios["ErrorMethodology"].ToString());
            }
            else
            {
                SMInfo.Add("Error Methodology: No Data (xNull).");
            }

            if (smbios["ErrorResolution"] != null)
            {
                SMInfo.Add("Error Resolution: " + smbios["ErrorResolution"].ToString());
            }
            else
            {
                SMInfo.Add("ErrorResolution: No Data (xNull).");
            }

            if (smbios["ErrorTime"] != null)
            {
                SMInfo.Add("Error Time: " + smbios["ErrorTime"].ToString());
            }
            else
            {
                SMInfo.Add("Error Time: No Data (xNull).");
            }

            if (smbios["ErrorTransferSize"] != null)
            {
                SMInfo.Add("Error Transfer Size: " + smbios["ErrorTransferSize"].ToString());
            }
            else
            {
                SMInfo.Add("Error Transfer Size: No Data (xNull).");
            }

            if (smbios["InstallDate"] != null)
            {
                SMInfo.Add("Install Date: " + smbios["InstallDate"].ToString());
            }
            else
            {
                SMInfo.Add("Install Date: No Data (xNull).");
            }

            if (smbios["LastErrorCode"] != null)
            {
                SMInfo.Add("Last Error Code: " + smbios["LastErrorCode"].ToString());
            }
            else
            {
                SMInfo.Add("Last Error Code: No Data (xNull).");
            }

            if (smbios["NumberOfBlocks"] != null)
            {
                SMInfo.Add("Number Of Blocks: " + smbios["NumberOfBlocks"].ToString());
            }
            else
            {
                SMInfo.Add("Number Of Blocks: No Data (xNull).");
            }

            if (smbios["OtherErrorDescription"] != null)
            {
                SMInfo.Add("Other Error Description: " + smbios["OtherErrorDescription"].ToString());
            }
            else
            {
                SMInfo.Add("Other Error Description: No Data (xNull).");
            }

            if (smbios["PNPDeviceID"] != null)
            {
                SMInfo.Add("PNP Dev ID: " + smbios["PNPDeviceID"].ToString());
            }
            else
            {
                SMInfo.Add("PNP Dev ID: No Data (xNull).");
            }

            if (smbios["PowerManagementSupported"] != null)
            {
                SMInfo.Add("Power Management Supported: " + smbios["Power Management Supported"].ToString());
            }
            else
            {
                SMInfo.Add("Power Management Supported: No Data (xNull).");
            }

            if (smbios["Purpose"] != null)
            {
                SMInfo.Add("Purpose: " + smbios["Purpose"].ToString());
            }
            else
            {
                SMInfo.Add("Purpose: No Data (xNull).");
            }

            if (smbios["Status"] != null)
            {
                SMInfo.Add("Status: " + smbios["Status"].ToString());
            }
            else
            {
                SMInfo.Add("Status: No Data (xNull).");
            }

            if (smbios["StatusInfo"] != null)
            {
                SMInfo.Add("Status Info: " + smbios["StatusInfo"].ToString());
            }
            else
            {
                SMInfo.Add("Status Info: No Data (xNull).");
            }

            if (smbios["SystemLevelAddress"] != null)
            {
                SMInfo.Add("System LevelA ddress: " + smbios["SystemLevelAddress"].ToString());
            }
            else
            {
                SMInfo.Add("SystemLevelAddress: No Data (xNull).");
            }
        }

        private string funcConvertCMErrorCode(string Error)
        {
            if (Error == "0")
            {
                Error = "Device is working properly.";
            }
            else if (Error == "1")
            {
                Error = "Device is not configured correctly.";
            }
            else if (Error == "2")
            {
                Error = "Windows cannot load the driver for this device.";
            }
            else if (Error == "3")
            {
                Error = "Driver for this device may be corrupted.";
            }
            else if (Error == "4")
            {
                Error = "Device is not working properly. Driver/Registry error.";
            }
            else if (Error == "5")
            {
                Error = "Driver for the device requires a resource that Windows cannot manage.";
            }
            else if (Error == "6")
            {
                Error = "Boot configuration for the device conflicts with the other devices.";
            }
            else if (Error == "7")
            {
                Error = "Cannot filter.";
            }
            else if (Error == "8")
            {
                Error = "Driver loader for the device is missing.";
            }
            else if (Error == "9")
            {
                Error = "Device is not working properly. The controlling firmware" +
                        " is incorrectly reporting the resources for the device.";
            }
            else if (Error == "10")
            {
                Error = "Device cannot start.";
            }
            else if (Error == "11")
            {
                Error = "Device failed.";
            }
            else if (Error == "12")
            {
                Error = "Device cannot find enough free resources to use.";
            }
            else if (Error == "13")
            {
                Error = "Windows cannot verify the device's resources.";
            }
            else if (Error == "14")
            {
                Error = "Device cannot work properly until the computer is restarted.";
            }
            else if (Error == "15")
            {
                Error = "Device is not working properly due to a possible re-enumeration problem.";
            }
            else if (Error == "16")
            {
                Error = "Windows cannot identify all of the resources that the device uses.";
            }
            else if (Error == "17")
            {
                Error = "Device is requesting an unknown resource type.";
            }
            else if (Error == "18")
            {
                Error = "Device drivers must be reinstalled.";
            }
            else if (Error == "19")
            {
                Error = "Failure using the VxD loader.";
            }
            else if (Error == "20")
            {
                Error = "Registry might be corrupted.";
            }
            else if (Error == "21")
            {
                Error = "System failure. Windows is removing the device.";
            }
            else if (Error == "22")
            {
                Error = "Device is disabled.";
            }
            else if (Error == "23")
            {
                Error = "System failure.";
            }
            else if (Error == "24")
            {
                Error = "Device is not present, not working, or missing drivers.";
            }
            else if (Error == "25")
            {
                Error = "Windows is still setting up the device.";
            }
            else if (Error == "26")
            {
                Error = "Windows is still setting up the device.";
            }
            else if (Error == "27")
            {
                Error = "Device does not have a valid log configuration.";
            }
            else if (Error == "28")
            {
                Error = "Device drivers are not installed.";
            }
            else if (Error == "29")
            {
                Error = "Device is disabled. Firmware did not provide required resources.";
            }
            else if (Error == "30")
            {
                Error = "Device is using an IRQ resource that another device is using.";
            }
            else if (Error == "31")
            {
                Error = "Device is not working properly.  Windows cannot load the drivers.";
            }
            else
            {
                Error = "WOLF - Windows has no clue. (" + Error + ")";
            }

            return Error;
        }
    }
}
