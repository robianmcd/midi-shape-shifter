using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    class NoteOnMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        public override MssMsgInfo CreateMsgInfo()
        {
            NoteOnMsgInfo midiMsgInfo = new NoteOnMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
