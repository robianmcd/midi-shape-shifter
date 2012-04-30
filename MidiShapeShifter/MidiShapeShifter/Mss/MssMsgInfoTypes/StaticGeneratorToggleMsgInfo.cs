using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticGeneratorToggleMsgInfo : StaticMssMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { throw new NotImplementedException(); }
        }

        public override double MaxData1Value
        {
            get { throw new NotImplementedException(); }
        }

        public override double MinData1Value
        {
            get { throw new NotImplementedException(); }
        }

        public override double MaxData2Value
        {
            get { throw new NotImplementedException(); }
        }

        public override double MinData2Value
        {
            get { throw new NotImplementedException(); }
        }

        public override double MaxData3Value
        {
            get { throw new NotImplementedException(); }
        }

        public override double MinData3Value
        {
            get { throw new NotImplementedException(); }
        }

        public override string Data1Name
        {
            get { return StaticMssMsgInfo.DATA1_NAME_GEN_ID; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return "Toggle Generator"; }
        }
    }
}
