using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    //Mss message types include a subset of midi message types as well as some messages that are 
    //generated within Midi Shape Shifter. Data1 Data2 and Data3 can have a different meaning for 
    //each message. The following table sumerizes these meanings. NOTE- % values go from 0 to 1.
    public enum MssMsgType { 
    //  Type                Data1               Data2                   Data3
        NoteOn,         //  Channel             Note number             Velocity
        NoteOff,        //  Channel             Note number             Velocity
        CC,             //  Channel             Controller number       Controller value
        PitchBend,      //  Channel             N/A                     Pitch bend value
        PolyAftertouch, //  Channel             Note number             Pressure
        ChanAftertouch, //  Channel             N/A                     Pressure
        Generator,      //  Generator ID        N/A                     % through period
        GeneratorToggle,//  Generator ID        N/A                     Less then 1 toggles off
        RelBarPeriodPos,//  N/A                 N/A                     % through beat synced period 
        RelTimePeriodPos,// N/A                 N/A                     % through timed period 
        Unsupported     //  N/A                 N/A                     N/A
    };

    //Used to distinguish between the three data fields in an mss msg.
    public enum MssMsgDataField { 
        Data1, Data2, Data3
    }

    /// <summary>
    ///     An MSS message is similar to a MIDI message. Infact most MIDI messages can be represented by MSS messages 
    ///     and vice versa. When MIDI comes into MIDI Shape Shifter all compatible MIDI messages are converted into MSS 
    ///     Messages and any other messages are ignored. The main difference between MSS messages and MIDI messages is 
    ///     that for MSS Messages the message type is limited to types in the MssMsgType enum and for MIDI 
    ///     Messages the message type is limited to types defined in the standard.
    /// </summary>
    public class MssMsg : ICloneable
    {


        public const int NUM_MSS_MSG_TYPES = 11;
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
            //The rest of these MssMsgTypeNames are not currently displayed anywhere.
            MssMsgTypeNames.Insert((int)MssMsgType.RelBarPeriodPos, "Beat Synced Period");
            MssMsgTypeNames.Insert((int)MssMsgType.RelTimePeriodPos, "Time based Period");
            MssMsgTypeNames.Insert((int)MssMsgType.Unsupported, "Unsupported");

        }

        public MssMsgType Type;
        //The data member variables can have different meanings depending on the type. For example if the type is 
        //noteMessage, data1 is the channel, data2 is the note number and data3 is the velocity.
        public double Data1;
        public double Data2;
        public double Data3;

        public int Data1AsInt { get { return (int)Math.Round(this.Data1); } }
        public int Data2AsInt { get { return (int)Math.Round(this.Data2); } }
        public int Data3AsInt { get { return (int)Math.Round(this.Data3); } }

        public MssMsg() { }

        public MssMsg(MssMsgType type, double data1, double data2, double data3)
        {
            this.Type = type;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
        }

        public double GetDataField(MssMsgDataField field)
        {
            if (field == MssMsgDataField.Data1)
            {
                return this.Data1;
            }
            else if (field == MssMsgDataField.Data2)
            {
                return this.Data2;
            }
            else if (field == MssMsgDataField.Data3)
            {
                return this.Data3;
            }
            else
            {
                //unknown MssMsgDataField
                Debug.Assert(false);
                return -1;
            }
        }

        public override bool Equals(object o) 
        {
            MssMsg compareToMsg = (MssMsg)o;
            return this.Type == compareToMsg.Type &&
                   this.Data1 == compareToMsg.Data1 &&
                   this.Data2 == compareToMsg.Data2 &&
                   this.Data3 == compareToMsg.Data3;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + (int)this.Type;
            hash = (hash * 7) + this.Data1.GetHashCode();
            hash = (hash * 7) + this.Data2.GetHashCode();
            hash = (hash * 7) + this.Data3.GetHashCode();
            return hash;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
