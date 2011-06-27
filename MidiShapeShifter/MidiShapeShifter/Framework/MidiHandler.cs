using System.Collections.Generic;
using System.Diagnostics;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object performs midi processing for your plugin.
    /// </summary>
    public class MidiHandler : IVstMidiProcessor, IVstPluginMidiSource
    {
        protected Plugin _plugin;

        protected VstEventCollection outEvents = new VstEventCollection();


        public MidiHandler(Plugin plugin)
        {
            _plugin = plugin;
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
            if (vstEvents == null || vstEvents.Count == 0) return;

            // a plugin must implement IVstPluginMidiSource or this call will throw an exception.
            IVstMidiProcessor midiHost = _plugin.Host.GetInstance<IVstMidiProcessor>();

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
                            this._plugin.MssHub.HandleIncomingMssEvent(mssEvent);
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

        /*public void ProcessCurrentEvents()
        {
            IVstHostSequencer midiHostSeq = _plugin.Host.GetInstance<IVstHostSequencer>();
            VstTimeInfoFlags defaultVstTimeFlags = VstTimeInfoFlags.AutomationReading |
                                                   VstTimeInfoFlags.AutomationWriting |
                                                   VstTimeInfoFlags.BarStartPositionValid |
                                                   VstTimeInfoFlags.ClockValid |
                                                   VstTimeInfoFlags.CyclePositionValid |
                                                   VstTimeInfoFlags.NanoSecondsValid |
                                                   VstTimeInfoFlags.PpqPositionValid |
                                                   VstTimeInfoFlags.SmpteValid |
                                                   VstTimeInfoFlags.TempoValid |
                                                   VstTimeInfoFlags.TimeSignatureValid |
                                                   VstTimeInfoFlags.TransportChanged |
                                                   VstTimeInfoFlags.TransportCycleActive |
                                                   VstTimeInfoFlags.TransportPlaying |
                                                   VstTimeInfoFlags.TransportRecording;

            VstTimeInfo timeInfo = midiHostSeq.GetTime(defaultVstTimeFlags);            
        }*/

        public void OnAudioProcessingCycleEnd(double cycleEndTimestampInMs)
        {
            // a plugin must implement IVstPluginMidiSource or this call will throw an exception.
            IVstMidiProcessor midiHost = _plugin.Host.GetInstance<IVstMidiProcessor>();

            // always expect some hosts not to support this.
            if (midiHost != null)
            {

                List<MssEvent> mssEventsForHost = this._plugin.MssHub.TransferMssEventsForHost();

                foreach(MssEvent mssEvent in mssEventsForHost)
                {
                    VstMidiEvent midiEvent = ConvertMssEventToVstMidiEvent(mssEvent, cycleEndTimestampInMs);
                    if (midiEvent != null)
                    {
                        this.outEvents.Add(midiEvent);
                    }
                }

                midiHost.Process(outEvents);
                outEvents.Clear();
            }
        }

        //Can return null if there is no valid conversion
        protected MssEvent ConvertVstMidiEventToMssEvent(VstMidiEvent midiEvent)
        {
            MssEvent mssEvent = new MssEvent();
            mssEvent.timestamp = TimeUtil.GetTimestampInMs();

            MssMsgType msgType = GetMssTypeFromMidiData(midiEvent.Data);
            mssEvent.mssMsg.Type = msgType;

            if (msgType == MssMsgType.Unsupported)
            {
                return null;
            }

            //add one because channels start at 1
            mssEvent.mssMsg.Data1 = ((int)midiEvent.Data[0] & 0x0F) + 1;

            if (msgType == MssMsgType.PitchBend)
            {
                //this might not work because each byte is preceeded by a 0
                mssEvent.mssMsg.Data3 = System.BitConverter.ToInt32(midiEvent.Data, 1/*skips the status byte*/);
            }
            else
            {
                mssEvent.mssMsg.Data2 = (int)midiEvent.Data[1];
                mssEvent.mssMsg.Data3 = (int)midiEvent.Data[2];
            }

            return mssEvent;

        }

        //Can return null if there is no valid conversion
        protected VstMidiEvent ConvertMssEventToVstMidiEvent(MssEvent mssEvent, double cycleEndTimestampInMs)
        {
            Debug.Assert(mssEvent.timestamp <= cycleEndTimestampInMs);

            //TODO: convert ms into samples
            int deltaFrames = (int)(cycleEndTimestampInMs - mssEvent.timestamp);

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
