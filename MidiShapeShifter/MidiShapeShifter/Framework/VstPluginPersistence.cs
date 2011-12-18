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
    class VstPluginPersistence : IVstPluginPersistence
    {
        protected Plugin plugin;

        public VstPluginPersistence()
        { 
            
        }

        public void Init(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public bool CanLoadChunk(VstPatchChunkInfo chunkInfo)
        {
            return true;
        }

        public void ReadPrograms(Stream stream, VstProgramCollection programs)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new DeserializationBinderForPlugins();
            this.plugin.MssHub = (MssComponentHub)formatter.Deserialize(stream);
            this.plugin.PluginEditor.OnDeserialized();
        }

        public void WritePrograms(Stream stream, VstProgramCollection programs)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this.plugin.MssHub);
        }
    }
}
