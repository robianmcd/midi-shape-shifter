using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{

    public abstract class MidiMsgInfo : MssMsgInfo
    {
        protected int chanRangeBottom;
        protected int chanRangeTop;
        protected int paramRangeBottom;
        protected int paramRangeTop;

        public void Initialize(int chanRangeBottom, int chanRangeTop, int paramRangeBottom, int paramRangeTop)
        {
            this.chanRangeBottom = chanRangeBottom;
            this.chanRangeTop = chanRangeTop;
            this.paramRangeBottom = paramRangeBottom;
            this.paramRangeTop = paramRangeTop;
        }

        public override string Field1
        {
            get
            {
                if (chanRangeBottom == chanRangeTop) {
                    return chanRangeBottom.ToString();
                }
                else if (chanRangeBottom == MssMsgUtil.MIN_CHANNEL && chanRangeTop == MssMsgUtil.MAX_CHANNEL)
                {
                    return MssMsgUtil.RANGE_ALL_STR;
                }
                else
                {
                    return chanRangeBottom.ToString() + "-" + chanRangeTop.ToString();       
                }
            }
        }

        public override string Field2
        {
            get
            {
                if (paramRangeBottom == paramRangeTop)
                {
                    return paramRangeBottom.ToString();
                }
                else if (paramRangeBottom == MssMsgUtil.MIN_PARAM && paramRangeTop == MssMsgUtil.MAX_PARAM)
                {
                    return MssMsgUtil.RANGE_ALL_STR;
                }
                else
                {
                    return paramRangeBottom.ToString() + "-" + paramRangeTop.ToString();
                }
            }
        }

        public override bool MatchesMssMsg(MssMsg mssMsg)
        {
            return mssMsg.Type == this.mssMsgType && 
                mssMsg.Data1 >= this.chanRangeBottom && mssMsg.Data1 <= this.chanRangeTop &&
                mssMsg.Data2 >= this.paramRangeBottom && mssMsg.Data2 <= this.paramRangeTop;
        }
    }
}
