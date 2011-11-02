using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts information about the host and sends messages notifying any subscribers of 
    ///     the information. The intended usage of this calss is that at the end of every 
    ///     processing cycle the following sequence of actions will occur:
    ///     1. StartUpdate() will be called 
    ///     2. Any information about the host will be sent to this relay through the 
    ///          Recieve...DuringUpdate() functions.
    ///     3. FinishUpdate() will be called.
    ///     4. TriggerProcessingCycleEnd() will be called with the sample time at the end of the 
    ///          processing cycle.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass host information from the "Framework" namespace to the 
    ///     "Mss" namespace.
    /// </remarks>
    public class HostInfoRelay : IHostInfoInputPort, IHostInfoOutputPort
    {

        /// <summary>
        /// Specifies wheather this relay is accepting updates or not. This will be set to true 
        /// when StartUpdate() is called and set to false when FinishUpdate() is called.
        /// </summary>
        protected bool CurrentlyUpdating = false;

        /// <summary>
        /// Specifies the types of host info that this relay recieved on the last update.
        /// </summary>
        public HostInfoFields UpdatedFieldsInLastUpdate {get; private set;}

        /// <summary>
        /// Specifies the host info that has changed on the last update.
        /// </summary>
        public HostInfoFields ChangedFieldsInLastUpdate {get; private set;}

        /// <summary>
        /// Event that is fired after an update of host info has finished.
        /// </summary>
        public event HostUpdateFinishedEventHandler HostUpdateFinished;

        /// <summary>
        /// Used to trigger any processing that needs to happen before a processing cycle ends.
        /// </summary>
        public event ProcessingCycleEndEventHandler BeforeProcessingCycleEnd;

        /// <summary>
        /// Used to trigger any processing that should happen at the very end of a processing cycle.
        /// </summary>
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


        /// <summary>
        /// The sample time that the transport started playing. This value is calculated off of the
        /// current information in HostInfoRelay so if for example the tempo changed then so would
        /// CalculatedBarZeroSampleTime.
        /// </summary>
        public long CalculatedBarZeroSampleTime {get; private set;}
        public bool CalculatedBarZeroIsInitialized { get; private set; }
        public event CalculatedBarZeroChangedEventHandler CalculatedBarZeroChanged;
        protected double samplesPerBar;

        public bool BarPosIsInitialized { get; private set; }
        protected double barPosOnLastUpdate;
        protected long sampleTimeAtLastUpdate;

        public HostInfoRelay()
        {
            this.SampleRateIsInitialized = false;
            this.TempoIsInitialized = false;
            this.TransportPlayingIsInitialized = false;
            this.TimeSignatureIsInitialized = false;
            this.BarPosIsInitialized = false;
            this.CalculatedBarZeroIsInitialized = false;

            this.UpdatedFieldsInLastUpdate = HostInfoFields.None;
            this.ChangedFieldsInLastUpdate = HostInfoFields.None;
        }

        /// <summary>
        /// Gets the number of bars into the song at a given sampleTime. This value is only 
        /// calculated off of the most recent information so the result may not be accurate
        /// if sampleTime is less then it was at the end of the last processing cycle. For example
        /// if the transport was playing, then not playing and then playing again, this functions 
        /// would return a negative bar position for any sample times before the second time the
        /// transport started playing.
        /// </summary>
        /// <remarks>
        /// Precondition: CalculatedBarZeroIsInitialized is true. TransportPlayingIsInitialized is 
        /// true and TransportPlaying is true.
        /// </remarks>
        public double GetBarPosAtSampleTime(long sampleTime)
        {
            Debug.Assert(this.CalculatedBarZeroIsInitialized);
            Debug.Assert(this.TransportPlayingIsInitialized);
            Debug.Assert(this.TransportPlaying == true);

            long samplesSinceStart = sampleTime - CalculatedBarZeroSampleTime;
            return samplesSinceStart / this.samplesPerBar;
        }

        /// <summary>
        /// This function must be called before any information is recieved through the
        /// Receive...DuringUpdate() functions.
        /// </summary>
        public void StartUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == false);

            CurrentlyUpdating = true;
            this.UpdatedFieldsInLastUpdate = HostInfoFields.None;
            this.ChangedFieldsInLastUpdate = HostInfoFields.None;
        }

        /// <summary>
        /// Triggers events for subscribers to this relay's output port which will notify them of 
        /// changes made to the information in this relay. This function should be called after a
        /// subscriber to this relay's input port is done sending information.
        /// </summary>
        public void FinishUpdate()
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            //Finish any updates that are dependant on multiple fields.

            if (this.TempoIsInitialized == true && 
                this.TimeSignatureIsInitialized == true &&
                this.BarPosIsInitialized == true &&
                this.SampleRateIsInitialized == true)
            {
                //If anything has changed that could have changed the position of the calculated 
                //bar 0 then it will be updated (as will this.samplesPerBar).
                if (this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.Tempo) ||
                    this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.TimeSignature) ||
                    this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.BarPos) ||
                    this.ChangedFieldsInLastUpdate.HasFlag(HostInfoFields.SampleRate))
                {
                    double timeSig = (double)this.TimeSignatureNumerator / this.TimeSignatureDenominator;
                    double beatsPerBar = timeSig / (1 / 4d);
                    this.samplesPerBar = (beatsPerBar / this.Tempo) * (this.SampleRate * 60);

                    long newCalculatedBarZeroSampleTime = this.sampleTimeAtLastUpdate -
                            (long)System.Math.Round(this.barPosOnLastUpdate * samplesPerBar);
                    if (this.CalculatedBarZeroSampleTime != newCalculatedBarZeroSampleTime)
                    {
                        this.CalculatedBarZeroSampleTime = newCalculatedBarZeroSampleTime;
                        this.ChangedFieldsInLastUpdate |= HostInfoFields.CalculatedBarZero;
                    }

                    this.UpdatedFieldsInLastUpdate |= HostInfoFields.CalculatedBarZero;
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
                CalculatedBarZeroChanged(this.CalculatedBarZeroSampleTime);
            }
        }


        public void TriggerProcessingCycleEnd(long cycleEndSampleTime)
        {
            if (BeforeProcessingCycleEnd != null)
            {
                BeforeProcessingCycleEnd(cycleEndSampleTime);
            }
            if (ProcessingCycleEnd != null)
            {
                ProcessingCycleEnd(cycleEndSampleTime);
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

        public void ReceiveBarPositionDuringUpdate(double barPos, long sampleTime)
        {
            Debug.Assert(this.CurrentlyUpdating == true);

            this.UpdatedFieldsInLastUpdate |= HostInfoFields.BarPos;

            if (this.BarPosIsInitialized == false ||
                this.barPosOnLastUpdate != barPos || this.sampleTimeAtLastUpdate != sampleTime)
            {
                this.ChangedFieldsInLastUpdate |= HostInfoFields.BarPos;
                this.barPosOnLastUpdate = barPos;
                this.sampleTimeAtLastUpdate = sampleTime;

                this.BarPosIsInitialized = true;
            }
        }

    }
}
