using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class MssMsgInfo
    {
        public abstract MssMsgType MsgType { get; }

        public abstract int MaxData1Value { get; }
        public abstract int MinData1Value { get; }
        public abstract int MaxData2Value { get; }
        public abstract int MinData2Value { get; }
        public abstract int MaxData3Value { get; }
        public abstract int MinData3Value { get; }

        public abstract string ConvertData1ToString(int Data1);
        public abstract string ConvertData2ToString(int Data2);
        public abstract string ConvertData3ToString(int Data3);

        //TODO: Add graph strings
    }
}
