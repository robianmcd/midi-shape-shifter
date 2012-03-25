using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Ninject;
using MidiShapeShifter.Ioc;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: write test casses for this calss
    /// <summary>
    /// Before the end of each audio processing cycle this class will iterate through the 
    /// entries in generatorMappingManager, create all nessary Mss Events and send them to 
    /// dryMssEventInputPort
    /// </summary>
    public class MssEventGenerator
    {
        public const int SAMPLES_PER_GENERATOR_UPDATE = 200;

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
        protected IGeneratorMappingManager generatorMappingMgr;

        /// <summary>
        /// Applies the equation the user has specified for each generator to generated events.
        /// </summary>
        protected IMssMsgProcessor mssMsgProcessor;

        /// <summary>
        /// Stores the sample time at the end of the last processing cycle. This should be updated at
        /// the end of the ProcessingCycleEnd event.
        /// </summary>
        protected long sampleTimeAtEndOfLastCycle = 0;

        //Constructor
        public MssEventGenerator()
        {
            this.mssMsgProcessor = IocMgr.Kernel.Get<IMssMsgProcessor>();
        }


        public void Init(IHostInfoOutputPort hostInfoOutputPort, 
                         IWetMssEventOutputPort wetMssEventOutputPort, 
                         IDryMssEventInputPort dryMssEventInputPort,
                         IGeneratorMappingManager generatorMappingMgr,
                         IMssParameterViewer mssParameters)
        {
            this.generatorMappingMgr = generatorMappingMgr;
            this.mssMsgProcessor.Init(generatorMappingMgr, mssParameters);

            //Adds listener for generator toggle messages.
            wetMssEventOutputPort.WetMssEventsReceived += new 
                    WetMssEventsReceivedEventHandler(WetMssEventOutputPort_WetMssEventsReceived);

            this.hostInfoOutputPort = hostInfoOutputPort;
            
            hostInfoOutputPort.BeforeProcessingCycleEnd += new
                    ProcessingCycleEndEventHandler(HostInfoOutputPort_BeforeProcessingCycleEnd);
            hostInfoOutputPort.ProcessingCycleEnd += new
                    ProcessingCycleEndEventHandler(HostInfoOutputPort_ProcessingCycleEnd);
            
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
        /// The sample time for when the current processing cycle will end.
        /// </param>
        protected void HostInfoOutputPort_BeforeProcessingCycleEnd(long sampleTimeAtEndOfCycle)
        {
            int numGens = this.generatorMappingMgr.GetNumEntries();
            for (int i = 0; i < numGens; i++)
            {
                IGeneratorMappingEntry curEntry = 
                        this.generatorMappingMgr.GetGenMappingEntryByIndex(i);

                //In order to generate events we need to some information about the 
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

                    //If the host stopped playing then nothing will be generated for this generator
                    //and it will need to be reinitialized when the host starts playing again.
                    if (this.hostInfoOutputPort.TransportPlaying == false)
                    {
                        curEntry.GenHistoryInfo.Initialized = false;
                        continue;
                    }
                }
                else if (curEntry.GenConfigInfo.PeriodType == GenPeriodType.Time)
                {
                    if (this.hostInfoOutputPort.TimeSignatureIsInitialized == false)
                    {
                        continue;
                    }
                }

                //Only enabled generators should generate anything
                if (curEntry.GenConfigInfo.Enabled == true)
                {
                    //Initializes the history info for this generator if it has not already been
                    //initialized.
                    if (curEntry.GenHistoryInfo.Initialized == false)
                    {
                        //The GenHistoryInfo will be initialized such that it appears to have been
                        //updated on the last processing cycle.
                        curEntry.GenHistoryInfo.InitAllMembers(
                                this.sampleTimeAtEndOfLastCycle,
                                0,
                                double.NaN);
                    }

                    //Generate events for this generator until the next event coming from this 
                    //generator would fall into the next audio processing cycle. The enabled status
                    //of a generator can change in a call to GenerateEvent() so we need to ensure 
                    //that this generator is still enabled.
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

        /// <summary>
        /// Each call to GenerateEvent() will generate the next event for genEntry. The 
        /// next event's sample time will be set to SAMPLES_PER_GENERATOR_UPDATE more then the 
        /// sample time for the event that was previously created for genEntry. Returns null if 
        /// no event currently needs to be generated for this generator.
        /// </summary>
        /// <remarks>
        /// Preconditions: genEntry is initilaized and enabled.
        /// </remarks>
        protected MssEvent GenerateEvent(IGeneratorMappingEntry genEntry)
        {
            Debug.Assert(genEntry.GenConfigInfo.Enabled == true);
            Debug.Assert(genEntry.GenHistoryInfo.Initialized == true);

            //Stores the relative position through the period that the next event for genEntry 
            //will occur.
            double relPosInPeriod;
            if (genEntry.GenConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                relPosInPeriod = GetRelPosInBeatSyncedPeriod(genEntry.GenConfigInfo,
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
                //Remove the interger component of relPosInPeriod so that it is between 0 and 1.
                relPosInPeriod = relPosInPeriod % 1;
            }
            else
            {
                //Unexpected period type
                Debug.Assert(false);
                return null;
            }

            //If this generator is not set to loop and it has finished one full period then disable it
            //it and return null so that no more events are sent.
            if (genEntry.GenConfigInfo.Loop == false && 
                (relPosInPeriod < genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate ||
                GetPeriodSizeInSamples(genEntry.GenConfigInfo) < SAMPLES_PER_GENERATOR_UPDATE))
            {
                genEntry.GenConfigInfo.Enabled = false;
                genEntry.GenHistoryInfo.Initialized = false;
                return null;
            }

            MssMsg relPosMsg = CreateInputMsgForGenMappingEntry(genEntry, relPosInPeriod);
            //Processing the relPosMsg should convert it into a Generator message and apply the 
            //equation for this generator to it's data3.
            List<MssMsg> processedMsgList = this.mssMsgProcessor.ProcessMssMsg(relPosMsg);

            //Sample time for new event.
            long updatedSampleTime = genEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate +
                                      SAMPLES_PER_GENERATOR_UPDATE;
            //Update the generator's history info.
            genEntry.GenHistoryInfo.SampleTimeAtLastGeneratorUpdate = updatedSampleTime;
            genEntry.GenHistoryInfo.PercentThroughPeriodOnLastUpdate = relPosInPeriod;

            //Count could equal 0 if data 3 has been mapped above 1.
            Debug.Assert(processedMsgList.Count <= 1);
            //Don't bother sending the event if it is the same as the last one sent.
            if (processedMsgList.Count == 0 || 
                processedMsgList[0].Data3 == genEntry.GenHistoryInfo.LastValueSent)
            {
                return null;
            }
            else
            {
                //Initialize the fields in the new event and return it.
                MssEvent generatedEvent = new MssEvent();
                generatedEvent.mssMsg = processedMsgList[0];
                generatedEvent.sampleTime = updatedSampleTime;

                genEntry.GenHistoryInfo.LastValueSent = generatedEvent.mssMsg.Data3;

                return generatedEvent;
            }
        }

        /// <summary>
        /// Returns the size of the period for genConfigInfo in samples
        /// </summary>
        /// <remarks>
        /// Preconditions: sample rate must be initialized in hostInfoOutputPort. If
        /// genConfigInfo is for a beat synced generator then the time signature must also be
        /// initialized in hostInfoOutputPort.
        /// </remarks>
        protected int GetPeriodSizeInSamples(GenEntryConfigInfo genConfigInfo)
        {
            if (genConfigInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                Debug.Assert(hostInfoOutputPort.TimeSignatureIsInitialized == true);
                Debug.Assert(hostInfoOutputPort.SampleRateIsInitialized == true);

                double periodSizeInBars =
                    genConfigInfo.GetSizeOfBarsPeriod(hostInfoOutputPort.TimeSignatureNumerator,
                                                      hostInfoOutputPort.TimeSignatureDenominator);

                double timeSig = (double)hostInfoOutputPort.TimeSignatureNumerator /
                                 hostInfoOutputPort.TimeSignatureDenominator;
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

        /// <summary>
        /// Create the relitive position message that genEntry should generate given relPosInPeriod.
        /// This message should match an entry in the GeneratorMappingManager so that when it is
        /// processed by mssMsgProcessor a Generator message will be returned.
        /// </summary>
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

        /// <summary>
        /// Get the relative position into the beat synced period for genInfo at sampleTime
        /// </summary>
        /// <remarks>
        /// Preconditions: genInfo refers to a beat synced generator. CalculatedBarZero and 
        /// TimeSignature have been initialized in hostInfoOutputPort.
        /// </remarks>
        protected double GetRelPosInBeatSyncedPeriod(GenEntryConfigInfo genInfo, 
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

        /// <summary>
        /// Handle the ProcessingCycleEnd event.
        /// </summary>
        protected void HostInfoOutputPort_ProcessingCycleEnd(long sampleTimeAtEndOfCycle)
        {
            this.sampleTimeAtEndOfLastCycle = sampleTimeAtEndOfCycle;
        }

    }
}
