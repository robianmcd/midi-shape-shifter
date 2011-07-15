using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Midi
{
    public class MidiUtil
    {
        public const int MIN_MIDI_CHAN_VAL = 1;
        public const int MAX_MIDI_CHAN_VAL = 16;
        public const int MIN_MIDI_PARAM_VAL = 0;
        public const int MAX_MIDI_PARAM_VAL = 127;
        //16384 is the max 0 based 14 bit number
        public const int MAX_LARGE_MIDI_PARAM_VAL = 16384;
    }
}
