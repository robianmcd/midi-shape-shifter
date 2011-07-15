using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    static class Factory_MssMsgRangeEntryMetadata
    {
        public static MssMsgRangeEntryMetadata Create(MssMsgType type)
        {
            MssMsgRangeEntryMetadata msgMetadata;

            switch (type)
            {
                case MssMsgType.NoteOn:
                    {
                        msgMetadata = new NoteOnMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.NoteOff:
                    {
                        msgMetadata = new NoteOffMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.CC:
                    {
                        msgMetadata = new CCMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.PitchBend:
                    {
                        msgMetadata = new PitchBendMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.PolyAftertouch:
                    {
                        msgMetadata = new PolyAftertouchMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.Generator:
                    {
                        msgMetadata = new GeneratorMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.GeneratorToggle:
                    {
                        msgMetadata = new GeneratorToggleMsgRangeEntryMetadata();
                        break;
                    }
                default:
                    {
                        //Unknown type
                        Debug.Assert(false);
                        msgMetadata = null;
                        break;
                    }
            }
            return msgMetadata;
        }
    }
}
