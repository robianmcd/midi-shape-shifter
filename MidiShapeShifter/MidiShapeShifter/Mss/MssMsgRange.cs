using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss
{
    public class MssMsgRange
    {
        public MssMsgInfo MsgInfo { get; protected set; }

        protected MssMsgType _msgType;
        public MssMsgType MsgType
        {
            get 
            { 
                return this._msgType; 
            }

            set
            {
                this._msgType = value;
                this.MsgInfo = Factory_MssMsgInfo.Create(value);
            }
        }

        public int Data1RangeBottom;
        public int Data1RangeTop;
        public int Data2RangeBottom;
        public int Data2RangeTop;


        public void InitAllMembers(MssMsgType msgType, 
                         int data1RangeBottom, int data1RangeTop, 
                         int data2RangeBottom, int data2RangeTop)
        {
            this.MsgType = msgType;

            this.Data1RangeBottom = data1RangeBottom;
            this.Data1RangeTop = data1RangeTop;
            this.Data2RangeBottom = data2RangeBottom;
            this.Data2RangeTop = data2RangeTop;
        }

        public void InitAllMembers(MssMsgType msgType, int data1, int data2)
        {
            InitAllMembers(msgType, data1, data1, data2, data2);
        }

        public string Data1RangeStr
        {
            get
            {
                return GetRangeString(this.Data1RangeBottom, this.Data1RangeTop, 
                                      this.MsgInfo.MinData1Value, this.MsgInfo.MaxData1Value);
            }
        }

        public string Data2RangeStr
        {
            get
            {
                return GetRangeString(this.Data2RangeBottom, this.Data2RangeTop,
                                      this.MsgInfo.MinData2Value, this.MsgInfo.MaxData2Value);
            }
        }

        protected string GetRangeString(int rangeBottom, int rangeTop, int minValue, int maxValue)
        {
            if (rangeBottom == MssMsgUtil.UNUSED_MSS_MSG_DATA_VAL ||
                   rangeTop == MssMsgUtil.UNUSED_MSS_MSG_DATA_VAL) 
            {
                return "";
            } 
            else if (rangeBottom == rangeTop) 
            {
                return rangeBottom.ToString();
            }
            else if (rangeBottom == minValue && rangeTop == maxValue)
            {
                return MssMsgUtil.RANGE_ALL_STR;
            }
            else
            {
                return rangeBottom.ToString() + "-" + rangeTop.ToString();                    
            }
        }

        public bool MsgIsInRange(MssMsg mssMsg)
        {
            return mssMsg.Type == this.MsgType &&
                mssMsg.Data1 >= this.Data1RangeBottom && mssMsg.Data1 <= this.Data1RangeTop &&
                mssMsg.Data2 >= this.Data2RangeBottom && mssMsg.Data2 <= this.Data2RangeTop;
        }
    }
}
