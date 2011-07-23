namespace MidiShapeShifter.CSharpUtil
{
    public class ReturnStatus<ReturnType>
    {
        public ReturnType ReturnVal { get; protected set;}
        public bool IsValid { get; protected set; }

        public ReturnStatus(ReturnType returnVal, bool returnValIsValid)
        {
            this.ReturnVal = returnVal;
            this.IsValid = returnValIsValid;
        }

        public ReturnStatus()
        {
            this.IsValid = false;
        }
    }
}
