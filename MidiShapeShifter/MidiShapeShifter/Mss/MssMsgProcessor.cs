using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    public class MssMsgProcessor
    {
        protected MssComponentHub mssHub;

        public MssMsgProcessor(MssComponentHub mssHub)
        {
            Debug.Assert(mssHub != null);

            this.mssHub = mssHub;
        }

        public List<MssMsg> ProcessMssMsg(MssMsg mssMsg)
        {
            IEnumerable<MappingEntry> mappingEntries = this.mssHub.MappingMgr.GetAssociatedEntries(mssMsg);

            List<MssMsg> outMessages = new List<MssMsg>();

            foreach (MappingEntry entry in mappingEntries)
            {
                //TODO: map mssMsg.Data3 to equation
                int mappedData3 = 100;

                if (mappedData3 >= 0 && mappedData3 <= 127)
                {
                    int mappedData1 = CalculateLinearMapping(entry.InMssMsgInfo.Data1RangeBottom,
                                                             entry.InMssMsgInfo.Data1RangeTop,
                                                             entry.OutMssMsgInfo.Data1RangeBottom,
                                                             entry.OutMssMsgInfo.Data1RangeTop,
                                                             mssMsg.Data1);
                    int mappedData2 = CalculateLinearMapping(entry.InMssMsgInfo.Data2RangeBottom,
                                                             entry.InMssMsgInfo.Data2RangeTop,
                                                             entry.OutMssMsgInfo.Data2RangeBottom,
                                                             entry.OutMssMsgInfo.Data2RangeTop,
                                                             mssMsg.Data2);

                    MssMsg outMsg = new MssMsg(entry.OutMssMsgInfo.mssMsgType, mappedData1, mappedData2, mappedData3);
                    outMessages.Add(outMsg);
                }
            }

            return outMessages;
        }

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
                int outRangeOffset = (int)percentIntoRange * outRangeSize;

                return outRangeBottom + outRangeOffset;
            }
        }
    }
}
