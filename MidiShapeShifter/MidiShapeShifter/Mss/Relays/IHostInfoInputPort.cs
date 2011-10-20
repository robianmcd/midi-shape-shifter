using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IHostInfoInputPort
    {
        void ReceiveProcessingCycleEndTimestampInTicks(long cycleEndTimeStampInTicks);
        void ReceiveSampleRate(double sampleRate);
        void ReceiveTempo(double tempo);
        void ReceiveTimeSignature(int numerator, int denominator);
        void ReceiveTransportPlaying(bool transportPlaying);
        void ReceiveBarPosition(double barPos, long timestampInTicks);
    }
}
