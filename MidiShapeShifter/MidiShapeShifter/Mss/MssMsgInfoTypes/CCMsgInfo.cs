using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    class CCMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.CC; }
        }

        public override string Data2Name
        {
            get { return "CC Number"; }
        }

        public override string Data3Name
        {
            get { return "CC Value"; }
        }
    }
}
