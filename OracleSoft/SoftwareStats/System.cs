using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OracleSoft.SoftwareStats
{
    class System:IDisposable
    { 
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SYS_TIMEUP.Close();
                SYS_SYSCALLSEC.Close();
                SYS_PROCESS.Close();
                SYS_THREADS.Close();
                SYS_PROCQUELEN.Close();

                //Specific Performance Info
                SYS_PREGQUOTA.Close();
                SYS_FLOATEMUSEC.Close();
                SYS_ALIFIXSEC.Close();
                SYS_CONSWISEC.Close();
                SYS_EXCDISPSEC.Close();

                //File I/O
                SYS_FDATOPSEC.Close();
                SYS_FCONBYTESEC.Close();
                SYS_FCONOPSEC.Close();
                SYS_FRBYTESEC.Close();
                SYS_FWBYTESEC.Close();
                SYS_FROPSEC.Close();
                SYS_FWOPSEC.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Loaded = false;
        List<double> NextValueList = new List<double>();

        //Generic Performance Info
        PerformanceCounter SYS_TIMEUP = new PerformanceCounter();
        PerformanceCounter SYS_SYSCALLSEC = new PerformanceCounter();
        PerformanceCounter SYS_PROCESS = new PerformanceCounter();
        PerformanceCounter SYS_THREADS = new PerformanceCounter();
        PerformanceCounter SYS_PROCQUELEN = new PerformanceCounter();

        //Specific Performance Info
        PerformanceCounter SYS_PREGQUOTA = new PerformanceCounter();
        PerformanceCounter SYS_FLOATEMUSEC = new PerformanceCounter();
        PerformanceCounter SYS_ALIFIXSEC = new PerformanceCounter();
        PerformanceCounter SYS_CONSWISEC = new PerformanceCounter();
        PerformanceCounter SYS_EXCDISPSEC = new PerformanceCounter();

        //File I/O
        PerformanceCounter SYS_FDATOPSEC = new PerformanceCounter();
        PerformanceCounter SYS_FCONBYTESEC = new PerformanceCounter();
        PerformanceCounter SYS_FCONOPSEC = new PerformanceCounter();
        PerformanceCounter SYS_FRBYTESEC = new PerformanceCounter();
        PerformanceCounter SYS_FWBYTESEC = new PerformanceCounter();
        PerformanceCounter SYS_FROPSEC = new PerformanceCounter();
        PerformanceCounter SYS_FWOPSEC = new PerformanceCounter();

        double LastTime = -1.0;
        double TickDrift = -1.0;

        public System ()
        {

        }

        public void LoadSystem()
        {
            try
            {
                SYS_TIMEUP = new PerformanceCounter("System", "System Up Time", true);
                SYS_SYSCALLSEC = new PerformanceCounter("System", "System Calls/sec", true);
                SYS_PROCESS = new PerformanceCounter("System", "Processes", true);
                SYS_PROCQUELEN = new PerformanceCounter("System", "Processor Queue Length", true);
                SYS_THREADS = new PerformanceCounter("System", "Threads", true);

                SYS_PREGQUOTA = new PerformanceCounter("System", "% Registry Quota In Use", true);
                SYS_FLOATEMUSEC = new PerformanceCounter("System", "Floating Emulations/sec", true);
                SYS_ALIFIXSEC = new PerformanceCounter("System", "Alignment Fixups/sec", true);
                SYS_CONSWISEC = new PerformanceCounter("System", "Context Switches/sec", true);
                SYS_EXCDISPSEC = new PerformanceCounter("System", "Exception Dispatches/sec", true);

                SYS_FDATOPSEC = new PerformanceCounter("System", "File Data Operations/sec", true);
                SYS_FCONBYTESEC = new PerformanceCounter("System", "File Control Bytes/sec", true);
                SYS_FCONOPSEC = new PerformanceCounter("System", "File Control Operations/sec", true);
                SYS_FRBYTESEC = new PerformanceCounter("System", "File Read Bytes/sec", true);
                SYS_FWBYTESEC = new PerformanceCounter("System", "File Write Bytes/sec", true);
                SYS_FROPSEC = new PerformanceCounter("System", "File Read Operations/sec", true);
                SYS_FWOPSEC = new PerformanceCounter("System", "File Write Operations/sec", true);

                Loaded = true;
            }
            catch (Exception EX)
            {
                //MessageBox.Show(EX.ToString());

                MessageBox.Show("EXCEPTION\n\n==== Loading SYS Exception ===\nSYS: " +
                        "\nMessage: " + EX.Message +
                        "\nHResult: " + EX.HResult.ToString() +
                        "\nData: " + EX.Data +
                        "\nStackTrace: " + EX.StackTrace + "\n");

                /*LSYSExm = EX.Message;
                LSYSExs = EX.StackTrace;
                SYSLoaded = false;
                ExceptionCounter++;*/
            }
        }

        public double nextSYS_TIMEUP()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_TIMEUP.NextValue();

                    if (temp > LastTime)
                    {
                        TickDrift = temp - LastTime;
                        LastTime = temp;
                    }
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_TICKDRIFT()
        {
            return TickDrift;
        }

        public double nextSYS_SYSCALLSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_SYSCALLSEC.NextValue();

                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_PROCESS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_PROCESS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_THREADS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_THREADS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_PROCQUELEN()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_PROCQUELEN.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_PREGQUOTA()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_PREGQUOTA.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FLOATEMUSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FLOATEMUSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_ALIFIXSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_ALIFIXSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }
        
        public double nextSYS_CONSWISEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_CONSWISEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_EXCDISPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_EXCDISPSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FDATOPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FDATOPSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FCONBYTESEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FCONBYTESEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FCONOPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FCONOPSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FRBYTESEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FRBYTESEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FWBYTESEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FWBYTESEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FROPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FROPSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextSYS_FWOPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.SYS_FWOPSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public List<double> getNextValues()
        {
            this.NextValueList.Clear();

            NextValueList.Add(nextSYS_TIMEUP());
            NextValueList.Add(nextSYS_TICKDRIFT());
            NextValueList.Add(nextSYS_SYSCALLSEC());
            NextValueList.Add(nextSYS_PROCESS());
            NextValueList.Add(nextSYS_THREADS());
            NextValueList.Add(nextSYS_PROCQUELEN());
            NextValueList.Add(nextSYS_PREGQUOTA());
            NextValueList.Add(nextSYS_FLOATEMUSEC());
            NextValueList.Add(nextSYS_ALIFIXSEC());
            NextValueList.Add(nextSYS_CONSWISEC());
            NextValueList.Add(nextSYS_EXCDISPSEC());
            NextValueList.Add(nextSYS_FDATOPSEC());

            //Converting to KB
            double temp = -1.0;
            temp = nextSYS_FCONBYTESEC();
            temp = temp / 1000.0;
            NextValueList.Add(temp);

            NextValueList.Add(nextSYS_FCONOPSEC());

            //Converting to KB
            temp = nextSYS_FRBYTESEC();
            temp = temp / 1000.0;
            NextValueList.Add(temp);

                        //Converting to KB
            temp = nextSYS_FWBYTESEC();
            temp = temp / 1000.0;
            NextValueList.Add(temp);

            NextValueList.Add(nextSYS_FROPSEC());
            NextValueList.Add(nextSYS_FWOPSEC());

            return this.NextValueList;
        }

    }
}
