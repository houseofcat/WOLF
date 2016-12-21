using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.SoftSpec
{
    class LicenseQuery
    {
        private string L_MachineName = "";
        private string L_OSVersion = "";
        private string L_OSKey = "";
        public List<string> L_OfficeKey = new List<string>();
        public List<string> L_OfficeVersion = new List<string>();

        public string GetMachineName()
        {
            return L_MachineName;
        }

        public string GetOSVersion()
        {
            return L_OSVersion;
        }

        public string GetOSKey()
        {
            return L_OSKey;
        }

        public List<string> GetOfficeVersion()
        {
            return L_OfficeVersion;
        }

        public List<string> GetOfficeKey()
        {
            return L_OfficeKey;
        }

        public void SetMachineName( string Input )
        {
            L_MachineName = Input;
        }

        public void SetOSVersion( string Input )
        {
            L_OSVersion = Input;
        }

        public void SetOSKey( string Input )
        {
            L_OSKey = Input;
        }

        public void SetOfficeVersion( string Key, string Version )
        {
            if (!L_OfficeKey.Contains(Key))
            {
                L_OfficeVersion.Add(Version);
                L_OfficeKey.Add(Key);
            }
        }
    }
}
