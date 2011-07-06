using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IWetMssEventInputPort
    {
        void ReceiveWetMssEventList(List<MssEvent> mssEventList);
        void OnProcessingCycleEnd(long cycleEndTimeInTicks);
    }
}
