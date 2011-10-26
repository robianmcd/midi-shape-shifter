using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts information from the host and sends a messages notifying any subscribers of the information.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass host information from the "Framework" namespace to the "Mss" namespace.
    /// </remarks>
    public class HostInfoRelay : IHostInfoInputPort, IHostInfoOutputPort
    {
        [Flags]
        public enum HostInfoFields
        { 
            None = 0x0,
            SampleRate = 0x1,
            Tempo = 0x2,
            TimeSignature = 0x4,
            TransportPlaying = 0x8,
            CalculatedBarZero = 0x10
        }

        protected bool CurrentlyUpdating = false;
        public HostInfoFields ChangesInLastUpdate;


        //used to trigger any processing that needs to happen before a processing cycle ends.
        public event ProcessingCycleEndEventHandler BeforeProcessingCycleEnd;
        //used to trigger any processing that should happen at the very end of a processing cycle.
        public event ProcessingCycleEndEventHandler ProcessingCycleEnd;

        public double SampleRate { get; private set; }
        public bool SampleRateIsInitialized { get; private set; }
        public event SampleRateChangedEventHandler SampleRateChanged;

        public double Tempo { get; private set; }
        public bool TempoIsInitialized { get; private set; }
        public event TempoChangedEventHandler TempoChanged;

        public int TimeSignatureNumerator { get; private set; }
        public int TimeSignatureDenominator { get; private set; }
        public bool TimeSignatureIsInitialized { get; private set; }
        public event TimeSignatureChangedEventHandler TimeSignatureChanged;

        public bool TransportPlaying { get; private set; }
        public bool TransportPlayingIsInitialized { get; private set; }
        public event TransportPlayingChangedEventHandler TransportPlayingChanged;


        public bool BarPosIsInitialized { get; private set; }
        private long calculatedBarZeroTimestamp;
        private double ticksPerBar;

        //Precondition: BarsPosIsInitialized is true.
        public double GetBarPosAtTimestamp(long timestamp)
        {
            Debug.Assert(BarPosIsInitialized);

            long timeSinceStart = timestamp - calculatedBarZeroTimestamp;
            return timeSinceStart / ticksPerBar;
        }

        public HostInfoRelay()
        {
            this.SampleRateIsInitialized = false;
            this.TempoIsInitialized = false;
            this.SampleRateIsInitialized = false;
            this.TransportPlayingIsInitialized = false;
            this.BarPosIsInitialized = false;
        }

        public void StartUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == false);

            ChangesInLastUpdate = HostInfoFields.None;
            CurrentlyUpdating = true;
        }

        public void FinishUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            CurrentlyUpdating = false;
        }


        public void TriggerProcessingCycleEnd(long cycleEndTimeStampInTicks)
        {
            if (BeforeProcessingCycleEnd != null)
            {
                BeforeProcessingCycleEnd(cycleEndTimeStampInTicks);
            }
            if (ProcessingCycleEnd != null)
            {
                ProcessingCycleEnd(cycleEndTimeStampInTicks);
            }
        }


        public void ReceiveSampleRateDuringUpdate(double sampleRate)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.SampleRateIsInitialized = true;

            if (this.SampleRate != sampleRate) 
            {
                this.SampleRate = sampleRate;
                if (SampleRateChanged != null) 
                {
                    SampleRateChanged(this.SampleRate);
                }                
            }
        }

        public void ReceiveTempoDuringUpdate(double tempo)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.TempoIsInitialized = true;

            if (this.Tempo != tempo)
            {
                this.Tempo = tempo;
                if (TempoChanged != null)
                {
                    TempoChanged(this.Tempo);
                }
            }
        }

        public void ReceiveTimeSignatureDuringUpdate(int numerator, int denominator)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.TimeSignatureIsInitialized = true;

            if (this.TimeSignatureNumerator != numerator || this.TimeSignatureDenominator != denominator)
            {
                this.TimeSignatureNumerator = numerator;
                this.TimeSignatureDenominator = denominator;
                if (TimeSignatureChanged != null)
                {
                    TimeSignatureChanged(this.TimeSignatureNumerator, this.TimeSignatureDenominator);
                }
            }
        }

        public void ReceiveTransportPlayingDuringUpdate(bool transportPlaying)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.TransportPlayingIsInitialized = true;

            if (this.TransportPlaying != transportPlaying)
            {
                this.TransportPlaying = transportPlaying;
                if (TransportPlayingChanged != null)
                {
                    TransportPlayingChanged(this.TransportPlaying);
                }
            }
        }

        public void ReceiveBarPositionDuringUpdate(double barPos, long timestampInTicks)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            if (this.TempoIsInitialized == false || this.TimeSignatureIsInitialized == false)
            {
                return;
            }
            this.BarPosIsInitialized = true;

            double timeSig = (double)this.TimeSignatureNumerator / this.TimeSignatureDenominator;
            double beatsPerBar = timeSig / (1/4d);
            this.ticksPerBar = (beatsPerBar / this.Tempo) * System.TimeSpan.TicksPerMinute;

            this.calculatedBarZeroTimestamp = timestampInTicks - (long)System.Math.Round(barPos * ticksPerBar);
        }


    }
}
