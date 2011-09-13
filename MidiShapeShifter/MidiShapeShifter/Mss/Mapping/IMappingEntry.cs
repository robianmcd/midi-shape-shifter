using System;
namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingEntry : ICurveShapeInfoContainer
    {
        MssMsgRange InMssMsgRange { get; set; }
        MssMsgRange OutMssMsgRange { get; set; }
        bool OverrideDuplicates { get; set; }


        string GetReadableMsgType(IoType ioCategory);
        string GetReadableOverrideDuplicates();
        void InitAllMembers(MidiShapeShifter.Mss.MssMsgRange inMsgRange, MidiShapeShifter.Mss.MssMsgRange outMsgRange, bool overrideDuplicates, MidiShapeShifter.Mss.CurveShapeInfo curveShapeInfo);
    }
}
