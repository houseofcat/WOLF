using System;
using System.Diagnostics;

namespace OracleSoft.HardwareStats
{
    class Drive: IDisposable
    {

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //LogicalDisk PerformanceCounters
                LD_PDRT.Close();
                LD_PDT.Close();
                LD_PDWT.Close();
                LD_PFS.Close();
                LD_PIT.Close();

                LD_ADBR.Close();
                LD_ADBT.Close();
                LD_ADBW.Close();
                LD_ADQL.Close();
                LD_ADRQL.Close();
                LD_ADWQL.Close();

                //Average Time Spent R,T,W
                LD_ADSECR.Close();
                LD_ADSECT.Close();
                LD_ADSECW.Close();

                LD_DBSEC.Close();
                LD_DBRSEC.Close();
                LD_DBWSEC.Close();
                LD_DREADSEC.Close();
                LD_DTRANSEC.Close();
                LD_DWRITSEC.Close();
                LD_SPLITIOSEC.Close();

                LD_CDQL.Close();
                LD_FREEMB.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //LogicalDisk PerformanceCounters
        PerformanceCounter LD_PDRT = new PerformanceCounter();
        PerformanceCounter LD_PDT = new PerformanceCounter();
        PerformanceCounter LD_PDWT = new PerformanceCounter();
        PerformanceCounter LD_PFS = new PerformanceCounter();
        PerformanceCounter LD_PIT = new PerformanceCounter();

        PerformanceCounter LD_ADBR = new PerformanceCounter();
        PerformanceCounter LD_ADBT = new PerformanceCounter();
        PerformanceCounter LD_ADBW = new PerformanceCounter();
        PerformanceCounter LD_ADQL = new PerformanceCounter();
        PerformanceCounter LD_ADRQL = new PerformanceCounter();
        PerformanceCounter LD_ADWQL = new PerformanceCounter();

        //Average Time Spent R,T,W
        PerformanceCounter LD_ADSECR = new PerformanceCounter();
        PerformanceCounter LD_ADSECT = new PerformanceCounter();
        PerformanceCounter LD_ADSECW = new PerformanceCounter();

        PerformanceCounter LD_DBSEC = new PerformanceCounter();
        PerformanceCounter LD_DBRSEC = new PerformanceCounter();
        PerformanceCounter LD_DBWSEC = new PerformanceCounter();
        PerformanceCounter LD_DREADSEC = new PerformanceCounter();
        PerformanceCounter LD_DTRANSEC = new PerformanceCounter();
        PerformanceCounter LD_DWRITSEC = new PerformanceCounter();
        PerformanceCounter LD_SPLITIOSEC = new PerformanceCounter();

        PerformanceCounter LD_CDQL = new PerformanceCounter();
        PerformanceCounter LD_FREEMB = new PerformanceCounter();

        public Drive()
        { }

        public Drive(string DriveLetter)
        {

        }
    }

    public enum SmartAttributeType : byte
    {
        ReadErrorRate = 0x01,
        ThroughputPerformance = 0x02,
        SpinUpTime = 0x03,
        StartStopCount = 0x04,
        ReallocatedSectorsCount = 0x05,
        ReadChannelMargin = 0x06,
        SeekErrorRate = 0x07,
        SeekTimePerformance = 0x08,
        PowerOnHoursPOH = 0x09,
        SpinRetryCount = 0x0A,
        CalibrationRetryCount = 0x0B,
        PowerCycleCount = 0x0C,
        SoftReadErrorRate = 0x0D,
        SATADownshiftErrorCount = 0xB7,
        EndtoEnderror = 0xB8,
        HeadStability = 0xB9,
        InducedOpVibrationDetection = 0xBA,
        ReportedUncorrectableErrors = 0xBB,
        CommandTimeout = 0xBC,
        HighFlyWrites = 0xBD,
        AirflowTemperatureWDC = 0xBE,
        TemperatureDifferencefrom100 = 0xBE,
        GSenseErrorRate = 0xBF,
        PoweroffRetractCount = 0xC0,
        LoadCycleCount = 0xC1,
        Temperature = 0xC2,
        HardwareECCRecovered = 0xC3,
        ReallocationEventCount = 0xC4,
        CurrentPendingSectorCount = 0xC5,
        UncorrectableSectorCount = 0xC6,
        UltraDMACRCErrorCount = 0xC7,
        MultiZoneErrorRate = 0xC8,
        WriteErrorRateFujitsu = 0xC8,
        OffTrackSoftReadErrorRate = 0xC9,
        DataAddressMarkerrors = 0xCA,
        RunOutCancel = 0xCB,
        SoftECCCorrection = 0xCC,
        ThermalAsperityRateTAR = 0xCD,
        FlyingHeight = 0xCE,
        SpinHighCurrent = 0xCF,
        SpinBuzz = 0xD0,
        OfflineSeekPerformance = 0xD1,
        VibrationDuringWrite = 0xD3,
        ShockDuringWrite = 0xD4,
        DiskShift = 0xDC,
        GSenseErrorRateAlt = 0xDD,
        LoadedHours = 0xDE,
        LoadUnloadRetryCount = 0xDF,
        LoadFriction = 0xE0,
        LoadUnloadCycleCount = 0xE1,
        LoadInTime = 0xE2,
        TorqueAmplificationCount = 0xE3,
        PowerOffRetractCycle = 0xE4,
        GMRHeadAmplitude = 0xE6,
        DriveTemperature = 0xE7,
        SSDLifeLeft_Kingston = 0xE7,
        EnduranceRemaining_Intel = 0xE8,
        AvailableReservedSpace = 0xE8,
        PowerOnHours = 0xE9,
        MediaWearoutIndicator_Intel = 0xE9,
        AvgEraseCount_MaxEraseCount = 0xEA,
        GBCAndSystemBC = 0xEB,
        HeadFlyingHours = 0xF0,
        TransferErrorRateFujitsu = 0xF0,
        TotalLBAsWritten = 0xF1,
        TotalLBAsRead = 0xF2,
        NANDWrites1GiB = 0xF9,
        ReadErrorRetryRate = 0xFA,
        FreeFallProtection = 0xFE,
    }
}
