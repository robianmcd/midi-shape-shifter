using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Parameters;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class Factory_MssMsgRangeEntryMetadata
    {
        protected IGeneratorMappingManager genMappingMgr;
        protected IMssParameterViewer paramViewer;


        public void Init(IGeneratorMappingManager genMappingMgr, IMssParameterViewer paramViewer)
        {
            this.genMappingMgr = genMappingMgr;
            this.paramViewer = paramViewer;

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
                        var genMsgMetadata = new GeneratorMsgRangeEntryMetadata();
                        genMsgMetadata.Init(this.genMappingMgr);
                        msgMetadata = genMsgMetadata;
                        break;
                    }
                case MssMsgType.GeneratorModify:
                    {
                        var genToggleMsgMetadata = new GeneratorModifyMsgRangeEntryMetadata();
                        genToggleMsgMetadata.Init(this.genMappingMgr);
                        msgMetadata = genToggleMsgMetadata;
                        break;
                    }
                case MssMsgType.Parameter:
                    {
                        var paramMsgRangeMetadata = new ParameterMsgRangeEntryMetadata();
                        paramMsgRangeMetadata.Init(this.paramViewer);
                        msgMetadata = paramMsgRangeMetadata;
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
