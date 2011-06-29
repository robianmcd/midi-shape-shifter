using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public class DryMssEventRelay : IDryMssEventReceiver, IDryMssEventEchoer
    {
        public void ReceiveDryMssEvent(MssEvent mssEvent)
        {
            if (DryMssEventRecieved != null)
            {
            DryMssEventRecieved(mssEvent);
            }
        }

        public event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
