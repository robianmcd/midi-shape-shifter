using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public interface IHostInfoInputPort
    {
        void StartUpdate();
        void FinishUpdate();

        void ReceiveSampleRateDuringUpdate(double sampleRate);
        void ReceiveTempoDuringUpdate(double tempo);
        void ReceiveTimeSignatureDuringUpdate(int numerator, int denominator);
        void ReceiveTransportPlayingDuringUpdate(bool transportPlaying);
        void ReceiveBarPositionDuringUpdate(double barPos, long sampleTime);

        void TriggerProcessingCycleEnd(long cycleEndSampleTime);
        void TriggerIdleProcessing();
    }
}
