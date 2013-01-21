using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.CSharpUtil
{
    public class XyPoint<CoordinateType> : ICloneable
    {
        public CoordinateType X { get; set; }
        public CoordinateType Y { get; set; }
        
        public XyPoint()
        { 
            
        }

        public XyPoint(CoordinateType x, CoordinateType y)
        {
            this.X = x;
            this.Y = y;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        //This will only create a deep copy if CoordinateType is a value type.
        XyPoint<CoordinateType> Clone()
        {
            return (XyPoint<CoordinateType>)this.MemberwiseClone();
        }
    }
}
