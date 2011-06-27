using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    static class Factory_MssMsgInfoEntryMetadata
    {
        public static MssMsgInfoEntryMetadata Create(MssMsgType type)
        {
            MssMsgInfoEntryMetadata msgMetadata;

            switch (type)
            {
                case MssMsgType.NoteOn:
                    {
                        msgMetadata = new NoteOnMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.NoteOff:
                    {
                        msgMetadata = new NoteOffMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.CC:
                    {
                        msgMetadata = new CCMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.PitchBend:
                    {
                        msgMetadata = new PitchBendMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.PolyAftertouch:
                    {
                        msgMetadata = new PolyAftertouchMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.Generator:
                    {
                        msgMetadata = new GeneratorMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgType.GeneratorToggle:
                    {
                        msgMetadata = new GeneratorToggleMsgInfoEntryMetadata();
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
