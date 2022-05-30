using MidiShapeShifter.Mss.MssMsgInfoTypes;
using System;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Specfies a range of MssMsgs. An MssMsg is considered to be in an MssMsgRange if its type matches this 
    ///     classes MsgType and its Data1 and Data2 fall into the ranges specified in this class. This class is
    ///     used to describe the set of messages affected by a mapping.
    /// </summary>
    [DataContract]
    public class MssMsgRange : IMssMsgRange
    {
        /// <summary>
        ///     The message type of all MssMsgs that match this range. When MsgType is changed, MsgInfo will be
        ///     recreated so that it contains information about the new MssMsgType
        /// </summary>
        [DataMember]
        public MssMsgType MsgType { get; set; }

        /// <summary>
        ///     Specifies the range of accepted Data1 values
        /// </summary>

        [DataMember(Name = "Data1RangeBottom")]
        protected double _data1RangeBottom;
        public double Data1RangeBottom
        {
            get => this._data1RangeBottom;
            set => this._data1RangeBottom = value;
        }
        [DataMember(Name = "Data1RangeTop")]
        protected double _data1RangeTop;
        public double Data1RangeTop
        {
            get => this._data1RangeTop;
            set => this._data1RangeTop = value;
        }

        /// <summary>
        ///     Specifies the range of accepted Data2 values
        /// </summary>
        [DataMember(Name = "Data2RangeBottom")]
        protected double _data2RangeBottom;
        public double Data2RangeBottom
        {
            get => this._data2RangeBottom;
            set => this._data2RangeBottom = value;
        }
        [DataMember(Name = "Data2RangeTop")]
        protected double _data2RangeTop;
        public double Data2RangeTop
        {
            get => this._data2RangeTop;
            set => this._data2RangeTop = value;
        }

        /// <summary>
        ///     Initializes the public member varialbes of this class. This method does not need to 
        ///     be called as the public members can be initialized individually.
        /// </summary>
        public void InitPublicMembers(MssMsgType msgType,
                         double data1RangeBottom, double data1RangeTop,
                         double data2RangeBottom, double data2RangeTop)
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
        public void InitPublicMembers(MssMsgType msgType, double data1, double data2)
        {
            InitPublicMembers(msgType, data1, data1, data2, data2);
        }

        /// <summary>
        ///     Returns a user readable string representing the range of Data1 values
        /// </summary>
        public string GetData1RangeStr(IFactory_MssMsgInfo msgInfoFactory)
        {
            IMssMsgInfo msgInfo = msgInfoFactory.Create(this.MsgType);
            return GetRangeString(msgInfo.ConvertData1ToString(this.Data1RangeBottom),
                                    msgInfo.ConvertData1ToString(this.Data1RangeTop),
                                    msgInfo.ConvertData1ToString(msgInfo.MinData1Value),
                                    msgInfo.ConvertData1ToString(msgInfo.MaxData1Value));
        }

        /// <summary>
        ///     Returns a user readable string representing the range of Data1 values
        /// </summary>
        public string GetData2RangeStr(IFactory_MssMsgInfo msgInfoFactory)
        {
            IMssMsgInfo msgInfo = msgInfoFactory.Create(this.MsgType);
            return GetRangeString(msgInfo.ConvertData2ToString(this.Data2RangeBottom),
                                      msgInfo.ConvertData2ToString(this.Data2RangeTop),
                                      msgInfo.ConvertData2ToString(msgInfo.MinData2Value),
                                      msgInfo.ConvertData2ToString(msgInfo.MaxData2Value));
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
            IStaticMssMsgInfo staticInfo = Factory_StaticMssMsgInfo.Create(this.MsgType);
            return staticInfo.TypeIsInRange(mssMsg.Type) &&
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
            hash = (hash * 7) + this.Data1RangeBottom.GetHashCode();
            hash = (hash * 7) + this.Data1RangeTop.GetHashCode();
            hash = (hash * 7) + this.Data2RangeBottom.GetHashCode();
            hash = (hash * 7) + this.Data2RangeTop.GetHashCode();
            return hash;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        public IMssMsgRange Clone()
        {
            MssMsgRange msgRangeClone = new MssMsgRange();
            msgRangeClone.InitPublicMembers(this.MsgType,
                                            this.Data1RangeBottom, this.Data1RangeTop,
                                            this.Data2RangeBottom, this.Data2RangeTop);

            return msgRangeClone;
        }

    }
}
