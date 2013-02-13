using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Generator
{
    /// <summary>
    /// Stores information about the recent activity of a generator entry. This class does not
    /// need to be initialized until there is some recent activity to store. MssEventGenerator 
    /// is responsible for intializing all instances of this class. 
    /// </summary>
    public class GenEntryHistoryInfo : ICloneable
    {
        /// <summary>
        /// Specifies wheather this the other members of this class store valid information. If
        /// this is false then all members should be initialized with InitAllMembers.
        /// </summary>
        public bool Initialized;

        /// <summary>
        /// Before the end of an audio processing cycle each GeneratorMappingEntry will be 
        /// brought up to date by the MssEventGenerator. This is done by generating all of the 
        /// MssEvents that should occur in that audio processing cycle. 
        /// SampleTimeAtLastGeneratorUpdate stores the sample time of the last time this 
        /// update has occured.
        /// </summary>
        public long SampleTimeAtLastGeneratorUpdate;

        /// <summary>
        /// Stores the percentage (as a number from 0 to 1) through the period at the last 
        /// update. Information about the period is stored in GenEntryConfigInfo.
        /// </summary>
        public double PercentThroughPeriodOnLastUpdate;

        /// <summary>
        /// Stores the value sent in an MssEvent for data3 on the last update.
        /// </summary>
        public double LastValueSent;

        public GenEntryHistoryInfo()
        {
            this.Initialized = false;
			this.PercentThroughPeriodOnLastUpdate = 0;
        }

        public void InitAllMembers(long sampleTimeAtLastGeneratorUpdate,
                                   double percentThroughPeriodOnLastUpdate,
                                   double lastValueSent)
        {
            this.SampleTimeAtLastGeneratorUpdate = sampleTimeAtLastGeneratorUpdate;
            this.PercentThroughPeriodOnLastUpdate = percentThroughPeriodOnLastUpdate;
            this.LastValueSent = lastValueSent;

            this.Initialized = true;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public GenEntryHistoryInfo Clone()
        {
            return (GenEntryHistoryInfo)this.MemberwiseClone();
        }
    }
}
