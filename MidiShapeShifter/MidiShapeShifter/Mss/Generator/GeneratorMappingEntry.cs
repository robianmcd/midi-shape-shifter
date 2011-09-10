using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    
    /// <summary>
    /// This class describes how and when generator messages will be generated. Instances of this
    /// class are stored in GeneratorMappingManager and each instances corresponds to a row in the
    /// generator list view on the PluginEditorView dialog. MssEventGenerator will use the 
    /// information in this class to create and send the generator messages. 
    /// GeneratorMappingEntries will always map one of the relative bar position MSS message types
    /// to a generator mss message. 
    /// </summary>
    public class GeneratorMappingEntry : MappingEntry
    {

        public GenEntryConfigInfo GenConfigInfo;
        
        /// <summary>
        /// Stores information about previously generated events.
        /// </summary>
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

        /// <summary>
        /// Returns a string representation of the period size.
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Returns a string representation of the Loop member variable in GenConfigInfo.
        /// </summary>
        public string GetReadableLoopStatus()
        {
            return GetReadableBool(this.GenConfigInfo.Loop);
        }

        /// <summary>
        /// Returns a string representation of the Enabled member variable in GenConfigInfo.
        /// </summary>
        public string GetReadableEnabledStatus()
        {
            return GetReadableBool(this.GenConfigInfo.Enabled);
        }
    }
}
