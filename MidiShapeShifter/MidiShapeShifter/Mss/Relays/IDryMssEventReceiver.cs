using System;
namespace MidiShapeShifter.Mss.Relays
{
    public interface IDryMssEventReceiver
    {
        void ReceiveDryMssEvent(MssEvent mssEvent);
    }
}
