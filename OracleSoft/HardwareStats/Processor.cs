using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OracleSoft.HardwareStats
{
    class Processor:IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CPU_PC1TIME.Close();
                CPU_PC2TIME.Close();
                CPU_PC3TIME.Close();

                //Percentage Times
                CPU_PPROCTIME.Close();
                CPU_PIDLETIME.Close();
                CPU_PINTERTIME.Close();
                CPU_PPRIVTIME.Close();
                CPU_PPRIOTIME.Close();
                CPU_PUSERTIME.Close();

                //Frequency/Performance
                CPU_PMAXFREQ.Close();
                CPU_PARKSTATUS.Close();
                CPU_PROCFREQ.Close();
                CPU_PROCSTATEFLAGS.Close();

                //Variables Per Second
                CPU_C1TRANSEC.Close();
                CPU_C2TRANSEC.Close();
                CPU_C3TRANSEC.Close();
                CPU_INTERSEC.Close();

                //DPC Variables & Percentage of Time Spent Receiving and Servicing Defferred Procedure Calls
                CPU_PDPCTIME.Close();
                CPU_DPCRATE.Close();
                CPU_DPCQUEUEDSEC.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Core> Cores = new List<Core>();
        public bool Loaded = false;

        List<double> NextValueList = new List<double>();

        //ProcessorObject => http://technet.microsoft.com/en-us/library/cc786359(v=ws.10).aspx
        //Percent of Time Spent In Each Power State
        public PerformanceCounter CPU_PC1TIME = new PerformanceCounter();
        public PerformanceCounter CPU_PC2TIME = new PerformanceCounter();
        public PerformanceCounter CPU_PC3TIME = new PerformanceCounter();

        //Percentage Times
        public PerformanceCounter CPU_PPROCTIME = new PerformanceCounter();
        public PerformanceCounter CPU_PIDLETIME = new PerformanceCounter();
        public PerformanceCounter CPU_PINTERTIME = new PerformanceCounter();
        public PerformanceCounter CPU_PPRIVTIME = new PerformanceCounter();
        public PerformanceCounter CPU_PPRIOTIME = new PerformanceCounter();
        public PerformanceCounter CPU_PUSERTIME = new PerformanceCounter();

        //Frequency/Performance
        public PerformanceCounter CPU_PMAXFREQ = new PerformanceCounter();
        public PerformanceCounter CPU_PARKSTATUS = new PerformanceCounter();
        public PerformanceCounter CPU_PROCFREQ = new PerformanceCounter();
        public PerformanceCounter CPU_PROCSTATEFLAGS = new PerformanceCounter();

        //Variables Per Second
        public PerformanceCounter CPU_C1TRANSEC = new PerformanceCounter();
        public PerformanceCounter CPU_C2TRANSEC = new PerformanceCounter();
        public PerformanceCounter CPU_C3TRANSEC = new PerformanceCounter();
        public PerformanceCounter CPU_INTERSEC = new PerformanceCounter();

        //DPC Variables & Percentage of Time Spent Receiving and Servicing Defferred Procedure Calls
        public PerformanceCounter CPU_PDPCTIME = new PerformanceCounter();
        public PerformanceCounter CPU_DPCRATE = new PerformanceCounter();
        public PerformanceCounter CPU_DPCQUEUEDSEC = new PerformanceCounter();

        public Processor()
        { }

        public Processor(int Processors, int LogicalProcessors)
        {
            int LPPP = LogicalProcessors / Processors;
            //Create a new core list.

            for (int i = 0; i < Processors; i++)
            {
                for (int j = 0; j < LPPP; j++)
                {
                    Cores.Add(new Core(i.ToString(), j.ToString()));
                }
            }
        }

        public void LoadCounters()
        {
            try
            {
                CPU_PC1TIME = new PerformanceCounter("Processor Information", "% C1 Time", "_Total", true);
                CPU_PC2TIME = new PerformanceCounter("Processor Information", "% C2 Time", "_Total", true);
                CPU_PC3TIME = new PerformanceCounter("Processor Information", "% C3 Time", "_Total", true);

                CPU_PPROCTIME = new PerformanceCounter("Processor Information", "% Processor Time", "_Total", true);
                CPU_PIDLETIME = new PerformanceCounter("Processor Information", "% Idle Time", "_Total", true);
                CPU_PINTERTIME = new PerformanceCounter("Processor Information", "% Interrupt Time", "_Total", true);
                CPU_PPRIVTIME = new PerformanceCounter("Processor Information", "% Privileged Time", "_Total", true);
                CPU_PPRIOTIME = new PerformanceCounter("Processor Information", "% Priority Time", "_Total", true);
                CPU_PUSERTIME = new PerformanceCounter("Processor Information", "% User Time", "_Total", true);

                CPU_C1TRANSEC = new PerformanceCounter("Processor Information", "C1 Transitions/sec", "_Total", true);
                CPU_C2TRANSEC = new PerformanceCounter("Processor Information", "C2 Transitions/sec", "_Total", true);
                CPU_C3TRANSEC = new PerformanceCounter("Processor Information", "C3 Transitions/sec", "_Total", true);
                CPU_INTERSEC = new PerformanceCounter("Processor Information", "Interrupts/sec", "_Total", true);

                CPU_PDPCTIME = new PerformanceCounter("Processor Information", "% DPC Time", "_Total", true);
                CPU_DPCRATE = new PerformanceCounter("Processor Information", "DPC Rate", "_Total", true);
                CPU_DPCQUEUEDSEC = new PerformanceCounter("Processor Information", "DPCs Queued/sec", "_Total", true);

                CPU_PMAXFREQ = new PerformanceCounter("Processor Information", "% of Maximum Frequency", "_Total", true);
                CPU_PARKSTATUS = new PerformanceCounter("Processor Information", "Parking Status", "_Total", true);
                CPU_PROCFREQ = new PerformanceCounter("Processor Information", "Processor Frequency", "_Total", true);
                CPU_PROCSTATEFLAGS = new PerformanceCounter("Processor Information", "Processor State Flags", "_Total", true);
                
                Loaded = true;
            }
            catch (Exception EX)
            {
                MessageBox.Show("EXCEPTION\n\n==== Loading CPU Exception ===\n" +
                        "\nMessage: " + EX.Message +
                        "\nHResult: " + EX.HResult.ToString() +
                        "\nData: " + EX.Data +
                        "\nStackTrace: " + EX.StackTrace + "\n");
            }
        }

        public double nextCPU_PPROCTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PPROCTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PROCFREQ()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PROCFREQ.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PIDLETIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PIDLETIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PINTERTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PINTERTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PPRIVTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PPRIVTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PPRIOTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PPRIOTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PUSERTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PUSERTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PMAXFREQ()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PMAXFREQ.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PARKSTATUS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PARKSTATUS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PROCSTATEFLAGS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PROCSTATEFLAGS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PC1TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PC1TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PC2TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PC2TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PC3TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PC3TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_C1TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_C1TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_C2TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_C2TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_C3TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_C3TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_INTERSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_INTERSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_PDPCTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_PDPCTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_DPCRATE()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_DPCRATE.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCPU_DPCQUEUEDSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.CPU_DPCQUEUEDSEC.NextValue();
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
            NextValueList.Clear();

            NextValueList.Add(nextCPU_PPROCTIME());
            NextValueList.Add(nextCPU_PROCFREQ());
            NextValueList.Add(nextCPU_PIDLETIME());
            NextValueList.Add(nextCPU_PINTERTIME());
            NextValueList.Add(nextCPU_PPRIVTIME());
            NextValueList.Add(nextCPU_PPRIOTIME());
            NextValueList.Add(nextCPU_PUSERTIME());
            NextValueList.Add(nextCPU_PMAXFREQ());
            NextValueList.Add(nextCPU_PARKSTATUS());
            NextValueList.Add(nextCPU_PROCSTATEFLAGS());
            NextValueList.Add(nextCPU_PC1TIME());
            NextValueList.Add(nextCPU_PC2TIME());
            NextValueList.Add(nextCPU_PC3TIME());
            NextValueList.Add(nextCPU_C1TRANSEC());
            NextValueList.Add(nextCPU_C2TRANSEC());
            NextValueList.Add(nextCPU_C3TRANSEC());
            NextValueList.Add(nextCPU_INTERSEC());
            NextValueList.Add(nextCPU_PDPCTIME());
            NextValueList.Add(nextCPU_DPCRATE());
            NextValueList.Add(nextCPU_DPCQUEUEDSEC());

            return NextValueList;
        }
    }
}
