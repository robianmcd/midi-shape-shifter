using System.Collections.Generic;

namespace MidiShapeShifter
{
    public static class MssMsgUtil
    {
        //Mss message types include a subset of midi message types as well as some messages that are generated within 
        //Midi Shape Shifter
        public enum MssMsgType { NoteOn, NoteOff, CC, PitchBend, PolyAftertouch, ChanAftertouch, Generator, GeneratorToggle };
        public const int NUM_MSS_MSG_TYPES = 8;
        public static readonly List<string> MssMsgTypeNames = new List<string>(NUM_MSS_MSG_TYPES);

        //Static constructor
        static MssMsgUtil()
        {
            MssMsgTypeNames.Insert((int)MssMsgType.NoteOn, "Note On");
            MssMsgTypeNames.Insert((int)MssMsgType.NoteOff, "Note Off");
            MssMsgTypeNames.Insert((int)MssMsgType.CC, "CC");
            MssMsgTypeNames.Insert((int)MssMsgType.PitchBend, "Pitch Bend");
            MssMsgTypeNames.Insert((int)MssMsgType.PolyAftertouch, "Poly Afto");
            MssMsgTypeNames.Insert((int)MssMsgType.ChanAftertouch, "Chan Afto");
            MssMsgTypeNames.Insert((int)MssMsgType.Generator, "Generator");
            MssMsgTypeNames.Insert((int)MssMsgType.GeneratorToggle, "Gen. Toggle");
        }

        public struct MidiMsg 
        {
            public MssMsgType type;
            public int channel;
            public int param1;
            public int param2;
        }

        //RANGE_ALL_STR is used to represent a midi message ranges that convere all channels or all parameter values
        public const string RANGE_ALL_STR = "All";
        public const int RANGE_INVALID = -1;
        public const int MIN_CHANNEL = 1;
        public const int MAX_CHANNEL = 16;
        public const int MIN_PARAM = 0;
        public const int MAX_PARAM = 127;

        public static bool isValidParamValue(int value)
        {
            return value >= MIN_PARAM && value <= MAX_PARAM;
        }

        public static bool isValidChannel(int channel)
        {
            return channel >= MIN_CHANNEL && channel <= MAX_CHANNEL;
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
