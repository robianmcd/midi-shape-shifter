using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    public class GeneratorMsgInfo : MssMsgInfo
    {
        [DataMember(Name = "GenMappingMgr")]
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

        public override MssMsgType MsgType
        {
            get { return MssMsgType.Generator; }
        }

        public override string ConvertData1ToString(double Data1)
        {

            IGeneratorMappingEntry genEntry = this.genMappingMgr.GetGenMappingEntryById((int)Data1);
            if (genEntry != null)
            {
                return genEntry.GenConfigInfo.Name;
            }
            else
            {
                return "";
            }
        }

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }

        public override string ConvertData3ToString(double Data3)
        {
            return Math.Round(Data3 * 100).ToString() + "%";
        }
    }
}
