using System.Collections.Generic;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object performs midi processing for your plugin.
    /// </summary>
    public class MidiHandler : IVstMidiProcessor, IVstPluginMidiSource
    {
        private Plugin _plugin;
        internal MssMsgProcessor processor {get; private set;}

        public MidiHandler(Plugin plugin)
        {
            _plugin = plugin;
            processor = _plugin.MssHub.MssMsgProcessor;

            // for most host midi output is expected during the audio processing cycle.
            SyncWithAudioProcessor = true;
        }

        /// <summary>
        /// Gets or sets a value indicating to sync with audio processing.
        /// </summary>
        /// <remarks>
        /// False: will output midi to the host in the MidiProcessor.
        /// True: will output midi to the host in the AudioProcessor.
        /// </remarks>
        public bool SyncWithAudioProcessor { get; set; }

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
        public void Process(VstEventCollection events)
        {
            CurrentEvents = events;

            if (!SyncWithAudioProcessor)
            {
                ProcessCurrentEvents();
            }
        }

        // cache of events (for when syncing up with the AudioProcessor).
        public VstEventCollection CurrentEvents { get; private set; }

        public void ProcessCurrentEvents()
        {
            IVstHostSequencer midiHostSeq = _plugin.Host.GetInstance<IVstHostSequencer>();
            VstTimeInfo timeInfo = midiHostSeq.GetTime(VstTimeInfoFlags.AutomationReading);
            VstTimeInfo timeInfo2 = midiHostSeq.GetTime(VstTimeInfoFlags.AutomationWriting);
            VstTimeInfo timeInfo3 = midiHostSeq.GetTime(VstTimeInfoFlags.BarStartPositionValid);
            VstTimeInfo timeInfo4 = midiHostSeq.GetTime(VstTimeInfoFlags.ClockValid);
            VstTimeInfo timeInfo5 = midiHostSeq.GetTime(VstTimeInfoFlags.CyclePositionValid);
            VstTimeInfo timeInfo6 = midiHostSeq.GetTime(VstTimeInfoFlags.NanoSecondsValid);
            VstTimeInfo timeInfo7 = midiHostSeq.GetTime(VstTimeInfoFlags.PpqPositionValid);
            VstTimeInfo timeInfo8 = midiHostSeq.GetTime(VstTimeInfoFlags.SmpteValid);
            VstTimeInfo timeInfo9 = midiHostSeq.GetTime(VstTimeInfoFlags.TempoValid);
            VstTimeInfo timeInfo10 = midiHostSeq.GetTime(VstTimeInfoFlags.TimeSignatureValid);
            VstTimeInfo timeInfo11 = midiHostSeq.GetTime(VstTimeInfoFlags.TransportChanged);
            VstTimeInfo timeInfo12 = midiHostSeq.GetTime(VstTimeInfoFlags.TransportCycleActive);
            VstTimeInfo timeInfo13 = midiHostSeq.GetTime(VstTimeInfoFlags.TransportPlaying);
            VstTimeInfo timeInfo14 = midiHostSeq.GetTime(VstTimeInfoFlags.TransportRecording);

            if (CurrentEvents == null || CurrentEvents.Count == 0) return;

            // a plugin must implement IVstPluginMidiSource or this call will throw an exception.
            IVstMidiProcessor midiHost = _plugin.Host.GetInstance<IVstMidiProcessor>();

            // always expect some hosts not to support this.
            if (midiHost != null)
            {
                VstEventCollection outEvents = new VstEventCollection();

                // NOTE: other types of events could be in the collection!
                foreach (VstEvent evnt in CurrentEvents)
                {
                    if (evnt.EventType == VstEventTypes.MidiEvent)
                    {
                        VstMidiEvent midiEvent = (VstMidiEvent)evnt;

                        MssMsg mssMsg = new MssMsg();
                        MssMsgUtil.MssMsgType msgType = MssMsgUtil.GetMssTypeFromMidiData(midiEvent.Data);

                        if (msgType == MssMsgUtil.MssMsgType.Unsupported)
                        {
                            outEvents.Add(evnt);
                            continue;
                        }

                        mssMsg.Data1 = (int) midiEvent.Data[0] & 0x0F;

                        if (msgType == MssMsgUtil.MssMsgType.PitchBend)
                        {
                            mssMsg.Data3 = System.BitConverter.ToInt32(midiEvent.Data, 1/*skips the status byte*/);
                        }
                        else
                        {
                            mssMsg.Data2 = (int)midiEvent.Data[1];
                            mssMsg.Data3 = (int)midiEvent.Data[2];
                        }

                        List<MssMsg> outMessages = new List<MssMsg>();
                        outMessages = processor.ProcessMssMsg(mssMsg);

                        //TODO: convert back into a VstMidiEvent

                        outEvents.Add(midiEvent);
                    }
                    else
                    {
                        // non VstMidiEvent
                        outEvents.Add(evnt);
                    }
                }

                midiHost.Process(outEvents);
            }
        }

        #region IVstPluginMidiSource Members

        int IVstPluginMidiSource.ChannelCount
        {
            get { return 16; }
        }

        #endregion
    }
}
