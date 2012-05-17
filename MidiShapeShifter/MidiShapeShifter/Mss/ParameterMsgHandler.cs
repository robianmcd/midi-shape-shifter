using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss
{
    public class ParameterMsgHandler
    {
        protected MssParameters mssParameters;
        protected IWetMssEventOutputPort wetEventOutput;
        protected IDryMssEventInputPort dryEventInput;
        protected IHostInfoOutputPort hostInfoOutput;

        protected Stopwatch stopwatch;
        protected long watchOffestAtLastCycleEnd;
        protected long sampleTimeAtLastCycleEnd = -1;

        public ParameterMsgHandler()
        {
            this.stopwatch = Stopwatch.StartNew();
        }

        public void Init(MssParameters mssParameters, 
                         IWetMssEventOutputPort wetEventOutput,
                         IDryMssEventInputPort dryEventInput,
                         IHostInfoOutputPort hostInfoOutput)
        {
            this.mssParameters = mssParameters;
            this.wetEventOutput = wetEventOutput;
            this.dryEventInput = dryEventInput;
            this.hostInfoOutput = hostInfoOutput;

            this.wetEventOutput.WetMssEventsReceived += 
                new WetMssEventsReceivedEventHandler(WetEventReceived);

            this.mssParameters.ParameterValueChanged +=
                new ParameterValueChangedEventHandler(ParameterValueChanged);

            this.hostInfoOutput.BeforeProcessingCycleEnd +=
                new ProcessingCycleEndEventHandler(OnBeforeProcessingCycleEnd);
        }

        protected void WetEventReceived(List<MssEvent> wetEventList)
        {
            lock (locker)
            {
                foreach (MssEvent wetEvent in wetEventList)
                {
                    if (wetEvent.mssMsg.Type == MssMsgType.Parameter)
                    {
                        if (Enum.IsDefined(typeof(MssParameterID), (int)wetEvent.mssMsg.Data1) == false)
                        {
                            //Unknown MssParameterId
                            Debug.Assert(false);
                            continue;
                        }

                        this.mssParameters.SetParameterRelativeValue(
                            (MssParameterID)(int)wetEvent.mssMsg.Data1,
                            wetEvent.mssMsg.Data3);
                    }
                }
            }
        }

        protected readonly object locker = new object();

        protected void ParameterValueChanged(MssParameterID paramId, double newValue)
        {
            lock (locker)
            {
                if (this.hostInfoOutput.SampleRateIsInitialized == false)
                {
                    return;
                }

                long watchOffestAtParamChanged = this.stopwatch.Elapsed.Ticks;

                MssParamInfo paramInfo = this.mssParameters.GetParameterInfo(paramId);
                double relValue = CustomMathUtils.AbsToRelVal(paramInfo.MinValue, paramInfo.MaxValue, newValue);

                MssMsg paramMsg = new MssMsg(MssMsgType.Parameter,
                                             (int)paramId,
                                             MssMsgUtil.UNUSED_MSS_MSG_DATA,
                                             relValue);
                MssEvent paramEvent = new MssEvent();

                paramEvent.mssMsg = paramMsg;

                long ticksSinceCycleEnd = watchOffestAtParamChanged - this.watchOffestAtLastCycleEnd;
                double secondsSinceCycleEnd = ticksSinceCycleEnd / (double)TimeSpan.TicksPerSecond;
                long samplesSinceCycleEnd = (long)Math.Round(secondsSinceCycleEnd * this.hostInfoOutput.SampleRate);

                //Sometimes the stopwatch may not be accurate enough to notice the difference 
                //between the end of the last cycle and now. Setting samplesSinceCycleEnd to
                //1 will cause this event to happen at the very start of this processing cycle.
                if (samplesSinceCycleEnd == 0)
                {
                    samplesSinceCycleEnd = 1;
                }

                paramEvent.sampleTime = this.sampleTimeAtLastCycleEnd + samplesSinceCycleEnd;

                this.dryEventInput.ReceiveDryMssEvent(paramEvent);
            }
        }

        protected void OnBeforeProcessingCycleEnd(long sampleTimeAtEndOfCycle)
        {
            lock(locker)
            {
                this.watchOffestAtLastCycleEnd = this.stopwatch.Elapsed.Ticks;
                this.sampleTimeAtLastCycleEnd = sampleTimeAtEndOfCycle;
            }
        }
    }
}
