using System;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    public interface IMssMsgProcessor
    {
        void Init(IGraphableMappingManager mappingMgr, IMssParameterViewer mssParameters);
        System.Collections.Generic.List<MssMsg> ProcessMssMsg(MssMsg mssMsg);
    }
}
