using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class Account
    {
        public List<string> ACCTInfo = new List<string>();
        public int intAcctLength = 0;

        public bool Local { get; set; } = false;
        public bool Active { get; set; } = false;

        public string AccountName { get; set; } = "";
        public string AccountType { get; set; } = "";
        public string SID { get; set; } = "";

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

            AccountName = AcctInfo["Caption"].ToString() ?? "No Data (xNull).";
            ACCTInfo.Add("Caption: "        + AccountName);
            ACCTInfo.Add("Description: "    + AcctInfo["Description"]?.ToString()   ?? "No Data (xNull).");
            ACCTInfo.Add("Domain: "         + AcctInfo["Domain"]?.ToString()        ?? "No Data (xNull).");
            ACCTInfo.Add("Install Date: "   + AcctInfo["InstallDate"]?.ToString()   ?? "No Data (xNull).");

            string temp = AcctInfo["LocalAccount"]?.ToString() ?? "No Data(xNull).";
            if (temp.Contains("True")) { Local = true; } else { Local = false; }

            ACCTInfo.Add("Local Account: " + temp);
            ACCTInfo.Add("Name: "   + AcctInfo["Name"]?.ToString()  ?? "No Data(xNull).");
            ACCTInfo.Add("SID: "    + AcctInfo["SID"]?.ToString()   ?? "No Data(xNull).");

            if (AcctInfo["SIDType"] != null)
            {
                int intTemp = -1;
                if (int.TryParse(AcctInfo["SIDType"].ToString(), out intTemp))
                {
                    switch (intTemp)
                    {
                        case 1: AccountType = "User";           break;
                        case 2: AccountType = "Group";          break;
                        case 3: AccountType = "Domain";         break;
                        case 4: AccountType = "Alias";          break;
                        case 5: AccountType = "WellKnownGroup"; break;
                        case 6: AccountType = "DeletedAccount"; break;
                        case 7: AccountType = "Invalid";        break;
                        case 8: AccountType = "Unknown";        break;
                        case 9: AccountType = "Computer";       break;
                        default: break;
                    }
                }
                else { AccountType = "No Data(xNull)."; }

                ACCTInfo.Add("SID Type: " + AccountType);
            }

            temp = AcctInfo["Status"]?.ToString() ?? "No Data(xNull).";
            if (temp.Contains("OK")) { Active = true; } else { Active = false; }

            ACCTInfo.Add("Status: " + temp);
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
