using System;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss
{
    public interface IMssMsgRange
    {
        string GetData1RangeStr(IFactory_MssMsgInfo msgInfoFactory);
        string GetData2RangeStr(IFactory_MssMsgInfo msgInfoFactory);
        bool Equals(object o);
        void InitPublicMembers(MssMsgType msgType, int data1, int data2);
        void InitPublicMembers(MssMsgType msgType, int data1RangeBottom, int data1RangeTop, int data2RangeBottom, int data2RangeTop);
        bool MsgIsInRange(MssMsg mssMsg);
        MssMsgType MsgType { get; set; }
        int Data1RangeBottom { get; set; }
        int Data1RangeTop { get; set; }
        int Data2RangeBottom { get; set; }
        int Data2RangeTop { get; set; }
    }
}
