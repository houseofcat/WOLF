using System;
using System.Collections.Generic;
using System.Management;
using Wolf.WolfSpec;

namespace Wolf.HardSpec
{
    class GPU
    {
        public List<string> GPUInfo = new List<string>();
        public int intGPULength = 0;

        private string Model = "";
        private string Vendor = "";
        private string VRAM = "";
        private string Version = "";
        private string DriverVersion = "";
        private string[] DriverFiles;
        private string DriverINF = "";
        private string INFSec = "";
        private string CurrentOutput = "";

        //Empty constructor
        public GPU()
        {

        }

        public GPU(ManagementObject gpuInfo)
        {
            setGPUInfo(gpuInfo);

            intGPULength = GPUInfo.Count;
        }

        private void setGPUInfo(ManagementObject gpuInfo)
        {
            //Just verifying their is no bad data involved
            GPUInfo.Clear();
            intGPULength = 0;

            /*
            if (gpuInfo["AcceleratorCapabilities"] != null)
            {
                //Casting the object returned to an array.
                UInt16[] tempArray = (UInt16[])(gpuInfo["AcceleratorCapabilities"]);

                foreach(UInt16 temp in tempArray)
                {
                    if (temp == 0)
                    {
                        GPUInfo.Add("Accelerator Capabilities: Unknown");
                    }
                    else if (temp == 1)
                    {
                        GPUInfo.Add("Accelerator Capabilities: Other");
                    }
                    else if (temp == 2)
                    {
                        GPUInfo.Add("Accelerator Capabilities: Graphics");
                    }
                    else if (temp == 3)
                    {
                        GPUInfo.Add("Accelerator Capabilities: 3-D");
                    }
                    else
                    {
                        GPUInfo.Add("Accelerator Capabilities: No Data. (xNull)");
                    }
                }
            }*/

            if (gpuInfo["AdapterCompatibility"] != null)
            {
                string temp = gpuInfo["AdapterCompatibility"].ToString();

                if (temp.Contains("NVIDIA"))
                {
                    Vendor = "nVidia";
                }
                else if ((temp.Contains("AMD")) || (temp.Contains("Advanced")))
                {
                    Vendor = "AMD";
                }
                else if (temp.Contains("Intel"))
                {
                    Vendor = "Intel";
                }
                else
                {
                    Vendor = temp;
                }

                GPUInfo.Add("Adapter Compatibility: " + temp);
            }
            else
            {
                GPUInfo.Add("Adapter Compatibility: No Data. (xNull)");
            }

            if (gpuInfo["Name"] != null)
            {
                string temp = gpuInfo["Name"].ToString();

                if (Vendor == "AMD")
                {
                    Model = temp;
                }
                else if (Vendor == "Intel")
                {
                    Model = temp;
                    Model = Model.Replace("(R)", "");
                    Model = Model.Replace(" Express Chipset","");
                }

                GPUInfo.Add("Name: " + temp);
            }
            else
            {
                GPUInfo.Add("Name: No Data. (xNull)");
            }

            if (gpuInfo["Caption"] != null)
            {
                GPUInfo.Add("Caption: " + gpuInfo["Caption"].ToString());
            }
            else
            {
                GPUInfo.Add("Caption: No Data. (xNull)");
            }

            if (gpuInfo["Description"] != null)
            {
                GPUInfo.Add("Description: " + gpuInfo["Description"].ToString());
            }
            else
            {
                GPUInfo.Add("Description: No Data. (xNull)");
            }

            if (gpuInfo["VideoProcessor"] != null)
            {
                string temp = gpuInfo["VideoProcessor"].ToString();

                if (Vendor == "nVidia")
                {
                    Model = temp;
                }

                GPUInfo.Add("Video Processor: " + temp);
            }
            else
            {
                GPUInfo.Add("Video Processor: No Data. (xNull)");
            }

            if (gpuInfo["AdapterDACType"] != null)
            {
                GPUInfo.Add("Adapter DAC Type: " + gpuInfo["AdapterDACType"].ToString());
            }
            else
            {
                GPUInfo.Add("Adapter DAC Type: No Data. (xNull)");
            }

            if (gpuInfo["AdapterRAM"] != null)
            {
                UInt32 byteRAM = 0;

                UInt32.TryParse(gpuInfo["AdapterRAM"].ToString(), out byteRAM);

                string temp = Tools.convertToGBFromBytes(byteRAM.ToString());
                VRAM = temp;
                GPUInfo.Add("Adapter RAM: " + temp);
            }
            else
            {
                GPUInfo.Add("Adapter RAM: No Data. (xNull)");
            }

            if (gpuInfo["CurrentHorizontalResolution"] != null)
            {
                string temp = gpuInfo["CurrentHorizontalResolution"].ToString();
                CurrentOutput = temp;

                GPUInfo.Add("Current Horizontal Resolution: " + temp);
            }
            else
            {
                GPUInfo.Add("Current Horizontal Resolution: No Data. (xNull)");
            }

            if (gpuInfo["CurrentVerticalResolution"] != null)
            {
                string temp = gpuInfo["CurrentVerticalResolution"].ToString();
                CurrentOutput += " x " + temp;
                GPUInfo.Add("Current Vertical Resolution: " + temp);
            }
            else
            {
                GPUInfo.Add("Current Vertical Resolution: No Data. (xNull)");
            }

            if (gpuInfo["CurrentRefreshRate"] != null)
            {
                string temp = gpuInfo["CurrentRefreshRate"].ToString();
                CurrentOutput += " @ " + temp + " Hz";
                GPUInfo.Add("Current Refresh Rate: " + gpuInfo["CurrentRefreshRate"] + " Hz");
            }
            else
            {
                GPUInfo.Add("Current Refresh Rate: No Data. (xNull)");
            }

            if (gpuInfo["CurrentBitsPerPixel"] != null)
            {
                string temp = gpuInfo["CurrentBitsPerPixel"].ToString();
                CurrentOutput += ", " + temp + "-bit";
                GPUInfo.Add("Current Bits Per Pixel: " + temp + "-bit");
            }
            else
            {
                GPUInfo.Add("Current Bits Per Pixel: No Data. (xNull)");
            }

            if (gpuInfo["DitherType"] != null)
            {
                if (gpuInfo["DitherType"].ToString() == "1")
                {
                    GPUInfo.Add("Dither Type: No Dithering");
                }
                else if (gpuInfo["DitherType"].ToString() == "2")
                {
                    GPUInfo.Add("Dither Type: Dithering with a coarse brush.");
                }
                else if (gpuInfo["DitherType"].ToString() == "3")
                {
                    GPUInfo.Add("Dither Type: Dithering with a fine brush.");
                }
                else if (gpuInfo["DitherType"].ToString() == "4")
                {
                    GPUInfo.Add("Dither Type: Line art dithering.");
                }
                else if (gpuInfo["DitherType"].ToString() == "5")
                {
                    GPUInfo.Add("Dither Type: Device does gray scaling.");
                }
                else
                {
                    GPUInfo.Add("Dither Type: Dithering is custom. (" + gpuInfo["DitherType"].ToString() + ")");
                }
            }
            else
            {
                GPUInfo.Add("Dither Type: No Data. (xNull)");
            }

            if (gpuInfo["Availability"] != null)
            {
                string temp = funcConvertAvail(gpuInfo["Availability"].ToString());

                GPUInfo.Add("Availability: " + temp);
            }
            else
            {
                GPUInfo.Add("Availability: No Data. (xNull)");
            }

            if (gpuInfo["Status"] != null)
            {
                GPUInfo.Add("Status: " + gpuInfo["Status"].ToString());
            }
            else
            {
                GPUInfo.Add("Status: No Data. (xNull)");
            }

            if (gpuInfo["StatusInfo"] != null)
            {
                string temp = gpuInfo["StatusInfo"].ToString();
                if (temp == "1")
                {
                    GPUInfo.Add("Status Info: Other");
                }
                else if (temp == "2")
                {
                    GPUInfo.Add("Status Info: Unknown");
                }
                else if (temp == "3")
                {
                    GPUInfo.Add("Status Info: Enabled");
                }
                else if (temp == "4")
                {
                    GPUInfo.Add("Status Info: Disabled");
                }
                else if (temp == "5")
                {
                    GPUInfo.Add("Status Info: Not Applicable");
                }
                else
                {
                    GPUInfo.Add("Status Info: Not Accounted For");
                }
            }
            else
            {
                GPUInfo.Add("Status Info: No Data. (xNull)");
            }

            if (gpuInfo["MinRefreshRate"] != null)
            {
                GPUInfo.Add("Min Refresh Rate: " + gpuInfo["MinRefreshRate"].ToString() + " Hz");
            }
            else
            {
                GPUInfo.Add("Min Refresh Rate: No Data. (xNull)");
            }

            if (gpuInfo["MaxRefreshRate"] != null)
            {
                GPUInfo.Add("Max Refresh Rate: " + gpuInfo["MaxRefreshRate"].ToString() + " Hz");
            }
            else
            {
                GPUInfo.Add("Max Refresh Rate: No Data. (xNull)");
            }

            if (gpuInfo["DriverVersion"] != null)
            {
                string temp = gpuInfo["DriverVersion"].ToString();

                DriverVersion = temp;
                GPUInfo.Add("Driver Version: " + temp);
            }
            else
            {
                GPUInfo.Add("Driver Version: No Data. (xNull)");
            }

            if (gpuInfo["DriverDate"] != null)
            {
                string format = "G";
                DateTime tempDate = new DateTime(1900, 1, 1);

                tempDate = ManagementDateTimeConverter.ToDateTime(gpuInfo["DriverDate"].ToString());
                GPUInfo.Add("Driver Date: " + tempDate.ToString(format));
            }
            else
            {
                GPUInfo.Add("Driver Date: No Data. (xNull)");
            }

            if (gpuInfo["InstalledDisplayDrivers"] != null)
            {
                string[] temp = (gpuInfo["InstalledDisplayDrivers"].ToString()).Split(',');
                DriverFiles = temp;
                short x = 0;

                foreach (string temp2 in temp)
                {
                    GPUInfo.Add("Installed Display Driver " + x + ": " + temp2);
                    x++;
                }
            }
            else
            {
                GPUInfo.Add("Installed Display Drivers: No Data. (xNull)");
            }

            if (gpuInfo["InfFilename"] != null)
            {
                string temp = gpuInfo["InfFilename"].ToString();

                DriverINF = temp;

                GPUInfo.Add("INF Filename: " + temp);
            }
            else
            {
                GPUInfo.Add("INF Filename: No Data. (xNull)");
            }

            if (gpuInfo["InfSection"] != null)
            {
                string temp = gpuInfo["InfSection"].ToString();

                INFSec = temp;
                GPUInfo.Add("INF Section: " + temp);
            }
            else
            {
                GPUInfo.Add("INF Section: No Data. (xNull)");
            }

            if (gpuInfo["VideoArchitecture"] != null)
            {
                GPUInfo.Add("Video Architecture: " + funcConvertVideoArch(gpuInfo["VideoArchitecture"].ToString()));
            }
            else
            {
                GPUInfo.Add("Video Architecture: No Data. (xNull)");
            }

            if (gpuInfo["VideoMemoryType"] != null)
            {
                GPUInfo.Add("Video Memory Type: " + funcConvertVideoMemType(gpuInfo["VideoMemoryType"].ToString()));
            }
            else
            {
                GPUInfo.Add("Video Memory Type: No Data. (xNull)");
            }

            if (gpuInfo["VideoMode"] != null)
            {
                GPUInfo.Add("Video Mode: " + gpuInfo["VideoMode"].ToString());
            }
            else
            {
                GPUInfo.Add("Video Mode: No Data. (xNull)");
            }

            if (gpuInfo["VideoModeDescription"] != null)
            {
                GPUInfo.Add("Video Mode Description: " + gpuInfo["VideoModeDescription"].ToString());
            }
            else
            {
                GPUInfo.Add("Video Mode Description: No Data. (xNull)");
            }

            if (gpuInfo["PNPDeviceID"] != null)
            {
                GPUInfo.Add("Device ID: " + gpuInfo["PNPDeviceID"].ToString());
            }
            else
            {
                GPUInfo.Add("Device ID: No Data. (xNull)");
            }

            if (gpuInfo["SystemCreationClassName"] != null)
            {
                GPUInfo.Add("System Creation Class Name: " + gpuInfo["SystemCreationClassName"].ToString());
            }
            else
            {
                GPUInfo.Add("System Creation Class Name: No Data. (xNull)");
            }

            if (gpuInfo["CreationClassName"] != null)
            {
                GPUInfo.Add("Creation Class Name: " + gpuInfo["CreationClassName"].ToString());
            }
            else
            {
                GPUInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (gpuInfo["SystemName"] != null)
            {
                GPUInfo.Add("System Name: " + gpuInfo["SystemName"].ToString());
            }
            else
            {
                GPUInfo.Add("System Name: No Data. (xNull)");
            }

            /*
            if (gpuInfo["CapabilityDescriptions"] != null)
            {
               * String Array
                GPUInfo.Add(": " + gpuInfo[""]);
            }
            else
            {
                GPUInfo.Add(": No Data. (xNull)");
            }*/

            if (gpuInfo["ColorTableEntries"] != null)
            {
                GPUInfo.Add("Color Table Entries: " + gpuInfo["ColorTableEntries"].ToString());
            }
            else
            {
                GPUInfo.Add("Color Table Entries: No Data. (xNull)");
            }

            if (gpuInfo["ConfigManagerErrorCode"] != null)
            {
                GPUInfo.Add("Config Manager Error Code: " +
                            funcConvertCMErrorCode(gpuInfo["ConfigManagerErrorCode"].ToString()));
            }
            else
            {
                GPUInfo.Add("Config Manager Error Code: No Data. (xNull)");
            }

            if (gpuInfo["ConfigManagerUserConfig"] != null)
            {
                GPUInfo.Add("Config Manager User Config: " + gpuInfo["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                GPUInfo.Add("Config Manager User Config: No Data. (xNull)");
            }

            if (gpuInfo["CurrentNumberOfColors"] != null)
            {
                GPUInfo.Add("Current Number Of Colors: " + gpuInfo["CurrentNumberOfColors"].ToString());
            }
            else
            {
                GPUInfo.Add("Current Number Of Colors: No Data. (xNull)");
            }

            if (gpuInfo["CurrentNumberOfColumns"] != null)
            {
                GPUInfo.Add("Current Number Of Columns: " + gpuInfo["CurrentNumberOfColumns"].ToString());
            }
            else
            {
                GPUInfo.Add("Current Number Of Columns: No Data. (xNull)");
            }

            if (gpuInfo["CurrentNumberOfRows"] != null)
            {
                GPUInfo.Add("Current Number Of Rows: " + gpuInfo["CurrentNumberOfRows"].ToString());
            }
            else
            {
                GPUInfo.Add("Current Number Of Rows: No Data. (xNull)");
            }

            if (gpuInfo["CurrentScanMode"] != null)
            {
                string temp = gpuInfo["CurrentScanMode"].ToString();

                if (temp == "1")
                {
                    temp = "Other";
                }
                else if (temp == "2")
                {
                    temp = "Unknown";
                }
                else if (temp == "3")
                {
                    temp = "Interlaced";
                }
                else if (temp == "4")
                {
                    temp = "Non-interlaced";
                }
                else
                {
                    temp = "Not recognized by Windows. " + "(" + gpuInfo["CurrentScanMode"].ToString() + ")";
                }

                GPUInfo.Add("Current Scan Mode: " + temp);
            }
            else
            {
                GPUInfo.Add("Current Scan Mode: No Data. (xNull)");
            }

            if (gpuInfo["DeviceID"] != null)
            {
                GPUInfo.Add("Device ID: " + gpuInfo["DeviceID"]);
            }
            else
            {
                GPUInfo.Add("Device ID: No Data. (xNull)");
            }

            if (gpuInfo["DeviceSpecificPens"] != null)
            {
                GPUInfo.Add("Device Specific Pens: " + gpuInfo["DeviceSpecificPens"].ToString());
            }
            else
            {
                GPUInfo.Add("Device Specific Pens: No Data. (xNull)");
            }

            if (gpuInfo["ErrorCleared"] != null)
            {
                GPUInfo.Add("Error Cleared: " + gpuInfo["ErrorCleared"].ToString());
            }
            else
            {
                GPUInfo.Add("Error Cleared: No Data. (xNull)");
            }

            if (gpuInfo["LastErrorCode"] != null)
            {
                GPUInfo.Add("Last Error Code: " + gpuInfo["LastErrorCode"].ToString());
            }
            else
            {
                GPUInfo.Add("Last Error Code: No Data. (xNull)");
            }

            if (gpuInfo["ErrorDescription"] != null)
            {
                GPUInfo.Add("Error Description: " + gpuInfo["ErrorDescription"].ToString());
            }
            else
            {
                GPUInfo.Add("Error Description: No Data. (xNull)");
            }

            if (gpuInfo["ICMIntent"] != null)
            {
                if (gpuInfo["ICMIntent"].ToString() == "1")
                {
                    GPUInfo.Add("ICM Intent: Saturation");
                }
                else if (gpuInfo["ICMIntent"].ToString() == "2")
                {
                    GPUInfo.Add("ICM Intent: Contrast");
                }
                else if (gpuInfo["ICMIntent"].ToString() == "3")
                {
                    GPUInfo.Add("ICM Intent: Exact Color");
                }
                else
                {
                    GPUInfo.Add("ICM Intent: Uknown");
                }
            }
            else
            {
                GPUInfo.Add("ICM Intent: No Data. (xNull)");
            }

            if (gpuInfo["ICMMethod"] != null)
            {
                if (gpuInfo["ICMMethod"].ToString() == "1")
                {
                    GPUInfo.Add("ICM Method: Disabled");
                }
                else if (gpuInfo["ICMMethod"].ToString() == "2")
                {
                    GPUInfo.Add("ICM Method: Windows");
                }
                else if (gpuInfo["ICMMethod"].ToString() == "3")
                {
                    GPUInfo.Add("ICM Method: Device Driver");
                }
                else if (gpuInfo["ICMMethod"].ToString() == "4")
                {
                    GPUInfo.Add("ICM Method: Destination Device");
                }
                else
                {
                    GPUInfo.Add("ICM Method: Unknown");
                }
            }
            else
            {
                GPUInfo.Add("ICM Method: No Data. (xNull)");
            }

            if (gpuInfo["InstallDate"] != null)
            {
                string format = "G";
                DateTime tempDate = new DateTime(1900, 1, 1);

                tempDate = ManagementDateTimeConverter.ToDateTime(gpuInfo["InstallDate"].ToString());
                GPUInfo.Add("Install Date: " + tempDate.ToString(format));
            }
            else
            {
                GPUInfo.Add("Install Date: No Data. (xNull)");
            }

            if (gpuInfo["MaxMemorySupported"] != null)
            {
                GPUInfo.Add("Max Memory Supported: " + gpuInfo["MaxMemorySupported"].ToString());
            }
            else
            {
                GPUInfo.Add("Max Memory Supported: No Data. (xNull)");
            }

            if (gpuInfo["MaxNumberControlled"] != null)
            {
                GPUInfo.Add("Max Number Controlled: " + gpuInfo["MaxNumberControlled"].ToString());
            }
            else
            {
                GPUInfo.Add("Max Number Controlled: No Data. (xNull)");
            }

            if (gpuInfo["Monochrome"] != null)
            {
                GPUInfo.Add("Monochrome: " + gpuInfo["Monochrome"].ToString());
            }
            else
            {
                GPUInfo.Add("Monochrome: No Data. (xNull)");
            }

            if (gpuInfo["NumberOfColorPlanes"] != null)
            {
                GPUInfo.Add("Number Of Color Planes: " + gpuInfo["NumberOfColorPlanes"].ToString());
            }
            else
            {
                GPUInfo.Add("Number Of Color Planes: No Data. (xNull)");
            }

            if (gpuInfo["NumberOfVideoPages"] != null)
            {
                GPUInfo.Add("Number Of Video Pages: " + gpuInfo["NumberOfVideoPages"].ToString());
            }
            else
            {
                GPUInfo.Add("Number Of Video Pages: No Data. (xNull)");
            }

            /*
            if (gpuInfo["PowerManagementCapabilities"] != null)
            {
                GPUInfo.Add("Power Management Capabilities: " + gpuInfo["PowerManagementCapabilities"]);
            }
            else
            {
                GPUInfo.Add("Power Management Capabilities: No Data. (xNull)");
            }
            */

            if (gpuInfo["ProtocolSupported"] != null)
            {
                GPUInfo.Add("Protocol Supported: " + funcConvertProtocol(gpuInfo["ProtocolSupported"].ToString()));
            }
            else
            {
                GPUInfo.Add("Protocol Supported: No Data. (xNull)");
            }

            if (gpuInfo["ReservedSystemPaletteEntries"] != null)
            {
                GPUInfo.Add("Reserved System Palette Entries: " + gpuInfo["ReservedSystemPaletteEntries"].ToString());
            }
            else
            {
                GPUInfo.Add("Reserved System Palette Entries: No Data. (xNull)");
            }

            if (gpuInfo["SpecificationVersion"] != null)
            {
                GPUInfo.Add("Specification Version: " + gpuInfo["SpecificationVersion"].ToString());
            }
            else
            {
                GPUInfo.Add("Specification Version: No Data. (xNull)");
            }

            if (gpuInfo["SystemPaletteEntries"] != null)
            {
                GPUInfo.Add("System Palette Entries: " + gpuInfo["SystemPaletteEntries"].ToString());
            }
            else
            {
                GPUInfo.Add("System Palette Entries: No Data. (xNull)");
            }

            if (gpuInfo["TimeOfLastReset"] != null)
            {
                string format = "G";
                DateTime tempDate = new DateTime(1900, 1, 1);

                tempDate = ManagementDateTimeConverter.ToDateTime(gpuInfo["TimeOfLastReset"].ToString());

                GPUInfo.Add("Time Of Last Reset: " + tempDate.ToString(format));
            }
            else
            {
                GPUInfo.Add("Time Of Last Reset: No Data. (xNull)");
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

            return Error;
        }

        private string funcConvertProtocol(string Protocol)
        {
            if (Protocol == "1")
            {
                Protocol = "Other";
            }
            else if (Protocol == "2")
            {
                Protocol = "Unknown";
            }
            else if (Protocol == "3")
            {
                Protocol = "EISA";
            }
            else if (Protocol == "4")
            {
                Protocol = "ISA";
            }
            else if (Protocol == "5")
            {
                Protocol = "PCI";
            }
            else if (Protocol == "6")
            {
                Protocol = "ATA or ATAPI";
            }
            else if (Protocol == "7")
            {
                Protocol = "Flexible Diskette";
            }
            else if (Protocol == "8")
            {
                Protocol = "1496";
            }
            else if (Protocol == "9")
            {
                Protocol = "SCSI Parallel Interface";
            }
            else if (Protocol == "10")
            {
                Protocol = "SCSI Fibre Channel Protocol";
            }
            else if (Protocol == "11")
            {
                Protocol = "SCSI Serial Bus Protocol";
            }
            else if (Protocol == "12")
            {
                Protocol = "SCSI Serial Bus Protocol-2 (1394)";
            }
            else if (Protocol == "13")
            {
                Protocol = "SCSI Serial Storage Architecture";
            }
            else if (Protocol == "14")
            {
                Protocol = "VESA";
            }
            else if (Protocol == "15")
            {
                Protocol = "PCMCIA";
            }
            else if (Protocol == "16")
            {
                Protocol = "Universal Serial Bus";
            }
            else if (Protocol == "17")
            {
                Protocol = "Parallel Protocol";
            }
            else if (Protocol == "18")
            {
                Protocol = "ESCON";
            }
            else if (Protocol == "19")
            {
                Protocol = "Diagnostic";
            }
            else if (Protocol == "20")
            {
                Protocol = "I2C";
            }
            else if (Protocol == "21")
            {
                Protocol = "Power";
            }
            else if (Protocol == "22")
            {
                Protocol = "HIPPI";
            }
            else if (Protocol == "23")
            {
                Protocol = "MultiBus";
            }
            else if (Protocol == "24")
            {
                Protocol = "VME";
            }
            else if (Protocol == "25")
            {
                Protocol = "IPI";
            }
            else if (Protocol == "26")
            {
                Protocol = "IEEE-488";
            }
            else if (Protocol == "27")
            {
                Protocol = "RS232";
            }
            else if (Protocol == "28")
            {
                Protocol = "IEEE 802.3 10BASE5";
            }
            else if (Protocol == "29")
            {
                Protocol = "IEEE 802.3 10BASE2";
            }
            else if (Protocol == "30")
            {
                Protocol = "IEEE 802.3 1BASE5";
            }
            else if (Protocol == "31")
            {
                Protocol = "IEEE 802.3 10BROAD36";
            }
            else if (Protocol == "32")
            {
                Protocol = "IEEE 802.3 100BASEVG";
            }
            else if (Protocol == "33")
            {
                Protocol = "IEEE 802.5 Token-Ring";
            }
            else if (Protocol == "34")
            {
                Protocol = "ANSI X3T9.5 FDDI";
            }
            else if (Protocol == "35")
            {
                Protocol = "MCA";
            }
            else if (Protocol == "36")
            {
                Protocol = "ESDI";
            }
            else if (Protocol == "37")
            {
                Protocol = "IDE";
            }
            else if (Protocol == "38")
            {
                Protocol = "CMD";
            }
            else if (Protocol == "39")
            {
                Protocol = "ST506";
            }
            else if (Protocol == "40")
            {
                Protocol = "DSSI";
            }
            else if (Protocol == "41")
            {
                Protocol = "QIC2";
            }
            else if (Protocol == "42")
            {
                Protocol = "Enhanced ATA/IDE";
            }
            else if (Protocol == "43")
            {
                Protocol = "AGP";
            }
            else if (Protocol == "44")
            {
                Protocol = "TWIRP (two-way infrared)";
            }
            else if (Protocol == "45")
            {
                Protocol = "FIR (fast infrared)";
            }
            else if (Protocol == "46")
            {
                Protocol = "SIR (serial infrared)";
            }
            else if (Protocol == "47")
            {
                Protocol = "IrBus";
            }

            return Protocol;
        }

        private string funcConvertVideoArch(string Arch)
        {
            if (Arch == "1")
            {
                Arch = "Other";
            }
            else if (Arch == "2")
            {
                Arch = "Unknown";
            }
            else if (Arch == "3")
            {
                Arch = "CGA";
            }
            else if (Arch == "4")
            {
                Arch = "EGA";
            }
            else if (Arch == "5")
            {
                Arch = "VGA";
            }
            else if (Arch == "6")
            {
                Arch = "SVGA";
            }
            else if (Arch == "7")
            {
                Arch = "MDA";
            }
            else if (Arch == "8")
            {
                Arch = "HGC";
            }
            else if (Arch == "9")
            {
                Arch = "MCGA";
            }
            else if (Arch == "10")
            {
                Arch = "8514A";
            }
            else if (Arch == "11")
            {
                Arch = "XGA";
            }
            else if (Arch == "12")
            {
                Arch = "Linear Frame Buffer";
            }
            else if (Arch == "160")
            {
                Arch = "PC-98";
            }

            return Arch;
        }

        private string funcConvertVideoMemType(string Memtype)
        {
            if (Memtype == "1")
            {
                Memtype = "Other";
            }
            else if (Memtype == "2")
            {
                Memtype = "Unknown";
            }
            else if (Memtype == "3")
            {
                Memtype = "VRAM";
            }
            else if (Memtype == "4")
            {
                Memtype = "DRAM";
            }
            else if (Memtype == "5")
            {
                Memtype = "SRAM";
            }
            else if (Memtype == "6")
            {
                Memtype = "WRAM";
            }
            else if (Memtype == "7")
            {
                Memtype = "EDO RAM";
            }
            else if (Memtype == "8")
            {
                Memtype = "Burst Synchronous DRAM";
            }
            else if (Memtype == "9")
            {
                Memtype = "Pipelined Burst SRAM";
            }
            else if (Memtype == "10")
            {
                Memtype = "CDRAM";
            }
            else if (Memtype == "11")
            {
                Memtype = "3DRAM";
            }
            else if (Memtype == "12")
            {
                Memtype = "SDRAM";
            }
            else if (Memtype == "13")
            {
                Memtype = "SGRAM";
            }

            return Memtype;
        }

        public string getVendor()
        {
            return Vendor;
        }

        public string getModel()
        {
            return Model;
        }

        public string getVersion()
        {
            return Version;
        }

        public string getVRAM()
        {
            return VRAM;
        }
        
        public string getDriverVersion()
        {
            return DriverVersion;
        }

        public string getINF()
        {
            return DriverINF;
        }

        public string[] getDriverFiles()
        {
            if (DriverFiles != null)
            {
                return DriverFiles;
            }
            else
            {
                string[] temp = { "N", "u", "l", "l" };
                return temp;
            }
        }

        public string getINFSEC()
        {
            return INFSec;
        }

        public string getOut()
        {
            return CurrentOutput;
        }
    }
}
