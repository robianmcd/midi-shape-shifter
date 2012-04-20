namespace MidiShapeShifter.CSharpUtil
{
    public enum ValidStatus { Valid, Invalid }

    public class ReturnStatus<ReturnType>
    {
        public ReturnType Value { get; set;}
        public bool IsValid { get; set; }

        public ReturnStatus(ReturnType returnVal, bool returnValIsValid)
        {
            this.Value = returnVal;
            this.IsValid = returnValIsValid;
        }

        public ReturnStatus()
        {
            this.IsValid = false;
        }
    }

    public class ReturnStatus<ReturnType, StatusType>
    {
        public ReturnType Value { get; set; }
        public StatusType Status { get; set; }

        public ReturnStatus(ReturnType returnVal, StatusType returnStatus)
        {
            this.Value = returnVal;
            this.Status = returnStatus;
        }

        //This constructor is intended to be used for invalid return statuses that would not 
        //require a return value.
        public ReturnStatus(StatusType returnStatus)
        {
            this.Status = returnStatus;
        }
    }
}
