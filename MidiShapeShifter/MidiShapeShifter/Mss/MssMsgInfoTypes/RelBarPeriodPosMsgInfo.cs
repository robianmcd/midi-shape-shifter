using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class RelBarPeriodPosMsgInfo : GenericRelitivePosMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.RelBarPeriodPos; }
        }
    }
}
