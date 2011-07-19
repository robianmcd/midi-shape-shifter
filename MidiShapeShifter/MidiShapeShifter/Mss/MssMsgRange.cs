using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Specfies a range of MssMsgs. An MssMsg is conidered to be in an MssMsgRange if its type matches this 
    ///     classes MsgType and its Data1 and Data2 fall into the ranges specified in this class. This class is
    ///     used to describe the set of messages affected by a mapping.
    /// </summary>
    public class MssMsgRange
    {
        /// <summary>
        ///     Contains information about the message type specified by MsgType. If MsgType has not been set then 
        ///     MsgInfo will be null.
        /// </summary>
        public MssMsgInfo MsgInfo { get; protected set; }

        /// <summary>
        ///     The message type of all MssMsgs that match this range. When MsgType is changed, MsgInfo will be
        ///     recreated so that it contains information about the new MssMsgType
        /// </summary>
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

        /// <summary>
        ///     Specifies the range of accepted Data1 values
        /// </summary>
        public int Data1RangeBottom;
        public int Data1RangeTop;

        /// <summary>
        ///     Specifies the range of accepted Data2 values
        /// </summary>
        public int Data2RangeBottom;
        public int Data2RangeTop;

        /// <summary>
        ///     Initializes the member varialbes of this class. This method does not need to be called as all required 
        ///     member variables are public and cal be initialized individually.
        /// </summary>
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

        /// <summary>
        ///     Initializes the member varialbes of this class. When this method is used to initialize the class, a 
        ///     MssMsg will have to match data1 and data2 exactally to fall into this range.
        /// </summary>
        public void InitAllMembers(MssMsgType msgType, int data1, int data2)
        {
            InitAllMembers(msgType, data1, data1, data2, data2);
        }

        /// <summary>
        ///     A user readable string representing the range of Data1 values
        /// </summary>
        public string Data1RangeStr
        {
            get
            {
                return GetRangeString(this.Data1RangeBottom, this.Data1RangeTop, 
                                      this.MsgInfo.MinData1Value, this.MsgInfo.MaxData1Value);
            }
        }

        /// <summary>
        ///     A user readable string representing the range of Data1 values
        /// </summary>
        public string Data2RangeStr
        {
            get
            {
                return GetRangeString(this.Data2RangeBottom, this.Data2RangeTop,
                                      this.MsgInfo.MinData2Value, this.MsgInfo.MaxData2Value);
            }
        }

        /// <summary>
        ///     Builds a string representing a range.
        /// </summary>
        /// <param name="rangeBottom">Lowest value in the range.</param>
        /// <param name="rangeTop">Highest value in the range</param>
        /// <param name="minValue">
        ///     Lowest value aloud for the type of data in the range. EG. if it is a range of channels then minValues 
        ///     would be 1 but if it was a range of note numbers then minValue would be 0
        /// </param>
        /// <param name="maxValue">Highest value aloud for the type of data in the range.</param>
        protected string GetRangeString(int rangeBottom, int rangeTop, int minValue, int maxValue)
        {
            if (rangeBottom == MssMsgUtil.UNUSED_MSS_MSG_DATA ||
                   rangeTop == MssMsgUtil.UNUSED_MSS_MSG_DATA) 
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

        /// <summary>
        ///     Determines weather <paramref name="mssMsg"/> falls into this range.
        /// </summary>
        /// <returns>True if mssMsg is a member of this range. False otherwise.</returns>
        public bool MsgIsInRange(MssMsg mssMsg)
        {
            return mssMsg.Type == this.MsgType &&
                mssMsg.Data1 >= this.Data1RangeBottom && mssMsg.Data1 <= this.Data1RangeTop &&
                mssMsg.Data2 >= this.Data2RangeBottom && mssMsg.Data2 <= this.Data2RangeTop;
        }
    }
}
