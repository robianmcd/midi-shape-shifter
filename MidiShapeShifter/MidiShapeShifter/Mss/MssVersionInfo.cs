using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss
{
    [DataContract]
    public class MssVersionInfo
    {
        public static readonly int CURENT_VERSION = 2;

        [DataMember]
        public int SerializedVersion { get; private set; }

        public MssVersionInfo()
        {
            this.SerializedVersion = CURENT_VERSION;
        }

        [OnSerializing]
        protected void BeforeSerializing(StreamingContext context)
        {
            this.SerializedVersion = CURENT_VERSION;
        }
    }
}
