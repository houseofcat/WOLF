using System;
using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class IRQ
    {
        public List<string> IRQInfo = new List<string>();
        public int intIRQLength = 0;

        private string IRQName = "";
        private string IRQNumber = "";

        public IRQ()
        {}

        public IRQ(ManagementObject irqInfo)
        {
            setIRQInfo(irqInfo);

            intIRQLength = IRQInfo.Count;
        }

        private void setIRQInfo(ManagementObject irqInfo)
        {
            IRQInfo.Clear();
            intIRQLength = 0;

            if (irqInfo["IRQNumber"] != null)
            {
                string temp = irqInfo["IRQNumber"].ToString();
                IRQNumber = temp;
                IRQInfo.Add("IRQ Number: " + temp);
            }
            else
            {
                IRQInfo.Add("IRQNumber: No Data. (xNull)");
            }

            if (irqInfo["Name"] != null)
            {
                string temp = irqInfo["Name"].ToString();
                IRQName = temp;
                IRQInfo.Add("Name: " + temp);
            }
            else
            {
                IRQInfo.Add("Name: No Data. (xNull)");
            }

            if (irqInfo["Caption"] != null)
            {
                IRQInfo.Add("Caption: " + irqInfo["Caption"].ToString());
            }
            else
            {
                IRQInfo.Add("Caption: No Data. (xNull)");
            }

            if (irqInfo["Description"] != null)
            {
                IRQInfo.Add("Description: " + irqInfo["Description"]);
            }
            else
            {
                IRQInfo.Add("Description: No Data. (xNull)");
            }

            if (irqInfo["Vector"] != null)
            {
                IRQInfo.Add("Memory Address (Vector): " + irqInfo["Vector"]);
            }
            else
            {
                IRQInfo.Add("Memory Address (Vector): No Data. (xNull)");
            }

            if (irqInfo["TriggerType"] != null)
            {
                string temp = irqInfo["TriggerLevel"].ToString();

                if (temp == "1")
                {
                    IRQInfo.Add("Trigger Type: Other");
                }
                else if (temp == "2")
                {
                    IRQInfo.Add("Trigger Type: Unknown");
                }
                else if (temp == "3")
                {
                    IRQInfo.Add("Trigger Type: Level");
                }
                else if (temp == "4")
                {
                    IRQInfo.Add("Trigger Type: Edge");
                }
            }
            else
            {
                IRQInfo.Add("Trigger Type: No Data. (xNull)");
            }

            if (irqInfo["TriggerLevel"] != null)
            {
                string temp = irqInfo["TriggerLevel"].ToString();

                if (temp == "1")
                {
                    IRQInfo.Add("Trigger Level: Other");
                }
                else if (temp == "2")
                {
                    IRQInfo.Add("Trigger Level: Unknown");
                }
                else if (temp == "3")
                {
                    IRQInfo.Add("Trigger Level: Active Low");
                }
                else if (temp == "4")
                {
                    IRQInfo.Add("Trigger Level: Active High");
                }
            }
            else
            {
                IRQInfo.Add("Trigger Level: No Data. (xNull)");
            }

            if (irqInfo["Availability"] != null)
            {
                string temp = funcConvertAvail(irqInfo["Availability"].ToString());

                IRQInfo.Add("Availability: " + temp);
            }
            else
            {
                IRQInfo.Add("Availability: No Data. (xNull)");
            }

            if (irqInfo["Status"] != null)
            {
                IRQInfo.Add("Status: " + irqInfo["Status"]);
            }
            else
            {
                IRQInfo.Add("Status: No Data. (xNull)");
            }

            if (irqInfo["CSName"] != null)
            {
                IRQInfo.Add("CSName: " + irqInfo["CSName"]);
            }
            else
            {
                IRQInfo.Add("CSName: No Data. (xNull)");
            }

            if (irqInfo["CSCreationClassName"] != null)
            {
                IRQInfo.Add("CS Creation Class Name: " + irqInfo["CSCreationClassName"]);
            }
            else
            {
                IRQInfo.Add("CS Creation Class Name: No Data. (xNull)");
            }

            if (irqInfo["CreationClassName"] != null)
            {
                IRQInfo.Add("Creation Class Name: " + irqInfo["CreationClassName"]);
            }
            else
            {
                IRQInfo.Add("Creation Class Name: No Data. (xNull)");
            }

            if (irqInfo["Status"] != null)
            {
                IRQInfo.Add("Status: " + irqInfo["Status"]);
            }
            else
            {
                IRQInfo.Add("Status: No Data. (xNull)");
            }

            if (irqInfo["InstallDate"] != null)
            {
                string format = "G";
                DateTime tempDate = new DateTime(1900, 1, 1);

                tempDate = ManagementDateTimeConverter.ToDateTime(irqInfo["InstallDate"].ToString());
                IRQInfo.Add("Install Date: " + tempDate.ToString(format));
            }
            else
            {
                IRQInfo.Add("Install Date: No Data. (xNull)");
            }

            if (irqInfo["Shareable"] != null)
            {
                IRQInfo.Add("Shareable: " + irqInfo["Shareable"]);
            }
            else
            {
                IRQInfo.Add("Shareable: No Data. (xNull)");
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

        public string getIRQName()
        {
            return this.IRQName;
        }

        public string getIRQNumber()
        {
            return this.IRQNumber;
        }
    }
}
