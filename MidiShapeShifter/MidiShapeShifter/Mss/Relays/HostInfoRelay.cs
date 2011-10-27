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


        protected bool CurrentlyUpdating = false;
        public HostInfoFields UpdatedFieldsInLastUpdate {get; private set;}
        public HostInfoFields ChangedFieldsInLastUpdate {get; private set;}

        public event HostUpdateFinishedEventHandler HostUpdateFinished;

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


        public bool CalculatedBarZeroIsInitialized { get; private set; }
        public long CalculatedBarZeroTimestamp {get; private set;}
        protected double ticksPerBar;
        protected double barPosOnLastUpdate;
        protected long timestampOnLastUpdate;
        public event CalculatedBarZeroChangedEventHandler CalculatedBarZeroChanged;

        public HostInfoRelay()
        {
            this.SampleRateIsInitialized = false;
            this.TempoIsInitialized = false;
            this.TransportPlayingIsInitialized = false;
            this.CalculatedBarZeroIsInitialized = false;

            this.UpdatedFieldsInLastUpdate = HostInfoFields.None;
            this.ChangedFieldsInLastUpdate = HostInfoFields.None;
        }

        //Precondition: BarsPosIsInitialized is true.
        public double GetBarPosAtTimestamp(long timestamp)
        {
            Debug.Assert(CalculatedBarZeroIsInitialized);

            long timeSinceStart = timestamp - CalculatedBarZeroTimestamp;
            return timeSinceStart / ticksPerBar;
        }

        public void StartUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == false);

            CurrentlyUpdating = true;
            this.UpdatedFieldsInLastUpdate = HostInfoFields.None;
            this.ChangedFieldsInLastUpdate = HostInfoFields.None;
        }

        public void FinishUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            //Finish any updates that are dependant on multiple fields.

            if (this.TempoIsInitialized == true && this.TimeSignatureIsInitialized == true)
            {
                double timeSig = (double)this.TimeSignatureNumerator / this.TimeSignatureDenominator;
                double beatsPerBar = timeSig / (1 / 4d);
                this.ticksPerBar = (beatsPerBar / this.Tempo) * System.TimeSpan.TicksPerMinute;

                if (this.UpdatedFieldsInLastUpdate.HasFlag(HostInfoFields.CalculatedBarZero))
                {
                    long newCalculatedBarZeroTimestamp = this.timestampOnLastUpdate -
                            (long)System.Math.Round(this.barPosOnLastUpdate * ticksPerBar);
                    if (this.CalculatedBarZeroTimestamp != newCalculatedBarZeroTimestamp)
                    {
                        this.CalculatedBarZeroTimestamp = newCalculatedBarZeroTimestamp;
                        this.ChangedFieldsInLastUpdate |= HostInfoFields.CalculatedBarZero;
                    }

                    this.CalculatedBarZeroIsInitialized = true;
                }
            }

            CurrentlyUpdating = false;

            //Send Events
            if (this.HostUpdateFinished != null)
            {
                HostUpdateFinished(this.ChangedFieldsInLastUpdate);
            }

            if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.SampleRate) && 
                SampleRateChanged != null)
            {
                SampleRateChanged(this.SampleRate);
            }

            if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.Tempo) && 
                TempoChanged != null)
            {
                TempoChanged(this.Tempo);
            }

            if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.TimeSignature) && 
                TimeSignatureChanged != null)
            {
                TimeSignatureChanged(this.TimeSignatureNumerator, this.TimeSignatureDenominator);
            }

            if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.TransportPlaying) && 
                TransportPlayingChanged != null)
            {
                TransportPlayingChanged(this.TransportPlaying);
            }

            if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.CalculatedBarZero) &&
                CalculatedBarZeroChanged != null)
            {
                CalculatedBarZeroChanged(this.CalculatedBarZeroTimestamp);
            }
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

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.SampleRate;

            if (this.SampleRateIsInitialized == false || this.SampleRate != sampleRate) 
            {
                this.ChangedFieldsInLastUpdate |= HostInfoFields.SampleRate;
                this.SampleRate = sampleRate;

                this.SampleRateIsInitialized = true;
            }
        }

        public void ReceiveTempoDuringUpdate(double tempo)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.Tempo;

            if (this.TempoIsInitialized == false || this.Tempo != tempo)
            {
                this.ChangedFieldsInLastUpdate |= HostInfoFields.Tempo;
                this.Tempo = tempo;

                this.TempoIsInitialized = true;
            }
        }

        public void ReceiveTimeSignatureDuringUpdate(int numerator, int denominator)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.TimeSignature;

            if (this.TimeSignatureIsInitialized == false ||
                this.TimeSignatureNumerator != numerator || 
                this.TimeSignatureDenominator != denominator)
            {
                this.ChangedFieldsInLastUpdate |= HostInfoFields.TimeSignature;
                this.TimeSignatureNumerator = numerator;
                this.TimeSignatureDenominator = denominator;

                this.TimeSignatureIsInitialized = true;
            }
        }

        public void ReceiveTransportPlayingDuringUpdate(bool transportPlaying)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.TransportPlaying;

            if (this.TransportPlayingIsInitialized == false || 
                this.TransportPlaying != transportPlaying)
            {
                this.ChangedFieldsInLastUpdate |= HostInfoFields.TransportPlaying;
                this.TransportPlaying = transportPlaying;

                this.TransportPlayingIsInitialized = true;
            }
        }

        public void ReceiveBarPositionDuringUpdate(double barPos, long timestampInTicks)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.CalculatedBarZero;

            this.barPosOnLastUpdate = barPos;
            this.timestampOnLastUpdate = timestampInTicks;
        }

    }
}
