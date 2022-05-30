using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public interface IFactory_MssMsgInfo
    {
        IMssMsgInfo Create(MidiShapeShifter.Mss.MssMsgType msgInfoType);
        void Init(IGeneratorMappingManager genMappingMgr, IMssParameterViewer paramViewer);
    }
}
