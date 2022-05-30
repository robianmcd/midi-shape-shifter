using System;

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
        private XyPoint<CoordinateType> Clone()
        {
            return (XyPoint<CoordinateType>)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return "X: " + string.Format("{0}", X) + " Y: " + string.Format("{0}", Y);
        }
    }
}
