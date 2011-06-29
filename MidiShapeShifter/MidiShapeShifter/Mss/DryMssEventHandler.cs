using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    public class DryMssEventHandler
    {
        protected MssMsgProcessor mssMsgProcessor;
        protected IWetMssEventReceiver wetMssEventReceiver;

        public DryMssEventHandler()
        {
            mssMsgProcessor = new MssMsgProcessor();
        }

        public void Init(IDryMssEventEchoer dryMssEventEchoer, IWetMssEventReceiver wetMssEventReceiver, MappingManager mappingMgr)
        {
            this.mssMsgProcessor.Init(mappingMgr);

            this.wetMssEventReceiver = wetMssEventReceiver;

            dryMssEventEchoer.DryMssEventRecieved += new DryMssEventRecievedEventHandler(dryMssEventEchoer_DryMssEventRecieved);
        }

        protected void dryMssEventEchoer_DryMssEventRecieved(MssEvent dryMssEvent)
        {

            List<MssMsg> mssMessages = this.mssMsgProcessor.ProcessMssMsg(dryMssEvent.mssMsg);

            List<MssEvent> wetEventList = new List<MssEvent>(mssMessages.Count);
            foreach (MssMsg mssMsg in mssMessages)
            {
                MssEvent wetEvent = new MssEvent();
                wetEvent.mssMsg = mssMsg;
                wetEvent.timestamp = dryMssEvent.timestamp;
                wetEventList.Add(wetEvent);
            }

            this.wetMssEventReceiver.ReceiveWetMssEventList(wetEventList);
        }
    }
}
