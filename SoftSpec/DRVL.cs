using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace Wolf
{
    class DRVL:IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dtOut.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private List<Driver> UnsortedList = new List<Driver>();
        private List<Driver> SortedList = new List<Driver>();
        private DataTable dtOut = new DataTable();

        public DRVL()
        {
            getInstalledDrivers();
        }

        private void getInstalledDrivers()
        {
            SelectQuery QueryDrivers = new SelectQuery("SELECT * FROM Win32_SystemDriver");
            ManagementObjectCollection moc = new ManagementObjectSearcher(QueryDrivers).Get();

            Parallel.ForEach(moc.Cast<ManagementObject>(), mo =>
            {
                Driver tempDriver = new Driver(mo);
                UnsortedList.Add(tempDriver);
            });

            if (UnsortedList.Any())
            {
                createSortedList();

                if (SortedList.Any())
                {
                    createDataTable();
                }
            }
        }

        private void createSortedList()
        {
            SortedList = new List<Driver>(UnsortedList);

            //Lambda Sort!
            SortedList.Sort((x, y) => string.Compare(x.getDriverName(), y.getDriverName()));
        }

        private void createDataTable()
        {
            dtOut.Clear();
            dtOut.Columns.Add("Driver Name", typeof(string));
            dtOut.Columns.Add("Name", typeof(string));
            dtOut.Columns.Add("Driver Description", typeof(string));
            dtOut.Columns.Add("Display Name", typeof(string));
            dtOut.Columns.Add("Path Name", typeof(string));
            dtOut.Columns.Add("Service Type", typeof(string));
            dtOut.Columns.Add("Start Mode", typeof(string));
            dtOut.Columns.Add("Started", typeof(string));
            dtOut.Columns.Add("State", typeof(string));
            dtOut.Columns.Add("Status", typeof(string));
            dtOut.Columns.Add("Accept Pause", typeof(string));
            dtOut.Columns.Add("Accept Stop", typeof(string));
            dtOut.Columns.Add("Error Control", typeof(string));
            dtOut.Columns.Add("TagId", typeof(string));
            dtOut.Columns.Add("Specific Exit Code", typeof(string));
            dtOut.Columns.Add("Install Date", typeof(string));
            dtOut.Columns.Add("Start Name", typeof(string));

            foreach (Driver temp in SortedList)
            {
                dtOut.Rows.Add(temp.getDriverName(), temp.getName(), temp.getDriverDescription(),
                temp.getDisplayName(), temp.getPathName(), temp.getServiceType(), temp.getStartMode(),
                temp.getStarted(), temp.getState(), temp.getStatus(), temp.getAcceptPause(),
                temp.getAcceptStop(), temp.getErrorControl(), temp.getTagId(), temp.getSpecificExitCode(),
                temp.getInstallDate(), temp.getStartName());
            }
        }

        public List<Driver> getSortedList()
        {
            return SortedList;
        }

        public List<Driver> getUnsortedList()
        {
            return UnsortedList;
        }

        public DataTable getDT()
        {
            return dtOut;
        }


    }
}
