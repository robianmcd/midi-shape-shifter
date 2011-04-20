using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter
{
    public class MappingEntry : ICloneable
    {
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

            if (range.bottomChannel == MidiHelper.RANGE_ALL_INT && range.topChannel == MidiHelper.RANGE_ALL_INT)
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

            if (range.bottomParam == MidiHelper.RANGE_ALL_INT && range.topParam == MidiHelper.RANGE_ALL_INT)
            {
                return MidiHelper.RANGE_ALL_STR;
            }
            else
            {
                //TODO: The binding should also get a property changed notification on the ActiveParameter property, 
                //indicating it should release the old instance and bind to the new.
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
