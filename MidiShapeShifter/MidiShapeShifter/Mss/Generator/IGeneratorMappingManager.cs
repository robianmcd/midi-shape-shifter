using System;
using MidiShapeShifter.Mss.Mapping;
using System.Windows.Forms;
using System.Collections.Generic;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Generator
{
    public interface IGeneratorMappingManager : IGraphableMappingManager<IGeneratorMappingEntry>
    {
        int CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo);
        bool UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo, int id);
    }
}
