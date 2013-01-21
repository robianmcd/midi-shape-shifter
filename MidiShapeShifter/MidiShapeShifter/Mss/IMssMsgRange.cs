using System;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss
{
    public interface IMssMsgRange : ICloneable
    {
        string GetData1RangeStr(IFactory_MssMsgInfo msgInfoFactory);
        string GetData2RangeStr(IFactory_MssMsgInfo msgInfoFactory);
        bool Equals(object o);
        void InitPublicMembers(MssMsgType msgType, double data1, double data2);
        void InitPublicMembers(MssMsgType msgType, double data1RangeBottom, double data1RangeTop, double data2RangeBottom, double data2RangeTop);
        bool MsgIsInRange(MssMsg mssMsg);
        MssMsgType MsgType { get; set; }
        double Data1RangeBottom { get; set; }
        double Data1RangeTop { get; set; }
        double Data2RangeBottom { get; set; }
        double Data2RangeTop { get; set; }

        new IMssMsgRange Clone();
    }
}
