using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Responsible for applying the mappings stored in MappingManager to incoming MSS messages. The resulting MSS
    ///     messages are then stored in the MssComponentHub.
    /// </summary>
    public class MssMsgProcessor
    {
        protected MappingManager mappingMgr;

        public MssMsgProcessor()
        {

        }

        public void Init(MappingManager mappingMgr)
        {
            Debug.Assert(mappingMgr != null);

            this.mappingMgr = mappingMgr;            
        }

        /// <summary>
        ///     Applies to <paramref name="mssMsg"/> any mappings in the MappingManager that are associated with it.
        /// </summary>
        /// <param name="mssMsg">A MSS message to process.</param>
        /// <returns> A list resulting messages.</returns>
        public List<MssMsg> ProcessMssMsg(MssMsg mssMsg)
        {
            //Retrieves mappings from the MappingManager that will affect mssMsg
            IEnumerable<MappingEntry> mappingEntries = this.mappingMgr.GetAssociatedEntries(mssMsg);

            List<MssMsg> outMessages = new List<MssMsg>();
            if (mappingEntries.Any() == false)
            {
                outMessages.Add(mssMsg);
            }
            else
            {
                foreach (MappingEntry entry in mappingEntries)
                {
                    //TODO: map mssMsg.Data3 to equation
                    int mappedData3 = 100;

                    //If data3 has been mapped outside of the range of MIDI values then this mapping will not output 
                    //anything
                    if (mappedData3 >= 0 && mappedData3 <= 127)
                    {
                        //Calculate what mssMsg.Data1 will be mapped to.
                        int mappedData1 = CalculateLinearMapping(entry.InMssMsgInfo.Data1RangeBottom,
                                                                 entry.InMssMsgInfo.Data1RangeTop,
                                                                 entry.OutMssMsgInfo.Data1RangeBottom,
                                                                 entry.OutMssMsgInfo.Data1RangeTop,
                                                                 mssMsg.Data1);
                        //Calculate what mssMsg.Data2 will be mapped to.
                        int mappedData2 = CalculateLinearMapping(entry.InMssMsgInfo.Data2RangeBottom,
                                                                 entry.InMssMsgInfo.Data2RangeTop,
                                                                 entry.OutMssMsgInfo.Data2RangeBottom,
                                                                 entry.OutMssMsgInfo.Data2RangeTop,
                                                                 mssMsg.Data2);

                        MssMsg outMsg = new MssMsg(entry.OutMssMsgInfo.mssMsgType, mappedData1, mappedData2, mappedData3);
                        outMessages.Add(outMsg);
                    }
                }
            }

            return outMessages;
        }

        /// <summary>
        ///     Maps a value from an input range to a value in an output range such that the values are the same 
        ///     percent of the way into the ranges. For example if the input range is 1 to 3, the output range is 
        ///     4 to 8 and the value is 2 then it would be mapped to 6.
        /// </summary>
        /// <param name="inRangeBottom">Bottom value of the input range.</param>
        /// <param name="inRangeTop">Top value of the input range.</param>
        /// <param name="outRangeBottom">Bottom value of the output range.</param>
        /// <param name="outRangeTop">Top value of the output range.</param>
        /// <param name="ValueToMap">A value in the input range.</param>
        /// <returns>
        ///     A value in the output range that is in the same relitive position that <paramref name="ValueToMap"/> 
        ///     was in the input range.
        /// </returns>
        protected int CalculateLinearMapping(int inRangeBottom, int inRangeTop,
                                             int outRangeBottom, int outRangeTop,
                                             int ValueToMap)
        {
            //if the in range is just a single value then treating it as a range would result in a divide by zero error. 
            //So this case must be handled seperateally.
            if (inRangeBottom == inRangeTop)
            {
                return outRangeBottom;
            }
            else
            {
                double percentIntoRange = (ValueToMap - inRangeBottom) / (inRangeTop - inRangeBottom);
                int outRangeSize = outRangeTop - outRangeBottom;

                //maybe we should round here
                int outRangeOffset = (int) percentIntoRange * outRangeSize;

                return outRangeBottom + outRangeOffset;
            }
        }
    }
}
