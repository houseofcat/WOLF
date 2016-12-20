using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace Wolf.HardSpec
{
    class Motherboard
    {
        public List<string> MBInfo = new List<string>();
        public int intMBLength = 0;

        private string Name = "";
        private string Manufacturer = "";
        private string Version = "";
        private string ProductId = "";
        private string Serial = "";

        public Motherboard()
        { }

        public Motherboard(ManagementObject mbInfo)
        {
            setMBInfo(mbInfo);

            intMBLength = MBInfo.Count;
        }

        private void setMBInfo(ManagementObject mbInfo)
        {
            MBInfo.Clear();
            intMBLength = 0;

            try
            {
                if (mbInfo["Caption"] != null)
                {
                    MBInfo.Add("Caption: " + mbInfo["Caption"].ToString());
                }
                else
                {
                    MBInfo.Add("Caption: No Data (xNull).");
                }

                if (mbInfo["CreationClassName"] != null)
                {
                    MBInfo.Add("Creation Class Name: " + mbInfo["CreationClassName"].ToString());
                }
                else
                {
                    MBInfo.Add("Creation Class Name: No Data (xNull).");
                }

                if (mbInfo["Description"] != null)
                {
                    MBInfo.Add("Description: " + mbInfo["Description"].ToString());
                }
                else
                {
                    MBInfo.Add("Description: No Data (xNull).");
                }

                if (mbInfo["Depth"] != null)
                {
                    MBInfo.Add("Depth: " + mbInfo["Depth"].ToString());
                }
                else
                {
                    MBInfo.Add("Depth: No Data (xNull).");
                }

                if (mbInfo["Height"] != null)
                {
                    MBInfo.Add("Height: " + mbInfo["Height"].ToString());
                }
                else
                {
                    MBInfo.Add("Height: No Data (xNull).");
                }

                if (mbInfo["Weight"] != null)
                {
                    MBInfo.Add("Weight: " + mbInfo["Weight"].ToString());
                }
                else
                {
                    MBInfo.Add("Weight: No Data (xNull).");
                }

                if (mbInfo["Width"] != null)
                {
                    MBInfo.Add("Width: " + mbInfo["Width"].ToString());
                }
                else
                {
                    MBInfo.Add("Width: No Data (xNull).");
                }

                if (mbInfo["HostingBoard"] != null)
                {
                    MBInfo.Add("Hosting Board: " + mbInfo["HostingBoard"].ToString());
                }
                else
                {
                    MBInfo.Add("Hosting Board: No Data (xNull).");
                }

                if (mbInfo["HotSwappable"] != null)
                {
                    MBInfo.Add("Hot Swappable: " + mbInfo["HotSwappable"].ToString());
                }
                else
                {
                    MBInfo.Add("Hot Swappable: No Data (xNull).");
                }

                if (mbInfo["InstallDate"] != null)
                {
                    MBInfo.Add("Install Date: " + mbInfo["InstallDate"].ToString());
                }
                else
                {
                    MBInfo.Add("Install Date: No Data (xNull).");
                }

                if (mbInfo["Manufacturer"] != null)
                {
                    Manufacturer = mbInfo["Manufacturer"].ToString();
                    MBInfo.Add("Manufacturer: " + Manufacturer);
                }
                else
                {
                    MBInfo.Add("Manufacturer: No Data (xNull).");
                }

                if (mbInfo["Model"] != null)
                {
                    MBInfo.Add("Model: " + mbInfo["Model"].ToString());
                }
                else
                {
                    MBInfo.Add("Model: No Data (xNull).");
                }

                if (mbInfo["Name"] != null)
                {
                    Name = mbInfo["Name"].ToString();
                    MBInfo.Add("Name: " + Name);
                }
                else
                {
                    MBInfo.Add("Name: No Data (xNull).");
                }

                if (mbInfo["OtherIdentifyingInfo"] != null)
                {
                    MBInfo.Add("Other Identifying Info: " + mbInfo["OtherIdentifyingInfo"].ToString());
                }
                else
                {
                    MBInfo.Add("Other Identifying Info: No Data (xNull).");
                }

                if (mbInfo["PartNumber"] != null)
                {
                    MBInfo.Add("Part Number: " + mbInfo["PartNumber"].ToString());
                }
                else
                {
                    MBInfo.Add("Part Number: No Data (xNull).");
                }

                if (mbInfo["PoweredOn"] != null)
                {
                    MBInfo.Add("Powered On: " + mbInfo["PoweredOn"].ToString());
                }
                else
                {
                    MBInfo.Add("Powered On: No Data (xNull).");
                }

                if (mbInfo["Product"] != null)
                {
                    ProductId = mbInfo["Product"].ToString();
                    MBInfo.Add("Product: " + ProductId);
                }
                else
                {
                    MBInfo.Add("Product: No Data (xNull).");
                }

                if (mbInfo["Removable"] != null)
                {
                    MBInfo.Add("Removable: " + mbInfo["Removable"].ToString());
                }
                else
                {
                    MBInfo.Add("Removable: No Data (xNull).");
                }

                if (mbInfo["Replaceable"] != null)
                {
                    MBInfo.Add("Replaceable: " + mbInfo["Replaceable"].ToString());
                }
                else
                {
                    MBInfo.Add("Replaceable: No Data (xNull).");
                }

                if (mbInfo["RequirementsDescription"] != null)
                {
                    MBInfo.Add("Requirements Description: " + mbInfo["RequirementsDescription"].ToString());
                }
                else
                {
                    MBInfo.Add("Requirements Description: No Data (xNull).");
                }

                if (mbInfo["RequiresDaughterBoard"] != null)
                {
                    MBInfo.Add("Requires Daughter Board: " + mbInfo["RequiresDaughterBoard"].ToString());
                }
                else
                {
                    MBInfo.Add("Requires Daughter Board: No Data (xNull).");
                }

                if (mbInfo["SerialNumber"] != null)
                {
                    Serial = mbInfo["SerialNumber"].ToString();
                    MBInfo.Add("Serial Number: " + Serial);
                }
                else
                {
                    MBInfo.Add("Serial Number: No Data (xNull).");
                }

                if (mbInfo["SKU"] != null)
                {
                    MBInfo.Add("SKU: " + mbInfo["SKU"].ToString());
                }
                else
                {
                    MBInfo.Add("SKU: No Data (xNull).");
                }

                if (mbInfo["SlotLayout"] != null)
                {
                    MBInfo.Add("Slot Layout: " + mbInfo["SlotLayout"].ToString());
                }
                else
                {
                    MBInfo.Add("Slot Layout: No Data (xNull).");
                }

                if (mbInfo["SpecialRequirements"] != null)
                {
                    MBInfo.Add("Special Requirements: " + mbInfo["SpecialRequirements"].ToString());
                }
                else
                {
                    MBInfo.Add("Special Requirements: No Data (xNull).");
                }

                if (mbInfo["Status"] != null)
                {
                    MBInfo.Add("Status: " + mbInfo["Status"].ToString());
                }
                else
                {
                    MBInfo.Add("Status: No Data (xNull).");
                }

                if (mbInfo["Tag"] != null)
                {
                    MBInfo.Add("Tag: " + mbInfo["Tag"].ToString());
                }
                else
                {
                    MBInfo.Add("Tag: No Data (xNull).");
                }

                if (mbInfo["Version"] != null)
                {
                    Version = mbInfo["Version"].ToString();
                    MBInfo.Add("Version: " + Version);
                }
                else
                {
                    MBInfo.Add("Version: No Data (xNull).");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string getName()
        {
            return this.Name;
        }

        public string getManu()
        {
            return this.Manufacturer;
        }

        public string getVersion()
        {
            return this.Version;
        }

        public string getSerial()
        {
            return this.Serial;
        }

        public string getPID()
        {
            return this.ProductId;
        }
    }
}
