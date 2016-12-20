using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OracleSoft.HardwareStats
{
    class Core: IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Core_PC1TIME.Close();
                Core_PC2TIME.Close();
                Core_PC3TIME.Close();

                //Percentage Times
                Core_PPROCTIME.Close();
                Core_PIDLETIME.Close();
                Core_PINTERTIME.Close();
                Core_PPRIVTIME.Close();
                Core_PPRIOTIME.Close();
                Core_PUSERTIME.Close();

                //Frequency/Performance
                Core_PMAXFREQ.Close();
                Core_PARKSTATUS.Close();
                Core_PROCFREQ.Close();
                Core_PROCSTATEFLAGS.Close();

                //Variables Per Second
                Core_C1TRANSEC.Close();
                Core_C2TRANSEC.Close();
                Core_C3TRANSEC.Close();
                Core_INTERSEC.Close();

                //DPC Variables & Percentage of Time Spent Receiving and Servicing Defferred Procedure Calls
                Core_PDPCTIME.Close();
                Core_DPCRATE.Close();
                Core_DPCQUEUEDSEC.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        string ProcessorNumber = "";
        string CoreNumber = "";
        bool Loaded = false;

        List<double> NextValueList = new List<double>();

        //ProcessorObject => http://technet.microsoft.com/en-us/library/cc786359(v=ws.10).aspx
        //Percent of Time Spent In Each Power State
        PerformanceCounter Core_PC1TIME = new PerformanceCounter();
        PerformanceCounter Core_PC2TIME = new PerformanceCounter();
        PerformanceCounter Core_PC3TIME = new PerformanceCounter();

        //Percentage Times
        PerformanceCounter Core_PPROCTIME = new PerformanceCounter();
        PerformanceCounter Core_PIDLETIME = new PerformanceCounter();
        PerformanceCounter Core_PINTERTIME = new PerformanceCounter();
        PerformanceCounter Core_PPRIVTIME = new PerformanceCounter();
        PerformanceCounter Core_PPRIOTIME = new PerformanceCounter();
        PerformanceCounter Core_PUSERTIME = new PerformanceCounter();

        //Frequency/Performance
        PerformanceCounter Core_PMAXFREQ = new PerformanceCounter();
        PerformanceCounter Core_PARKSTATUS = new PerformanceCounter();
        PerformanceCounter Core_PROCFREQ = new PerformanceCounter();
        PerformanceCounter Core_PROCSTATEFLAGS = new PerformanceCounter();

        //Variables Per Second
        PerformanceCounter Core_C1TRANSEC = new PerformanceCounter();
        PerformanceCounter Core_C2TRANSEC = new PerformanceCounter();
        PerformanceCounter Core_C3TRANSEC = new PerformanceCounter();
        PerformanceCounter Core_INTERSEC = new PerformanceCounter();

        //DPC Variables & Percentage of Time Spent Receiving and Servicing Defferred Procedure Calls
        PerformanceCounter Core_PDPCTIME = new PerformanceCounter();
        PerformanceCounter Core_DPCRATE = new PerformanceCounter();
        PerformanceCounter Core_DPCQUEUEDSEC = new PerformanceCounter();

        public Core(string ProcessorNumber, string CoreNumber)
        {
            this.ProcessorNumber = ProcessorNumber;
            this.CoreNumber = CoreNumber;
        }

        public void LoadCore()
        {
            if (CoreNumber != "")
            {
                try
                {
                    Core_PC1TIME = new PerformanceCounter("Processor Information", "% C1 Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PC2TIME = new PerformanceCounter("Processor Information", "% C2 Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PC3TIME = new PerformanceCounter("Processor Information", "% C3 Time", ProcessorNumber + "," + CoreNumber, true);

                    Core_PPROCTIME = new PerformanceCounter("Processor Information", "% Processor Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PIDLETIME = new PerformanceCounter("Processor Information", "% Idle Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PINTERTIME = new PerformanceCounter("Processor Information", "% Interrupt Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PPRIVTIME = new PerformanceCounter("Processor Information", "% Privileged Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PPRIOTIME = new PerformanceCounter("Processor Information", "% Priority Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_PUSERTIME = new PerformanceCounter("Processor Information", "% User Time", ProcessorNumber + "," + CoreNumber, true);

                    Core_C1TRANSEC = new PerformanceCounter("Processor Information", "C1 Transitions/sec", ProcessorNumber + "," + CoreNumber, true);
                    Core_C2TRANSEC = new PerformanceCounter("Processor Information", "C2 Transitions/sec", ProcessorNumber + "," + CoreNumber, true);
                    Core_C3TRANSEC = new PerformanceCounter("Processor Information", "C3 Transitions/sec", ProcessorNumber + "," + CoreNumber, true);
                    Core_INTERSEC = new PerformanceCounter("Processor Information", "Interrupts/sec", ProcessorNumber + "," + CoreNumber, true);

                    Core_PDPCTIME = new PerformanceCounter("Processor Information", "% DPC Time", ProcessorNumber + "," + CoreNumber, true);
                    Core_DPCRATE = new PerformanceCounter("Processor Information", "DPC Rate", ProcessorNumber + "," + CoreNumber, true);
                    Core_DPCQUEUEDSEC = new PerformanceCounter("Processor Information", "DPCs Queued/sec", ProcessorNumber + "," + CoreNumber, true);

                    Core_PMAXFREQ = new PerformanceCounter("Processor Information", "% of Maximum Frequency", ProcessorNumber + "," + CoreNumber, true);
                    Core_PARKSTATUS = new PerformanceCounter("Processor Information", "Parking Status", ProcessorNumber + "," + CoreNumber, true);
                    Core_PROCFREQ = new PerformanceCounter("Processor Information", "Processor Frequency", ProcessorNumber + "," + CoreNumber, true);
                    //Core_PROCFREQ = new PerformanceCounter("Processor Performance", "Processor Frequency", "PPM_Processor_" + CoreNumber, true);
                    Core_PROCSTATEFLAGS = new PerformanceCounter("Processor Information", "Processor State Flags", ProcessorNumber + "," + CoreNumber, true);

                    Loaded = true;
                }
                catch (Exception EX)
                {
                    MessageBox.Show("EXCEPTION\n\n==== Loading Core Exception ===\nCore: " + this.CoreNumber +
                            "\nMessage: " + EX.Message +
                            "\nHResult: " + EX.HResult.ToString() +
                            "\nData: " + EX.Data +
                            "\nStackTrace: " + EX.StackTrace + "\n");
                }
            }
        }

        public double nextCore_PC1TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PC1TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PC2TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PC2TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PC3TIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PC3TIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PPROCTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PPROCTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PIDLETIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PIDLETIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PINTERTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PINTERTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PPRIVTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PPRIVTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PPRIOTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PPRIOTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PUSERTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PUSERTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PMAXFREQ()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PMAXFREQ.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PARKSTATUS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PARKSTATUS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PROCFREQ()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PROCFREQ.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PROCSTATEFLAGS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PROCSTATEFLAGS.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_C1TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_C1TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_C2TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_C2TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_C3TRANSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_C3TRANSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_INTERSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_INTERSEC.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_PDPCTIME()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_PDPCTIME.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_DPCRATE()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_DPCRATE.NextValue();
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }

            return temp;
        }

        public double nextCore_DPCQUEUEDSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = this.Core_DPCQUEUEDSEC.NextValue();
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
            double temp = -1.0;
            double.TryParse(CoreNumber, out temp);
            NextValueList.Add(temp);

            try
            {
                NextValueList.Add(nextCore_PPROCTIME());
                NextValueList.Add(nextCore_PROCFREQ());
                NextValueList.Add(nextCore_PIDLETIME());
                NextValueList.Add(nextCore_PINTERTIME());
                NextValueList.Add(nextCore_PPRIVTIME());
                NextValueList.Add(nextCore_PPRIOTIME());
                NextValueList.Add(nextCore_PUSERTIME());
                NextValueList.Add(nextCore_PMAXFREQ());
                NextValueList.Add(nextCore_PARKSTATUS());
                NextValueList.Add(nextCore_PROCSTATEFLAGS());
                NextValueList.Add(nextCore_PC1TIME());
                NextValueList.Add(nextCore_PC2TIME());
                NextValueList.Add(nextCore_PC3TIME());
                NextValueList.Add(nextCore_C1TRANSEC());
                NextValueList.Add(nextCore_C2TRANSEC());
                NextValueList.Add(nextCore_C3TRANSEC());
                NextValueList.Add(nextCore_INTERSEC());
                NextValueList.Add(nextCore_PDPCTIME());
                NextValueList.Add(nextCore_DPCRATE());
                NextValueList.Add(nextCore_DPCQUEUEDSEC());
            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }

            return NextValueList;
        }
    }
}

