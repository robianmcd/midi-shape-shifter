using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Midi
{
    public static class MidiUtil
    {
        public const int MIN_MIDI_CHAN_VAL = 1;
        public const int MAX_MIDI_CHAN_VAL = 16;
        public const int MIN_MIDI_PARAM_VAL = 0;
        public const int MAX_MIDI_PARAM_VAL = 127;
        //16384 is the max 0 based 14 bit number
        public const int MAX_LARGE_MIDI_PARAM_VAL = 16384;

        public static bool isValidChannel(int channel)
        {
            return channel >= MIN_MIDI_CHAN_VAL && channel <= MAX_MIDI_CHAN_VAL;
        }

        public static bool isValidParamValue(int value)
        {
            return value >= MIN_MIDI_PARAM_VAL && value <= MAX_MIDI_PARAM_VAL;
        }

        public static int ConvertTicksToSamples(long ticks, double sampleRate)
        {
            double samplesPerTick = sampleRate / (double)System.TimeSpan.TicksPerSecond;

            return (int)System.Math.Round(ticks * samplesPerTick);
        }

        public static long ConvertSamplesToTicks(int samples, double sampleRate)
        {
            double ticksPerSample = (double)System.TimeSpan.TicksPerSecond / sampleRate;

            return (long)System.Math.Round(samples * ticksPerSample);
        }

        /// <summary>
        ///     Get the MssMsgType associated with the message type of <paramref name="midiData"/>. 
        /// </summary>
        /// <param name="midiData"> A byte array representing a midi message</param>
        public static MssMsgType GetMssTypeFromMidiData(byte[] midiData)
        {
            //anding 0xF0 gets rid if the second half of the byte which contains the channel.
            switch (midiData[0] & 0xF0)
            {
                case 0x80:
                    return MssMsgType.NoteOff;
                case 0x90:
                    return MssMsgType.NoteOn;
                case 0xA0:
                    return MssMsgType.PolyAftertouch;
                case 0xB0:
                    return MssMsgType.CC;
                case 0xC0:
                    //Program change messages are not supported
                    return MssMsgType.Unsupported;
                case 0xD0:
                    return MssMsgType.ChanAftertouch;
                case 0xE0:
                    return MssMsgType.PitchBend;
                default:
                    Debug.Assert(false);
                    return MssMsgType.Unsupported;
            }
        }

        /// <summary>
        ///     Converts an mssMsgType into the status byte for a midi message. The channel half of the status byte is 
        ///     left as 0000.
        /// </summary>
        /// <returns>True if a valid conversion exists. False otherwise.</returns>
        public static bool GetStatusFromMssMsgType(MssMsgType mssMsgType, out byte statusByte)
        {
            bool validConversionExists;
            statusByte = 0x00;

            switch (mssMsgType)
            {
                case MssMsgType.NoteOff:
                    validConversionExists = true;
                    statusByte = 0x80;
                    break;
                case MssMsgType.NoteOn:
                    validConversionExists = true;
                    statusByte = 0x90;
                    break;
                case MssMsgType.PolyAftertouch:
                    validConversionExists = true;
                    statusByte = 0xA0;
                    break;
                case MssMsgType.CC:
                    validConversionExists = true;
                    statusByte = 0xB0;
                    break;
                case MssMsgType.ChanAftertouch:
                    validConversionExists = true;
                    statusByte = 0xD0;
                    break;
                case MssMsgType.PitchBend:
                    validConversionExists = true;
                    statusByte = 0xE0;
                    break;
                default:
                    validConversionExists = false;
                    break;
            }

            return validConversionExists;
        }

        public static byte[] CreateMidiData(MssMsgType msgType, int channel, byte byte2, byte byte3)
        {
            Debug.Assert(channel >= 1 && channel <= 16);
            //Ensures the first bit is 0
            Debug.Assert((byte2 >> 7) == 0);
            Debug.Assert((byte3 >> 7) == 0);

            byte[] midiData = new byte[3];
            byte statusByte;
            bool successfullyConvertedType = GetStatusFromMssMsgType(msgType, out statusByte);
            Debug.Assert(successfullyConvertedType);
            statusByte |= (byte)(channel - 1);
            
            midiData[0] = statusByte;
            midiData[1] = byte2;
            midiData[2] = byte3;

            return midiData;
        }
    }
}
