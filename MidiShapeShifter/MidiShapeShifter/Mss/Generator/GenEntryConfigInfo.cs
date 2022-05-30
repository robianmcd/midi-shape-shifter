using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Generator
{

    /// <summary>
    /// If a generator uses a time based period then it will generate even when the transport is 
    /// stopped and it's period will be a fixed amount of time.
    /// 
    /// A bar based generator is the same as a time based generator except the that amount of time 
    /// in a period will be calculated from a number of bars or beats.
    /// 
    /// Beat Synced generators are not currently supported so a user will not be able to select them.
    /// If a generator uses a beat synced period then it will only generate while the transport is 
    /// playing. Also the size of the period will change with the tempo and the start of the period
    /// will be synced with a bar position.
    /// </summary>
    public enum GenPeriodType { Time, Bars, BeatSynced };

    /// <summary>
    /// Specifies the size of a beat synced period.
    /// </summary>
    public enum GenBarsPeriod
    {
        Bars32, Bars24, Bars16, Bars12, Bars8, Bars6, Bars4, Bars2,
        Bars1Point5, Bars1, DottedHalf, Half, DottedQuarter, Quarter, QuarterTriplet, DottedEighth,
        Eighth, EighthTriplet, Sixteenth, SixteenthTriplet, ThirtySecond
    };

    /// <summary>
    /// Stores configuration info about a GeneratorMappingEntry. Most of this information is
    /// specified by the user with the GeneratorDlg
    /// </summary>
    [DataContract]
    public class GenEntryConfigInfo : ICloneable
    {
        public const int NUM_GEN_PERIOD_TYPES = 2;
        /// <summary>
        /// Stores string representation of the GenPeriodType types.
        /// </summary>
        public static readonly List<string> GenPeriodTypeNames = new List<string>(NUM_GEN_PERIOD_TYPES);

        public const int NUM_GEN_BARS_PERIOD = 20;
        /// <summary>
        /// Sores string representations of the GenBarsPeriod types.
        /// </summary>
        public static readonly List<string> GenBarsPeriodNames = new List<string>(NUM_GEN_BARS_PERIOD);
        /// <summary>
        /// Stores the number of bars as a double that each GenBarsPeriod type represents in a 
        /// 4/4 time signature. To get an accurate bar size of a GenBarsPeriod for any time
        /// signature use the function GetSizeOfBarsPeriod().
        /// </summary>
        public static readonly List<double> GenBarsPeriodValues = new List<double>(NUM_GEN_BARS_PERIOD);

        public const string DEFAULT_NAME = "Untitled";
        public const GenPeriodType DEFAULT_PERIOD_TYPE = GenPeriodType.Bars;
        public const int DEFAULT_TIME_PERIOD = 100;
        public const GenBarsPeriod DEFAULT_BARS_PERIOD = GenBarsPeriod.Bars1;
        public const bool DEFAULT_LOOP = true;
        public const bool DEFAULT_ENABLED = true;

        /// <summary>
        /// The name of a generator entry.
        /// </summary>
        [DataMember]
        public string Name;

        /// <summary>
        /// see GenPeriodType definition
        /// </summary>
        [DataMember]
        public GenPeriodType PeriodType;

        /// <summary>
        /// Size of a time period in milliseconds. This is only used if PeriodType is set to Time.
        /// </summary>
        [DataMember]
        public int TimePeriodInMs;

        /// <summary>
        /// Type of a bar based period. This is only used if PeriodType is set to Bars or BeatSynced.
        /// </summary>
        [DataMember]
        public GenBarsPeriod BarsPeriod;

        /// <summary>
        /// Specifies wheather a generator entry will loop back to the start of a period when it 
        /// reaches the end. If this is false then the Enabled property will be set to false once 
        /// the end of the period is reached and the generator entry will stop generating.
        /// </summary>
        [DataMember]
        public bool Loop;

        /// <summary>
        /// Specifies wheather a generator entry can generate or not. GeneratorToggle messages will 
        /// toggle this value.
        /// </summary>
        [DataMember]
        public bool Enabled;

        //Static Constructor
        static GenEntryConfigInfo()
        {
            GenPeriodTypeNames.Insert((int)GenPeriodType.Time, "Time");
            GenPeriodTypeNames.Insert((int)GenPeriodType.Bars, "Bars");

            //Beat synced generators are currently not supported
            //GenPeriodTypeNames.Insert((int)GenPeriodType.BeatSynced, "Beat Synced");


            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars32, "32 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars24, "24 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars16, "16 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars12, "12 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars8, "8 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars6, "6 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars4, "4 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars2, "2 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars1Point5, "1.5 Bars");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Bars1, "1 Bar");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.DottedHalf, "Dotted 1/2");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Half, "1/2");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.DottedQuarter, "Dotted 1/4");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Quarter, "1/4");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.QuarterTriplet, "1/4 Triplet");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.DottedEighth, "Dotted 1/8");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Eighth, "1/8");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.EighthTriplet, "1/8 Triplet");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.Sixteenth, "1/16");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.SixteenthTriplet, "1/16 Triplet");
            GenBarsPeriodNames.Insert((int)GenBarsPeriod.ThirtySecond, "1/32");

            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars32, 32);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars24, 24);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars16, 16);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars12, 12);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars8, 8);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars6, 6);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars4, 4);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars2, 2);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars1Point5, 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Bars1, 1);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedHalf, 1 / 2d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Half, 1 / 2d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedQuarter, 1 / 4d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Quarter, 1 / 4d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.QuarterTriplet, 1 / 4d * 2 / 3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedEighth, 1 / 8d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Eighth, 1 / 8d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.EighthTriplet, 1 / 8d * 2 / 3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Sixteenth, 1 / 16d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.SixteenthTriplet, 1 / 16d * 2 / 3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.ThirtySecond, 1 / 32d);
        }

        public void InitWithDefaultValues()
        {
            this.Name = DEFAULT_NAME;
            this.PeriodType = DEFAULT_PERIOD_TYPE;
            this.TimePeriodInMs = DEFAULT_TIME_PERIOD;
            this.BarsPeriod = DEFAULT_BARS_PERIOD;
            this.Loop = DEFAULT_LOOP;
            this.Enabled = DEFAULT_ENABLED;
        }

        //TODO: Comment
        //Preconditions: The period type is Bars or BeatSynced.
        public double GetSizeOfBarsPeriod(int timeSignatureNumerator, int timeSignatureDenominator)
        {
            Debug.Assert(this.PeriodType == GenPeriodType.Bars || this.PeriodType == GenPeriodType.BeatSynced);

            double barsPeriodVal = GenBarsPeriodValues[(int)this.BarsPeriod];

            if (barsPeriodVal >= 1)
            {
                return barsPeriodVal;
            }
            else
            {
                double timeSig = (double)timeSignatureNumerator / timeSignatureDenominator;
                return barsPeriodVal / (timeSig);
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public GenEntryConfigInfo Clone()
        {
            return (GenEntryConfigInfo)this.MemberwiseClone();
        }
    }
}
