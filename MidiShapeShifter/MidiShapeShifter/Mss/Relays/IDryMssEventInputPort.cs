using System;
namespace MidiShapeShifter.Mss.Relays
{
    public interface IDryMssEventInputPort
    {
        void ReceiveDryMssEvent(MssEvent mssEvent);
    }
}
