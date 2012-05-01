using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class Factory_MssMsgRangeEntryMetadata
    {
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

        public MssMsgRangeEntryMetadata Create(MssMsgType type)
        {
            MssMsgRangeEntryMetadata msgMetadata;

            switch (type)
            {
                case MssMsgType.Note:
                    {
                        msgMetadata = new NoteMsgRangeEntryMetadata();
                        break;
                    }
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
                case MssMsgType.ChanAftertouch:
                    {
                        msgMetadata = new ChanAftertouchMsgRangeEntryMetadata();
                        break;
                    }
                case MssMsgType.Generator:
                    {
                        GeneratorMsgRangeEntryMetadata genMsgMetadata = 
                                new GeneratorMsgRangeEntryMetadata();
                        genMsgMetadata.Init(this.genMappingMgr);
                        msgMetadata = genMsgMetadata;
                        break;
                    }
                case MssMsgType.GeneratorToggle:
                    {
                        GeneratorToggleMsgRangeEntryMetadata genToggleMsgMetadata =
                                new GeneratorToggleMsgRangeEntryMetadata();
                        genToggleMsgMetadata.Init(this.genMappingMgr);
                        msgMetadata = genToggleMsgMetadata;
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
