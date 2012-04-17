using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    class ChanAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.ChanAftertouch; }
        }

        public override string Data1Name
        {
            get { throw new NotImplementedException(); }
        }

        public override string Data2Name
        {
            get { throw new NotImplementedException(); }
        }

        public override string Data3Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}
