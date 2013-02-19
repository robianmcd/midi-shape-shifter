using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    enum GenOperation {OnOff, PlayPause, SetPosition}

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
        
        public override MssMsgType MsgType
        {
            get { return MssMsgType.GeneratorModify; }
        }

        public override double MaxData1Value
        {
            get { return double.MaxValue; }
        }

        public override double MinData1Value
        {
            get { return 0; }
        }

        public override double MaxData2Value
        {
            get { return StaticGeneratorModifyMsgInfo.NUM_GEN_OPERATIONS - 1; }
        }

        public override double MinData2Value
        {
            get { return 0; }
        }

        public override double MaxData3Value
        {
            get { return 1; }
        }

        public override double MinData3Value
        {
            get { return 0; }
        }

        public override string Data1Name
        {
            //This field is used for the Generator ID which should not be used as a source of input 
            //on the graph so DATA_NAME_UNUSED is returned instead of DATA1_NAME_GEN_ID.
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data2Name
        {
            //This field is used to specify which generator operation should be used. This should not be used
            //as a source of input on the graph so DATA_NAME_UNUSED is returned.
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return "Operation Value"; }
        }
    }
}
