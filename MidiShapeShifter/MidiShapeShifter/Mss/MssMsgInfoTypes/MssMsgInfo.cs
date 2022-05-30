using System.Diagnostics;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    //See IMssMsgInfo for more doc.
    public abstract class MssMsgInfo : IMssMsgInfo
    {
        public abstract string ConvertData1ToString(double Data1);
        public abstract string ConvertData2ToString(double Data2);
        public abstract string ConvertData3ToString(double Data3);

        public abstract MssMsgType MsgType { get; }

        private IStaticMssMsgInfo _staticInfo = null;
        protected IStaticMssMsgInfo staticInfo
        {
            get
            {
                if (_staticInfo == null)
                {
                    _staticInfo = Factory_StaticMssMsgInfo.Create(this.MsgType);

                    //Ensure the right type of static info is being used for this class.
                    Debug.Assert(_staticInfo.MsgType == this.MsgType);
                }
                return _staticInfo;
            }
        }

        //Static info wrappers

        public string Data1Name => staticInfo.Data1Name;

        public string Data2Name => staticInfo.Data2Name;

        public string Data3Name => staticInfo.Data3Name;

        public string GetDataFieldName(MssMsgDataField field)
        {
            return staticInfo.GetDataFieldName(field);
        }

        public double MaxData1Value => staticInfo.MaxData1Value;

        public double MaxData2Value => staticInfo.MaxData2Value;

        public double MaxData3Value => staticInfo.MaxData3Value;

        public double MinData1Value => staticInfo.MinData1Value;

        public double MinData2Value => staticInfo.MinData2Value;

        public double MinData3Value => staticInfo.MinData3Value;


        public void ApplyPreMappingQueryProcessing(MssMsg msgToProcess)
        {
            staticInfo.ApplyPreMappingQueryProcessing(msgToProcess);
        }

        public void ApplyPreProcessing(MssMsg msgToProcess)
        {
            staticInfo.ApplyPreProcessing(msgToProcess);
        }

        public void ApplyPostProcessing(MssMsg preProcessedMsg, MssMsg msgToProcess)
        {
            staticInfo.ApplyPostProcessing(preProcessedMsg, msgToProcess);
        }

        public bool TypeIsInRange(MssMsgType msgType)
        {
            return staticInfo.TypeIsInRange(msgType);
        }
    }
}
