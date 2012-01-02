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
    [Serializable]
    public class MssMsgRange : IMssMsgRange
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
                this.MsgInfo = this.msgInfoFactory.Create(value);
            }
        }

        /// <summary>
        ///     Specifies the range of accepted Data1 values
        /// </summary>

        protected int _data1RangeBottom;
        public int Data1RangeBottom { get { return this._data1RangeBottom; }
                                      set { this._data1RangeBottom = value; } }
        protected int _data1RangeTop;
        public int Data1RangeTop { get { return this._data1RangeTop; }
                                      set { this._data1RangeTop = value; } }

        /// <summary>
        ///     Specifies the range of accepted Data2 values
        /// </summary>
        protected int _data2RangeBottom;
        public int Data2RangeBottom { get { return this._data2RangeBottom; } 
                                      set { this._data2RangeBottom = value; } }
        protected int _data2RangeTop;
        public int Data2RangeTop { get { return this._data2RangeTop; } 
                                      set { this._data2RangeTop = value; } }

        protected IFactory_MssMsgInfo msgInfoFactory;

        public void Init(IFactory_MssMsgInfo msgInfoFactory)
        {
            this.msgInfoFactory = msgInfoFactory;
        }

        /// <summary>
        ///     Initializes the public member varialbes of this class. This method does not need to 
        ///     be called as the public members can be initialized individually.
        /// </summary>
        public void InitPublicMembers(MssMsgType msgType, 
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
        ///     Initializes the public member varialbes of this class. When this method is used to 
        ///     initialize the class, a MssMsg will have to match data1 and data2 exactally to fall
        ///     into this range.
        /// </summary>
        public void InitPublicMembers(MssMsgType msgType, int data1, int data2)
        {
            InitPublicMembers(msgType, data1, data1, data2, data2);
        }

        /// <summary>
        ///     A user readable string representing the range of Data1 values
        /// </summary>
        public string Data1RangeStr
        {
            get
            {
                return GetRangeString(this.MsgInfo.ConvertData1ToString(this.Data1RangeBottom), 
                                      this.MsgInfo.ConvertData1ToString(this.Data1RangeTop), 
                                      this.MsgInfo.ConvertData1ToString(this.MsgInfo.MinData1Value),
                                      this.MsgInfo.ConvertData1ToString(this.MsgInfo.MaxData1Value));
            }
        }

        /// <summary>
        ///     A user readable string representing the range of Data1 values
        /// </summary>
        public string Data2RangeStr
        {
            get
            {
                return GetRangeString(this.MsgInfo.ConvertData2ToString(this.Data2RangeBottom),
                                      this.MsgInfo.ConvertData2ToString(this.Data2RangeTop),
                                      this.MsgInfo.ConvertData2ToString(this.MsgInfo.MinData2Value),
                                      this.MsgInfo.ConvertData2ToString(this.MsgInfo.MaxData2Value));
            }
        }

        /// <summary>
        ///     Builds a string representing a range.
        /// </summary>
        /// <param name="rangeBottomStr">String representation of the lowest value in the range.</param>
        /// <param name="rangeTopStr">String representation of the highest value in the range</param>
        /// <param name="minValueStr">
        ///     String representation of the lowest value aloud for the type of data in the range. 
        ///     EG. if it is a range of channels then minValues would be 1 but if it was a range of
        ///     note numbers then minValue would be 0
        /// </param>
        /// <param name="maxValueStr">
        ///     String representation of the highest value aloud for the type of data in the range.
        /// </param>
        protected string GetRangeString(string rangeBottomStr, string rangeTopStr, 
                                        string minValueStr, string maxValueStr)
        {
            if (rangeBottomStr == MssMsgUtil.UNUSED_MSS_MSG_STRING ||
                   rangeTopStr == MssMsgUtil.UNUSED_MSS_MSG_STRING) 
            {
                return MssMsgUtil.UNUSED_MSS_MSG_STRING;
            } 
            else if (rangeBottomStr == rangeTopStr) 
            {
                return rangeBottomStr;
            }
            else if (rangeBottomStr == minValueStr && rangeTopStr == maxValueStr)
            {
                return MssMsgUtil.RANGE_ALL_STR;
            }
            else
            {
                return rangeBottomStr + "-" + rangeTopStr;                    
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

        public override bool Equals(object o)
        {
            IMssMsgRange compareToRange = (IMssMsgRange)o;
            return this.MsgType == compareToRange.MsgType &&
                   this.Data1RangeBottom == compareToRange.Data1RangeBottom &&
                   this.Data1RangeTop == compareToRange.Data1RangeTop &&
                   this.Data2RangeBottom == compareToRange.Data2RangeBottom &&
                   this.Data2RangeTop == compareToRange.Data2RangeTop;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + (int)this.MsgType;
            hash = (hash * 7) + this.Data1RangeBottom;
            hash = (hash * 7) + this.Data1RangeTop;
            hash = (hash * 7) + this.Data2RangeBottom;
            hash = (hash * 7) + this.Data2RangeTop;
            return hash;
        }
    }
}
