using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     An MSS message is similar to a MIDI message. Infact most MIDI messages can be represented by MSS messages 
    ///     and vice versa. When MIDI comes into MIDI Shape Shifter all compatible MIDI messages are converted into MSS 
    ///     Messages and any other messages are ignored. The main difference between MSS messages and MIDI messages is 
    ///     that for MSS Messages the message type is limited to types in the MssMsgUtil.MssMsgType enum and for MIDI 
    ///     Messages the message type is limited to types defined in the standard.
    /// </summary>
    public class MssMsg
    {
        public MssMsgUtil.MssMsgType Type;
        //The data member variables can have different meaninds depending on the type. For example if the type is 
        //noteMessage, data1 is the channel, data2 is the note number and data3 is the velocity.
        public int Data1;
        public int Data2;
        public int Data3;

        public MssMsg() { }

        public MssMsg(MssMsgUtil.MssMsgType type, int data1, int data2, int data3)
        {
            this.Type = type;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
        }
    }
}
