﻿using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// The Plugin root object.
    /// </summary>
    public class Plugin : VstPluginWithInterfaceManagerBase
    {
        //TODO: Register an actual code for this
        private static readonly int UniquePluginId = new FourCharacterCode("1132").ToInt32();
        private static readonly string PluginName = "Midi Shape Shifter";
        private static readonly string ProductName = "Midi Shape Shifter";
        private static readonly string VendorName = "SpeqSoft";
        private static readonly int PluginVersion = 1;

        public MssComponentHub MssHub;
        protected VstParameters vstParameters;

        /// <summary>
        /// Initializes the one an only instance of the Plugin root object.
        /// </summary>
        public Plugin()
            : base(PluginName,
            new VstProductInfo(ProductName, VendorName, PluginVersion),
                VstPluginCategory.Effect,
                VstPluginCapabilities.NoSoundInStop,
                // initial delay: number of samples your plugin lags behind.
                0,
                UniquePluginId)
        {
            this.MssHub = new MssComponentHub();
            this.MssHub.Init();

            this.vstParameters = new VstParameters(this);
            this.vstParameters.Init(this.MssHub);
        }

        /// <summary>
        /// Gets the audio processor object.
        /// </summary>
        public DummyAudioHandler AudioHandler
        {
            get { return GetInstance<DummyAudioHandler>(); }
        }

        /// <summary>
        /// Gets the midi processor object.
        /// </summary>
        public MidiHandler MidiHandler
        {
            get { return GetInstance<MidiHandler>(); }
        }

        /// <summary>
        /// Gets the plugin editor object.
        /// </summary>
        public PluginEditor PluginEditor
        {
            get { return GetInstance<PluginEditor>(); }
        }

        /// <summary>
        /// Gets the plugin programs object.
        /// </summary>
        public PluginPrograms PluginPrograms
        {
            get { return GetInstance<PluginPrograms>(); }
        }

        /// <summary>
        /// Implement this when you do audio processing.
        /// </summary>
        /// <param name="instance">A previous instance returned by this method. 
        /// When non-null, return a thread-safe version (or wrapper) for the object.</param>
        /// <returns>Returns null when not supported by the plugin.</returns>
        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (!MidiHandler.SyncWithAudioProcessor) return null;

            if (instance == null)
            {
                return new DummyAudioHandler(this);
            }

            return base.CreateAudioProcessor(instance); 
        }

        /// <summary>
        /// Implement this when you do midi processing.
        /// </summary>
        /// <param name="instance">A previous instance returned by this method. 
        /// When non-null, return a thread-safe version (or wrapper) for the object.</param>
        /// <returns>Returns null when not supported by the plugin.</returns>
        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null)
            {
                return new MidiHandler(this);
            }

            return base.CreateMidiProcessor(instance);
        }

        /// <summary>
        /// Implement this when you output midi events to the host.
        /// </summary>
        /// <param name="instance">A previous instance returned by this method. 
        /// When non-null, return a thread-safe version (or wrapper) for the object.</param>
        /// <returns>Returns null when not supported by the plugin.</returns>
        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            // we implement this interface on out midi processor.
            return (IVstPluginMidiSource)MidiHandler;
        }

        /// <summary>
        /// Implement this when you need a custom editor (UI).
        /// </summary>
        /// <param name="instance">A previous instance returned by this method. 
        /// When non-null, return a thread-safe version (or wrapper) for the object.</param>
        /// <returns>Returns null when not supported by the plugin.</returns>
        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null)
            {
                return new PluginEditor(this);
            }

            return base.CreateEditor(instance);
        }

        /// <summary>
        /// Implement this when you implement plugin programs or presets.
        /// </summary>
        /// <param name="instance">A previous instance returned by this method. 
        /// When non-null, return a thread-safe version (or wrapper) for the object.</param>
        /// <returns>Returns null when not supported by the plugin.</returns>
        protected override IVstPluginPrograms CreatePrograms(IVstPluginPrograms instance)
        {
            if (instance == null)
            {
                return new PluginPrograms(this);
            }

            return base.CreatePrograms(instance);
        }
    }
}
