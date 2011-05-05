using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{
    public class CycleMsgInfo : MssMsgInfo
    {
        public override MssMsgUtil.MssMsgType mssMsgType
        {
            get 
            {
                return MssMsgUtil.MssMsgType.Cycle;
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
