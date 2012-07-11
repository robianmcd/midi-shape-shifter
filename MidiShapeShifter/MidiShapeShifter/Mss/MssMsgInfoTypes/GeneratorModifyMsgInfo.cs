using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class GeneratorModifyMsgInfo : MssMsgInfo
    {
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

        public override MssMsgType MsgType
        {
            get { return MssMsgType.GeneratorModify; }
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
            return StaticGeneratorModifyMsgInfo.GenOperationNames[(int)Data2];
        }

        public override string ConvertData3ToString(double Data3)
        {
            return Math.Round(Data3, 2).ToString();
        }
    }
}
