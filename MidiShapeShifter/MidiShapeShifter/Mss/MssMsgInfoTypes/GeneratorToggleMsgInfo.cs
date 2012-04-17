using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    public class GeneratorToggleMsgInfo : MssMsgInfo
    {
        [DataMember(Name = "GenMappingMgr")]
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

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

        public override string ConvertData1ToString(double Data1)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData2ToString(double Data2)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData3ToString(double Data3)
        {
            throw new NotImplementedException();
        }

        public override string Data1Name
        {
            get { return DATA1_NAME_GEN_ID; }
        }

        public override string Data2Name
        {
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return "Toggle Generator"; }
        }
    }
}
