using System;
namespace MidiShapeShifter.Mss.Relays
{
    public delegate void DryMssEventRecievedEventHandler(MssEvent mssEvent);

    public interface IDryMssEventEchoer
    {
        /// <summary>
        ///     Sends an unprocessed MssEvent to all subscribers.
        /// </summary>
        event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
