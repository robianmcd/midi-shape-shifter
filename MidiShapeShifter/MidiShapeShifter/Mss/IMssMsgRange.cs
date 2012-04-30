using System;
namespace MidiShapeShifter.Mss
{
    public interface IMssMsgRange
    {
        string Data1RangeStr { get; }
        string Data2RangeStr { get; }
        bool Equals(object o);
        void Init(MidiShapeShifter.Mss.MssMsgInfoTypes.IFactory_MssMsgInfo msgInfoFactory);
        void InitPublicMembers(MssMsgType msgType, int data1, int data2);
        void InitPublicMembers(MssMsgType msgType, int data1RangeBottom, int data1RangeTop, int data2RangeBottom, int data2RangeTop);
        MidiShapeShifter.Mss.MssMsgInfoTypes.IMssMsgInfo MsgInfo { get; }
        bool MsgIsInRange(MssMsg mssMsg);
        MssMsgType MsgType { get; set; }
        int Data1RangeBottom { get; set; }
        int Data1RangeTop { get; set; }
        int Data2RangeBottom { get; set; }
        int Data2RangeTop { get; set; }
    }
}
