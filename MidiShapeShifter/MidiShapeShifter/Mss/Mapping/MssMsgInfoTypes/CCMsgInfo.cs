using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class CCMsgInfo : MidiMsgInfo
    {
        public override MssMsgType mssMsgType
        {
            get
            {
                return MssMsgType.CC;
            }
        }
    }
}
