using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class Account
    {
        public List<string> ACCTInfo = new List<string>();
        public int intAcctLength = 0;

        bool Local = false;
        bool Active = false;

        string AccountName = "";
        string AccountType = "";
        string SID = "";

        public Account()
        { }

        public Account(ManagementObject AcctInfo)
        {
            setAcctInfo(AcctInfo);

            intAcctLength = ACCTInfo.Count;
        }

        private void setAcctInfo(ManagementObject AcctInfo)
        {
            intAcctLength = 0;
            ACCTInfo.Clear();

            if (AcctInfo["Caption"] != null)
            {
                string temp = AcctInfo["Caption"].ToString();
                AccountName = temp;
                ACCTInfo.Add("Caption: " + temp);
            }
            else
            {
                ACCTInfo.Add("Caption: No Data (xNull).");
            }

            if (AcctInfo["Description"] != null)
            {
                ACCTInfo.Add("Description: " + AcctInfo["Description"].ToString());
            }
            else
            {
                ACCTInfo.Add("Description: No Data (xNull).");
            }

            if (AcctInfo["Domain"] != null)
            {
                ACCTInfo.Add("Domain: " + AcctInfo["Domain"].ToString());
            }
            else
            {
                ACCTInfo.Add("Domain: No Data (xNull).");
            }

            if (AcctInfo["InstallDate"] != null)
            {
                ACCTInfo.Add("Install Date: " + AcctInfo["InstallDate"].ToString());
            }
            else
            {
                ACCTInfo.Add("Install Date: No Data (xNull).");
            }

            if (AcctInfo["LocalAccount"] != null)
            {
                string temp = "";
                temp = AcctInfo["LocalAccount"].ToString();

                if (temp.Contains("True"))
                {
                    Local = true;
                }
                else if (temp.Contains("False"))
                {
                    Local = false;
                }

                ACCTInfo.Add("Local Account: " + Local);
            }
            else
            {
                ACCTInfo.Add("Local Account: No Data (xNull).");
            }

            if (AcctInfo["Name"] != null)
            {
                ACCTInfo.Add("Name: " + AcctInfo["Name"].ToString());
            }
            else
            {
                ACCTInfo.Add("Name: No Data (xNull).");
            }

            if (AcctInfo["SID"] != null)
            {
                SID = AcctInfo["SID"].ToString();
                ACCTInfo.Add("SID: " + SID);
            }
            else
            {
                ACCTInfo.Add("SID: No Data (xNull).");
            }

            if (AcctInfo["SIDType"] != null)
            {
                string temp = AcctInfo["SIDType"].ToString();

                if (temp == "1")
                {
                    AccountType = "User";
                    ACCTInfo.Add("SID Type: User");
                }
                else if (temp == "2")
                {
                    AccountType = "Group";
                    ACCTInfo.Add("SID Type: Group");
                }
                else if (temp == "3")
                {
                    AccountType = "Domain";
                    ACCTInfo.Add("SID Type: Domain");
                }
                else if (temp == "4")
                {
                    AccountType = "Alias";
                    ACCTInfo.Add("SID Type: Alias");
                }
                else if (temp == "5")
                {
                    AccountType = "WellKnownGroup";
                    ACCTInfo.Add("SID Type: Well Known Group");
                }
                else if (temp == "6")
                {
                    AccountType = "DeletedAccount";
                    ACCTInfo.Add("SID Type: Deleted Account");
                }
                else if (temp == "7")
                {
                    AccountType = "Invalid";
                    ACCTInfo.Add("SID Type: Invalid");
                }
                else if (temp == "8")
                {
                    AccountType = "Unknown";
                    ACCTInfo.Add("SID Type: Unknown");
                }
                else if (temp == "9")
                {
                    AccountType = "Computer";
                    ACCTInfo.Add("SID Type: Computer");
                }
            }
            else
            {
                ACCTInfo.Add("SID Type: No Data (xNull).");
            }

            if (AcctInfo["Status"] != null)
            {
                string temp = AcctInfo["Status"].ToString();

                if (temp == "OK")
                {
                    Active = true;
                }
                else
                {
                    Active = false;
                }

                ACCTInfo.Add("Status: " + temp);
            }
            else
            {
                ACCTInfo.Add("Status: No Data (xNull).");
            }
        }

        public string getName()
        {
            return AccountName;
        }

        public string getType()
        {
            return AccountType;
        }

        public bool isLocal()
        {
            return Local;
        }

        public bool isActive()
        {
            return Active;
        }

        public List<string> ExportData()
        {
            List<string> AccountInfo = new List<string>();

            AccountInfo.Add(AccountName);
            AccountInfo.Add(AccountType);
            AccountInfo.Add(SID);
            AccountInfo.Add(Local.ToString());
            AccountInfo.Add(Active.ToString());

            return AccountInfo;
        }
    }
}
