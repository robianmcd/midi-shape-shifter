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
        ///     The system time that MssMsg occured in ticks
        /// </summary>
        public long timestamp;

        public MssEvent()
        {
            this.mssMsg = new MssMsg();
        }
    }
}
