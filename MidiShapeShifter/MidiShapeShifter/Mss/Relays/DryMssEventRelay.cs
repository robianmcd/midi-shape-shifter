namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts unprocessed MssEvents and sends a message notifying any subscribers of the MssEvent.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass unprocessed MssEvents from the "Framework" namespace to the "Mss" namespace.
    /// </remarks>
    public class DryMssEventRelay : IDryMssEventRelay
    {
        //See IDryMssEventInputPort
        public void ReceiveDryMssEvent(MssEvent mssEvent)
        {
            if (DryMssEventRecieved != null)
            {
                DryMssEventRecieved(mssEvent);
            }
        }

        //See IDryMssEventOutputPort
        public event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
