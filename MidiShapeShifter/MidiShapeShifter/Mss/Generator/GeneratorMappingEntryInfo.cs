using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Generator
{
    public enum GenPeriodType { Beats, Time };
    public enum GenBarsPeriod { Bars32, Bars24, Bars16, Bars12, Bars8, Bars6, Bars4, Bars2, 
        Bars1Point5, Bars1, DottedHalf, Half, DottedQuarter, Quarter, QuarterTriplet, DottedEighth, 
        Eighth, EighthTriplet, Sixteenth, SixteenthTriplet, ThirtySecond };

    // TODO: comment this calss
    public class GeneratorMappingEntryInfo : ICloneable
    {
        public const int NUM_GEN_PERIOD_TYPES = 2;
        public static readonly List<string> GenPeriodTypeNames = new List<string>(NUM_GEN_PERIOD_TYPES);

        public const int NUM_GEN_BARS_PERIOD = 20;
        public static readonly List<string> GenBarsPeriodNames = new List<string>(NUM_GEN_BARS_PERIOD);
        public static readonly List<double> GenBarsPeriodValues = new List<double>(NUM_GEN_BARS_PERIOD);

        //Static Constructro
        static GeneratorMappingEntryInfo()
        {
            GenPeriodTypeNames.Insert((int)GenPeriodType.Beats, "Beats");
            GenPeriodTypeNames.Insert((int)GenPeriodType.Time, "Time");


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
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedHalf, 1/2d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Half, 1/2d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedQuarter, 1/4d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Quarter, 1/4d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.QuarterTriplet, 1/4d * 2/3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.DottedEighth, 1 / 8d * 1.5);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Eighth, 1 / 8d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.EighthTriplet, 1/8d * 2/3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.Sixteenth, 1/16d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.SixteenthTriplet, 1/16d * 2/3d);
            GenBarsPeriodValues.Insert((int)GenBarsPeriod.ThirtySecond, 1/32d);
        }
        public const string DEFAULT_NAME = "Untitled";
        public const GenPeriodType DEFAULT_PERIOD_TYPE = GenPeriodType.Beats;
        public const int DEFAULT_TIME_PERIOD = 100;
        public const GenBarsPeriod DEFAULT_BARS_PERIOD = GenBarsPeriod.Bars1;
        public const bool DEFAULT_LOOP = true;
        public const bool DEFAULT_IS_GENERATING = true;

        public int Id;
        public string Name;
        public GenPeriodType PeriodType;
        public int TimePeriod;
        public GenBarsPeriod BarsPeriod;
        public bool Loop;
        public bool IsGenerating;

        public void InitWithDefaultValues()
        {
            this.Name = DEFAULT_NAME;
            this.PeriodType = DEFAULT_PERIOD_TYPE;
            this.TimePeriod = DEFAULT_TIME_PERIOD;
            this.BarsPeriod = DEFAULT_BARS_PERIOD;
            this.Loop = DEFAULT_LOOP;
            this.IsGenerating = DEFAULT_IS_GENERATING;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
