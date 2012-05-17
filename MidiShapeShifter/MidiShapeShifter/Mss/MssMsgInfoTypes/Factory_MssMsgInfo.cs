using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class Factory_MssMsgInfo : IFactory_MssMsgInfo
    {
        protected IGeneratorMappingManager genMappingMgr;
        protected IMssParameterViewer paramViewer;

        public void Init(IGeneratorMappingManager genMappingMgr, IMssParameterViewer paramViewer)
        {
            this.genMappingMgr = genMappingMgr;
            InitOptionallySerializedDependencies(paramViewer);

        }

        public void InitOptionallySerializedDependencies(IMssParameterViewer paramViewer)
        {
            this.paramViewer = paramViewer;
        }

        public IMssMsgInfo Create(MssMsgType msgInfoType)
        {
            IMssMsgInfo msgInfo;

            switch (msgInfoType)
            {
                case MssMsgType.Note:
                    {
                        msgInfo = new NoteMsgInfo();
                        break;
                    }
                case MssMsgType.NoteOn:
                    {
                        msgInfo = new NoteOnMsgInfo();
                        break;
                    }
                case MssMsgType.NoteOff:
                    {
                        msgInfo = new NoteOffMsgInfo();
                        break;
                    }
                case MssMsgType.CC:
                    {
                        msgInfo = new CCMsgInfo();
                        break;
                    }
                case MssMsgType.PitchBend:
                    {
                        msgInfo = new PitchBendMsgInfo();
                        break;
                    }
                case MssMsgType.PolyAftertouch:
                    {
                        msgInfo = new PolyAftertouchMsgInfo();
                        break;
                    }
                case MssMsgType.ChanAftertouch:
                    {
                        msgInfo = new ChanAftertouchMsgInfo();
                        break;
                    }
                case MssMsgType.Generator:
                    {
                        GeneratorMsgInfo genMsgInfo = new GeneratorMsgInfo();
                        genMsgInfo.Init(this.genMappingMgr);
                        msgInfo = genMsgInfo;
                        break;
                    }
                case MssMsgType.GeneratorToggle:
                    {
                        GeneratorToggleMsgInfo genToggleMsgInfo = new GeneratorToggleMsgInfo();
                        genToggleMsgInfo.Init(this.genMappingMgr);
                        msgInfo = genToggleMsgInfo;
                        break;
                    }
                case MssMsgType.RelBarPeriodPos:
                    {
                        msgInfo = new RelBarPeriodPosMsgInfo();
                        break;
                    }
                case MssMsgType.RelTimePeriodPos:
                    {
                        msgInfo = new RelTimePeriodPosMsgInfo();
                        break;
                    }
                case MssMsgType.Parameter:
                    {
                        ParameterMsgInfo paramMsgInfo = new ParameterMsgInfo();
                        paramMsgInfo.Init(this.paramViewer);
                        msgInfo = paramMsgInfo;
                        break;
                    }
                default:
                    {
                        //Unknown type
                        Debug.Assert(false);
                        msgInfo = null;
                        break;
                    }
            }
            return msgInfo;
        }
    }
}
