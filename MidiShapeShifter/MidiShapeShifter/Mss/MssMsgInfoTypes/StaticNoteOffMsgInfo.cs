using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticNoteOffMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.NoteOff; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA2_NAME_NOTE; }
        }

        public override string Data3Name
        {
            get { return StaticMssMsgInfo.DATA3_NAME_VELOCITY; }
        }
    }
}
