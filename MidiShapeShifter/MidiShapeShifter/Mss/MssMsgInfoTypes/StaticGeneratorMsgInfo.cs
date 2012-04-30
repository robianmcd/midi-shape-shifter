using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticGeneratorMsgInfo : StaticMssMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Generator; }
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
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MinData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
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
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return DATA3_NAME_PERIOD_POSITION; }
        }
    }
}
