using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    //See IMssMsgInfo for more doc.
    [DataContract]
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

        public string Data1Name
        {
            get { return staticInfo.Data1Name; }
        }

        public string Data2Name
        {
            get { return staticInfo.Data2Name; }
        }

        public string Data3Name
        {
            get { return staticInfo.Data3Name; }
        }

        public string GetDataFieldName(MssMsgDataField field)
        {
            return staticInfo.GetDataFieldName(field);
        }

        public double MaxData1Value
        {
            get { return staticInfo.MaxData1Value; }
        }

        public double MaxData2Value
        {
            get { return staticInfo.MaxData2Value; }
        }

        public double MaxData3Value
        {
            get { return staticInfo.MaxData3Value; }
        }

        public double MinData1Value
        {
            get { return staticInfo.MinData1Value; }
        }

        public double MinData2Value
        {
            get { return staticInfo.MinData2Value; }
        }

        public double MinData3Value
        {
            get { return staticInfo.MinData3Value; }
        }
    }
}
