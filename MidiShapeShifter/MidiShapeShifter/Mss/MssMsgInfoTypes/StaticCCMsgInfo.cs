namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticCCMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.CC;

        public override string Data2Name => "CC Number";

        public override string Data3Name => "CC Value";
    }
}
