using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class MidiMsgInfo : MssMsgInfo
    {
        public override string ConvertData1ToString(double Data1)
        {
            return Data1.ToString();
        }

        public override string ConvertData2ToString(double Data2)
        {
            return Data2.ToString();
        }

        public override string ConvertData3ToString(double Data3)
        {
            return Data3.ToString();
        }
    }
}
