using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts unprocessed MssEvents and sends a message notifying any subscribers of the MssEvent.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass unprocessed MssEvents from the "Framework" namespace to the "Mss" namespace.
    /// </remarks>
    public class DryMssEventRelay : IDryMssEventReceiver, IDryMssEventEchoer
    {
        //See IDryMssEventReceiver
        public void ReceiveDryMssEvent(MssEvent mssEvent)
        {
            if (DryMssEventRecieved != null)
            {
                DryMssEventRecieved(mssEvent);
            }
        }

        //See IDryMssEventEchoer
        public event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
