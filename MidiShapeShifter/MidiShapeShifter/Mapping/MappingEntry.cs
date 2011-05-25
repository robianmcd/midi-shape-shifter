using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping
{
    public class MappingEntry
    {
        public enum EquationInputMode { Text, Preset };
        public enum IO { Input, Output };

        public MssMsgInfo InMssMsgInfo;
        public MssMsgInfo OutMssMsgInfo;

        //If there are multiple mapping entries with overlapping input ranges then a single mss message can 
        //generate several messages. This can be disabled by setting the override duplicates flag to true.
        //If there are two mapping entries with an overlapping inMsgRange and overrideDuplicates is set to 
        //true, then the one closer to the top of the mapping list box overrides the other.
        public bool OverrideDuplicates;

        public EquationInputMode EqInputMode;
        public string Equation;
        public int PresetIndex;
        public double[] PresetParamValues = new double[4];

        public MappingEntry() 
        { 
        
        }

        public string GetReadableMsgType(IO ioCategory)
        {
            if (ioCategory == IO.Input)
            {
                return MssMsgUtil.MssMsgTypeNames[(int)this.InMssMsgInfo.mssMsgType];
            }
            else if (ioCategory == IO.Output)
            {
                return MssMsgUtil.MssMsgTypeNames[(int)this.InMssMsgInfo.mssMsgType];
            }
            else
            {
                //Unknown IO type
                Debug.Assert(false);
                return "";
            }
        }

        public string GetReadableOverrideDuplicates()
        {
            if (this.OverrideDuplicates == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }
    }
}
