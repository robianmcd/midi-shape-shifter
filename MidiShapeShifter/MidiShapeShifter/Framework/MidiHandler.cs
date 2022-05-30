using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using MidiShapeShifter.Midi;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Relays;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        private Func<IDryMssEventInputPort> getDryMssEventInputPort;
        protected IDryMssEventInputPort dryMssEventInputPort
        {
            get { return this.getDryMssEventInputPort(); }
        }

        //Allows this class to receive MssEvents from the WetMssEventRelay
        private Func<IWetMssEventOutputPort> getWetMssEventOutputPort;
        protected IWetMssEventOutputPort wetMssEventOutputPort
        {
            get { return this.getWetMssEventOutputPort(); }
        }

        //Allows this class to receive host information from the HostInfoRelay
        private Func<IHostInfoOutputPort> getHostInfoOutputPort;
        protected IHostInfoOutputPort hostInfoOutputPort
        {
            get { return this.getHostInfoOutputPort(); }
        }


        //The sample time at the start of the current processing cycle.
        protected long SampleTimeAtStartOfProcessingCycle = 0;

        protected IVstHost vstHost;

        public MidiHandler()
        {
        }

        public void Init(Func<IDryMssEventInputPort> getDryMssEventInputPort,
                         Func<IWetMssEventOutputPort> getWetMssEventOutputPort,
                         Func<IHostInfoOutputPort> getHostInfoOutputPort)
        {
            //Connects up this class to the required relays.
            this.getDryMssEventInputPort = getDryMssEventInputPort;
            this.getWetMssEventOutputPort = getWetMssEventOutputPort;
            this.getHostInfoOutputPort = getHostInfoOutputPort;

            AttachHandlersToRelayEvents();
        }

        protected void AttachHandlersToRelayEvents()
        {
            this.wetMssEventOutputPort.SendingWetMssEvents +=
                new SendingWetMssEventsEventHandler(IWetMssEventOutputPort_SendingWetMssEvents);
        }

        public void OnRelayInstancesReplaced()
        {
            //When the plugin is deserialized a new instance of the classes in the MSS namespace 
            //will be created. These classes will assume that the sample time starts at 0.
            SampleTimeAtStartOfProcessingCycle = 0;
            AttachHandlersToRelayEvents();
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
        ///     Midi events are received from the host on this method and are either send to the DryMssEventRelay or 
        ///     stored internally.
        /// </summary>
        /// <param name="events">A collection with midi events. Never null.</param>
        /// <remarks>
        ///     Note that some hosts will only receieve midi events during audio processing.
        ///     See also <see cref="IVstPluginAudioProcessor"/>.
        /// </remarks>
        public void Process(VstEventCollection vstEvents)
        {
            if (vstEvents == null || vstEvents.Count == 0 || this.vstHost == null) return;

            // NOTE: other types of events could be in the collection!
            foreach (VstEvent evnt in vstEvents)
            {
                if (evnt.EventType == VstEventTypes.MidiEvent)
                {
                    VstMidiEvent midiEvent = (VstMidiEvent)evnt;
                    MssEvent mssEvent = ConvertVstMidiEventToMssEvent(midiEvent,
                                                                      this.SampleTimeAtStartOfProcessingCycle,
                                                                      this.hostInfoOutputPort.SampleRate);

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

        /// <summary>
        ///     Receives processed MssEvents from the IWetMssEventOutputPort.
        /// </summary>
        /// <param name="mssEvents">List of processed mss events</param>
        /// <param name="sampleTimeAtEndOfProcessingCycle">
        ///     If wetMssEventOutputPort is configured to only output when a processing cycle ends then 
        ///     sampleTimeAtEndOfProcessingCycle will contain the end time the cycle that just ended. 
        /// </param>
        public void IWetMssEventOutputPort_SendingWetMssEvents(List<MssEvent> mssEvents,
            long sampleTimeAtEndOfProcessingCycle)
        {
            if (this.vstHost == null)
            {
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
                    VstMidiEvent midiEvent =
                        ConvertMssEventToVstMidiEvent(mssEvent,
                                                      this.SampleTimeAtStartOfProcessingCycle,
                                                      this.hostInfoOutputPort.SampleRate);
                    if (midiEvent != null)
                    {
                        this.outEvents.Add(midiEvent);
                        //midiHost.Process(outEvents);
                        //outEvents.Clear();
                    }
                }

                //TODO: Figure out why it doesn't work to send all of the events at once.
                //Sends VstMidiEvents to host
                midiHost.Process(outEvents);
                outEvents.Clear();
            }

            OnProcessingCycleEnd(sampleTimeAtEndOfProcessingCycle);
        }


        /// <summary>
        ///     Handles any operations that need to happen at the end of a processing cycle and prepares this class for 
        ///     the next processing cycle. This method should not be called until all processing associated with the 
        ///     ending cycle has finished.
        /// </summary>
        public void OnProcessingCycleEnd(long sampleTimeAtEndOfLastProcessingCycle)
        {
            //Sets the sample time that the next processing cycle will start
            SampleTimeAtStartOfProcessingCycle = sampleTimeAtEndOfLastProcessingCycle + 1;
        }

        /// <summary>
        ///     Attempts to create an MssEvent representation of <paramref name="midiEvent"/>.
        /// </summary>
        /// <returns>The MssEvent representation of midiEvent or null of there is no valid conversion.</returns>
        protected static MssEvent ConvertVstMidiEventToMssEvent(VstMidiEvent midiEvent,
                                                         long sampleTimeAtStartOfProcessingCycle,
                                                         double sampleRate)
        {
            MssEvent mssEvent = new MssEvent();

            //Sets the sample time for mssEvent.
            mssEvent.sampleTime = sampleTimeAtStartOfProcessingCycle + midiEvent.DeltaFrames;

            MssMsgType msgType = MidiUtil.GetMssTypeFromMidiData(midiEvent.Data);
            mssEvent.mssMsg.Type = msgType;

            //If msgType is "Unsupported" then midiEvent cannot be represented as an MssEvent
            if (msgType == MssMsgType.Unsupported)
            {
                return null;
            }

            //Sets mssEvent's Data1 (midi channel).
            //Adds one because channels in an MssMsg start at 1 but channels in a VstMidiEvent start at 0
            mssEvent.mssMsg.Data1 = (midiEvent.Data[0] & 0x0F) + 1;

            //Sets mssEvent's Data2 and Data3 (the midi message's data bytes).
            if (msgType == MssMsgType.PitchBend)
            {
                //The two data bytes for pitch bend are used to store one 14-bit number so this number is stored in 
                //mssEvent.Data3 and mssEvent.Data2 is not used.
                mssEvent.mssMsg.Data2 = MssMsgUtil.UNUSED_MSS_MSG_DATA;

                //data1 contains the least significant 7 bits of the pitch bend value and data2 contains the most 
                //significant 7 bits
                mssEvent.mssMsg.Data3 = (midiEvent.Data[2] << 7) + midiEvent.Data[1];
            }
            else if (msgType == MssMsgType.ChanAftertouch)
            {
                mssEvent.mssMsg.Data2 = MssMsgUtil.UNUSED_MSS_MSG_DATA;
                mssEvent.mssMsg.Data3 = midiEvent.Data[1];
            }
            else
            {
                mssEvent.mssMsg.Data2 = midiEvent.Data[1];
                mssEvent.mssMsg.Data3 = midiEvent.Data[2];
            }

            return mssEvent;

        }

        /// <summary>
        ///     Attempts to create a VstMidiEvent representation of <paramref name="mssEvent"/>.
        /// </summary>
        /// <returns>The VstMidiEvent representation of mssEvent or null of there is no valid conversion.</returns>
        protected static VstMidiEvent ConvertMssEventToVstMidiEvent(MssEvent mssEvent,
                                                                    long sampleTimeAtStartOfProcessingCycle,
                                                                    double sampleRate)
        {
            Debug.Assert(mssEvent.sampleTime >= sampleTimeAtStartOfProcessingCycle);

            //Calculate delta frames
            int deltaFrames = (int)(mssEvent.sampleTime - sampleTimeAtStartOfProcessingCycle);

            byte[] midiData = new byte[3];

            //Get status half of first byte.
            byte statusByte;

            if (MidiUtil.GetStatusFromMssMsgType(mssEvent.mssMsg.Type, out statusByte) == false)
            {
                return null;
            }

            //subtract 1 becasue channels are 0 based in midi but 1 based in mss
            byte channelByte = (byte)(mssEvent.mssMsg.Data1AsInt - 1);

            midiData[0] = (byte)(statusByte | channelByte);

            if (mssEvent.mssMsg.Type == MssMsgType.PitchBend)
            {
                //most significant bits
                int MsbVal = (mssEvent.mssMsg.Data3AsInt) >> 7;
                midiData[2] = (byte)MsbVal;
                midiData[1] = (byte)((mssEvent.mssMsg.Data3AsInt) - (MsbVal << 7));
            }
            else if (mssEvent.mssMsg.Type == MssMsgType.ChanAftertouch)
            {
                midiData[1] = (byte)mssEvent.mssMsg.Data3AsInt;
                midiData[2] = 0;
            }
            else
            {
                midiData[1] = (byte)mssEvent.mssMsg.Data2AsInt;
                midiData[2] = (byte)mssEvent.mssMsg.Data3AsInt;
            }
            VstMidiEvent midiEvent = new VstMidiEvent(deltaFrames, 0, 0, midiData, 0, 0);

            return midiEvent;
        }

        #region IVstPluginMidiSource Members

        int IVstPluginMidiSource.ChannelCount
        {
            get { return 16; }
        }

        #endregion
    }
}
