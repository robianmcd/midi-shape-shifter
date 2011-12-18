using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping
{
    public enum IoType { Input, Output };


    /// <summary>
    ///     A MappingEntry stores all information associated with a mapping. This information can be broken down into:
    ///     1. Information about which MSS messages will be accepted for input by the mapping (eg. InMssMsgRange)
    ///     2. Information about how to modify incomeing MSS messages. OnMssMsgRange is used to map the incoming MSS 
    ///         message's type, data1, and data2. The equation is used to map the incoming MSS message's data3.
    /// </summary>
    [Serializable]
    public class MappingEntry : IMappingEntry
    {
        /// <summary>
        ///     Specifies which MSS messages will be accepted for input as well as additional information about the 
        ///     input type
        /// </summary>
        protected IMssMsgRange _inMssMsgRange;
        public IMssMsgRange InMssMsgRange { get { return this._inMssMsgRange; } 
                                            set { this._inMssMsgRange = value; } }

        /// <summary>
        ///     Specifies the range of messages that can be output as well as additional information about the output 
        ///     type.
        /// </summary>
        protected IMssMsgRange _outMssMsgRange;
        public IMssMsgRange OutMssMsgRange { get { return this._outMssMsgRange; }
                                             set { this._outMssMsgRange = value; } }

        /// <summary>
        ///     If there are multiple mapping entries with overlapping input ranges then a single mss message can
        ///     generate several messages. This can be disabled by setting the override duplicates flag to true.
        ///     If there are two mapping entries with an overlapping inMsgRange and overrideDuplicates is set to 
        ///     true for each one, then the one closer to the top of the mapping list box overrides the other.
        /// </summary>
        public bool OverrideDuplicates { get; set; }

        /// <summary>
        ///     Contains information about the curve shape for this mapping and how it is being entered.
        /// </summary>
        protected CurveShapeInfo _curveShapeInfo;        
        public CurveShapeInfo CurveShapeInfo { get { return this._curveShapeInfo; }
                                               set { this._curveShapeInfo = value; } }

        public MappingEntry() 
        {
            
        }

        public void InitAllMembers(IMssMsgRange inMsgRange, IMssMsgRange outMsgRange,
                            bool overrideDuplicates, CurveShapeInfo curveShapeInfo)
        {
            this.InMssMsgRange = inMsgRange;
            this.OutMssMsgRange = outMsgRange;
            this.OverrideDuplicates = overrideDuplicates;
            this.CurveShapeInfo = curveShapeInfo;
        }

        /// <summary>
        ///     Gets a string representing an MssMsgType used by this MappingEntry
        /// </summary>
        /// <param name="ioCategory">Specifies wheather to use the type from InMssMsgRange or OutMssMsgRange</param>
        public string GetReadableMsgType(IoType ioCategory)
        {
            if (ioCategory == IoType.Input)
            {
                return MssMsg.MssMsgTypeNames[(int)this.InMssMsgRange.MsgType];
            }
            else if (ioCategory == IoType.Output)
            {
                return MssMsg.MssMsgTypeNames[(int)this.OutMssMsgRange.MsgType];
            }
            else
            {
                //Unknown IO type
                Debug.Assert(false);
                return "";
            }
        }

        /// <summary>
        ///     Gets a string representing this MappingEntry's OverrideDuplicates status
        /// </summary>
        public string GetReadableOverrideDuplicates()
        {
            return GetReadableBool(this.OverrideDuplicates);
        }

        protected string GetReadableBool(bool yesNoBool)
        {
            if (yesNoBool == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }
    }
}
