using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void ProcessingCycleEndEventHandler(long cycleEndTimestampInTicks);
    public delegate void SampleRateChangedEventHandler(double sampleRate);
    public delegate void TempoChangedEventHandler(double tempo);
    public delegate void BarsPerTimestampTickUpdatedEventHandler(double barsPerTimestampTick);


    public interface IHostInfoOutputPort
    {
        event ProcessingCycleEndEventHandler BeforeProcessingCycleEnd;
        event ProcessingCycleEndEventHandler ProcessingCycleEnd;

        double SampleRate { get; }
        bool SampleRateIsInitialized { get; }
        event SampleRateChangedEventHandler SampleRateChanged;

        double Tempo { get; }
        bool TempoIsInitialized { get; }
        event TempoChangedEventHandler TempoChanged;

        double BarsPerTimestampTick { get; }
        bool BarsPerTimestampTickIsInitialized { get; }
        event BarsPerTimestampTickUpdatedEventHandler BarsPerTimestampTickUpdated;
    }
}
