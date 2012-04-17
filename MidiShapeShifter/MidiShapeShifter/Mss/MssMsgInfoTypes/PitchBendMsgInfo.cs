using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    [DataContract]
    public class PitchBendMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PitchBend; }
        }

        public override double MinData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData3Value
        {
            get { return MidiUtil.MAX_LARGE_MIDI_PARAM_VAL; }
        }

        public override string Data2Name
        {
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return "Pitch Bend"; }
        }
    }
}
