using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        //Multiply BarsPerTimestampTick by a timestamp to get the number of bars into a song at 
        //that timestamp.
        public double BarsPerTimestampTick { get; private set; }
        public bool BarsPerTimestampTickIsInitialized { get; private set; }
        public event BarsPerTimestampTickUpdatedEventHandler BarsPerTimestampTickUpdated;

        public HostInfoRelay()
        {
            this.SampleRateIsInitialized = false;
            this.TempoIsInitialized = false;
            this.SampleRateIsInitialized = false;
            this.BarsPerTimestampTickIsInitialized = false;
        }


        public void ReceiveProcessingCycleEndTimestampInTicks(long cycleEndTimeStampInTicks)
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


        public void ReceiveSampleRate(double sampleRate)
        {
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

        public void ReceiveTempo(double tempo)
        {
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

        public void ReceiveBarPosition(double barPos, long timestampInTicks)
        {
            this.BarsPerTimestampTickIsInitialized = true;

            this.BarsPerTimestampTick = barPos / timestampInTicks;
            if (BarsPerTimestampTickUpdated != null)
            {
                BarsPerTimestampTickUpdated(this.BarsPerTimestampTick);
            }
        }


    }
}
