namespace MidiShapeShifter
{
    public static class MidiHelper
    {
        public enum MidiMsgType { NoteOn, NoteOff, CC, PitchBend, Aftertouch };
        public static readonly string[] MidiMsgTypeStr = { "NoteOn", "NoteOff", "CC", "PitchBend", "Aftertouch" };

        public struct MidiMsg 
        {
            public MidiMsgType type;
            public int channel;
            public int param1;
            public int param2;
        }

        //Each instance represents a range of midi messages. For example: All note on message from C1 to C2
        public struct MidiMsgRange
        {
            public MidiMsgType msgType;
            public int topChannel;
            public int bottomChannel;
            public int topParam;
            public int bottomParam;

        }

        //The following constants are used to specify midi message ranges that convere all channels or all 
        //parameter values
        public const int RANGE_ALL_INT = -1;
        public const string RANGE_ALL_STR = "All";

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
