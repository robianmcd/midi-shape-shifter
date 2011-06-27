using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class NoteOffMsgInfo : MidiMsgInfo
    {
        public override MssMsgType mssMsgType
        {
            get
            {
                return MssMsgType.NoteOff;
            }
        }
    }
}
