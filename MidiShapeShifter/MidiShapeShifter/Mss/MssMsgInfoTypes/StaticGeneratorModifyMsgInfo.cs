using System;
using System.Collections.Generic;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    internal enum GenOperation { OnOff, PlayPause, SetPosition }

    public class StaticGeneratorModifyMsgInfo : StaticMssMsgInfo
    {
        public static readonly int NUM_GEN_OPERATIONS;
        public static readonly List<string> GenOperationNames;

        static StaticGeneratorModifyMsgInfo()
        {
            NUM_GEN_OPERATIONS = Enum.GetNames(typeof(GenOperation)).Length;

            GenOperationNames = new List<string>(NUM_GEN_OPERATIONS);

            GenOperationNames.Insert((int)GenOperation.OnOff, "On/Off");
            GenOperationNames.Insert((int)GenOperation.PlayPause, "Play/Pause");
            GenOperationNames.Insert((int)GenOperation.SetPosition, "SetPosition");
        }

        public override MssMsgType MsgType => MssMsgType.GeneratorModify;

        public override double MaxData1Value => double.MaxValue;

        public override double MinData1Value => 0;

        public override double MaxData2Value => StaticGeneratorModifyMsgInfo.NUM_GEN_OPERATIONS - 1;

        public override double MinData2Value => 0;

        public override double MaxData3Value => 1;

        public override double MinData3Value => 0;

        public override string Data1Name => DATA_NAME_UNUSED;

        public override string Data2Name => DATA_NAME_UNUSED;

        public override string Data3Name => "Operation Value";
    }
}
