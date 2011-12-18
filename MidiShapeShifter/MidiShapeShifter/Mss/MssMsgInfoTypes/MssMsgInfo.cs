using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [Serializable]
    public abstract class MssMsgInfo
    {
        public abstract MssMsgType MsgType { get; }

        public abstract double MaxData1Value { get; }
        public abstract double MinData1Value { get; }
        public abstract double MaxData2Value { get; }
        public abstract double MinData2Value { get; }
        public abstract double MaxData3Value { get; }
        public abstract double MinData3Value { get; }

        public abstract string ConvertData1ToString(double Data1);
        public abstract string ConvertData2ToString(double Data2);
        public abstract string ConvertData3ToString(double Data3);

        //TODO: Add graph strings
    }
}
