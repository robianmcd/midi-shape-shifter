namespace MidiShapeShifter.Mss.Relays
{
    public delegate void DryMssEventRecievedEventHandler(MssEvent mssEvent);

    public interface IDryMssEventOutputPort
    {
        /// <summary>
        ///     Sends an unprocessed MssEvent to all subscribers.
        /// </summary>
        event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
