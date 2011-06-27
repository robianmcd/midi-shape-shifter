﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public static class Factory_MssMsgInfo
    {
        public static MssMsgInfo Create(MssMsgType type)
        {
            MssMsgInfo msgInfo;

            switch (type)
            {
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
                case MssMsgType.Generator:
                    {
                        msgInfo = new GeneratorMsgInfo();
                        break;
                    }
                case MssMsgType.GeneratorToggle:
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
