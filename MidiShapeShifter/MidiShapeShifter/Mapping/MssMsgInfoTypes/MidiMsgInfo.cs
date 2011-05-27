using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping.MssMsgInfoTypes
{

    public abstract class MidiMsgInfo : MssMsgInfo
    {

        public override string Field1
        {
            get
            {
                if (Data1RangeBottom == Data1RangeTop) {
                    return Data1RangeBottom.ToString();
                }
                else if (Data1RangeBottom == MssMsgUtil.MIN_CHANNEL && Data1RangeTop == MssMsgUtil.MAX_CHANNEL)
                {
                    return MssMsgUtil.RANGE_ALL_STR;
                }
                else
                {
                    return Data1RangeBottom.ToString() + "-" + Data1RangeTop.ToString();       
                }
            }
        }

        public override string Field2
        {
            get
            {
                if (Data2RangeBottom == Data2RangeTop)
                {
                    return Data2RangeBottom.ToString();
                }
                else if (Data2RangeBottom == MssMsgUtil.MIN_PARAM && Data2RangeTop == MssMsgUtil.MAX_PARAM)
                {
                    return MssMsgUtil.RANGE_ALL_STR;
                }
                else
                {
                    return Data2RangeBottom.ToString() + "-" + Data2RangeTop.ToString();
                }
            }
        }
    }
}
