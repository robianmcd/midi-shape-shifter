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
        protected const int SAMPLES_PER_GENERATOR_UPDATE = 600;

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
        /// <param name="sampleTimeAtEndOfCycle">
        /// The sample time for when the current processing cycle will end
        /// </param>
        protected void HostInfoOutputPort_BeforeProcessingCycleEnd(long sampleTimeAtEndOfCycle)
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
                        this.hostInfoOutputPort.CalculatedBarZeroIsInitialized == false ||
                        this.hostInfoOutputPort.TransportPlayingIsInitialized == false)
                    {
                        continue;
                    }

                    if (this.hostInfoOutputPort.TransportPlaying == false)
                    {
                        curEntry.GenHistoryInfo.Initialized = false;
                        continue;
                    }
                }

                if (curEntry.GenConfigInfo.Enabled == true)
                {
                    if (curEntry.GenHistoryInfo.Initialized == false)
                    {
                        //The GenHistoryInfo will be initialized such that it appears to have been
                        //updated on the last update.
                        long sampleTimeOfLastUpdate = 
                                sampleTimeAtEndOfCycle - SAMPLES_PER_GENERATOR_UPDATE;

                        curEntry.GenHistoryInfo.InitAllMembers(
                                sampleTimeOfLastUpdate,
                                0,
                                double.NaN);
                    }

                    while (curEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate + 
                           SAMPLES_PER_GENERATOR_UPDATE <= sampleTimeAtEndOfCycle && 
                           curEntry.GenConfigInfo.Enabled == true)
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

            double relPosInPeriod;
            if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                relPosInPeriod = GetPercentThroughBeatSyncedPeriod(genEntry.GenConfigInfo,
                        genEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate + 
                        SAMPLES_PER_GENERATOR_UPDATE);
            }
            else if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
            {
                int periodSizeInSamples = GetPeriodSizeInSamples(genEntry.GenConfigInfo);
                double RelativeperiodIncrement = 
                        ((double)SAMPLES_PER_GENERATOR_UPDATE) / ((double)periodSizeInSamples);
                relPosInPeriod = genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate +
                                        RelativeperiodIncrement;
                if (relPosInPeriod > 1)
                {
                    relPosInPeriod--;                    
                }
            }
            else
            {
                //Unexpected period type
                Debug.Assert(false);
                return null;
            }

                if (genEntry.GenConfigInfo.Loop == false && 
                    relPosInPeriod < genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate)
                {
                    genEntry.GenConfigInfo.Enabled = false;
                    genEntry.GenHistoryInfo.Initialized = false;
                    return null;
                }

            MssMsg relPosMsg = CreateInputMsgForGenMappingEntry(genEntry, relPosInPeriod);
            List<MssMsg> processedMsgList = this.mssMsgProcessor.ProcessMssMsg(relPosMsg);

            long updatedSampleTime = genEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate +
                                      SAMPLES_PER_GENERATOR_UPDATE;
            genEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate = updatedSampleTime;
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
                generatedEvent.sampleTime = updatedSampleTime;

                genEntry.GenHistoryInfo.LastValueSent = generatedEvent.mssMsg.Data3;

                return generatedEvent;
            }
        }

        protected int GetPeriodSizeInSamples(GenEntryConfigInfo genConfigInfo)
        {
            if (genConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                Debug.Assert(hostInfoOutputPort.CalculatedBarZeroIsInitialized == true);
                Debug.Assert(hostInfoOutputPort.TimeSignatureIsInitialized == true);
                Debug.Assert(hostInfoOutputPort.SampleRateIsInitialized == true);

                double periodSizeInBars =
                    genConfigInfo.GetSizeOfBarsPeriod(hostInfoOutputPort.TimeSignatureNumerator,
                                                      hostInfoOutputPort.TimeSignatureDenominator);

                double timeSig = (double)hostInfoOutputPort.TimeSignatureNumerator / hostInfoOutputPort.TimeSignatureDenominator;
                double beatsPerBar = timeSig / (1/4d);
                double secondsPerBar = (beatsPerBar / hostInfoOutputPort.Tempo) * 60;
                return (int)System.Math.Round(
                    secondsPerBar * periodSizeInBars * hostInfoOutputPort.SampleRate);
            }
            else if (genConfigInfo.PeriodType == GenPeriodType.Time)
            {
                Debug.Assert(hostInfoOutputPort.SampleRateIsInitialized == true);

                return (int)System.Math.Round(
                    (genConfigInfo.TimePeriodInMs / 1000d) * hostInfoOutputPort.SampleRate);
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

        //Preconditions: genInfo refers to a beat synced generator. CalculatedBarZero and 
        //TimeSignature have been initialized in hostInfoOutputPort
        protected double GetPercentThroughBeatSyncedPeriod(GenEntryConfigInfo genInfo, 
                                                              long sampleTime)
        {
            Debug.Assert(genInfo.PeriodType == GenPeriodType.BeatSynced);
            Debug.Assert(hostInfoOutputPort.CalculatedBarZeroIsInitialized == true);
            Debug.Assert(hostInfoOutputPort.TimeSignatureIsInitialized == true);


            double barPos = hostInfoOutputPort.GetBarPosAtSampleTime(sampleTime);
            double periodSizeInBars = 
                genInfo.GetSizeOfBarsPeriod(hostInfoOutputPort.TimeSignatureNumerator,
                                            hostInfoOutputPort.TimeSignatureDenominator);
            return barPos % periodSizeInBars / periodSizeInBars;
        }
    }
}
