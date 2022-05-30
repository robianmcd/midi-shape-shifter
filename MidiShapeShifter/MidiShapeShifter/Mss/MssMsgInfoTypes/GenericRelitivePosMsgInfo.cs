using System;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class GenericRelitivePosMsgInfo : MssMsgInfo
    {
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
