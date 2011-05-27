using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class PolyAftertouchMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        protected override MssMsgInfo CreateMsgInfoFromValidatedFields()
        {
            PolyAftertouchMsgInfo midiMsgInfo = new PolyAftertouchMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
