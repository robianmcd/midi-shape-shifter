using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public class WetMssEventRelay : IWetMssEventReceiver, IWetMssEventEchoer
    {
        protected List<MssEvent> mssEventBuffer = new List<MssEvent>();
        protected bool _onlyEchoOnAudioCycleEnd = true;

        protected List<MssEvent> transferMssEventBufferContentToNewList()
        {
            List<MssEvent> eventsToTransfer = new List<MssEvent>(this.mssEventBuffer.Count);
            eventsToTransfer.AddRange(this.mssEventBuffer);

            this.mssEventBuffer.Clear();

            return eventsToTransfer;
        }



        public bool OnlyEchoOnAudioCycleEnd
        {
            get
            {
                return this._onlyEchoOnAudioCycleEnd;
            }
            set
            {
                this._onlyEchoOnAudioCycleEnd = value;
            }
        }

        public void ReceiveWetMssEventList(List<MssEvent> mssEventList)
        {
            this.mssEventBuffer.AddRange(mssEventList);


            if (this.OnlyEchoOnAudioCycleEnd == false && EchoingWetMssEvents != null)
            {
                EchoingWetMssEvents(transferMssEventBufferContentToNewList(), 0);
            }
        }

        



        public event EchoingWetMssEventsEventHandler EchoingWetMssEvents;

        public void OnProcessingCycleEnd(long cycleEndTimeInTicks)
        {
            if (this.OnlyEchoOnAudioCycleEnd == true && EchoingWetMssEvents != null)
            {
                EchoingWetMssEvents(transferMssEventBufferContentToNewList(), cycleEndTimeInTicks);
            }
        }
    }
}
