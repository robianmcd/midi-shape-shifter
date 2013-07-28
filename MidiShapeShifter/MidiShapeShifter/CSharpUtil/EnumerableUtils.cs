using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.CSharpUtil
{
    public static class EnumerableUtils
    {
        public static string ToString<T>(IEnumerable<T> collection) {
            if (collection == null) {
                return "NULL";
            }
            else if (!collection.Any())
            {
                return "[]";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("['");

                sb.Append(string.Join("', '", collection.ToArray()));

                sb.Append("']");

                return sb.ToString();
            }

        }
    }
}
