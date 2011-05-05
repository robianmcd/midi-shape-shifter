using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{
    abstract public class MssMsgInfo
    {
        public abstract MssMsgUtil.MssMsgType mssMsgType
        {
            get;
        }

        public abstract string Field1
        {
            get;
        }

        public abstract string Field2
        {
            get;
        }
    }
}
