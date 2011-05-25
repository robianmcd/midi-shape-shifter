using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    static class Factory_MssMsgInfoEntryMetadata
    {
        public static MssMsgInfoEntryMetadata Create(MssMsgUtil.MssMsgType type)
        {
            MssMsgInfoEntryMetadata msgMetadata;

            switch (type)
            {
                case MssMsgUtil.MssMsgType.NoteOn:
                    {
                        msgMetadata = new NoteOnMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.NoteOff:
                    {
                        msgMetadata = new NoteOffMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.CC:
                    {
                        msgMetadata = new CCMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.PitchBend:
                    {
                        msgMetadata = new PitchBendMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.PolyAftertouch:
                    {
                        msgMetadata = new PolyAftertouchMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.Generator:
                    {
                        msgMetadata = new GeneratorMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.GeneratorToggle:
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
