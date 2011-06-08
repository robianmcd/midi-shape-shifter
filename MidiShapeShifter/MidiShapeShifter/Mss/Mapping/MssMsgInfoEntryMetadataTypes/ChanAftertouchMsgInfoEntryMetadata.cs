using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    class ChanAftertouchMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        protected override MssMsgInfo CreateMsgInfoFromStoredContent()
        {
            ChanAftertouchMsgInfo midiMsgInfo = new ChanAftertouchMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
