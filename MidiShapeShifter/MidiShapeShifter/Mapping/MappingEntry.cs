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

        public MssMsgInfo inMssMsgInfo;
        public MssMsgInfo outMssMsgInfo;

        //If there are multiple mapping entries with overlapping input ranges then a single midi message can 
        //generate several messages. This can be disabled by setting the override duplicates flag to true
        public bool overrideDuplicates;

        //specifies the order in which the override duplicates flags are considered. For example if there are two
        //mapping entries with an overlapping inMsgRange and overrideDuplicates is set to true, then the one 
        //with a priority closer to 0 will override the other. This priority also corresponds to the index of an 
        //entry in the listbox it is displayed in. Each entry should have a unique priority.
        public int priority;

        public EquationInputMode inputMode;
        public string equation;
        public int presetIndex;
        public double[] presetParamValues = new double[4];

        public MappingEntry() 
        { 
        
        }

        public string GetReadableMsgType(IO ioCategory)
        {
            if (ioCategory == IO.Input)
            {
                return MssMsgUtil.MssMsgTypeNames[(int)this.inMssMsgInfo.mssMsgType];
            }
            else if (ioCategory == IO.Output)
            {
                return MssMsgUtil.MssMsgTypeNames[(int)this.inMssMsgInfo.mssMsgType];
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
            if (this.overrideDuplicates == true)
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
