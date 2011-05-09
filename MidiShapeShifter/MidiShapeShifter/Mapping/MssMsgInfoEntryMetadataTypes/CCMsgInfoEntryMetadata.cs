using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class CCMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        public override MssMsgInfo CreateMsgInfo()
        {
            CCMsgInfo midiMsgInfo = new CCMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
