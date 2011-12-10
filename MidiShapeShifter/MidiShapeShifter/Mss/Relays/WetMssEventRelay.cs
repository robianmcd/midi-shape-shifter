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
    ///     Depending on the value of OnlySendOnProcessingCycleEnd this class will either send received events 
    ///     immeadateally or wait for the end of the next processing cycle to send the events.
    /// </remarks>
    public class WetMssEventRelay : IWetMssEventRelay
    {
        protected List<MssEvent> mssEventBuffer = new List<MssEvent>();
        protected bool _onlySendOnProcessingCycleEnd = true;

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
        ///     Depermines wheather events are sent immeatateally after they are received or after the end of the 
        ///     next processing cycle.
        /// </summary>
        public bool OnlySendOnProcessingCycleEnd
        {
            get
            {
                return this._onlySendOnProcessingCycleEnd;
            }
            set
            {
                this._onlySendOnProcessingCycleEnd = value;
            }
        }

        public void ReceiveWetMssEventList(List<MssEvent> mssEventList)
        {
            this.mssEventBuffer.AddRange(mssEventList);

            if (WetMssEventsReceived != null)
            {
                WetMssEventsReceived(mssEventList);
            }

            if (this.OnlySendOnProcessingCycleEnd == false && SendingWetMssEvents != null)
            {
                SendingWetMssEvents(transferMssEventBufferContentToNewList(), 0);
            }
        }

        //Always sent immediately after recieving Mss Events
        public event WetMssEventsReceivedEventHandler WetMssEventsReceived;
        //If OnlySendOnProcessingCycleEnd is set to true then this event will 
        //only be sent at the end of a processing cycle.
        public event SendingWetMssEventsEventHandler SendingWetMssEvents;

        public void OnProcessingCycleEnd(long SampleTimeAtEndOfCycle)
        {
            if (this.OnlySendOnProcessingCycleEnd == true && SendingWetMssEvents != null)
            {
                SendingWetMssEvents(transferMssEventBufferContentToNewList(), SampleTimeAtEndOfCycle);
            }
        }
    }
}
