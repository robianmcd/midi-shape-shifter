using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter
{
    public class MssMsg
    {
        public MssMsgUtil.MssMsgType Type;
        //in the case of a note messages, data1 is the channel, data2 is the note number and data3 is the velocity.
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
