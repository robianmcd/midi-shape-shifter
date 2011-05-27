using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class PitchBendMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        protected override MssMsgInfo CreateMsgInfoFromValidatedFields()
        {
            PitchBendMsgInfo midiMsgInfo = new PitchBendMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
