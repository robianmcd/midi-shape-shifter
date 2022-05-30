using MidiShapeShifter.Mss.Parameters;
using System;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class ParameterMsgInfo : MssMsgInfo
    {
        protected IMssParameterViewer paramViewer;

        public void Init(IMssParameterViewer paramViewer)
        {
            this.paramViewer = paramViewer;
        }

        public override MssMsgType MsgType
        {
            get { return MssMsgType.Parameter; }
        }

        public override string ConvertData1ToString(double Data1)
        {
            if (Enum.IsDefined(typeof(MssParameterID), (int)Data1))
            {
                MssParamInfo paramInfo = this.paramViewer.GetParameterInfoCopy((MssParameterID)Data1);
                return paramInfo.Name;
            }
            else
            {
                return "";
            }
        }

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }

        public override string ConvertData3ToString(double Data3)
        {
            return Math.Round(Data3 * 100).ToString() + "%";
        }
    }
}
