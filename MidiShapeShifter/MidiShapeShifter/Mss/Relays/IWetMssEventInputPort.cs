using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IWetMssEventInputPort
    {
        void ReceiveWetMssEvent(MssEvent mssEvent);
        void OnProcessingCycleEnd(long SampleTimeAtEndOfCycle);
    }
}
