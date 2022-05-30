namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticParameterMsgInfo : StaticMssMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Parameter; }
        }

        public override double MaxData1Value
        {
            get { return int.MaxValue; }
        }

        public override double MinData1Value
        {
            get { return 0; }
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
            get { return 1; }
        }

        public override double MinData3Value
        {
            get { return 0; }
        }

        public override string Data1Name
        {
            //This field is used for the MssParameterId which should not be used as a source of input 
            //on the graph so DATA_NAME_UNUSED is returned.
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
