using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class NoteOffMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        public override MssMsgInfo CreateMsgInfo()
        {
            NoteOffMsgInfo midiMsgInfo = new NoteOffMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
