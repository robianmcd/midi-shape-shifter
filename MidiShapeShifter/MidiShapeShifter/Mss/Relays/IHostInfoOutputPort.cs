using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void ProcessingCycleEndTimestampRecievedEventHandler(long cycleEndTimestampInTicks);
    public delegate void SampleRateChangedEventHandler(double sampleRate);


    public interface IHostInfoOutputPort
    {
        event ProcessingCycleEndTimestampRecievedEventHandler ProcessingCycleEndTimestampRecieved;

        double SampleRate { get; }
        bool SampleRateIsInitialized { get; }
        event SampleRateChangedEventHandler SampleRateChanged;
    }
}
