using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    public class MssEvent
    {
        public MssMsg mssMsg;
        public double timestamp;

        public MssEvent()
        {
            this.mssMsg = new MssMsg();
        }
    }
}
