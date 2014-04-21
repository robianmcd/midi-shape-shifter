using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Mss
{
    public class EventLogger
    {
        protected bool enabled;
        public EventLogger() {
            this.enabled = false;
        }

        public void Init(IDryMssEventOutputPort dryMssEventOutputPort,
                         IWetMssEventOutputPort wetMssEventOutputPort)
        {
            dryMssEventOutputPort.DryMssEventRecieved += new DryMssEventRecievedEventHandler(DryEventRecieved);
            wetMssEventOutputPort.WetMssEventsReceived += new WetMssEventReceivedEventHandler(WetEventRecieved);

            wetMssEventOutputPort.SendingWetMssEvents += new SendingWetMssEventsEventHandler(OnCycleEnd);
        }

        protected void DryEventRecieved(MssEvent dryEvent)
        {
            if (!enabled) {
                return;
            }
            Debug.WriteLine("In: " + FormatEvent(dryEvent));
        }

        protected void WetEventRecieved(MssEvent wetEvent)
        {
            if (!enabled)
            {
                return;
            }
            Debug.WriteLine("Out: " + FormatEvent(wetEvent));
        }

        protected void OnCycleEnd(List<MssEvent> mssEventList, long sampleTimeAtEndOfProcessingCycle) {
            if (!enabled)
            {
                return;
            }
            Debug.Write(".");
        }

        protected string FormatEvent(MssEvent mssEvent) {
            return string.Format("{0} {1}, {2}, {3} at {4}",
                    mssEvent.mssMsg.Type,
                    Math.Round(mssEvent.mssMsg.Data1, 3),
                    Math.Round(mssEvent.mssMsg.Data2, 3),
                    Math.Round(mssEvent.mssMsg.Data3, 3),
                    mssEvent.sampleTime);
            
        }

    }
}
