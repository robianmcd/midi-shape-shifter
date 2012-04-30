using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{

    //See IStaticMssMsgInfo for more doc.
    public abstract class StaticMssMsgInfo : IStaticMssMsgInfo
    {
        public const string DATA1_NAME_CHANNEL = "Channel";
        public const string DATA1_NAME_GEN_ID = "Generator ID";

        public const string DATA2_NAME_NOTE = "Note Number";

        public const string DATA3_NAME_VELOCITY = "Velocity";
        public const string DATA3_NAME_PERIOD_POSITION = "Position in Period";
        public const string DATA3_NAME_PRESSURE = "Pressure";

        public const string DATA_NAME_UNUSED = "";

        public abstract MssMsgType MsgType { get; }

        public abstract double MaxData1Value { get; }
        public abstract double MinData1Value { get; }
        public abstract double MaxData2Value { get; }
        public abstract double MinData2Value { get; }
        public abstract double MaxData3Value { get; }
        public abstract double MinData3Value { get; }

        public abstract string Data1Name { get; }
        public abstract string Data2Name { get; }
        public abstract string Data3Name { get; }
        public string GetDataFieldName(MssMsgDataField field)
        {
            if (field == MssMsgDataField.Data1)
            {
                return this.Data1Name;
            }
            else if (field == MssMsgDataField.Data2)
            {
                return this.Data2Name;
            }
            else if (field == MssMsgDataField.Data3)
            {
                return this.Data3Name;
            }
            else
            {
                //unknown MssMsgDataField
                Debug.Assert(false);
                return "";
            }
        }
    }
}
