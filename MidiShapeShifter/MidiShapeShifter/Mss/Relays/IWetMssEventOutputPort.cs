using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void WetMssEventReceivedEventHandler(MssEvent mssEventList);
    public delegate void SendingWetMssEventsEventHandler(List<MssEvent> mssEventList, 
            long sampleTimeAtEndOfProcessingCycle);

    public interface IWetMssEventOutputPort
    {
        bool OnlySendOnProcessingCycleEnd { get; set; }

        event WetMssEventReceivedEventHandler WetMssEventsReceived;
        event SendingWetMssEventsEventHandler SendingWetMssEvents;
    }
}
