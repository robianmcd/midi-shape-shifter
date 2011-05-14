using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class PolyAftertouchMsgInfoEntryMetadata : MidiMsgInfoEntryMetadata
    {
        public override MssMsgInfo CreateMsgInfo()
        {
            PolyAftertouchMsgInfo midiMsgInfo = new PolyAftertouchMsgInfo();
            InitializeMidiMsgInfo(midiMsgInfo);
            return midiMsgInfo;
        }
    }
}
