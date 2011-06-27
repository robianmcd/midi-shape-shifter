using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    public class GeneratorToggleMsgInfo : MssMsgInfo
    {
        public override MssMsgType mssMsgType
        {
            get
            {
                return MssMsgType.GeneratorToggle;
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
