using System;
using System.IO;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

using MidiShapeShifter.CSharpUtil;

using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This class is responsible for serializing and deserializing any information required to store
    /// the state of the plugin. All state information should be de/serialized from one object graph 
    /// using Csharp's serialization framework. 
    /// </summary>
    /// <typeparam name="SerializableRootType">
    /// Specifies type of the root object of the object graph used to store state information. This 
    /// will probably always be MssComponentHub as it is the root object for most of the MSS namespace.
    /// </typeparam>
    /// <remarks>
    /// The intended use of the IVstPluginPersistence calss is to persist the state of every program.
    /// However due to some drawbacks to that approach this class only persists the state of the active 
    /// program. The state of unactive programs are saved in .mssp files in the settings folder.
    /// </remarks>
    public class VstPluginPersistence<SerializableRootType> : IVstPluginPersistence
    {
        public delegate void PluginDeserializedEventHandler(SerializableRootType deserializedRoot);
        //triggered after deserialization has finished. This event is ment to give casses in the 
        //Framework namespace a chance to link up with the new calsses in the MSS namespace. It is also
        //used to overwrite the old instance of the serializable root which should be stored in Plugin.
        public event PluginDeserializedEventHandler PluginDeserialized;

        public delegate void BeforePluginDeserializedEventHandler();
        public event BeforePluginDeserializedEventHandler BeforePluginDeserialized;


        //Gets the object that will be serialized and deserialized.
        protected Func<SerializableRootType> getSerializableRootFromPlugin;

        protected Func<MssProgramMgr> getMssProgramMgr;

        protected PluginPrograms pluginPrograms;

        public VstPluginPersistence()
        { 
            
        }

        /// <summary>
        /// Initialized the class
        /// </summary>
        /// <param name="getSerializableRootFromPlugin">
        /// Gets the object that will be serialized and deserialized
        /// </param>
        public void Init(Func<SerializableRootType> getSerializableRootFromPlugin,
                         PluginPrograms pluginPrograms,
                         Func<MssProgramMgr> getMssProgramMgr)
        {
            this.getSerializableRootFromPlugin = getSerializableRootFromPlugin;
            this.pluginPrograms = pluginPrograms;

            this.getMssProgramMgr = getMssProgramMgr;
            AttachHandlersToMssProgramMgrEvents();
        }

        protected void AttachHandlersToMssProgramMgrEvents()
        {
            this.getMssProgramMgr().SaveProgramRequest += 
                new SaveProgramRequestEventHandler(WritePluginState);
            this.getMssProgramMgr().LoadProgramRequest +=
                new LoadProgramRequestEventHandler(ReadPluginState);
        }

        public void OnMssProgramMgrInstanceReplaced()
        {
            AttachHandlersToMssProgramMgrEvents();
        }

        public bool CanLoadChunk(VstPatchChunkInfo chunkInfo)
        {
            return true;
        }

        /// <summary>
        /// Loads the state of the plugin. This is likely to be called when the user changes the 
        /// active program.
        /// </summary>
        /// <param name="stream">
        /// Stream containing an object graph which contains the state of the program. The root of 
        /// this object graph should have the same type specified by SerializableRootType
        /// </param>
        public void ReadPluginState(Stream stream)
        {
            ReadPrograms(stream, null);
        }

        /// <summary>
        /// Loads the state of the plugin and initializes the the programs. In most hosts, this will 
        /// be called when the plugin first loads.
        /// </summary>
        /// <param name="stream">
        /// Stream containing an object graph which contains the state of the program. The root of 
        /// this object graph should have the same type specified by SerializableRootType
        /// </param>
        /// <param name="programs">This collection must be populated with new VstPrograms unless it 
        /// is null.</param>
        public void ReadPrograms(Stream stream, VstProgramCollection programs)
        {
            if (BeforePluginDeserialized != null)
            {
                BeforePluginDeserialized();
            }

            SerializableRootType deserializedRoot;
            deserializedRoot = ContractSerializer.Deserialize<SerializableRootType>(stream);

            if (programs != null)
            {
                programs.AddRange(this.pluginPrograms.CreatePrograms());
            }

            if (PluginDeserialized != null)
            {
                PluginDeserialized(deserializedRoot);
            }
        }

        /// <summary>
        /// Saves the state of the plugin to the stream. This should be called when the user wants to 
        /// create a new program.
        /// </summary>
        public void WritePluginState(Stream stream)
        {
            WritePrograms(stream, null);
        }

        /// <summary>
        /// Saves the state of the plugin to the stream. This will likely be called by a host when
        /// saving a file containing this plugin.
        /// </summary>
        public void WritePrograms(Stream stream, VstProgramCollection programs)
        {
            ContractSerializer.Serialize<SerializableRootType>(stream, getSerializableRootFromPlugin());
        }
    }
}
