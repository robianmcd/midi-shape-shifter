using System;
namespace MidiShapeShifter.Mss.Relays
{
    public delegate void DryMssEventRecievedEventHandler(MssEvent mssEvent);

    public interface IDryMssEventEchoer
    {
        event DryMssEventRecievedEventHandler DryMssEventRecieved;
    }
}
