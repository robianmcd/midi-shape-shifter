using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping
{
    abstract class MappableData
    {
        public abstract MidiHelper.MssMsgType MssMsgType 
        {
            get;
        }
    }
}
