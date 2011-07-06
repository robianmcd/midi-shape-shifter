using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void SendingWetMssEventsEventHandler(List<MssEvent> mssEventList, long processingCycleEndTimeInTicks);

    public interface IWetMssEventOutputPort
    {
        bool OnlySendOnProcessingCycleEnd { get; set; }

        event SendingWetMssEventsEventHandler SendingWetMssEvents;
    }
}
