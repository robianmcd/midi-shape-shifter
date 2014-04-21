using System;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;
using System.Collections.Generic;

namespace MidiShapeShifter.Mss
{
    public interface IMssMsgProcessor
    {
        void Init(IBaseGraphableMappingManager mappingMgr, IMssParameterViewer mssParameters);
        IEnumerable<MssMsg> ProcessMssMsg(MssMsg mssMsg);
    }
}
