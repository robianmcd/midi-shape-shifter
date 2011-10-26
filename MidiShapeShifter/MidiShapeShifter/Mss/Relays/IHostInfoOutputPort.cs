using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void ProcessingCycleEndEventHandler(long cycleEndTimestampInTicks);
    public delegate void SampleRateChangedEventHandler(double sampleRate);
    public delegate void TempoChangedEventHandler(double tempo);
    public delegate void TimeSignatureChangedEventHandler(int numerator, int denominator);
    public delegate void TransportPlayingChangedEventHandler(bool TransportPlaying);


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

        int TimeSignatureNumerator { get; }
        int TimeSignatureDenominator { get; }
        bool TimeSignatureIsInitialized { get; }
        event TimeSignatureChangedEventHandler TimeSignatureChanged;

        bool TransportPlaying { get; }
        bool TransportPlayingIsInitialized { get; }
        event TransportPlayingChangedEventHandler TransportPlayingChanged;

        bool BarPosIsInitialized { get; }
        double GetBarPosAtTimestamp(long timestamp);
    }
}
