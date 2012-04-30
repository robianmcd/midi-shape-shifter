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
    }
}
