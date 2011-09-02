using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Generator
{
    public class GenEntryHistoryInfo
    {
        public bool Initialized;
        public long LastGeneratorUpdateTimestamp;
        public double PercentThroughPeriodOnLastUpdate;
        public double LastValueSent;

        public GenEntryHistoryInfo()
        {
            this.Initialized = false;
        }

        public void InitAllMembers(long lastGeneratorUpdateTimestamp,
                                   double percentThroughPeriodOnLastUpdate,
                                   double lastValueSent)
        {
            this.LastGeneratorUpdateTimestamp = lastGeneratorUpdateTimestamp;
            this.PercentThroughPeriodOnLastUpdate = percentThroughPeriodOnLastUpdate;
            this.LastValueSent = lastValueSent;

            this.Initialized = true;
        }
    }
}
