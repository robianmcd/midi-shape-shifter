using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: comment this calss
    public class GeneratorMappingEntry : MappingEntry
    {
        public GeneratorMappingEntryInfo GeneratorInfo;

        public void InitAllMembers(MssMsgRange inMsgRange, MssMsgRange outMsgRange,
                            bool overrideDuplicates, CurveShapeInfo curveShapeInfo, GeneratorMappingEntryInfo generatorInfo)
        {
            this.GeneratorInfo = generatorInfo;

            InitAllMembers(inMsgRange, outMsgRange, overrideDuplicates, curveShapeInfo);
        }

        public string GetReadablePeriod()
        { 
            if (this.GeneratorInfo.PeriodType == GenPeriodType.Beats)
            {
                return GeneratorMappingEntryInfo.GenBarsPeriodNames[(int)this.GeneratorInfo.BarsPeriod];
            }
            else if (this.GeneratorInfo.PeriodType == GenPeriodType.Time)
            {
                return this.GeneratorInfo.TimePeriod.ToString() + " ms";
            }
            else
            {
                //Unexpected GenPeriodType
                Debug.Assert(false);
                return "";
            }
        }

        public string GetReadableLoopStatus()
        {
            return GetReadableBool(this.GeneratorInfo.Loop);
        }

        public string GetReadableIsGeneratingStatus()
        {
            return GetReadableBool(this.GeneratorInfo.IsGenerating);
        }
    }
}
