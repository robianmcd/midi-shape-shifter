using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

using MidiShapeShifter.CSharpUtil;

using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Framework
{

    class VstPluginPersistence<SerializableRootType> : IVstPluginPersistence
    {
        public delegate void PluginDeserializedEventHandler(SerializableRootType deserializedRoot);
        public event PluginDeserializedEventHandler PluginDeserialized;

        protected Func<SerializableRootType> getSerializableRootFromPlugin;

        public VstPluginPersistence()
        { 
            
        }

        public void Init(Func<SerializableRootType> getSerializableRootFromPlugin)
        {
            this.getSerializableRootFromPlugin = getSerializableRootFromPlugin;
        }

        public bool CanLoadChunk(VstPatchChunkInfo chunkInfo)
        {
            return true;
        }

        public void ReadPrograms(Stream stream, VstProgramCollection programs)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new DeserializationBinderForPlugins();
            SerializableRootType deserializedRoot = (SerializableRootType)formatter.Deserialize(stream);

            if (PluginDeserialized != null)
            {
                PluginDeserialized(deserializedRoot);
            }
        }

        public void WritePrograms(Stream stream, VstProgramCollection programs)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, getSerializableRootFromPlugin());
        }
    }
}
