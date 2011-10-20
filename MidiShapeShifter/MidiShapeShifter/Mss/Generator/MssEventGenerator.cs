using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: comment this calss and write test casses for it
    /// <summary>
    /// Before the end of each audio processing cycle this class will iterate through the 
    /// entries in generatorMappingManager, create all nessary Mss Events and send them to 
    /// dryMssEventInputPort
    /// </summary>
    public class MssEventGenerator
    {
        /// <summary>
        /// Specifies the maximum number of events that could be generated in a single bar
        /// for a beat synced GeneratorMappingEntry
        /// </summary>
        protected const int GENERATOR_UPDATES_PER_BAR = 128;

        /// <summary>
        /// Specifies the maximum number of events that could be generated in one second
        /// for a time based GeneratorMappingEntry
        /// </summary>
        protected const int GENERATOR_UPDATES_PER_SECOND = 64;

        /// <summary>
        /// This class sends generated events to this input port
        /// </summary>
        protected IDryMssEventInputPort dryMssEventInputPort;

        /// <summary>
        /// Recieves information about the host form this output port such as: bar position,
        /// tempo, and notifications when an audio processing cycle is comming to an end
        /// </summary>
        protected IHostInfoOutputPort hostInfoOutputPort;

        /// <summary>
        /// Stores the GeneratorMappingEntries
        /// </summary>
        protected GeneratorMappingManager generatorMappingMgr;

        /// <summary>
        /// Applies the equation the user has specified for each generator to generated events.
        /// </summary>
        protected MssMsgProcessor mssMsgProcessor;


        public MssEventGenerator()
        {
            this.mssMsgProcessor = new MssMsgProcessor();
        }

        public void Init(IHostInfoOutputPort hostInfoOutputPort, 
                         IWetMssEventOutputPort wetMssEventOutputPort, 
                         IDryMssEventInputPort dryMssEventInputPort,
                         GeneratorMappingManager generatorMappingMgr)
        {
            this.generatorMappingMgr = generatorMappingMgr;
            this.mssMsgProcessor.Init(generatorMappingMgr);

            //Adds listener for generator toggle messages
            wetMssEventOutputPort.WetMssEventsReceived += new 
                    WetMssEventsReceivedEventHandler(WetMssEventOutputPort_WetMssEventsReceived);

            this.hostInfoOutputPort = hostInfoOutputPort;
            hostInfoOutputPort.BeforeProcessingCycleEnd += new
                    ProcessingCycleEndEventHandler(HostInfoOutputPort_BeforeProcessingCycleEnd);
            
            this.dryMssEventInputPort = dryMssEventInputPort;
        }

        /// <summary>
        /// Listens for GeneratorToggle messages
        /// </summary>
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

        /// <summary>
        /// Iterates through the entries in generatorMappingManager, creates all nessary Mss 
        /// Events, and send them to dryMssEventInputPort. This function will be called before 
        /// each audio processing cycle ends. 
        /// </summary>
        /// <param name="cycleEndTimestamp">
        /// The timestamp for when the current processing cycle will end
        /// </param>
        protected void HostInfoOutputPort_BeforeProcessingCycleEnd(long cycleEndTimestamp)
        {
            int numGens = this.generatorMappingMgr.GetNumEntries();
            for (int i = 0; i < numGens; i++)
            {
                IGeneratorMappingEntry curEntry = 
                        this.generatorMappingMgr.GetGenMappingEntryByIndex(i);

                //In order to generate beat synced events we need to some information about the 
                //host. If any of this information hasn't been initialized yet then just don't 
                //generate anything for this generator.
                if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
                {
                    if (this.hostInfoOutputPort.TempoIsInitialized == false ||
                        this.hostInfoOutputPort.TimeSignatureIsInitialized == false ||
                        this.hostInfoOutputPort.BarPosIsInitialized == false ||
                        this.hostInfoOutputPort.TransportPlayingIsInitialized == false)
                    {
                        continue;
                    }

                    if (this.hostInfoOutputPort.TransportPlaying == false)
                    {
                        continue;
                    }
                }

                //ticksPerUpdate stores the number of ticks in between each update
                int ticksPerUpdate = GetTicksPerGenUpdate(curEntry.GenConfigInfo.PeriodType);

                if (curEntry.GenConfigInfo.Enabled == true)
                {
                    if (curEntry.GenHistoryInfo.Initialized == false)
                    {
                        //The GenHistoryInfo will be initialized such that it appears to have been
                        //updated on the last update.
                        long lastUpdateTimestamp = cycleEndTimestamp - ticksPerUpdate;

                        if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
                        {
                            curEntry.GenHistoryInfo.InitAllMembers(
                                lastUpdateTimestamp,
                                GetPercentThroughBeatSyncedPeriod(curEntry.GenConfigInfo, lastUpdateTimestamp),
                                double.NaN);
                        }
                        else if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
                        {
                            curEntry.GenHistoryInfo.InitAllMembers(
                                lastUpdateTimestamp,
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

        protected MssEvent GenerateEvent(IGeneratorMappingEntry genEntry)
        {
            Debug.Assert(genEntry.GenHistoryInfo.Initialized == true);

            int ticksPerUpdate = GetTicksPerGenUpdate(genEntry.GenConfigInfo.PeriodType);

            double relPosInPeriod;
            if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                relPosInPeriod = GetPercentThroughBeatSyncedPeriod(genEntry.GenConfigInfo,
                        genEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp + ticksPerUpdate);
            }
            else if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
            {
                int periodSizeInTicks = GetPeriodSizeInTicks(genEntry.GenConfigInfo);
                double RelativeperiodIncrement = ((double)ticksPerUpdate) / ((double)periodSizeInTicks);
                relPosInPeriod = genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate +
                                        RelativeperiodIncrement;
            }
            else
            {
                //Unexpected period type
                Debug.Assert(false);
                return null;
            }

            if (relPosInPeriod > 1)
            {
                relPosInPeriod--;
            }

            MssMsg relPosMsg = CreateInputMsgForGenMappingEntry(genEntry, relPosInPeriod);
            List<MssMsg> processedMsgList = this.mssMsgProcessor.ProcessMssMsg(relPosMsg);

            long GenUpdateTimestamp = genEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp +
                                      ticksPerUpdate;
            genEntry.GenHistoryInfo.LastGeneratorUpdateTimestamp = GenUpdateTimestamp;
            genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate = relPosInPeriod;
            
            //Count could equal 0 if data 3 has been mapped above 1.
            Debug.Assert(processedMsgList.Count <= 1);

            if (processedMsgList.Count == 0 || 
                processedMsgList[0].Data3 == genEntry.GenHistoryInfo.LastValueSent)
            {
                return null;
            }
            else
            {
                MssEvent generatedEvent = new MssEvent();
                generatedEvent.mssMsg = processedMsgList[0];
                generatedEvent.timestamp = GenUpdateTimestamp;

                genEntry.GenHistoryInfo.LastValueSent = generatedEvent.mssMsg.Data3;

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

            return (int)System.Math.Round((1.0 / updatesPerSecond) * (int)System.TimeSpan.TicksPerSecond);
        }

        protected int GetPeriodSizeInTicks(GenEntryConfigInfo genConfigInfo)
        {
            if (genConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                Debug.Assert(hostInfoOutputPort.BarPosIsInitialized == true);
                Debug.Assert(hostInfoOutputPort.TimeSignatureIsInitialized == true);
                
                double periodSizeInBars =
                    genConfigInfo.GetSizeOfBarsPeriod(hostInfoOutputPort.TimeSignatureNumerator,
                                                      hostInfoOutputPort.TimeSignatureDenominator);

                double timeSig = (double)hostInfoOutputPort.TimeSignatureNumerator / hostInfoOutputPort.TimeSignatureDenominator;
                double beatsPerBar = timeSig / (1/4d);
                double secondsPerBar = (beatsPerBar / hostInfoOutputPort.Tempo) * 60;
                return (int)System.Math.Round(
                    secondsPerBar * periodSizeInBars * System.TimeSpan.TicksPerSecond);
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

        protected MssMsg CreateInputMsgForGenMappingEntry(IGeneratorMappingEntry genEntry, 
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

        //Preconditions: genInfo refers to a beat synced generator. BarsPerTimestampTick and 
        //TimeSignature have been initialized in hostInfoOutputPort
        protected double GetPercentThroughBeatSyncedPeriod(GenEntryConfigInfo genInfo, 
                                                              long timestamp)
        {
            Debug.Assert(genInfo.PeriodType == GenPeriodType.BeatSynced);
            Debug.Assert(hostInfoOutputPort.BarPosIsInitialized == true);
            Debug.Assert(hostInfoOutputPort.TimeSignatureIsInitialized == true);


            double barPos = hostInfoOutputPort.GetBarPosAtTimestamp(timestamp);
            double periodSizeInBars = 
                genInfo.GetSizeOfBarsPeriod(hostInfoOutputPort.TimeSignatureNumerator,
                                            hostInfoOutputPort.TimeSignatureDenominator);
            return barPos % periodSizeInBars;
        }
    }
}
