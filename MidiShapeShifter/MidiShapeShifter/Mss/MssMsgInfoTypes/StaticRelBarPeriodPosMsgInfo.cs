namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticRelBarPeriodPosMsgInfo : StaticGenericRelitivePosMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.RelBarPeriodPos; }
        }

        public override string Data1Name
        {
            get { return StaticMssMsgInfo.DATA_NAME_UNUSED; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return StaticMssMsgInfo.DATA3_NAME_PERIOD_POSITION; }
        }
    }
}
