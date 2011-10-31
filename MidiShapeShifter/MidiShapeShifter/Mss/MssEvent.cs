using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Represents an MssMsg that occured at a particular time.
    /// </summary>
    public class MssEvent
    {
        public MssMsg mssMsg;
        /// <summary>
        ///     The time in samples of when MssMsg occured
        /// </summary>
        public long sampleTime;

        public MssEvent()
        {
            this.mssMsg = new MssMsg();
        }

        public override bool Equals(object o)
        {
            MssEvent compareToEvent = (MssEvent)o;
            return this.mssMsg.Equals(compareToEvent.mssMsg) && this.sampleTime == compareToEvent.sampleTime;
        }
    }
}
