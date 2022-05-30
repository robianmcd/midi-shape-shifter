using MidiShapeShifter.Mss.Generator;
using System;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class GeneratorModifyMsgInfo : MssMsgInfo
    {
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

        public override MssMsgType MsgType => MssMsgType.GeneratorModify;

        public override string ConvertData1ToString(double Data1)
        {
            string data1String = "";

            bool idExists = this.genMappingMgr.RunFuncOnMappingEntry((int)Data1,
                (genEntry) => data1String = genEntry.GenConfigInfo.Name);

            if (idExists)
            {
                return data1String;
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
