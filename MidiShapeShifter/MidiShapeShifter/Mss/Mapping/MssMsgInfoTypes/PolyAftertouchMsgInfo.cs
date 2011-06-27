using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class PolyAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType mssMsgType
        {
            get
            {
                return MssMsgType.PolyAftertouch;
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
