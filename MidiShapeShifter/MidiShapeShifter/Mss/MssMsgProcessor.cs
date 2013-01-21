using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Ninject;

using MidiShapeShifter.Ioc;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Evaluation;
using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Responsible for applying the mappings stored in MappingManager to incoming MSS messages. The resulting MSS
    ///     messages are then stored in the MssComponentHub.
    /// </summary>
    public class MssMsgProcessor : IMssMsgProcessor
    {
        protected IBaseGraphableMappingManager mappingMgr;

        protected IMssParameterViewer mssParameterViewer;
        protected IEvaluator evaluator;

        public MssMsgProcessor()
        {
            this.evaluator = IocMgr.Kernel.Get<IEvaluator>();
        }

        public void Init(IBaseGraphableMappingManager mappingMgr, IMssParameterViewer mssParameterViewer)
        {
            Debug.Assert(mappingMgr != null);

            this.mssParameterViewer = mssParameterViewer;
            this.mappingMgr = mappingMgr;
        }

        /// <summary>
        ///     Applies to <paramref name="mssMsg"/> any mappings in the MappingManager that are associated with it.
        /// </summary>
        /// <param name="mssMsg">A MSS message to process.</param>
        /// <returns> A list resulting messages.</returns>
        public List<MssMsg> ProcessMssMsg(MssMsg mssMsg)
        {
            //Apply the pre mapping query processing. If this causes the msg type to change then
            //apply it again for the new type.
            MssMsgType curMsgType;
            do 
            {
                curMsgType = mssMsg.Type;
                IStaticMssMsgInfo staticMsgInfo = Factory_StaticMssMsgInfo.Create(mssMsg.Type);
                staticMsgInfo.ApplyPreMappingQueryProcessing(mssMsg);
            } while(curMsgType != mssMsg.Type);

            //Retrieves mappings from the MappingManager that will affect mssMsg
            IEnumerable<IMappingEntry> mappingEntries = this.mappingMgr.GetCopiesOfMappingEntriesForMsg(mssMsg);

            List<MssMsg> outMessages = new List<MssMsg>();
            if (mappingEntries.Any() == false)
            {
                outMessages.Add(mssMsg);
            }
            else
            {
                foreach (IMappingEntry entry in mappingEntries)
                {
                    MssMsg inMsg = (MssMsg)mssMsg.Clone();
                    IStaticMssMsgInfo inMsgInfo = Factory_StaticMssMsgInfo.Create(entry.InMssMsgRange.MsgType);
                    inMsgInfo.ApplyPreProcessing(inMsg);
                    MssMsg preProcessedMsg = (MssMsg)inMsg.Clone();

                    EvaluationCurveInput evalInput = new EvaluationCurveInput();
                    evalInput.Init(inMsg, this.mssParameterViewer.GetVariableParamInfoList(), entry);
                    ReturnStatus<double> evalReturnStatus = this.evaluator.Evaluate(evalInput);

                    if (evalReturnStatus.IsValid == false)
                    {
                        continue;                        
                    }

                    double mappedRelativeData3 = evalReturnStatus.Value;
                    IStaticMssMsgInfo outMsgInfo = Factory_StaticMssMsgInfo.Create(entry.OutMssMsgRange.MsgType);

                    double data3RangeSize = outMsgInfo.MaxData3Value - outMsgInfo.MinData3Value;
                    double mappedData3 = mappedRelativeData3 * data3RangeSize + outMsgInfo.MinData3Value;

                    //If data3 has been mapped outside of the range of values for its message type then this 
                    //mapping will not output anything.
                    if (mappedData3 >= outMsgInfo.MinData3Value &&
                        mappedData3 <= outMsgInfo.MaxData3Value)
                    {
                        //Calculate what mssMsg.Data1 will be mapped to.
                        double mappedData1 = CalculateLinearMapping(entry.InMssMsgRange.Data1RangeBottom,
                                                                 entry.InMssMsgRange.Data1RangeTop,
                                                                 entry.OutMssMsgRange.Data1RangeBottom,
                                                                 entry.OutMssMsgRange.Data1RangeTop,
                                                                 mssMsg.Data1);
                        //Calculate what mssMsg.Data2 will be mapped to.
                        double mappedData2 = CalculateLinearMapping(entry.InMssMsgRange.Data2RangeBottom,
                                                                 entry.InMssMsgRange.Data2RangeTop,
                                                                 entry.OutMssMsgRange.Data2RangeBottom,
                                                                 entry.OutMssMsgRange.Data2RangeTop,
                                                                 mssMsg.Data2);

                        MssMsg outMsg = new MssMsg(entry.OutMssMsgRange.MsgType, mappedData1, mappedData2, mappedData3);

                        //Apply the post processing. If this causes the msg type to change then
                        //apply it again for the new type.
                        do
                        {
                            curMsgType = mssMsg.Type;
                            IStaticMssMsgInfo staticMsgInfo = Factory_StaticMssMsgInfo.Create(outMsg.Type);
                            staticMsgInfo.ApplyPostProcessing(preProcessedMsg, outMsg);
                        } while (curMsgType != mssMsg.Type);

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
        protected double CalculateLinearMapping(double inRangeBottom, double inRangeTop,
                                             double outRangeBottom, double outRangeTop,
                                             double ValueToMap)
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
                double outRangeSize = outRangeTop - outRangeBottom;

                //maybe we should round here
                double outRangeOffset = percentIntoRange * outRangeSize;

                return outRangeBottom + outRangeOffset;
            }
        }
    }
}
