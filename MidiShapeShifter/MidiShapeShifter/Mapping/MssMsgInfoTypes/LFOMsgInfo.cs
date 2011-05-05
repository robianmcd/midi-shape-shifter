﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{
    public class LFOMsgInfo : MssMsgInfo
    {
        public override MssMsgUtil.MssMsgType mssMsgType
        {
            get
            {
                return MssMsgUtil.MssMsgType.LFO;
            }
        }

        public override string Field1
        {
            get
            {
                return "";
            }
        }

        public override string Field2
        {
            get
            {
                return "";
            }
        }
    }
}
