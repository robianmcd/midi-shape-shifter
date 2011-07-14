using System.Collections.Generic;
using System.Diagnostics;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    ///     Receives MIDI messages from the host (through the jacobi framework), converts them into MSS messages and 
    ///     sends them off to the IDryMssEventInputPort. This class is also responcible for receiving MSS messages from 
    ///     the IWetMssEventOutputPort and sending them out to the host as MIDI.
    /// </summary>
    public class MidiHandler : IVstMidiProcessor, IVstPluginMidiSource
    {
        //Temporarily stores VstEvents that are waiting to be sent to the host.
        protected VstEventCollection outEvents = new VstEventCollection();

        //Allows this class to send MssEvents to the DryMssEventRelay
        protected IDryMssEventInputPort dryMssEventInputPort;
        //Allows this class to receive MssEvents from the WetMssEventRelay
        protected IWetMssEventOutputPort wetMssEventOutputPort;
        //Allows this class to receive host information from the HostInfoRelay
        protected IHostInfoOutputPort hostInfoOutputPort;

        //ProcessingCycleStartTime is not set until the end of the first processing cycle so it will be set to
        //UNINITIALIZED_CYCLE_START_TIME before that.
        protected const long UNINITIALIZED_CYCLE_START_TIME = -1;
        //The start time of the current processing cycle in ticks.
        protected long ProcessingCycleStartTime = UNINITIALIZED_CYCLE_START_TIME;

        protected IVstHost vstHost;

        public MidiHandler()
        {
        }

        public void Init(IDryMssEventInputPort dryMssEventInputPort, IWetMssEventOutputPort wetMssEventOutputPort, IHostInfoOutputPort hostInfoOutputPort)
        {
            //Connects up this class to the required relays.

            this.dryMssEventInputPort = dryMssEventInputPort;

            this.wetMssEventOutputPort = wetMssEventOutputPort;
            this.wetMssEventOutputPort.SendingWetMssEvents += new SendingWetMssEventsEventHandler(IWetMssEventOutputPort_SendingWetMssEvents);

            this.hostInfoOutputPort = hostInfoOutputPort;
        }

        //This cannot be done during Init() because the IVstHost is still null
        public void InitVstHost(IVstHost vstHost)
        {
            this.vstHost = vstHost;
        }

        public int ChannelCount
        {
            get { return 16; }
        }

        /// <summary>
        /// Midi events are received from the host on this method.
        /// </summary>
        /// <param name="events">A collection with midi events. Never null.</param>
        /// <remarks>
        /// Note that some hosts will only receieve midi events during audio processing.
        /// See also <see cref="IVstPluginAudioProcessor"/>.
        /// </remarks>
        public void Process(VstEventCollection vstEvents)
        {
            if (vstEvents == null || vstEvents.Count == 0 || this.vstHost == null) return;

            // a plugin must implement IVstPluginMidiSource or this call will throw an exception.
            IVstMidiProcessor midiHost = this.vstHost.GetInstance<IVstMidiProcessor>();

            // always expect some hosts not to support this.
            if (midiHost != null)
            {

                // NOTE: other types of events could be in the collection!
                foreach (VstEvent evnt in vstEvents)
                {
                    if (evnt.EventType == VstEventTypes.MidiEvent)
                    {
                        VstMidiEvent midiEvent = (VstMidiEvent)evnt;
                        MssEvent mssEvent = ConvertVstMidiEventToMssEvent(midiEvent);

                        if (mssEvent == null)
                        {
                            outEvents.Add(evnt);
                        }
                        else 
                        {
                            this.dryMssEventInputPort.ReceiveDryMssEvent(mssEvent);
                        }
                    }
                    else
                    {
                        // non VstMidiEvent
                        outEvents.Add(evnt);
                    }
                }
            }
        }

        /// <summary>
        ///     Receives processed MssEvents from the IWetMssEventOutputPort.
        /// </summary>
        /// <param name="mssEvents">List of processed mss events</param>
        /// <param name="processingCycleEndTimeInTicks">
        ///     If wetMssEventOutputPort is configured to only output when a processing cycle ends then 
        ///     processingCycleEndTimeInTicks will contain the end time the cycle that just ended. 
        /// </param>
        public void IWetMssEventOutputPort_SendingWetMssEvents(List<MssEvent> mssEvents, long processingCycleEndTimeInTicks)
        { 
            if (this.vstHost == null) {
                return;
            }

            // a plugin must implement IVstPluginMidiSource or this call will throw an exception.
            IVstMidiProcessor midiHost = this.vstHost.GetInstance<IVstMidiProcessor>();

            // always expect some hosts not to support this.
            if (midiHost != null)
            {

                //Attempts to convert each MssEvent to a VstMidiEvent and add it to outEvents
                foreach (MssEvent mssEvent in mssEvents)
                {
                    //This will return null if there is no valid conversion.
                    VstMidiEvent midiEvent = ConvertMssEventToVstMidiEvent(mssEvent);
                    if (midiEvent != null)
                    {
                        this.outEvents.Add(midiEvent);
                    }
                }

                //Sends VstMidiEvents to host
                midiHost.Process(outEvents);
                outEvents.Clear();
            }

            OnProcessingCycleEnd(processingCycleEndTimeInTicks);
        }


        /// <summary>
        ///     Handles any operations that need to happen at the end of a processing cycle and prepares this class for 
        ///     the next processing cycle. This method should not be called until all processing associated with the 
        ///     ending cycle has finished.
        /// </summary>
        public void OnProcessingCycleEnd(long previousProcessingCycleEndTimeInTicks)
        { 
            //Sets the time that the next processing cycle will start
            ProcessingCycleStartTime = previousProcessingCycleEndTimeInTicks;
        }

        /// <summary>
        ///     Attempts to create an MssEvent representation of <paramref name="midiEvent"/>.
        /// </summary>
        /// <returns>The MssEvent representation of midiEvent or null of there is no valid conversion.</returns>
        protected MssEvent ConvertVstMidiEventToMssEvent(VstMidiEvent midiEvent)
        {
            MssEvent mssEvent = new MssEvent();

            //Sets the timestamp for mssEvent.
            //This will only be true if the very first processing cycle has not ended yet
            if (this.ProcessingCycleStartTime == UNINITIALIZED_CYCLE_START_TIME)
            {
                //If we cannot calculate the timestamp then a good approximation for it is the current time
                mssEvent.timestamp = System.DateTime.Now.Ticks;
            }
            else
            {
                //the sample rate should be initialized when ProcessingCycleStartTime is initialized.
                Debug.Assert(this.hostInfoOutputPort.SampleRateIsInitialized == true);

                //Calculates mssEvent's timestamp.
                long ticksOffset = ConvertSamplesToTicks(midiEvent.DeltaFrames, this.hostInfoOutputPort.SampleRate);
                mssEvent.timestamp = this.ProcessingCycleStartTime + ticksOffset;
            }

            MssMsgType msgType = GetMssTypeFromMidiData(midiEvent.Data);
            mssEvent.mssMsg.Type = msgType;

            //If msgType is "Unsupported" then midiEvent cannot be represented as an MssEvent
            if (msgType == MssMsgType.Unsupported)
            {
                return null;
            }

            //Sets mssEvent's Data1 (midi channel).
            //Adds one because channels in an MssMsg start at 1 but channels in a VstMidiEvent start at 0
            mssEvent.mssMsg.Data1 = ((int)midiEvent.Data[0] & 0x0F) + 1;

            //Sets mssEvent's Data2 and Data3 (the midi message's data bytes).
            if (msgType == MssMsgType.PitchBend)
            {
                //The two data bytes for pitch bend are used to store one 14-bit number so this number is stored in mssEvent.Data3 and mssEvent.Data2 is not used.
                mssEvent.mssMsg.Data2 = 0;
                //TODO: this might not work because each byte is preceeded by a 0
                mssEvent.mssMsg.Data3 = System.BitConverter.ToInt32(midiEvent.Data, 1/*skips the status byte*/);
            }
            else
            {
                mssEvent.mssMsg.Data2 = (int)midiEvent.Data[1];
                mssEvent.mssMsg.Data3 = (int)midiEvent.Data[2];
            }

            return mssEvent;

        }

        /// <summary>
        ///     Attempts to create a VstMidiEvent representation of <paramref name="mssEvent"/>.
        /// </summary>
        /// <returns>The VstMidiEvent representation of mssEvent or null of there is no valid conversion.</returns>
        protected VstMidiEvent ConvertMssEventToVstMidiEvent(MssEvent mssEvent)
        {
            Debug.Assert(mssEvent.timestamp >= this.ProcessingCycleStartTime);

            //Calculate delta frames
            long ticksElapsed = mssEvent.timestamp - this.ProcessingCycleStartTime;
            Debug.Assert(this.hostInfoOutputPort.SampleRateIsInitialized == true);
            int deltaFrames = ConvertTicksToSamples(ticksElapsed, this.hostInfoOutputPort.SampleRate);

            byte[] midiData = new byte[3];

            //Get status half of first byte.
            byte statusByte;

            if (GetStatusFromMssMsgType(mssEvent.mssMsg.Type, out statusByte) == false)
            {
                return null;
            }

            //subtract 1 becasue channels are 0 based in midi but 1 based in mss
            byte channelByte = (byte)(mssEvent.mssMsg.Data1 - 1);

            Debug.Assert(MssMsgUtil.isValidChannel(mssEvent.mssMsg.Data1));
            //TODO: add assertions for data2 and data3

            midiData[0] = (byte) (statusByte | channelByte);
            //TODO: add special case for pitch bend
            midiData[1] = (byte)mssEvent.mssMsg.Data2;
            midiData[2] = (byte)mssEvent.mssMsg.Data3;

            VstMidiEvent midiEvent = new VstMidiEvent(deltaFrames, 0, 0, midiData, 0, 0);

            return midiEvent;
        }

        protected int ConvertTicksToSamples(long ticks, double sampleRate)
        {
            double samplesPerTick = sampleRate / (double)System.TimeSpan.TicksPerSecond;

            return (int)System.Math.Round(ticks * samplesPerTick);
        }

        protected long ConvertSamplesToTicks(int samples, double sampleRate)
        {
            double ticksPerSample = (double)System.TimeSpan.TicksPerSecond / sampleRate;

            return (long)System.Math.Round(samples * ticksPerSample);
        }

        protected MssMsgType GetMssTypeFromMidiData(byte[] midiData)
        {
            //anding 0xF0 gets rid if the second half of the byte which contains the channel.
            switch (midiData[0] & 0xF0)
            {
                case 0x80:
                    return MssMsgType.NoteOff;
                case 0x90:
                    return MssMsgType.NoteOn;
                case 0xA0:
                    return MssMsgType.PolyAftertouch;
                case 0xB0:
                    return MssMsgType.CC;
                case 0xC0:
                    //Program change messages are not supported
                    return MssMsgType.Unsupported;
                case 0xD0:
                    return MssMsgType.ChanAftertouch;
                case 0xE0:
                    return MssMsgType.PitchBend;
                default:
                    Debug.Assert(false);
                    return MssMsgType.Unsupported;
            }
        }

        protected bool GetStatusFromMssMsgType(MssMsgType mssMsgType, out byte statusByte)
        {
            bool validConversionExists;
            statusByte = 0x00;

            switch (mssMsgType)
            {
                case MssMsgType.NoteOff:
                    validConversionExists = true;
                    statusByte = 0x80;
                    break;
                case MssMsgType.NoteOn:
                    validConversionExists = true;
                    statusByte = 0x90;
                    break;
                case MssMsgType.PolyAftertouch:
                    validConversionExists = true;
                    statusByte = 0xA0;
                    break;
                case MssMsgType.CC:
                    validConversionExists = true;
                    statusByte = 0xB0;
                    break;
                case MssMsgType.ChanAftertouch:
                    validConversionExists = true;
                    statusByte = 0xD0;
                    break;
                case MssMsgType.PitchBend:
                    validConversionExists = true;
                    statusByte = 0xE0;
                    break;
                default:
                    validConversionExists = false;
                    break;
            }

            return validConversionExists;
        }

        #region IVstPluginMidiSource Members

        int IVstPluginMidiSource.ChannelCount
        {
            get { return 16; }
        }

        #endregion
    }
}
