using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class SoundDevice
    {
        public List<string> SDInfo = new List<string>();
        public int intSDLength = 0;

        private string Name = "";
        private string Model = "";
        private string Manu = "";

        private bool OnGPU = false;

        public SoundDevice()
        {}

        public SoundDevice(ManagementObject soundInfo)
        {
            setSDInfo(soundInfo);

            intSDLength = SDInfo.Count;

            if ((Name.Contains("NVIDIA")) || (Name.Contains("nvidia")))
            {
                OnGPU = true;
            }
            else if (((Name.Contains("Advanced"))||(Name.Contains("amd"))||(Name.Contains("AMD"))))
            {
                OnGPU = true;
            }
        }

        private void setSDInfo(ManagementObject soundInfo)
        {
            SDInfo.Clear();
            intSDLength = 0;

            if (soundInfo["Caption"] != null)
            {
                Name = soundInfo["Caption"].ToString();
                SDInfo.Add("Caption: " + Name);
            }
            else
            {
                SDInfo.Add("Caption: No Data (xNull).");
            }

            if (soundInfo["Manufacturer"] != null)
            {
                Manu = soundInfo["Manufacturer"].ToString();
                SDInfo.Add("Manufacturer: " + Manu);
            }
            else
            {
                SDInfo.Add("Manufacturer: No Data (xNull).");
            }

            if (soundInfo["ProductName"] != null)
            {
                Model = soundInfo["ProductName"].ToString();
                SDInfo.Add("Product Name: " + Model);
            }
            else
            {
                SDInfo.Add("Product Name: No Data (xNull).");
            }

            if (soundInfo["Description"] != null)
            {
                SDInfo.Add("Description: " + soundInfo["Description"].ToString());
            }
            else
            {
                SDInfo.Add("Description: No Data (xNull).");
            }

            if (soundInfo["ConfigManagerErrorCode"] != null)
            {
                SDInfo.Add("Config Manager Error Code: " + funcConvertCMErrorCode(soundInfo["ConfigManagerErrorCode"].ToString()));
            }
            else
            {
                SDInfo.Add("Config Manager Error Code: No Data (xNull).");
            }

            if (soundInfo["ConfigManagerUserConfig"] != null)
            {
                SDInfo.Add("Config Manager User Config: " + soundInfo["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                SDInfo.Add("Config Manager User Config: No Data (xNull).");
            }

            if (soundInfo["SystemName"] != null)
            {
                SDInfo.Add("System Name: " + soundInfo["SystemName"].ToString());
            }
            else
            {
                SDInfo.Add("System Name: No Data (xNull).");
            }

            if (soundInfo["SystemCreationClassName"] != null)
            {
                SDInfo.Add("System Creation Class Name: " + soundInfo["SystemCreationClassName"].ToString());
            }
            else
            {
                SDInfo.Add("System Creation Class Name: No Data (xNull).");
            }

            if (soundInfo["CreationClassName"] != null)
            {
                SDInfo.Add("Creation Class Name: " + soundInfo["CreationClassName"].ToString());
            }
            else
            {
                SDInfo.Add("Creation Class Name: No Data (xNull).");
            }

            if (soundInfo["DeviceID"] != null)
            {
                SDInfo.Add("Device ID: " + soundInfo["DeviceID"].ToString());
            }
            else
            {
                SDInfo.Add("Device ID: No Data (xNull).");
            }

            if (soundInfo["Availability"] != null)
            {
                SDInfo.Add("Availability: " + funcConvertAvail(soundInfo["Availability"].ToString()));
            }
            else
            {
                SDInfo.Add("Availability: No Data (xNull).");
            }

            if (soundInfo["DMABufferSize"] != null)
            {
                SDInfo.Add("DMA Buffer Size: " + soundInfo["DMABufferSize"].ToString());
            }
            else
            {
                SDInfo.Add("DMA Buffer Size: No Data (xNull).");
            }

            if (soundInfo["ErrorCleared"] != null)
            {
                SDInfo.Add("Error Cleared: " + soundInfo["ErrorCleared"].ToString());
            }
            else
            {
                SDInfo.Add("Error Cleared: No Data (xNull).");
            }

            if (soundInfo["ErrorDescription"] != null)
            {
                SDInfo.Add("Error Description: " + soundInfo["ErrorDescription"].ToString());
            }
            else
            {
                SDInfo.Add("Error Description: No Data (xNull).");
            }

            if (soundInfo["InstallDate"] != null)
            {
                SDInfo.Add("Install Date: " + soundInfo["InstallDate"].ToString());
            }
            else
            {
                SDInfo.Add("Install Date: No Data (xNull).");
            }

            if (soundInfo["LastErrorCode"] != null)
            {
                SDInfo.Add("Last Error Code: " + soundInfo["LastErrorCode"].ToString());
            }
            else
            {
                SDInfo.Add("Last Error Code: No Data (xNull).");
            }

            if (soundInfo["MPU401Address"] != null)
            {
                SDInfo.Add("MPU401 Address: " + soundInfo["MPU401Address"].ToString());
            }
            else
            {
                SDInfo.Add("MPU401 Address: No Data (xNull).");
            }

            if (soundInfo["Name"] != null)
            {
                SDInfo.Add("Name: " + soundInfo["Name"].ToString());
            }
            else
            {
                SDInfo.Add("Name: No Data (xNull).");
            }

            if (soundInfo["PNPDeviceID"] != null)
            {
                SDInfo.Add("PNP Dev ID: " + soundInfo["PNPDeviceID"].ToString());
            }
            else
            {
                SDInfo.Add("PNP Dev ID: No Data (xNull).");
            }

            if (soundInfo["PowerManagementSupported"] != null)
            {
                SDInfo.Add("Power Management Supported: " + soundInfo["PowerManagementSupported"].ToString());
            }
            else
            {
                SDInfo.Add("Power Management Supported: No Data (xNull).");
            }

            if (soundInfo["Status"] != null)
            {
                SDInfo.Add("Status: " + soundInfo["Status"].ToString());
            }
            else
            {
                SDInfo.Add("Status: No Data (xNull).");
            }

            if (soundInfo["StatusInfo"] != null)
            {
                SDInfo.Add("Status Info: " + funcConvertStatusInfo(soundInfo["StatusInfo"].ToString()));
            }
            else
            {
                SDInfo.Add("Status Info: No Data (xNull).");
            }
        }

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

        private string funcConvertStatusInfo(string Info)
        {
            if (Info == "1")
            {
                Info = "Other";
            }
            else if (Info == "2")
            {
                Info = "Unknown";
            }
            else if (Info == "3")
            {
                Info = "Enabled";
            }
            else if (Info == "4")
            {
                Info = "Disabled";
            }
            else if (Info == "5")
            {
                Info = "Not Applicable";
            }

            return Info;
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

            return Error;
        }

        public string getName()
        {
            return this.Name;
        }

        public string getManufacturer()
        {
            return this.Manu;
        }

        public string getModel()
        {
            return this.Model;
        }

        public bool IsOnGPU()
        {
            return OnGPU;
        }
    }
}
