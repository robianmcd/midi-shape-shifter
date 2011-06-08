using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class NoteOnMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        protected override MssMsgInfo CreateMsgInfoFromStoredContent()
        {
            NoteOnMsgInfo midiMsgInfo = new NoteOnMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
