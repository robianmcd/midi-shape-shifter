namespace MidiShapeShifter.Mss.Relays
{
    public interface IWetMssEventInputPort
    {
        void ReceiveWetMssEvent(MssEvent mssEvent);
        void OnProcessingCycleEnd(long SampleTimeAtEndOfCycle);
    }
}
