using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class PolyAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgUtil.MssMsgType mssMsgType
        {
            get
            {
                return MssMsgUtil.MssMsgType.PolyAftertouch;
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
