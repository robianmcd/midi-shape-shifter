using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes
{
    abstract public class MssMsgInfo
    {
        public abstract MssMsgType mssMsgType
        {
            get;
        }

        public int Data1RangeBottom {get; protected set;}
        public int Data1RangeTop { get; protected set; }
        public int Data2RangeBottom { get; protected set; }
        public int Data2RangeTop { get; protected set; }

        public void Initialize(int data1RangeBottom, int data1RangeTop, int data2RangeBottom, int data2RangeTop)
        {
            this.Data1RangeBottom = data1RangeBottom;
            this.Data1RangeTop = data1RangeTop;
            this.Data2RangeBottom = data2RangeBottom;
            this.Data2RangeTop = data2RangeTop;
        }

        public void Initialize(int data1, int data2)
        {
            Initialize(data1, data1, data2, data2);
        }

        public abstract string Field1
        {
            get;
        }

        public abstract string Field2
        {
            get;
        }

        public bool MatchesMssMsg(MssMsg mssMsg)
        {
            return mssMsg.Type == this.mssMsgType &&
                mssMsg.Data1 >= this.Data1RangeBottom && mssMsg.Data1 <= this.Data1RangeTop &&
                mssMsg.Data2 >= this.Data2RangeBottom && mssMsg.Data2 <= this.Data2RangeTop;
        }
    }
}
