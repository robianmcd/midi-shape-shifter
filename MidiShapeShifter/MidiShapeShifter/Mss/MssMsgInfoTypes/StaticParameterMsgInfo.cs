namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticParameterMsgInfo : StaticMssMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.Parameter;

        public override double MaxData1Value => int.MaxValue;

        public override double MinData1Value => 0;

        public override double MaxData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MinData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MaxData3Value => 1;

        public override double MinData3Value => 0;

        public override string Data1Name => DATA_NAME_UNUSED;

        public override string Data2Name => DATA_NAME_UNUSED;

        public override string Data3Name => DATA3_NAME_PERIOD_POSITION;
    }
}
