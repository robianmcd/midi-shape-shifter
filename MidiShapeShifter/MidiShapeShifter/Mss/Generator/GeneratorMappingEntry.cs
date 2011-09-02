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
        public GenEntryConfigInfo GenConfigInfo;
        //Stores information about previously generated events. If this is null then 
        //MssEventGenerator will initialize it at the end of the next processing cycle.
        public GenEntryHistoryInfo GenHistoryInfo;

        public GeneratorMappingEntry()
        {
            this.GenHistoryInfo = new GenEntryHistoryInfo();
        }

        public void InitAllMembers(MssMsgRange inMsgRange, MssMsgRange outMsgRange,
                            bool overrideDuplicates, CurveShapeInfo curveShapeInfo, GenEntryConfigInfo generatorInfo)
        {
            this.GenConfigInfo = generatorInfo;

            InitAllMembers(inMsgRange, outMsgRange, overrideDuplicates, curveShapeInfo);
        }

        public string GetReadablePeriod()
        { 
            if (this.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                return GenEntryConfigInfo.GenBarsPeriodNames[(int)this.GenConfigInfo.BarsPeriod];
            }
            else if (this.GenConfigInfo.PeriodType == GenPeriodType.Time)
            {
                return this.GenConfigInfo.TimePeriodInMs.ToString() + " ms";
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
            return GetReadableBool(this.GenConfigInfo.Loop);
        }

        public string GetReadableEnabledStatus()
        {
            return GetReadableBool(this.GenConfigInfo.Enabled);
        }
    }
}
