using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class BIOS
    {
        public List<string> BIOSInfo = new List<string>();
        public int intBIOSLength = 0;

        string Manufacturer = "";
        string Name = "";
        string SMBIOS = "";
        string PrimaryBios = "";
        string Serial = "";
        string Date = "";

        public BIOS()
        {

        }

        public BIOS(ManagementObject input)
        {
            BIOSInfo.Clear();
            intBIOSLength = 0;

            funcPopulateBIOS(input);
        }

        private void funcPopulateBIOS(ManagementObject input)
        {
            if (input["PrimaryBIOS"] != null)
            {
                PrimaryBios = input["PrimaryBIOS"].ToString();

                BIOSInfo.Add("Primary BIOS: " + PrimaryBios);
            }
            else
            {
                BIOSInfo.Add("Primary BIOS:  No Data. (xNull)");
            }

            if (input["Manufacturer"] != null)
            {
                Manufacturer = input["Manufacturer"].ToString();
                BIOSInfo.Add("Manufacturer: " + Manufacturer);
            }
            else
            {
                BIOSInfo.Add("Manufacturer:  No Data. (xNull)");
            }

            if (input["SerialNumber"] != null)
            {
                Serial = input["SerialNumber"].ToString();
                BIOSInfo.Add("Serial: " + Serial);
            }
            else
            {
                BIOSInfo.Add("Serial:  No Data. (xNull)");
            }

            if (input["Name"] != null)
            {
                string temp = "";

                temp = input["Name"].ToString();
                temp = temp.Replace("BIOS Date: ", "");

                BIOSInfo.Add("Name: " + temp);
            }
            else
            {
                BIOSInfo.Add("Name:  No Data. (xNull)");
            }

            if (input["Caption"] != null)
            {
                string temp = "";

                temp = input["Caption"].ToString();
                temp = temp.Replace("BIOS Date: ", "");

                BIOSInfo.Add("Caption: " + temp);
            }
            else
            {
                BIOSInfo.Add("Caption:  No Data. (xNull)");
            }

            if (input["ReleaseDate"] != null)
            {
                Date = ManagementDateTimeConverter.ToDateTime(input["ReleaseDate"].ToString()).ToString();
                BIOSInfo.Add("Release Date: " + Date);
            }
            else
            {
                BIOSInfo.Add("Release Date:  No Data. (xNull)");
            }

            if (input["BuildNumber"] != null)
            {
                BIOSInfo.Add("Build Number: " + input["BuildNumber"].ToString());
            }
            else
            {
                BIOSInfo.Add("Build Number:  No Data. (xNull)");
            }

            if (input["Version"] != null)
            {
                BIOSInfo.Add("Version: " + input["Version"].ToString());
            }
            else
            {
                BIOSInfo.Add("Version:  No Data. (xNull)");
            }

            if (input["BIOSVersion"] != null)
            {
                string[] newArray;

                newArray = input["BIOSVersion"] as string[];
                BIOSInfo.Add("BIOS Version: " + newArray[0].ToString());
            }
            else
            {
                BIOSInfo.Add("BIOS Version:  No Data. (xNull)");
            }

            if (input["SMBIOSBIOSVersion"] != null)
            {
                Name = input["SMBIOSBIOSVersion"].ToString();
                SMBIOS = Name + " v";
                BIOSInfo.Add("SMBIOS Version: " + input["SMBIOSBIOSVersion"].ToString());
            }
            else
            {
                BIOSInfo.Add("SMBIOS Version:  No Data. (xNull)");
            }

            if (input["SMBIOSMajorVersion"] != null)
            {
                SMBIOS += input["SMBIOSMajorVersion"].ToString() + ".";
                BIOSInfo.Add("SMBIOS Major Version: " + input["SMBIOSMajorVersion"].ToString());
            }
            else
            {
                BIOSInfo.Add("SMBIOS Major Version:  No Data. (xNull)");
            }

            if (input["SMBIOSMinorVersion"] != null)
            {
                SMBIOS += input["SMBIOSMinorVersion"].ToString();
                BIOSInfo.Add("SMBIOS Minor Version: " + input["SMBIOSMinorVersion"].ToString());
            }
            else
            {
                BIOSInfo.Add("SMBIOS Minor Version:  No Data. (xNull)");
            }

            if (input["CodeSet"] != null)
            {
                BIOSInfo.Add("Code Set: " + input["CodeSet"].ToString());
            }
            else
            {
                BIOSInfo.Add("Code Set:  No Data. (xNull)");
            }

            if (input["CurrentLanguage"] != null)
            {
                BIOSInfo.Add("Current Language: " + input["CurrentLanguage"].ToString());
            }
            else
            {
                BIOSInfo.Add("Current Language:  No Data. (xNull)");
            }

            if (input["LanguageEdition"] != null)
            {
                BIOSInfo.Add("Language Edition: " + input["LanguageEdition"].ToString());
            }
            else
            {
                BIOSInfo.Add("Language Edition:  No Data. (xNull)");
            }

            if (input["Description"] != null)
            {
                string temp = "";

                temp = input["Description"].ToString();
                temp = temp.Replace("BIOS Date: ", "");

                BIOSInfo.Add("Description: " + temp);
            }
            else
            {
                BIOSInfo.Add("Description:  No Data. (xNull)");
            }

            if (input["IdentificationCode"] != null)
            {
                BIOSInfo.Add("Identification Code: " + input["IdentificationCode"].ToString());
            }
            else
            {
                BIOSInfo.Add("Identification Code:  No Data. (xNull)");
            }

            if (input["InstallableLanguages"] != null)
            {
                BIOSInfo.Add("Installable Languages: " + input["InstallableLanguages"].ToString());
            }
            else
            {
                BIOSInfo.Add("Installable Languages:  No Data. (xNull)");
            }

            if (input["TargetOperatingSystem"] != null)
            {
                BIOSInfo.Add("Target Operating System: " + input["TargetOperatingSystem"].ToString());
            }
            else
            {
                BIOSInfo.Add("Target Operating System:  No Data. (xNull)");
            }

            if (input["OtherTargetOS"] != null)
            {
                BIOSInfo.Add("Other Target OS: " + input["OtherTargetOS"].ToString());
            }
            else
            {
                BIOSInfo.Add("Other Target OS:  No Data. (xNull)");
            }

            if (input["SoftwareElementID"] != null)
            {
                string temp = "";

                temp = input["SoftwareElementID"].ToString();
                temp = temp.Replace("BIOS Date: ", "");

                BIOSInfo.Add("Software Element ID: " + temp);
            }
            else
            {
                BIOSInfo.Add("Software Element ID:  No Data. (xNull)");
            }

            if (input["SoftwareElementState"] != null)
            {
                BIOSInfo.Add("Software Element State: " + input["SoftwareElementState"].ToString());
            }
            else
            {
                BIOSInfo.Add("Software Element State:  No Data. (xNull)");
            }

            if (input["Status"] != null)
            {
                BIOSInfo.Add("Status: " + input["Status"].ToString());
            }
            else
            {
                BIOSInfo.Add("Status:  No Data. (xNull)");
            }

            intBIOSLength = 23;
        }

        public string getBIOSManufacturer()
        {
            return this.Manufacturer;
        }

        public string getBIOSDate()
        {
            string temp = "";

            temp = this.Date.Replace("BIOS Date: ", "");

            return temp;
        }

        public string getBIOSVersion()
        {
            return this.SMBIOS;
        }

        public string getBIOSPrimary()
        {
            return this.PrimaryBios;
        }

        public string getBIOSName()
        {
            return this.Name;
        }

        public string getBIOSSerial()
        {
            return this.Serial;
        }
    }
}
