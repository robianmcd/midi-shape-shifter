using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts processed MssEvents and sends a message notifying any subscribers of the MssEvent.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass processed MssEvents from the "Mss" namespace to the "Framework" namespace. 
    ///     Depending on the value of OnlyEchoOnProcessingCycleEnd this class will either echo received events 
    ///     immeadateally or wait for the end of the next processing cycle to echo the events.
    /// </remarks>
    public class WetMssEventRelay : IWetMssEventReceiver, IWetMssEventEchoer
    {
        protected List<MssEvent> mssEventBuffer = new List<MssEvent>();
        protected bool _onlyEchoOnProcessingCycleEnd = true;

        /// <summary>
        ///     Moves the content of mssEventBuffer into a new list.
        /// </summary>
        /// <returns>A list containing the elements that were moved out of mssEventBuffer.</returns>
        protected List<MssEvent> transferMssEventBufferContentToNewList()
        {
            List<MssEvent> eventsToTransfer = new List<MssEvent>(this.mssEventBuffer.Count);
            eventsToTransfer.AddRange(this.mssEventBuffer);

            this.mssEventBuffer.Clear();

            return eventsToTransfer;
        }


        /// <summary>
        ///     Depermines wheather events are echoed immeatateally after they are received or after the end of the 
        ///     next processing cycle.
        /// </summary>
        public bool OnlyEchoOnProcessingCycleEnd
        {
            get
            {
                return this._onlyEchoOnProcessingCycleEnd;
            }
            set
            {
                this._onlyEchoOnProcessingCycleEnd = value;
            }
        }

        public void ReceiveWetMssEventList(List<MssEvent> mssEventList)
        {
            this.mssEventBuffer.AddRange(mssEventList);


            if (this.OnlyEchoOnProcessingCycleEnd == false && EchoingWetMssEvents != null)
            {
                EchoingWetMssEvents(transferMssEventBufferContentToNewList(), 0);
            }
        }

        

        public event EchoingWetMssEventsEventHandler EchoingWetMssEvents;

        public void OnProcessingCycleEnd(long cycleEndTimeInTicks)
        {
            if (this.OnlyEchoOnProcessingCycleEnd == true && EchoingWetMssEvents != null)
            {
                EchoingWetMssEvents(transferMssEventBufferContentToNewList(), cycleEndTimeInTicks);
            }
        }
    }
}
