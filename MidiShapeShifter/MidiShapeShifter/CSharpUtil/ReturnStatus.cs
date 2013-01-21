namespace MidiShapeShifter.CSharpUtil
{
    public enum ValidStatus { Valid, Invalid }

    public interface IReturnStatus<out ReturnType> {
        ReturnType Value {get;}
        bool IsValid {get;}
    }

    public interface IReturnStatus<out ReturnType, out StatusType>
    {
        ReturnType Value {get;}
        StatusType Status {get;}
    }

    public class ReturnStatus<ReturnType> : IReturnStatus<ReturnType>
    {
        public ReturnType Value { get; set;}
        public bool IsValid { get; set; }

        public ReturnStatus(ReturnType returnVal, bool returnValIsValid)
        {
            this.Value = returnVal;
            this.IsValid = returnValIsValid;
        }

        public ReturnStatus(ReturnType validReturnVal)
            : this(validReturnVal, true)
        {
            //Create a valid return status.
        }

        public ReturnStatus()
            : this(default(ReturnType), false)
        {
            //Create an invalid return status.
        }
    }

    public class ReturnStatus<ReturnType, StatusType> : IReturnStatus<ReturnType, StatusType>
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
