
using Jacobi.Vst.Core;

using MidiShapeShifter.Framework;
using MidiShapeShifter.Mss;

namespace MidiShapeShifterTest.Framework
{
    public class MidiHandlerProtectedWrapper : MidiHandler
    {
        public MssEvent ConvertVstMidiEventToMssEventWrapper(VstMidiEvent midiEvent,
                                                         long processingCycleStartTime,
                                                         double sampleRate)
        {
            return ConvertVstMidiEventToMssEvent(midiEvent, processingCycleStartTime, sampleRate);
        }

        public VstMidiEvent ConvertMssEventToVstMidiEventWrapper(MssEvent mssEvent,
                                                         long processingCycleStartTime,
                                                         double sampleRate)
        {
            return ConvertMssEventToVstMidiEvent(mssEvent, processingCycleStartTime, sampleRate);
        }

    }
}
