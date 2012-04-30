using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class StaticGenericRelitivePosMsgInfo : StaticMssMsgInfo
    {
        public override double MaxData1Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MinData1Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MinData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData3Value
        {
            get { return MssMsgUtil.MAX_RELATIVE_MSG_VAL; }
        }

        public override double MinData3Value
        {
            get { return MssMsgUtil.MIN_RELATIVE_MSG_VAL; }
        }
    }
}
