using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.Relays;
using System;
using System.Diagnostics;

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
                new WetMssEventReceivedEventHandler(WetEventReceived);

            this.mssParameters.ParameterValueChanged +=
                new ParameterValueChangedEventHandler(ParameterValueChanged);

            this.hostInfoOutput.BeforeProcessingCycleEnd +=
                new ProcessingCycleEndEventHandler(OnBeforeProcessingCycleEnd);
        }

        protected void WetEventReceived(MssEvent wetEvent)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                if (wetEvent.mssMsg.Type == MssMsgType.Parameter)
                {
                    if (Enum.IsDefined(typeof(MssParameterID), (int)wetEvent.mssMsg.Data1) == false)
                    {
                        //Unknown MssParameterId
                        Debug.Assert(false);
                        return;
                    }

                    this.mssParameters.SetParameterRelativeValue(
                        (MssParameterID)(int)wetEvent.mssMsg.Data1,
                        wetEvent.mssMsg.Data3);
                }

            }
        }

        protected readonly object locker = new object();

        protected void ParameterValueChanged(MssParameterID paramId, double newValue)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                if (this.hostInfoOutput.SampleRateIsInitialized == false)
                {
                    return;
                }

                long watchOffestAtParamChanged = this.stopwatch.Elapsed.Ticks;

                MssParamInfo paramInfo = this.mssParameters.GetParameterInfoCopy(paramId);
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
            lock (MssComponentHub.criticalSectioinLock)
            {
                this.watchOffestAtLastCycleEnd = this.stopwatch.Elapsed.Ticks;
                this.sampleTimeAtLastCycleEnd = sampleTimeAtEndOfCycle;
            }
        }
    }
}
