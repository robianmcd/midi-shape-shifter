using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    class ChanAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgUtil.MssMsgType mssMsgType
        {
            get
            {
                return MssMsgUtil.MssMsgType.ChanAftertouch;
            }
        }

        public override string Field1
        {
            get
            {
                return "";
            }
        }

        public override string Field2
        {
            get
            {
                return "";
            }
        }
    }
}
