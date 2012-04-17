using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.CSharpUtil
{
    public class XyPoint<CoordinateType>
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
    }
}
