using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiShapeShifter.CSharpUtil
{
    public static class CustomListUtils
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
