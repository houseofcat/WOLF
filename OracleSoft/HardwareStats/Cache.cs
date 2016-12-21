using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OracleSoft.SoftwareStats
{
    class Cache: IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                CAC_DP.Close();
                CAC_DPT.Close();

                CAC_DP.Close();
                CAC_DPT.Close();

            //Async
                CAC_ADMSEC.Close();
                CAC_ACRSEC.Close();
                CAC_AFRSEC.Close();
                CAC_AMDLRSEC.Close();
                CAC_APRSEC.Close();

            //Sync
                CAC_SDMSEC.Close();
                CAC_SCRSEC.Close();
                CAC_SFRSEC.Close();
                CAC_SMDLRSEC.Close();
                CAC_SPRSEC.Close();

            //Reads
                CAC_READAH.Close();
                CAC_PCRHITS.Close();
                CAC_CRSEC.Close();
                CAC_FRSEC.Close();
                CAC_FRRMSEC.Close();
                CAC_FRNPSEC.Close();
                CAC_PMDLRHIT.Close();
                CAC_MDLRSEC.Close();
                CAC_PPRHIT.Close();
                CAC_PRSEC.Close();

            //Writes
                CAC_LWFSEC.Close();
                CAC_LWPSEC.Close();

            //DATA
                CAC_DFSEC.Close();
                CAC_DFPSEC.Close();
                CAC_PDMHIT.Close();
                CAC_DMPSEC.Close();
                CAC_DMSEC.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Loaded = false;
        public List<double> NextValues = new List<double>();
        //Generic
        PerformanceCounter CAC_DP = new PerformanceCounter();
        PerformanceCounter CAC_DPT = new PerformanceCounter();

        //Async
        PerformanceCounter CAC_ADMSEC = new PerformanceCounter();
        PerformanceCounter CAC_ACRSEC = new PerformanceCounter();
        PerformanceCounter CAC_AFRSEC = new PerformanceCounter();
        PerformanceCounter CAC_AMDLRSEC = new PerformanceCounter();
        PerformanceCounter CAC_APRSEC = new PerformanceCounter();

        //Sync
        PerformanceCounter CAC_SDMSEC = new PerformanceCounter();
        PerformanceCounter CAC_SCRSEC = new PerformanceCounter();
        PerformanceCounter CAC_SFRSEC = new PerformanceCounter();
        PerformanceCounter CAC_SMDLRSEC = new PerformanceCounter();
        PerformanceCounter CAC_SPRSEC = new PerformanceCounter();

        //Reads
        PerformanceCounter CAC_READAH = new PerformanceCounter();
        PerformanceCounter CAC_PCRHITS = new PerformanceCounter();
        PerformanceCounter CAC_CRSEC = new PerformanceCounter();
        PerformanceCounter CAC_FRSEC = new PerformanceCounter();
        PerformanceCounter CAC_FRRMSEC = new PerformanceCounter();
        PerformanceCounter CAC_FRNPSEC = new PerformanceCounter();
        PerformanceCounter CAC_PMDLRHIT = new PerformanceCounter();
        PerformanceCounter CAC_MDLRSEC = new PerformanceCounter();
        PerformanceCounter CAC_PPRHIT = new PerformanceCounter();
        PerformanceCounter CAC_PRSEC = new PerformanceCounter();

        //Writes
        PerformanceCounter CAC_LWFSEC = new PerformanceCounter();
        PerformanceCounter CAC_LWPSEC = new PerformanceCounter();

        //DATA
        PerformanceCounter CAC_DFSEC = new PerformanceCounter();
        PerformanceCounter CAC_DFPSEC = new PerformanceCounter();
        PerformanceCounter CAC_PDMHIT = new PerformanceCounter();
        PerformanceCounter CAC_DMPSEC = new PerformanceCounter();
        PerformanceCounter CAC_DMSEC = new PerformanceCounter();

        public Cache()
        {

        }

        public void LoadCache()
        {
            try
            {
                CAC_DP = new PerformanceCounter("Cache", "Dirty Pages", true);
                CAC_DPT = new PerformanceCounter("Cache", "Dirty Page Threshold", true);

                //Async
                CAC_ADMSEC = new PerformanceCounter("Cache", "Async Data Maps/sec", true);
                CAC_ACRSEC = new PerformanceCounter("Cache", "Async Copy Reads/sec", true);
                CAC_AFRSEC = new PerformanceCounter("Cache", "Async Fast Reads/sec", true);
                CAC_AMDLRSEC = new PerformanceCounter("Cache", "Async MDL Reads/sec", true);
                CAC_APRSEC = new PerformanceCounter("Cache", "Async Pin Reads/sec", true);

                //Sync
                CAC_SDMSEC = new PerformanceCounter("Cache", "Sync Data Maps/sec", true);
                CAC_SCRSEC = new PerformanceCounter("Cache", "Sync Copy Reads/sec", true);
                CAC_SFRSEC = new PerformanceCounter("Cache", "Sync Fast Reads/sec", true);
                CAC_SMDLRSEC = new PerformanceCounter("Cache", "Sync MDL Reads/sec", true);
                CAC_SPRSEC = new PerformanceCounter("Cache", "Sync Pin Reads/sec", true);

                //Reads
                CAC_READAH = new PerformanceCounter("Cache", "Read Aheads/sec", true);
                CAC_PCRHITS = new PerformanceCounter("Cache", "Copy Read Hits %", true);
                CAC_CRSEC = new PerformanceCounter("Cache", "Copy Reads/sec", true);
                CAC_FRSEC = new PerformanceCounter("Cache", "Fast Reads/sec", true);
                CAC_FRRMSEC = new PerformanceCounter("Cache", "Fast Read Resource Misses/sec", true);
                CAC_FRNPSEC = new PerformanceCounter("Cache", "Fast Read Not Possibles/sec", true);
                CAC_PMDLRHIT = new PerformanceCounter("Cache", "MDL Read Hits %", true);
                CAC_MDLRSEC = new PerformanceCounter("Cache", "MDL Reads/sec", true);
                CAC_PPRHIT = new PerformanceCounter("Cache", "Pin Read Hits %", true);
                CAC_PRSEC = new PerformanceCounter("Cache", "Pin Reads/sec", true);

                //Writes
                CAC_LWFSEC = new PerformanceCounter("Cache", "Lazy Write Flushes/sec", true);
                CAC_LWPSEC = new PerformanceCounter("Cache", "Lazy Write Pages/sec", true);

                //DATA
                CAC_DFSEC = new PerformanceCounter("Cache", "Data Flushes/sec", true);
                CAC_DFPSEC = new PerformanceCounter("Cache", "Data Flush Pages/sec", true);
                CAC_PDMHIT = new PerformanceCounter("Cache", "Data Map Hits %", true);
                CAC_DMPSEC = new PerformanceCounter("Cache", "Data Map Pins/sec", true);
                CAC_DMSEC = new PerformanceCounter("Cache", "Data Maps/sec", true);

                Loaded = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());

                MessageBox.Show("EXCEPTION\n\n==== Loading CACHE Exception ===\nCACHE: " +
                        "\nMessage: " + ex.Message +
                        "\nHResult: " + ex.HResult.ToString() +
                        "\nData: " + ex.Data +
                        "\nStackTrace: " + ex.StackTrace + "\n");

                /*LSYSExm = ex.Message;
                LSYSExs = ex.StackTrace;
                SYSLoaded = false;
                ExceptionCounter++;*/
            }
        }

        public double nextCAC_DP()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DP.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_DPT()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DPT.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_ADMSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_ADMSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_ACRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_ACRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_AFRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_AFRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_AMDLRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_AMDLRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_APRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_APRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_SDMSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_SDMSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_SCRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_SCRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_SFRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_SFRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_SMDLRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_SMDLRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_SPRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_SPRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_READAH()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_READAH.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_PCRHITS()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_PCRHITS.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_CRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_CRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_FRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_FRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_FRRMSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_FRRMSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_FRMPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_FRNPSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_PMDLRHIT()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_PMDLRHIT.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_MDLRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_MDLRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_PPRHIT()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_PPRHIT.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_PRSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_PRSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_LWFSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_LWFSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_LWPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_LWPSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_DFSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DFSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_DFPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DFPSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_PDMHIT()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_LWPSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_DMPSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DMPSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public double nextCAC_DMSEC()
        {
            double temp = -1.0;

            if (Loaded)
            {
                try
                {
                    temp = CAC_DMSEC.NextValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return temp;
        }

        public List<double> getNextValues()
        {
            NextValues.Clear();

            NextValues.Add(nextCAC_DP());
            NextValues.Add(nextCAC_DPT());

            //Async
            NextValues.Add(nextCAC_ADMSEC());
            NextValues.Add(nextCAC_ACRSEC());
            NextValues.Add(nextCAC_AFRSEC());
            NextValues.Add(nextCAC_AMDLRSEC());
            NextValues.Add(nextCAC_APRSEC());

            //Sync
            NextValues.Add(nextCAC_SDMSEC());
            NextValues.Add(nextCAC_SCRSEC());
            NextValues.Add(nextCAC_SFRSEC());
            NextValues.Add(nextCAC_SMDLRSEC());
            NextValues.Add(nextCAC_SPRSEC());

            //Reads
            NextValues.Add(nextCAC_READAH());
            NextValues.Add(nextCAC_PCRHITS());
            NextValues.Add(nextCAC_CRSEC());
            NextValues.Add(nextCAC_FRSEC());
            NextValues.Add(nextCAC_FRRMSEC());
            NextValues.Add(nextCAC_FRMPSEC());
            NextValues.Add(nextCAC_PMDLRHIT());
            NextValues.Add(nextCAC_MDLRSEC());
            NextValues.Add(nextCAC_PPRHIT());
            NextValues.Add(nextCAC_PRSEC());

            //Writes
            NextValues.Add(nextCAC_LWFSEC());
            NextValues.Add(nextCAC_LWPSEC());

            //DATA
            NextValues.Add(nextCAC_DFSEC());
            NextValues.Add(nextCAC_DFPSEC());
            NextValues.Add(nextCAC_PDMHIT());
            NextValues.Add(nextCAC_DMPSEC());
            NextValues.Add(nextCAC_DMSEC());

            return NextValues;
        }
    }
}
