using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class ThermalProbe
    {
        public List<string> TPInfo = new List<string>();

        public int intTPLength = 0;

        private string Name = "";

        public ThermalProbe()
        {

        }

        public ThermalProbe(ManagementObject Probe)
        {
            setProbeInfo(Probe);

            intTPLength = TPInfo.Count;
        }

        private void setProbeInfo(ManagementObject Probe)
        {
            intTPLength = 0;
            TPInfo.Clear();

            if (Probe["Description"] != null)
            {
                Name = Probe["Description"].ToString();
                TPInfo.Add("Description: " + Name);
            }
            else
            {
                TPInfo.Add("Description: No Data (xNull).");
            }

            if (Probe["CurrentReading"] != null)
            {
                TPInfo.Add("Current Reading: " + Probe["CurrentReading"].ToString());
            }
            else
            {
                TPInfo.Add("Current Reading: No Data (xNull).");
            }

            if (Probe["Accuracy"] != null)
            {
                TPInfo.Add("Accuracy: " + Probe["Accuracy"].ToString());
            }
            else
            {
                TPInfo.Add("Accuracy: No Data (xNull).");
            }

            if (Probe["Resolution"] != null)
            {
                TPInfo.Add("Resolution: " + Probe["Resolution"].ToString());
            }
            else
            {
                TPInfo.Add("Resolution: No Data (xNull).");
            }

            if (Probe["Tolerance"] != null)
            {
                TPInfo.Add("Tolerance: " + Probe["Tolerance"].ToString());
            }
            else
            {
                TPInfo.Add("Tolerance: No Data (xNull).");
            }

            if (Probe["MinReadable"] != null)
            {
                TPInfo.Add("Min Readable: " + Probe["MinReadable"].ToString());
            }
            else
            {
                TPInfo.Add("Min Readable: No Data (xNull).");
            }

            if (Probe["MaxReadable"] != null)
            {
                TPInfo.Add("Max Readable: " + Probe["MaxReadable"].ToString());
            }
            else
            {
                TPInfo.Add("Max Readable: No Data (xNull).");
            }

            if (Probe["Caption"] != null)
            {
                TPInfo.Add("Caption: " + Probe["Caption"].ToString());
            }
            else
            {
                TPInfo.Add("Caption: No Data (xNull).");
            }

            if (Probe["DeviceID"] != null)
            {
                TPInfo.Add("Device ID: " + Probe["DeviceID"].ToString());
            }
            else
            {
                TPInfo.Add("Device ID: No Data (xNull).");
            }

            if (Probe["ConfigManagerErrorCode"] != null)
            {
                TPInfo.Add("Config Manager Error Code: " + Probe["ConfigManagerErrorCode"].ToString());
            }
            else
            {
                TPInfo.Add("Config Manager Error Code: No Data (xNull).");
            }

            if (Probe["ConfigManagerUserConfig"] != null)
            {
                TPInfo.Add("Config Manager User Config: " + Probe["ConfigManagerUserConfig"].ToString());
            }
            else
            {
                TPInfo.Add("Config Manager User Config: No Data (xNull).");
            }

            if (Probe["SystemName"] != null)
            {
                TPInfo.Add("System Name: " + Probe["SystemName"].ToString());
            }
            else
            {
                TPInfo.Add("System Name: No Data (xNull).");
            }

            if (Probe["SystemCreationClassName"] != null)
            {
                TPInfo.Add("System Creation Class Name: " + Probe["SystemCreationClassName"].ToString());
            }
            else
            {
                TPInfo.Add("System Creation Class Name: No Data (xNull).");
            }

            if (Probe["CreationClassName"] != null)
            {
                TPInfo.Add("Creation Class Name: " + Probe["CreationClassName"].ToString());
            }
            else
            {
                TPInfo.Add("Creation Class Name: No Data (xNull).");
            }

            if (Probe["Status"] != null)
            {
                TPInfo.Add("Status: " + Probe["Status"].ToString());
            }
            else
            {
                TPInfo.Add("Status: No Data (xNull).");
            }

            if (Probe["StatusInfo"] != null)
            {
                TPInfo.Add("Status Info: " + Probe["StatusInfo"].ToString());
            }
            else
            {
                TPInfo.Add("Status Info: No Data (xNull).");
            }

            if (Probe["Availability"] != null)
            {
                TPInfo.Add("Availability: " + Probe["Availability"].ToString());
            }
            else
            {
                TPInfo.Add("Availability: No Data (xNull).");
            }

            if (Probe["ErrorCleared"] != null)
            {
                TPInfo.Add("Error Cleared: " + Probe["ErrorCleared"].ToString());
            }
            else
            {
                TPInfo.Add("Error Cleared: No Data (xNull).");
            }

            if (Probe["ErrorDescription"] != null)
            {
                TPInfo.Add("Error Description: " + Probe["ErrorDescription"].ToString());
            }
            else
            {
                TPInfo.Add("Error Description: No Data (xNull).");
            }

            if (Probe["InstallDate"] != null)
            {
                TPInfo.Add("Install Date: " + Probe["InstallDate"].ToString());
            }
            else
            {
                TPInfo.Add("Install Date: No Data (xNull).");
            }

            if (Probe["IsLinear"] != null)
            {
                TPInfo.Add("Is Linear: " + Probe["IsLinear"].ToString());
            }
            else
            {
                TPInfo.Add("Is Linear: No Data (xNull).");
            }

            if (Probe["LastErrorCode"] != null)
            {
                TPInfo.Add("Last Error Code: " + Probe["LastErrorCode"].ToString());
            }
            else
            {
                TPInfo.Add("Last Error Code: No Data (xNull).");
            }

            if (Probe["LowerThresholdCritical"] != null)
            {
                TPInfo.Add("Lower Threshold Critical: " + Probe["LowerThresholdCritical"].ToString());
            }
            else
            {
                TPInfo.Add("Lower Threshold Critical: No Data (xNull).");
            }

            if (Probe["LowerThresholdFatal"] != null)
            {
                TPInfo.Add("Lower Threshold Fatal: " + Probe["LowerThresholdFatal"].ToString());
            }
            else
            {
                TPInfo.Add("Lower Threshold Fatal: No Data (xNull).");
            }

            if (Probe["LowerThresholdNonCritical"] != null)
            {
                TPInfo.Add("Lower Threshold Noncritical: " + Probe["LowerThresholdNonCritical"].ToString());
            }
            else
            {
                TPInfo.Add("Lower Threshold Noncritical: No Data (xNull).");
            }

            if (Probe["UpperThresholdCritical"] != null)
            {
                TPInfo.Add("Upper Threshold Critical: " + Probe["UpperThresholdCritical"].ToString());
            }
            else
            {
                TPInfo.Add("Upper Threshold Critical: No Data (xNull).");
            }

            if (Probe["UpperThresholdFatal"] != null)
            {
                TPInfo.Add("Upper Threshold Fatal: " + Probe["UpperThresholdFatal"].ToString());
            }
            else
            {
                TPInfo.Add("Upper Threshold Fatal: No Data (xNull).");
            }

            if (Probe["UpperThresholdNonCritical"] != null)
            {
                TPInfo.Add("Upper Threshold Noncritical: " + Probe["UpperThresholdNonCritical"].ToString());
            }
            else
            {
                TPInfo.Add("UpperThresholdNonCritical: No Data (xNull).");
            }

            if (Probe["Name"] != null)
            {
                TPInfo.Add("Name: " + Probe["Name"].ToString());
            }
            else
            {
                TPInfo.Add("Name: No Data (xNull).");
            }

            if (Probe["NominalReading"] != null)
            {
                TPInfo.Add("Nominal Reading: " + Probe["NominalReading"].ToString());
            }
            else
            {
                TPInfo.Add("Nominal Reading: No Data (xNull).");
            }

            if (Probe["NormalMax"] != null)
            {
                TPInfo.Add("Normal Max: " + Probe["NormalMax"].ToString());
            }
            else
            {
                TPInfo.Add("Normal Max: No Data (xNull).");
            }

            if (Probe["NormalMin"] != null)
            {
                TPInfo.Add("Normal Min: " + Probe["NormalMin"].ToString());
            }
            else
            {
                TPInfo.Add("Normal Min: No Data (xNull).");
            }

            if (Probe["PNPDeviceID"] != null)
            {
                TPInfo.Add("PNP Device ID: " + Probe["PNPDeviceID"].ToString());
            }
            else
            {
                TPInfo.Add("PNP Device ID: No Data (xNull).");
            }

            if (Probe["PowerManagementSupported"] != null)
            {
                TPInfo.Add("Power Management Supported: " + Probe["PowerManagementSupported"].ToString());
            }
            else
            {
                TPInfo.Add("Power Management Supported: No Data (xNull).");
            }
        }

        public string getName()
        {
            return this.Name;
        }
    }
}
