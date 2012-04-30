using System;
namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public interface IFactory_MssMsgInfo
    {
        IMssMsgInfo Create(MidiShapeShifter.Mss.MssMsgType msgInfoType);
        void Init(MidiShapeShifter.Mss.Generator.IGeneratorMappingManager genMappingMgr);
    }
}
