using System.Diagnostics;


namespace Wolf
{
    public class TaskManager
    {
        private Process[] processList;

        public TaskManager()
        {
        }

        public void setAllProcesses()
        {
            processList = Process.GetProcesses();
        }

        public Process[] getAllProcesses()
        {
            return processList;
        }
    }
}
