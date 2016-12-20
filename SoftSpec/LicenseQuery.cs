using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.SoftSpec
{
    class LicenseQuery
    {
        private String L_MachineName = "";
        private String L_OSVersion = "";
        private String L_OSKey = "";
        public List<String> L_OfficeKey = new List<String>();
        public List<String> L_OfficeVersion = new List<String>();

        public String GetMachineName()
        {
            return L_MachineName;
        }

        public String GetOSVersion()
        {
            return L_OSVersion;
        }

        public String GetOSKey()
        {
            return L_OSKey;
        }

        public List<String> GetOfficeVersion()
        {
            return L_OfficeVersion;
        }

        public List<String> GetOfficeKey()
        {
            return L_OfficeKey;
        }

        public void SetMachineName(String Input)
        {
            this.L_MachineName = Input;
        }

        public void SetOSVersion(String Input)
        {
            this.L_OSVersion = Input;
        }

        public void SetOSKey(String Input)
        {
            this.L_OSKey = Input;
        }

        public void SetOfficeVersion(String Key, String Version)
        {
            if (!this.L_OfficeKey.Contains(Key))
            {
                this.L_OfficeVersion.Add(Version);
                this.L_OfficeKey.Add(Key);
            }
        }
    }
}
