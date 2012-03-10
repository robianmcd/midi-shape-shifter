using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [Serializable]
    public class PolyAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PolyAftertouch; }
        }

        public override string Data2Name
        {
            get { return DATA2_NAME_NOTE; }
        }

        public override string Data3Name
        {
            get { return DATA3_NAME_PRESSURE; }
        }
    }
}
