using System.Diagnostics;

namespace MidiShapeShifter.CSharpUtil
{
    public static class CustomMathUtils
    {
        public static double AbsToRelVal(double min, double max, double absVal)
        {
            Debug.Assert(min < max);
            Debug.Assert(absVal <= max);
            Debug.Assert(absVal >= min);
            return (absVal - min) / (max - min);
        }

        public static double RelToAbsVal(double min, double max, double relVal)
        {
            Debug.Assert(min < max);
            Debug.Assert(relVal <= 1);
            Debug.Assert(relVal >= 0);
            return relVal * (max - min) + min;
        }
    }
}
