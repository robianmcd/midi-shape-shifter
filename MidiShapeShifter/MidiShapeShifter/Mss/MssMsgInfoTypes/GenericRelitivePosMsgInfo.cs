using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    public abstract class GenericRelitivePosMsgInfo : MssMsgInfo
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

        public override string ConvertData1ToString(double Data1)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }

        public override string ConvertData3ToString(double Data3)
        {
            return Math.Round(Data3 * 100).ToString() + "%";
        }
    }
}
