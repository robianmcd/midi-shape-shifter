using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.UI
{
    public enum GraphableEntryType { Mapping, Generator }

    [DataContract]
    public class SerializablePluginEditorInfo
    {
        [DataMember]
        public int ActiveGraphableEntryId = -1;
        [DataMember]
        public GraphableEntryType ActiveGraphableEntryType;
    }
}
