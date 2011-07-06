using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IHostInfoInputPort
    {
        void ReceiveProcessingCycleEndTimestampInTicks(long cycleEndTimeStampInTicks);
        void ReceiveSampleRate(double sampleRate);
    }
}
