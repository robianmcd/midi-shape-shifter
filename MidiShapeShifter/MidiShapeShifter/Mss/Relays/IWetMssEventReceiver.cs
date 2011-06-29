using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IWetMssEventReceiver
    {
        void ReceiveWetMssEventList(List<MssEvent> mssEventList);
        void OnProcessingCycleEnd(long cycleEndTimeInTicks);
    }
}
