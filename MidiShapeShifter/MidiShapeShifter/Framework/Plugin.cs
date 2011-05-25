using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using MidiShapeShifter.Mapping;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// The Plugin root object.
    /// </summary>
    public class Plugin : VstPluginWithInterfaceManagerBase
    {
        private static readonly int UniquePluginId = new FourCharacterCode("1132").ToInt32();
        private static readonly string PluginName = "Midi Shape Shifter";
        private static readonly string ProductName = "Midi Shape Shifter";
        private static readonly string VendorName = "SpeqSoft";
        private static readonly int PluginVersion = 1;

        public MappingManager mappingManager = new MappingManager();

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
        { }

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

            // TODO: implement a thread-safe wrapper.
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

            // TODO: implement a thread-safe wrapper.
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

            // TODO: implement a thread-safe wrapper.
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
