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
        public event ProcessingCycleEndTimestampRecievedEventHandler ProcessingCycleEndTimestampRecieved;

        public double SampleRate { get; private set; }
        public bool SampleRateIsInitialized { get; private set; }
        public event SampleRateChangedEventHandler SampleRateChanged;

        public HostInfoRelay()
        {
            SampleRateIsInitialized = false;
        }


        public void ReceiveProcessingCycleEndTimestampInTicks(long cycleEndTimeStampInTicks)
        {
            if (ProcessingCycleEndTimestampRecieved != null)
            {
                ProcessingCycleEndTimestampRecieved(cycleEndTimeStampInTicks);
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

    }
}
