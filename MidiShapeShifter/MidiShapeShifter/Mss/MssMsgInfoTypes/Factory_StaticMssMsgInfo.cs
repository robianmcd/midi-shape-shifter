using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public static class Factory_StaticMssMsgInfo
    {
        public static IStaticMssMsgInfo Create(MssMsgType msgInfoType)
        {
            IStaticMssMsgInfo msgInfo;

            switch (msgInfoType)
            {
                case MssMsgType.NoteOn:
                    {
                        msgInfo = new StaticNoteOnMsgInfo();
                        break;
                    }
                case MssMsgType.NoteOff:
                    {
                        msgInfo = new StaticNoteOffMsgInfo();
                        break;
                    }
                case MssMsgType.CC:
                    {
                        msgInfo = new StaticCCMsgInfo();
                        break;
                    }
                case MssMsgType.PitchBend:
                    {
                        msgInfo = new StaticPitchBendMsgInfo();
                        break;
                    }
                case MssMsgType.PolyAftertouch:
                    {
                        msgInfo = new StaticPolyAftertouchMsgInfo();
                        break;
                    }
                case MssMsgType.ChanAftertouch:
                    {
                        msgInfo = new StaticChanAftertouchMsgInfo();
                        break;
                    }
                case MssMsgType.Generator:
                    {
                        msgInfo = new StaticGeneratorMsgInfo();
                        break;
                    }
                case MssMsgType.GeneratorToggle:
                    {
                        msgInfo = new StaticGeneratorToggleMsgInfo();
                        break;
                    }
                case MssMsgType.RelBarPeriodPos:
                    {
                        msgInfo = new StaticRelBarPeriodPosMsgInfo();
                        break;
                    }
                case MssMsgType.RelTimePeriodPos:
                    {
                        msgInfo = new StaticRelTimePeriodPosMsgInfo();
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
