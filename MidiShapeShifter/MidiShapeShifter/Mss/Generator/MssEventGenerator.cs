using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: comment this calss
    public class MssEventGenerator
    {
        protected const int GENERATOR_UPDATES_PER_BAR = 128;
        protected const int GENERATOR_UPDATES_PER_SECOND = 64;

        protected IDryMssEventInputPort dryMssEventInputPort;
        protected IHostInfoOutputPort hostInfoOutputPort;

        protected GeneratorMappingManager generatorMappingMgr;

        protected MssMsgProcessor mssMsgProcessor;

        public int PreviousGenId { get; private set; }

        public MssEventGenerator()
        {
            PreviousGenId = -1;
            this.mssMsgProcessor = new MssMsgProcessor();
            this.generatorMappingMgr = new GeneratorMappingManager();

        }

        public void Init(IHostInfoOutputPort hostInfoOutputPort, 
                         IWetMssEventOutputPort wetMssEventOutputPort, 
                         IDryMssEventInputPort dryMssEventInputPort,
                         GeneratorMappingManager generatorMappingMgr)
        {
            this.mssMsgProcessor.Init(this.generatorMappingMgr);

            wetMssEventOutputPort.WetMssEventsReceived += new 
                    WetMssEventsReceivedEventHandler(WetMssEventOutputPort_WetMssEventsReceived);

            this.hostInfoOutputPort = hostInfoOutputPort;
            hostInfoOutputPort.BeforeProcessingCycleEnd += new
                    ProcessingCycleEndEventHandler(HostInfoOutputPort_BeforeProcessingCycleEnd);
            
            this.dryMssEventInputPort = dryMssEventInputPort;

            this.generatorMappingMgr = generatorMappingMgr;
        }

        protected void WetMssEventOutputPort_WetMssEventsReceived(List<MssEvent> mssEventList)
        {
            foreach(MssEvent mssEvent in mssEventList)
            {
                if (mssEvent.mssMsg.Type == MssMsgType.GeneratorToggle) 
                {
                    //TODO: deal with GeneratorToggle message
                }
            }
        }

        protected void HostInfoOutputPort_BeforeProcessingCycleEnd(long cycleEndTimestamp)
        {
            int numGens = this.generatorMappingMgr.GetNumEntries();
            //TODO: call generateEvent() when nessary in this loop
            for (int i = 0; i < numGens; i++)
            {
                GeneratorMappingEntry curEntry = 
                        this.generatorMappingMgr.GetGenMappingEntryByIndex(i);

                int ticksPerUpdate = GetTicksPerGenUpdate(curEntry.GenConfigInfo.PeriodType);

                if (curEntry.GenConfigInfo.Enabled == true)
                {
                    if (curEntry.GenHistoryInfo.Initialized == false)
                    {
                        if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
                        {

                        }
                        else if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
                        {
                            curEntry.GenHistoryInfo.InitAllMembers(
                                cycleEndTimestamp - ticksPerUpdate,
                                0,
                                double.NaN);
                        }
                        else
                        {
                            //Unexpected period type
                            Debug.Assert(false);
                            return;
                        }
                    }

                    while (curEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp + ticksPerUpdate
                                <= cycleEndTimestamp)
                    {
                        MssEvent generatedEvent = GenerateEvent(curEntry);
                        if (generatedEvent != null)
                        {
                            this.dryMssEventInputPort.ReceiveDryMssEvent(generatedEvent);
                        }
                    }
                }
            }
        }

        protected MssEvent GenerateEvent(GeneratorMappingEntry genEntry)
        {
            Debug.Assert(genEntry.GenHistoryInfo.Initialized == true);

            int ticksPerUpdate = GetTicksPerGenUpdate(genEntry.GenConfigInfo.PeriodType);
            int periodSizeInTicks = GetPeriodSizeInTicks(genEntry.GenConfigInfo);
            double RelativeperiodIncrement = ticksPerUpdate / periodSizeInTicks;
            double relPosInPeriod = genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate +
                                    RelativeperiodIncrement;

            if (relPosInPeriod > 1)
            {
                relPosInPeriod--;
            }

            MssMsg relPosMsg = CreateInputMsgForGenMappingEntry(genEntry, relPosInPeriod);
            List<MssMsg> processedMsgList = this.mssMsgProcessor.ProcessMssMsg(relPosMsg);
            Debug.Assert(processedMsgList.Count == 1);

            if (processedMsgList[0].Data3 == genEntry.GenHistoryInfo.LastValueSent)
            {
                return null;
            }
            else
            {
                long GenUpdateTimestamp = genEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp +
                                          ticksPerUpdate;

                MssEvent generatedEvent = new MssEvent();
                generatedEvent.mssMsg = processedMsgList[0];
                generatedEvent.timestamp = GenUpdateTimestamp;


                genEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp = GenUpdateTimestamp;
                genEntry.GenHistoryInfo.LastValueSent = generatedEvent.mssMsg.Data3;
                genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate = relPosInPeriod;

                return generatedEvent;
            }
        }

        protected int GetTicksPerGenUpdate(GenPeriodType periodType)
        {
            int updatesPerSecond;
            if (periodType == GenPeriodType.BeatSynced)
            {
                Debug.Assert(this.hostInfoOutputPort.TempoIsInitialized);
                //TODO: impliment this
                updatesPerSecond = 64;

            }
            else if (periodType == GenPeriodType.Time)
            {
                updatesPerSecond = GENERATOR_UPDATES_PER_SECOND;
            }
            else
            {
                //Unexpected period type
                Debug.Assert(false);
                return 1000;
            }

            return (1 / updatesPerSecond) * (int)System.TimeSpan.TicksPerSecond;
        }

        protected int GetPeriodSizeInTicks(GenEntryConfigInfo genConfigInfo)
        {
            if (genConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                //TODO: Impliment this
                return 1000;
            }
            else if (genConfigInfo.PeriodType == GenPeriodType.Time)
            {
                return genConfigInfo.TimePeriodInMs * (int)System.TimeSpan.TicksPerMillisecond;
            }
            else
            {
                //Unexpected GenPeriodType
                Debug.Assert(false);
                return 1000;
            }
        }

        protected MssMsg CreateInputMsgForGenMappingEntry(GeneratorMappingEntry genEntry, 
                                                          double relPosInPeriod)
        {
            MssMsg relPosMsg = new MssMsg();

            if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                relPosMsg.Type = MssMsgType.RelBarPeriodPos;
            }
            else if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
            {
                relPosMsg.Type = MssMsgType.RelTimePeriodPos;
            }
            else
            {
                //Unexpected GenPeriodType
                Debug.Assert(false);
                return null;
            }

            //Data1RangeBottom and Data2RangeBottom should be the same as the top of the range
            relPosMsg.Data1 = genEntry.InMssMsgRange.Data1RangeBottom;
            relPosMsg.Data2 = genEntry.InMssMsgRange.Data2RangeBottom;
            relPosMsg.Data3 = relPosInPeriod;

            return relPosMsg;
        }
    }
}
