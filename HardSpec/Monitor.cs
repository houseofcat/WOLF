using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class Monitor
    {
        public List<string> MONInfo = new List<string>();
        public int intMonLength = 0;

        private string Name = "";
        private string Manufacturer = "";
        private string Type = "";
        private string DeviceID = "";
        private string Status = "";
        private string PNPID = "";

        public Monitor()
        {

        }

        public Monitor (ManagementObject monInfo)
        {
            setMonitorInfo(monInfo);

            intMonLength = MONInfo.Count;
        }

        private void setMonitorInfo(ManagementObject monInfo)
        {
            MONInfo.Clear();
            intMonLength = 0;

            if (monInfo["Name"] != null)
            {
                string temp = "";
                temp = monInfo["Name"].ToString();
                Name = temp;
                MONInfo.Add("Name: " + temp);
            }
            else
            {
                MONInfo.Add("Name: No Data (xNull).");
            }

            if (monInfo["MonitorManufacturer"] != null)
            {
                string temp = monInfo["MonitorManufacturer"].ToString();
                Manufacturer = temp;
                MONInfo.Add("Monitor Manufacturer: " + temp);
            }
            else
            {
                MONInfo.Add("Monitor Manufacturer: No Data (xNull).");
            }

            if (monInfo["MonitorType"] != null)
            {
                string temp = monInfo["MonitorType"].ToString();
                Type = temp;
                MONInfo.Add("Monitor Type: " + temp);
            }
            else
            {
                MONInfo.Add("Monitor Type: No Data (xNull).");
            }

            if (monInfo["Description"] != null)
            {
                MONInfo.Add("Description: " + monInfo["Description"].ToString());
            }
            else
            {
                MONInfo.Add("Description: No Data (xNull).");
            }

            if (monInfo["DeviceID"] != null)
            {
                string temp = monInfo["DeviceID"].ToString();
                DeviceID = temp;
                MONInfo.Add("Device ID: " + temp);
            }
            else
            {
                MONInfo.Add("Device ID: No Data (xNull).");
            }

            if (monInfo["PixelsPerXLogicalInch"] != null)
            {
                MONInfo.Add("Pixels Per X Logical Inch: " + monInfo["PixelsPerXLogicalInch"].ToString());
            }
            else
            {
                MONInfo.Add("Pixels Per X Logical Inch: No Data (xNull).");
            }

            if (monInfo["PixelsPerYLogicalInch"] != null)
            {
                MONInfo.Add("Pixels Per Y Logical Inch: " + monInfo["PixelsPerYLogicalInch"].ToString());
            }
            else
            {
                MONInfo.Add("Pixels Per Y Logical Inch: No Data (xNull).");
            }

            if (monInfo["Bandwidth"] != null)
            {
                MONInfo.Add("Bandwidth: " + monInfo["Bandwidth"].ToString());
            }
            else
            {
                MONInfo.Add("Bandwidth: No Data (xNull).");
            }

            if (monInfo["Availability"] != null)
            {
                MONInfo.Add("Availability: " + funcConvertAvail(monInfo["Availability"].ToString()));
            }
            else
            {
                MONInfo.Add("Availability: No Data (xNull).");
            }

            if (monInfo["Status"] != null)
            {
                MONInfo.Add("Status: " + monInfo["Status"].ToString());
            }
            else
            {
                MONInfo.Add("Status: No Data (xNull).");
            }

            if (monInfo["StatusInfo"] != null)
            {
                MONInfo.Add("Status Info: " + monInfo["StatusInfo"].ToString());
            }
            else
            {
                MONInfo.Add("Status Info: No Data (xNull).");
            }

            if (monInfo["PNPDeviceID"] != null)
            {
                string temp = monInfo["PNPDeviceID"].ToString();
                PNPID = temp;
                MONInfo.Add("PNP Dev ID: " + temp);
            }
            else
            {
                MONInfo.Add("PNP Dev ID: No Data (xNull).");
            }

            if (monInfo["ConfigManagerErrorCode"] != null)
            {
                string temp = funcConvertCMErrorCode(monInfo["ConfigManagerErrorCode"].ToString());
                Status = temp;
                MONInfo.Add("Config Manager Error Code: " + temp);
            }
            else
            {
                MONInfo.Add("Config Manager Error Code: No Data (xNull).");
            }

            if (monInfo["ConfigManagerUserConfig"] != null)
            {
                MONInfo.Add("Config Manager User Config: " + monInfo["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                MONInfo.Add("Config Manager User Config: No Data (xNull).");
            }

            if (monInfo["SystemCreationClassName"] != null)
            {
                MONInfo.Add("System Creation Class Name: " + monInfo["SystemCreationClassName"].ToString());
            }
            else
            {
                MONInfo.Add("System Creation Class Name: No Data (xNull).");
            }

            if (monInfo["CreationClassName"] != null)
            {
                MONInfo.Add("Creation Class Name: " + monInfo["CreationClassName"].ToString());
            }
            else
            {
                MONInfo.Add("Creation Class Name: No Data (xNull).");
            }

            if (monInfo["SystemName"] != null)
            {
                MONInfo.Add("System Name: " + monInfo["SystemName"].ToString());
            }
            else
            {
                MONInfo.Add("System Name: No Data (xNull).");
            }

            if (monInfo["DisplayType"] != null)
            {
                MONInfo.Add("Display Type: " + monInfo["DisplayType"].ToString());
            }
            else
            {
                MONInfo.Add("Display Type: No Data (xNull).");
            }

            if (monInfo["ErrorCleared"] != null)
            {
                MONInfo.Add("Error Cleared: " + monInfo["ErrorCleared"].ToString());
            }
            else
            {
                MONInfo.Add("Error Cleared: No Data (xNull).");
            }

            if (monInfo["ErrorDescription"] != null)
            {
                MONInfo.Add("Error Description: " + monInfo["ErrorDescription"].ToString());
            }
            else
            {
                MONInfo.Add("Error Description: No Data (xNull).");
            }

            if (monInfo["InstallDate"] != null)
            {
                MONInfo.Add("Install Date: " + monInfo["InstallDate"].ToString());
            }
            else
            {
                MONInfo.Add("Install Date: No Data (xNull).");
            }

            if (monInfo["IsLocked"] != null)
            {
                MONInfo.Add("Is Locked: " + monInfo["IsLocked"].ToString());
            }
            else
            {
                MONInfo.Add("Is Locked: No Data (xNull).");
            }

            if (monInfo["LastErrorCode"] != null)
            {
                MONInfo.Add("Last Error Code: " + monInfo["LastErrorCode"].ToString());
            }
            else
            {
                MONInfo.Add("Last Error Code: No Data (xNull).");
            }

            if (monInfo["PowerManagementSupported"] != null)
            {
                MONInfo.Add("Power Management Supported: " + monInfo["PowerManagementSupported"].ToString());
            }
            else
            {
                MONInfo.Add("Power Management Supported: No Data (xNull).");
            }

            if (monInfo["ScreenHeight"] != null)
            {
                MONInfo.Add("Screen Height: " + monInfo["ScreenHeight"].ToString());
            }
            else
            {
                MONInfo.Add("Screen Height: No Data (xNull).");
            }

            if (monInfo["ScreenWidth"] != null)
            {
                MONInfo.Add("Screen Width: " + monInfo["ScreenWidth"].ToString());
            }
            else
            {
                MONInfo.Add("Screen Width: No Data (xNull).");
            }
        }

        //Converts WMI number to meaningful String.
        private string funcConvertAvail(string Avail)
        {
            if (Avail == "0")
            {
                Avail = "Super Unknown";
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
                Avail = "Running or Full Power";
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
                Avail = "Offline";
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
                Avail = "Power Save Mode: Unknown";
            }
            else if (Avail == "14")
            {
                Avail = "Power Save Mode: Low Power Mode";
            }
            else if (Avail == "15")
            {
                Avail = "Power Save Mode: Standy";
            }
            else if (Avail == "15")
            {
                Avail = "Power Cycling";
            }
            else if (Avail == "15")
            {
                Avail = "Power Save Mode Warning";
            }

            return Avail;
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

        public string getName()
        {
            return Name;
        }

        public string getManu()
        {
            return Manufacturer;
        }

        public string getType()
        {
            return Type;
        }

        public string getStatus()
        {
            return Status;
        }

        public string getDeviceID()
        {
            return DeviceID;
        }

        public string getPNPID()
        {
            return PNPID;
        }
    }
}
