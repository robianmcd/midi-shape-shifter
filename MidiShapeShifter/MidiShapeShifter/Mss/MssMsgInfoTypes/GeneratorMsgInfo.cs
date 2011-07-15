using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class GeneratorMsgInfo : MssMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { throw new NotImplementedException(); }
        }

        public override int MaxData1Value
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinData1Value
        {
            get { throw new NotImplementedException(); }
        }

        public override int MaxData2Value
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinData2Value
        {
            get { throw new NotImplementedException(); }
        }

        public override int MaxData3Value
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinData3Value
        {
            get { throw new NotImplementedException(); }
        }

        public override string ConvertData1ToString(int Data1)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData2ToString(int Data2)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData3ToString(int Data3)
        {
            throw new NotImplementedException();
        }
    }
}
