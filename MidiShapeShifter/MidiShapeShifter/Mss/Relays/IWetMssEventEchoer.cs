using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void EchoingWetMssEventsEventHandler(List<MssEvent> mssEventList, long processingCycleEndTimeInTicks);

    public interface IWetMssEventEchoer
    {
        bool OnlyEchoOnAudioCycleEnd { get; set; }

        event EchoingWetMssEventsEventHandler EchoingWetMssEvents;
    }
}
