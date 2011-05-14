using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    class ChanAftertouchMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        public override MssMsgInfo CreateMsgInfo()
        {
            ChanAftertouchMsgInfo midiMsgInfo = new ChanAftertouchMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
