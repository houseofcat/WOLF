using System.Collections.Generic;
using System.Management;

namespace Wolf.HardSpec
{
    class MEM
    {
        public List<string> MEMInfo = new List<string>();
        public int intMEMLength = 0;
        public string Size = "";
        public string MEMSpeed = "";
        public string MEMManu = "";
        public ulong intTotMem = 0;

        public MEM(ManagementObject memInfo)
        {
            setRAMInfo(memInfo);
        }

        public void setRAMInfo(ManagementObject memInfo)
        {
            MEMInfo.Clear();

            intMEMLength = 0;

            if (memInfo["DeviceLocator"] != null)
            {
                MEMInfo.Add("Location: " + memInfo["DeviceLocator"].ToString());
            }
            else
            {
                MEMInfo.Add("Location: No Data. (xNull)");
            }

            if (memInfo["Name"] != null)
            {
                MEMInfo.Add("Name: " + memInfo["Name"].ToString());
            }
            else
            {
                MEMInfo.Add("Name: No Data. (xNull)");
            }

            if (memInfo["Manufacturer"] != null)
            {
                MEMInfo.Add("Manufacturer: " + memInfo["Manufacturer"].ToString());
                MEMManu = memInfo["Manufacturer"].ToString();
            }
            else
            {
                MEMInfo.Add("Manufacturer: No Data. (xNull)");
            }

            if (memInfo["SerialNumber"] != null)
            {
                MEMInfo.Add("Serial Number: " + memInfo["SerialNumber"].ToString());
            }
            else
            {
                MEMInfo.Add("Serial Number: No Data. (xNull)");
            }

            if (memInfo["Capacity"] != null)
            {
                Size = Tools.convertToGBFromBytes(memInfo["Capacity"].ToString());
                MEMInfo.Add("Capacity: " + Size);
            }
            else
            {
                MEMInfo.Add("Capacity: No Data. (xNull)");
            }

            if (memInfo["Speed"] != null)
            {
                MEMInfo.Add("Speed: " + memInfo["Speed"].ToString() + " MHz");
                MEMSpeed = memInfo["Speed"].ToString() + " MHz";
            }
            else
            {
                MEMInfo.Add("Speed: No Data. (xNull)");
            }

            if (memInfo["MemoryType"] != null)
            {
                MEMInfo.Add("Memory Type: " + funcConvertMemType(memInfo["MemoryType"].ToString()));
            }
            else
            {
                MEMInfo.Add("Memory Type: No Data. (xNull)");
            }

            if (memInfo["TypeDetail"] != null)
            {
                MEMInfo.Add("Type Detail: " + funcConvertTypeDet(memInfo["TypeDetail"].ToString()));
            }
            else
            {
                MEMInfo.Add("Type Detail: No Data. (xNull)");
            }

            if (memInfo["DataWidth"] != null)
            {
                MEMInfo.Add("Data Width: " + memInfo["DataWidth"].ToString() + "-bit");
            }
            else
            {
                MEMInfo.Add("DataWidth: No Data. (xNull)");
            }

            if (memInfo["TotalWidth"] != null)
            {
                MEMInfo.Add("Total Width: " + memInfo["TotalWidth"].ToString() + "-bit");
            }
            else
            {
                MEMInfo.Add("TotalWidth: No Data. (xNull)");
            }

            if (memInfo["FormFactor"] != null)
            {

                MEMInfo.Add("Form Factor: " + funcConvertForm(memInfo["FormFactor"].ToString()));
            }
            else
            {
                MEMInfo.Add("FormFactor: No Data. (xNull)");
            }

            if (memInfo["InterleavePosition"] != null)
            {
                MEMInfo.Add("Interleave Position: " + funcConvertInterPosition(memInfo["InterleavePosition"].ToString()));
            }
            else
            {
                MEMInfo.Add("Interleave Position: No Data. (xNull)");
            }

            if (memInfo["InterleaveDataDepth"] != null)
            {
                MEMInfo.Add("Interleave Data Depth: " + memInfo["InterleaveDataDepth"].ToString());
            }
            else
            {
                MEMInfo.Add("InterleaveDataDepth: No Data. (xNull)");
            }

            if (memInfo["PositionInRow"] != null)
            {
                MEMInfo.Add("Position In Row: " + memInfo["PositionInRow"].ToString());
            }
            else
            {
                MEMInfo.Add("Position In Row: No Data. (xNull)");
            }

            if (memInfo["BankLabel"] != null)
            {
                MEMInfo.Add("Bank Label: " + memInfo["BankLabel"].ToString());
            }
            else
            {
                MEMInfo.Add("Bank Label: No Data. (xNull)");
            }

            if (memInfo["Caption"] != null)
            {
                MEMInfo.Add("Caption: " + memInfo["Caption"].ToString());
            }
            else
            {
                MEMInfo.Add("Caption: No Data. (xNull)");
            }

            if (memInfo["CreationClassName"] != null)
            {
                MEMInfo.Add("Creation Class Name: " + memInfo["CreationClassName"].ToString());
            }
            else
            {
                MEMInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (memInfo["Description"] != null)
            {
                MEMInfo.Add("Description: " + memInfo["Description"].ToString());
            }
            else
            {
                MEMInfo.Add("Description: No Data. (xNull)");
            }

            if (memInfo["HotSwappable"] != null)
            {
                MEMInfo.Add("Hot Swappable: " + memInfo["HotSwappable"].ToString());
            }
            else
            {
                MEMInfo.Add("Hot Swappable: No Data. (xNull)");
            }

            if (memInfo["InstallDate"] != null)
            {
                MEMInfo.Add("Install Date: " + memInfo["InstallDate"].ToString());
            }
            else
            {
                MEMInfo.Add("Install Date: No Data. (xNull)");
            }

            if (memInfo["Model"] != null)
            {
                MEMInfo.Add("Model: " + memInfo["Model"].ToString());
            }
            else
            {
                MEMInfo.Add("Model: No Data. (xNull)");
            }

            if (memInfo["OtherIdentifyingInfo"] != null)
            {
                MEMInfo.Add("Other Identifying Info: " + memInfo["OtherIdentifyingInfo"].ToString());
            }
            else
            {
                MEMInfo.Add("Other Idnetifying Info: No Data. (xNull)");
            }

            if (memInfo["PartNumber"] != null)
            {
                MEMInfo.Add("Part Number: " + memInfo["PartNumber"].ToString());
            }
            else
            {
                MEMInfo.Add("Part Number: No Data. (xNull)");
            }

            if (memInfo["PoweredOn"] != null)
            {
                MEMInfo.Add("Powered On: " + memInfo["PoweredOn"].ToString());
            }
            else
            {
                MEMInfo.Add("Powered On: No Data. (xNull)");
            }

            if (memInfo["Removable"] != null)
            {
                MEMInfo.Add("Removable: " + memInfo["Removable"].ToString());
            }
            else
            {
                MEMInfo.Add("Removable: No Data. (xNull)");
            }

            if (memInfo["Replaceable"] != null)
            {
                MEMInfo.Add("Replaceable: " + memInfo["Replaceable"].ToString());
            }
            else
            {
                MEMInfo.Add("Replaceable: No Data. (xNull)");
            }

            if (memInfo["SKU"] != null)
            {
                MEMInfo.Add("SKU: " + memInfo["SKU"].ToString());
            }
            else
            {
                MEMInfo.Add("SKU: No Data. (xNull)");
            }

            if (memInfo["Status"] != null)
            {
                MEMInfo.Add("Status: " + memInfo["Status"].ToString());
            }
            else
            {
                MEMInfo.Add("Status: No Data. (xNull)");
            }

            if (memInfo["Tag"] != null)
            {
                MEMInfo.Add("Tag: " + memInfo["Tag"].ToString());
            }
            else
            {
                MEMInfo.Add("Tag: No Data. (xNull)");
            }

            if (memInfo["Version"] != null)
            {
                MEMInfo.Add("Version: " + memInfo["Version"].ToString());
            }
            else
            {
                MEMInfo.Add("Version: No Data. (xNull)");
            }


            intMEMLength = 30;

            ulong.TryParse(memInfo["Capacity"].ToString(), out intTotMem);
        }

        private string funcConvertMemType(string Type)
        {
            if (Type == "0")
            {
                Type = "Unknown";
            }
            else if (Type == "1")
            {
                Type = "Other";
            }
            else if (Type == "2")
            {
                Type = "DRAM";
            }
            else if (Type == "3")
            {
                Type = "Synchronous DRAM";
            }
            else if (Type == "4")
            {
                Type = "Cache DRAM";
            }
            else if (Type == "5")
            {
                Type = "EDO";
            }
            else if (Type == "6")
            {
                Type = "EDRAM";
            }
            else if (Type == "7")
            {
                Type = "VRAM";
            }
            else if (Type == "8")
            {
                Type = "SRAM";
            }
            else if (Type == "9")
            {
                Type = "RAM";
            }
            else if (Type == "10")
            {
                Type = "ROM";
            }
            else if (Type == "11")
            {
                Type = "Flash";
            }
            else if (Type == "12")
            {
                Type = "EEPROM";
            }
            else if (Type == "13")
            {
                Type = "FEPROM";
            }
            else if (Type == "14")
            {
                Type = "EPROM";
            }
            else if (Type == "15")
            {
                Type = "CDRAM";
            }
            else if (Type == "16")
            {
                Type = "3DRAM";
            }
            else if (Type == "17")
            {
                Type = "SDRAM";
            }
            else if (Type == "18")
            {
                Type = "SGRAM";
            }
            else if (Type == "19")
            {
                Type = "RDRAM";
            }
            else if (Type == "20")
            {
                Type = "DDR";
            }
            else if (Type == "21")
            {
                Type = "DDR-2";
            }

            return Type;
        }

        private string funcConvertForm(string Form)
        {
            if (Form == "0")
            {
                Form = "Uknown";
            }
            else if (Form == "1")
            {
                Form = "Other";
            }
            else if (Form == "2")
            {
                Form = "SIP";
            }
            else if (Form == "3")
            {
                Form = "DIP";
            }
            else if (Form == "4")
            {
                Form = "ZIP";
            }
            else if (Form == "5")
            {
                Form = "SOJ";
            }
            else if (Form == "6")
            {
                Form = "Proprietary";
            }
            else if (Form == "7")
            {
                Form = "SIMM";
            }
            else if (Form == "8")
            {
                Form = "DIMM";
            }
            else if (Form == "9")
            {
                Form = "TSOP";
            }
            else if (Form == "10")
            {
                Form = "PGA";
            }
            else if (Form == "11")
            {
                Form = "RIMM";
            }
            else if (Form == "12")
            {
                Form = "SODIMM";
            }
            else if (Form == "13")
            {
                Form = "SRIMM";
            }
            else if (Form == "14")
            {
                Form = "SMD";
            }
            else if (Form == "15")
            {
                Form = "SSMP";
            }
            else if (Form == "16")
            {
                Form = "QFP";
            }
            else if (Form == "17")
            {
                Form = "TQFP";
            }
            else if (Form == "18")
            {
                Form = "SOIC";
            }
            else if (Form == "19")
            {
                Form = "LCC";
            }
            else if (Form == "20")
            {
                Form = "PLCC";
            }
            else if (Form == "21")
            {
                Form = "BGA";
            }
            else if (Form == "22")
            {
                Form = "FPBGA";
            }
            else if (Form == "23")
            {
                Form = "LGA";
            }

            return Form;
        }

        private string funcConvertTypeDet(string TypeDet)
        {
            if (TypeDet == "0")
            {
                TypeDet = "Super Unknown.";
            }
            else if (TypeDet == "1")
            {
                TypeDet = "Reserved";
            }
            else if (TypeDet == "2")
            {
                TypeDet = "Other";
            }
            else if (TypeDet == "4")
            {
                TypeDet = "Unknown";
            }
            else if (TypeDet == "8")
            {
                TypeDet = "Fast-Paged";
            }
            else if (TypeDet == "16")
            {
                TypeDet = "Static Column";
            }
            else if (TypeDet == "32")
            {
                TypeDet = "Pseudo-Static";
            }
            else if (TypeDet == "64")
            {
                TypeDet = "RAMBUS";
            }
            else if (TypeDet == "128")
            {
                TypeDet = "Synchronous";
            }
            else if (TypeDet == "256")
            {
                TypeDet = "CMOS";
            }
            else if (TypeDet == "512")
            {
                TypeDet = "EDO";
            }
            else if (TypeDet == "1024")
            {
                TypeDet = "Window DRAM";
            }
            else if (TypeDet == "2048")
            {
                TypeDet = "Cache DRAM";
            }
            else if (TypeDet == "4096")
            {
                TypeDet = "Nonvolatile";
            }

            return TypeDet;
        }

        private string funcConvertInterPosition(string Inter)
        {
            if (Inter == "0")
            {
                Inter = "Non-Interleaved";
            }
            else if (Inter == "1")
            {
                Inter = "First Position";
            }
            else if (Inter == "2")
            {
                Inter = "Second Position";
            }
            return Inter;
        }

        public string getMEMManufacturer()
        {
            return this.MEMManu;
        }

        public string getMEMSpeed()
        {
            return this.MEMSpeed;
        }

        public ulong getMemSize()
        {
            return this.intTotMem;
        }
    }
}