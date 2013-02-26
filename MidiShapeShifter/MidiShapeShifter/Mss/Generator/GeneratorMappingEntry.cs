using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

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
    [DataContract]
    public class GeneratorMappingEntry : MappingEntry, IGeneratorMappingEntry
    {
        [DataMember(Name = "GenConfigInfo")]
        protected GenEntryConfigInfo _genConfigInfo;
        public GenEntryConfigInfo GenConfigInfo { get { return this._genConfigInfo; } 
                                                  set { this._genConfigInfo = value; } }
        
        /// <summary>
        /// Stores information about previously generated events.
        /// </summary>
        protected GenEntryHistoryInfo _genHistoryInfo;
        public GenEntryHistoryInfo GenHistoryInfo { get { return this._genHistoryInfo; } 
                                                    set { this._genHistoryInfo = value; } }

        public GeneratorMappingEntry()
        {
            this.GenHistoryInfo = new GenEntryHistoryInfo();
        }

        public void InitAllMembers(IMssMsgRange inMsgRange, IMssMsgRange outMsgRange,
                            bool overrideDuplicates, CurveShapeInfo curveShapeInfo, 
                            GenEntryConfigInfo generatorConfigInfo)
        {
            this.GenConfigInfo = generatorConfigInfo;

            InitAllMembers(inMsgRange, outMsgRange, overrideDuplicates, curveShapeInfo);
        }

        [OnDeserializing]
        protected void OnBeforeDeserialize(StreamingContext context)
        {
            this.GenHistoryInfo = new GenEntryHistoryInfo();
        }

        /// <summary>
        /// Returns a string representation of the period size.
        /// </summary>
        /// <returns></returns>
        public string GetReadablePeriod()
        {
            switch (this.GenConfigInfo.PeriodType)
            {
                case GenPeriodType.Bars:
                case GenPeriodType.BeatSynced:
                    return GenEntryConfigInfo.GenBarsPeriodNames[(int)this.GenConfigInfo.BarsPeriod];

                case GenPeriodType.Time:
                    return this.GenConfigInfo.TimePeriodInMs.ToString() + " ms";

                default:
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

        public override string ToString()
        {
            if (GenConfigInfo != null)
            {
                return this.GenConfigInfo.Name;
            }
            else {
                return "";
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        new public GeneratorMappingEntry Clone() {
            GeneratorMappingEntry mappingEntryClone = new GeneratorMappingEntry();
            DeepCloneIntoExistingInstance(mappingEntryClone);
            return mappingEntryClone;
        }

        protected void DeepCloneIntoExistingInstance(GeneratorMappingEntry entry)
        {
            base.DeepCloneIntoExistingInstance(entry);

            entry.GenConfigInfo = this.GenConfigInfo.Clone();
            entry.GenHistoryInfo = this.GenHistoryInfo.Clone();

        }

    }
}
