using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.UI
{
    public enum GraphableEntryType { Mapping, Generator }

    [Serializable]
    public class SerializablePluginEditorInfo
    {
        public int activeGraphableEntryIndex = -1;
        public GraphableEntryType activeGraphableEntryType;
    }
}
