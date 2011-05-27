using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class PitchBendMsgInfo : MidiMsgInfo
    {
        public override MssMsgUtil.MssMsgType mssMsgType
        {
            get
            {
                return MssMsgUtil.MssMsgType.PitchBend;
            }
        }
    }
}
