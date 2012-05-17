using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class RelTimePeriodPosMsgInfo : GenericRelitivePosMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.RelTimePeriodPos; }
        }
    }
}
