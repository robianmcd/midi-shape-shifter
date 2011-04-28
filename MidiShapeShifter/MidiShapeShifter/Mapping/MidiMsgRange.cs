using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping
{
    abstract class MidiMsgRange : MappableData
    {
        public override abstract MidiHelper.MssMsgType MssMsgType 
        {
            get;
        }
        
    }
}
