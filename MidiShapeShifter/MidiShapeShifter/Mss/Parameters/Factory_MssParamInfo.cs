using System.Diagnostics;

namespace MidiShapeShifter.Mss.Parameters
{
    public static class Factory_MssParamInfo
    {
        public static MssParamInfo Create(MssParamType paramType, string paramName)
        {
            MssParamInfo paramInfo;

            switch (paramType)
            {
                case MssParamType.Number:
                    {
                        paramInfo = new MssNumberParamInfo();
                        paramInfo.Init(paramName);
                        break;
                    }
                case MssParamType.Waveform:
                    {
                        paramInfo = new MssWaveformParamInfo();
                        paramInfo.Init(paramName);
                        break;
                    }
                case MssParamType.Integer:
                    {
                        paramInfo = new MssIntegerParamInfo();
                        paramInfo.Init(paramName);
                        break;
                    }
                default:
                    {
                        paramInfo = null;
                        //Unknown parameter type
                        Debug.Assert(false);
                        break;
                    }
            }
            return paramInfo;
        }
    }
}
