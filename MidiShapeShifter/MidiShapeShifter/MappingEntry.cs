using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter
{
    public class MappingEntry : ICloneable
    {
        public enum EquationInputMode { Text, Preset };

        public MidiHelper.MidiMsgRange inMsgRange;
        public MidiHelper.MidiMsgRange outMsgRange;

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

        public MappingEntry(int inBottomParam, int inTopParam, int inBottomChannel, int inTopChannel, 
                            MidiHelper.MidiMsgType inMsgType, int outBottomParam, int outTopParam, int outBottomChannel,
                            int outTopChannel, MidiHelper.MidiMsgType outMsgType, int priority, bool overrideDuplicates) 
        {
            this.inMsgRange = new MidiHelper.MidiMsgRange();
            this.inMsgRange.bottomParam = inBottomParam;
            this.inMsgRange.topParam = inTopParam;
            this.inMsgRange.bottomChannel = inBottomChannel;
            this.inMsgRange.topChannel = inTopChannel;
            this.inMsgRange.msgType = inMsgType;

            this.outMsgRange = new MidiHelper.MidiMsgRange();
            this.outMsgRange.bottomParam = outBottomParam;
            this.outMsgRange.topParam = outTopParam;
            this.outMsgRange.bottomChannel = outBottomChannel;
            this.outMsgRange.topChannel = outTopChannel;
            this.outMsgRange.msgType = outMsgType;

            this.priority = priority;
            this.overrideDuplicates = overrideDuplicates;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public MappingEntry Clone()
        {
            return (MappingEntry)this.MemberwiseClone();
        }


        public string GetReadableMsgType(bool input)
        {
            if (input == true)
            {
                return MidiHelper.MidiMsgTypeStr[(int)this.inMsgRange.msgType];
            }
            else
            {
                return MidiHelper.MidiMsgTypeStr[(int)this.outMsgRange.msgType];
            }
        }

        public string GetReadableChannelRange(bool input)
        {
            MidiHelper.MidiMsgRange range;
            if (input == true)
            {
                range = this.inMsgRange;
            }
            else
            {
                range = this.outMsgRange;
            }

            if (range.bottomChannel == MidiHelper.MIN_CHANNEL && range.topChannel == MidiHelper.MAX_CHANNEL)
            {
                return MidiHelper.RANGE_ALL_STR;
            }
            else
            {
                return range.topChannel.ToString() + "-" + range.bottomChannel.ToString();
            }
        }

        public string GetReadableParamRange(bool input)
        {
            MidiHelper.MidiMsgRange range;
            if (input == true)
            {
                range = this.inMsgRange;
            }
            else
            {
                range = this.outMsgRange;
            }

            if (range.bottomParam == MidiHelper.MIN_PARAM && range.topParam == MidiHelper.MAX_PARAM)
            {
                return MidiHelper.RANGE_ALL_STR;
            }
            else
            {
                //TODO: Should display note names if the message type is a note type
                return range.topParam.ToString() + "-" + range.bottomParam.ToString();
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
