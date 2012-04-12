using System;
namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingEntry : ICurveShapeInfoContainer
    {
        IMssMsgRange InMssMsgRange { get; set; }
        IMssMsgRange OutMssMsgRange { get; set; }
        bool OverrideDuplicates { get; set; }
        MssMsgDataField PrimaryInputSource{get; set;}

        string GetReadableMsgType(IoType ioCategory);
        string GetReadableOverrideDuplicates();
        void InitAllMembers(MidiShapeShifter.Mss.IMssMsgRange inMsgRange, MidiShapeShifter.Mss.IMssMsgRange outMsgRange, bool overrideDuplicates, MidiShapeShifter.Mss.CurveShapeInfo curveShapeInfo);
    }
}
