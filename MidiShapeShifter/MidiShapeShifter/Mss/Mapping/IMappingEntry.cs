using System;
using System.Runtime.Serialization;
using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingEntry : ICurveShapeInfoContainer, ICloneable
    {
        int Id { get; set; }
        IMssMsgRange InMssMsgRange { get; set; }
        IMssMsgRange OutMssMsgRange { get; set; }
        bool OverrideDuplicates { get; set; }
        MssMsgDataField PrimaryInputSource{get; set;}
        string ActiveTransformPresetName{get; set;}

        string GetReadableMsgType(IoType ioCategory);
        string GetReadableOverrideDuplicates();
        void InitAllMembers(MidiShapeShifter.Mss.IMssMsgRange inMsgRange, MidiShapeShifter.Mss.IMssMsgRange outMsgRange, bool overrideDuplicates, MidiShapeShifter.Mss.CurveShapeInfo curveShapeInfo);
    }
}
