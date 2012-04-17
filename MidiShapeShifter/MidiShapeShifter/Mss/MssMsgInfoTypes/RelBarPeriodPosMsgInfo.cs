using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    class RelBarPeriodPosMsgInfo : GenericRelitivePosMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.RelBarPeriodPos; }
        }

        public override string Data1Name
        {
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data2Name
        {
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return DATA3_NAME_PERIOD_POSITION; }
        }
    }
}
