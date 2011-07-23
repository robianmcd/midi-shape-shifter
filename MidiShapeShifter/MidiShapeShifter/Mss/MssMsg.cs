using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    //Mss message types include a subset of midi message types as well as some messages that are generated within 
    //Midi Shape Shifter
    public enum MssMsgType { NoteOn, NoteOff, CC, PitchBend, PolyAftertouch, ChanAftertouch, Generator, GeneratorToggle, Unsupported };


    /// <summary>
    ///     An MSS message is similar to a MIDI message. Infact most MIDI messages can be represented by MSS messages 
    ///     and vice versa. When MIDI comes into MIDI Shape Shifter all compatible MIDI messages are converted into MSS 
    ///     Messages and any other messages are ignored. The main difference between MSS messages and MIDI messages is 
    ///     that for MSS Messages the message type is limited to types in the MssMsgType enum and for MIDI 
    ///     Messages the message type is limited to types defined in the standard.
    /// </summary>
    public class MssMsg : ICloneable
    {


        public const int NUM_MSS_MSG_TYPES = 8;
        public static readonly List<string> MssMsgTypeNames = new List<string>(NUM_MSS_MSG_TYPES);

        //Static constructor
        static MssMsg()
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

        public MssMsgType Type;
        //The data member variables can have different meaninds depending on the type. For example if the type is 
        //noteMessage, data1 is the channel, data2 is the note number and data3 is the velocity.
        public int Data1;
        public int Data2;
        public int Data3;

        public MssMsg() { }

        public MssMsg(MssMsgType type, int data1, int data2, int data3)
        {
            this.Type = type;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
        }

        public override bool Equals(object o) 
        {
            MssMsg compareToEvent = (MssMsg)o;
            return this.Type == compareToEvent.Type &&
                   this.Data1 == compareToEvent.Data1 &&
                   this.Data2 == compareToEvent.Data2 &&
                   this.Data3 == compareToEvent.Data3;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
