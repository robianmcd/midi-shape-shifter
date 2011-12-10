using System;
namespace MidiShapeShifter.Mss
{
    public interface IMssMsgProcessor
    {
        void Init(MidiShapeShifter.Mss.Mapping.IGraphableMappingManager mappingMgr);
        System.Collections.Generic.List<MssMsg> ProcessMssMsg(MssMsg mssMsg);
    }
}
