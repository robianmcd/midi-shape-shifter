namespace MidiShapeShifter
{
    internal static class MidiHelper
    {
        public enum MidiMsgType { NoteOn, NoteOff, CC, PitchBend, Aftertouch }

        public struct MidiMsg 
        {
            public MidiMsgType type;
            public int channel;
            public int param1;
            public int param2;
        }

        public static bool IsNoteOn(byte[] dataBuffer)
        {
            return IsNoteOn(dataBuffer[0]);
        }

        public static bool IsNoteOn(byte data)
        {
            return ((data & 0xF0) == 0x90);
        }

        public static bool IsNoteOff(byte[] dataBuffer)
        {
            return IsNoteOff(dataBuffer[0]);
        }

        public static bool IsNoteOff(byte data)
        {
            return ((data & 0xF0) == 0x80);
        }
    }
}
