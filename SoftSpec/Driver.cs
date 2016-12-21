using System.Collections.Generic;
using System.Management;

namespace Wolf
{
    class Driver
    {
        public List<string> DRVInfo = new List<string>();
        public int DriverLength = 0;

        private string DriverName = "";
        private string DriverDescription = "";
        private string DisplayName = "";
        private string ErrorControl = "";
        private string InstallDate = "";
        private string Name = "";
        private string PathName = "";
        private string TagId = "";
        private string ServiceType = "";
        private string SpecificExitCode = "";
        private string StartMode = "";
        private string StartName = "";
        private string Status = "";
        private string State = "";

        private bool Started = false;
        private bool DesktopInteract = false;
        private bool AcceptPause = false;
        private bool AcceptStop = false;

        public Driver()
        { }

        public Driver(ManagementObject DRIVER)
        {
            setDriverInfo(DRIVER);

            DriverLength = DRVInfo.Count;
        }

        private void setDriverInfo(ManagementObject DRIVER)
        {
            if (DRIVER["Caption"] != null)
            {
                DriverName = DRIVER["Caption"].ToString();

                DRVInfo.Add("Caption: " + DriverName);
            }
            else
            {
                DRVInfo.Add("Caption: No Data (xNull).");
            }

            if (DRIVER["Description"] != null)
            {
                DriverDescription = DRIVER["Description"].ToString();
                DRVInfo.Add("Description: " + DriverDescription);
            }
            else
            {
                DRVInfo.Add("Description: No Data (xNull).");
            }


            if (DRIVER["AcceptStop"] != null)
            {
                AcceptStop = (bool)DRIVER["AcceptStop"];
                DRVInfo.Add("Accept Stop: " + DRIVER["AcceptStop"]);
            }
            else
            {
                DRVInfo.Add("Accept Stop: No Data (xNull).");
            }

            if (DRIVER["AcceptPause"] != null)
            {
                AcceptPause = (bool)DRIVER["AcceptPause"];
                DRVInfo.Add("Accept Pause: " + AcceptPause);
            }
            else
            {
                DRVInfo.Add("Accept Pause: No Data (xNull).");
            }

            /*
            if (DRIVER["SystemName"] != null)
            {
                DRVInfo.Add("System Name: " + DRIVER["SystemName"].ToString());
            }
            else
            {
                DRVInfo.Add("System Name: No Data (xNull).");
            }

            if (DRIVER["SystemCreationClassName"] != null)
            {
                DRVInfo.Add("System Creation Class Name: " + DRIVER["SystemCreationClassName"].ToString());
            }
            else
            {
                DRVInfo.Add("System Creation Class Name: No Data (xNull).");
            }

            if (DRIVER["CreationClassName"] != null)
            {
                DRVInfo.Add("Creation Class Name: " + DRIVER["CreationClassName"].ToString());
            }
            else
            {
                DRVInfo.Add("Creation Class Name: No Data (xNull).");
            }*/

            if (DRIVER["DesktopInteract"] != null)
            {
                DesktopInteract = (bool)DRIVER["DesktopInteract"];
                DRVInfo.Add("Desktop Interact: " + DesktopInteract);
            }
            else
            {
                DRVInfo.Add("Desktop Interact: No Data (xNull).");
            }

            if (DRIVER["DisplayName"] != null)
            {
                DisplayName = DRIVER["DisplayName"].ToString();
                DRVInfo.Add("Display Name: " + DisplayName);
            }
            else
            {
                DRVInfo.Add("Display Name: No Data (xNull).");
            }

            if (DRIVER["ErrorControl"] != null)
            {
                ErrorControl = DRIVER["ErrorControl"].ToString();
                DRVInfo.Add("Error Control: " + ErrorControl);
            }
            else
            {
                DRVInfo.Add("Error Control: No Data (xNull).");
            }

            if (DRIVER["InstallDate"] != null)
            {
                DRVInfo.Add("Install Date: " + DRIVER["InstallDate"].ToString());
            }
            else
            {
                DRVInfo.Add("Install Date: No Data (xNull).");
            }

            if (DRIVER["Name"] != null)
            {
                Name = DRIVER["Name"].ToString();
                DRVInfo.Add("Name: " + Name);
            }
            else
            {
                DRVInfo.Add("Name: No Data (xNull).");
            }

            if (DRIVER["PathName"] != null)
            {
                PathName = DRIVER["PathName"].ToString();
                DRVInfo.Add("Path Name: " + PathName);
            }
            else
            {
                DRVInfo.Add("Path Name: No Data (xNull).");
            }

            if (DRIVER["ServiceSpecificExitCode"] != null)
            {
                SpecificExitCode = DRIVER["ServiceSpecificExitCode"].ToString();
                DRVInfo.Add("Service Specific Exit Code: " + SpecificExitCode);
            }
            else
            {
                DRVInfo.Add("Service Specific Exit Code: No Data (xNull).");
            }

            if (DRIVER["ServiceType"] != null)
            {
                ServiceType = DRIVER["ServiceType"].ToString();
                DRVInfo.Add("Service Type: " + ServiceType);
            }
            else
            {
                DRVInfo.Add("Service Type: No Data (xNull).");
            }

            if (DRIVER["Started"] != null)
            {
                Started = (bool)DRIVER["Started"];
                DRVInfo.Add("Started: " + Started);
            }
            else
            {
                DRVInfo.Add("Started: No Data (xNull).");
            }

            if (DRIVER["StartMode"] != null)
            {
                StartMode = DRIVER["StartMode"].ToString();

                DRVInfo.Add("Start Mode: " + StartMode);
            }
            else
            {
                DRVInfo.Add("Start Mode: No Data (xNull).");
            }

            if (DRIVER["StartName"] != null)
            {
                StartName = DRIVER["StartName"].ToString();
                DRVInfo.Add("Start Name: " + StartName);
            }
            else
            {
                DRVInfo.Add("Start Name: No Data (xNull).");
            }

            if (DRIVER["State"] != null)
            {
                State = DRIVER["State"].ToString();
                DRVInfo.Add("State: " + State);
            }
            else
            {
                DRVInfo.Add("State: No Data (xNull).");
            }

            if (DRIVER["Status"] != null)
            {
                Status = DRIVER["Status"].ToString();
                DRVInfo.Add("Status: " + Status);
            }
            else
            {
                DRVInfo.Add("Status: No Data (xNull).");
            }

            if (DRIVER["TagId"] != null)
            {
                TagId = DRIVER["TagId"].ToString();
                DRVInfo.Add("Tag Id: " + TagId);
            }
            else
            {
                DRVInfo.Add("Tag Id: No Data (xNull).");
            }
        }

        public string getDisplayName()
        {
            return DisplayName;
        }

        public string getDriverName()
        {
            return DriverName;
        }

        public string getDriverDescription()
        {
            return DriverDescription;
        }

        public string getErrorControl()
        {
            return ErrorControl;
        }

        public string getInstallDate()
        {
            return InstallDate;
        }

        public string getName()
        {
            return Name;
        }

        public string getPathName()
        {
            return PathName;
        }

        public string getTagId()
        {
            return TagId;
        }

        public string getServiceType()
        {
            return ServiceType;
        }

        public string getSpecificExitCode()
        {
            return SpecificExitCode;
        }

        public string getStartMode()
        {
            return StartMode;
        }

        public string getStartName()
        {
            return StartName;
        }

        public string getStatus()
        {
            return Status;
        }

        public string getState()
        {
            return State;
        }

        public bool getStarted()
        {
            return Started;
        }

        public bool getAcceptPause()
        {
            return AcceptPause;
        }

        public bool getAcceptStop()
        {
            return AcceptStop;
        }
    }
}
