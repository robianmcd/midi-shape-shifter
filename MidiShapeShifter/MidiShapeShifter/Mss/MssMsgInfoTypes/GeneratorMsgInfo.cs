using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class GeneratorMsgInfo : MssMsgInfo
    {
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

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
