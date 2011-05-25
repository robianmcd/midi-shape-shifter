using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{
    public static class Factory_MssMsgInfo
    {
        public static MssMsgInfo Create(MssMsgUtil.MssMsgType type)
        {
            MssMsgInfo msgInfo;

            switch (type)
            {
                case MssMsgUtil.MssMsgType.NoteOn:
                    {
                        msgInfo = new NoteOnMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.NoteOff:
                    {
                        msgInfo = new NoteOffMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.CC:
                    {
                        msgInfo = new CCMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.PitchBend:
                    {
                        msgInfo = new PitchBendMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.PolyAftertouch:
                    {
                        msgInfo = new PolyAftertouchMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.Generator:
                    {
                        msgInfo = new GeneratorMsgInfo();
                        break;
                    }
                case MssMsgUtil.MssMsgType.GeneratorToggle:
                    {
                        msgInfo = new GeneratorToggleMsgInfo();
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
