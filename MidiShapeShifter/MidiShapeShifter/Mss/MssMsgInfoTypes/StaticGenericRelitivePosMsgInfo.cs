namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class StaticGenericRelitivePosMsgInfo : StaticMssMsgInfo
    {
        public override double MaxData1Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MinData1Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MaxData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MinData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MaxData3Value => MssMsgUtil.MAX_RELATIVE_MSG_VAL;

        public override double MinData3Value => MssMsgUtil.MIN_RELATIVE_MSG_VAL;
    }
}
