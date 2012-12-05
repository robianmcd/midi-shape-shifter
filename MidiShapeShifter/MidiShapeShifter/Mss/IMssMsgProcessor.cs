using System;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss
{
    public interface IMssMsgProcessor
    {
        void Init(IBaseGraphableMappingManager mappingMgr, IMssParameterViewer mssParameters);
        System.Collections.Generic.List<MssMsg> ProcessMssMsg(MssMsg mssMsg);
    }
}
