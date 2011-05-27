using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class NoteOffMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        protected override MssMsgInfo CreateMsgInfoFromValidatedFields()
        {
            NoteOffMsgInfo midiMsgInfo = new NoteOffMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
