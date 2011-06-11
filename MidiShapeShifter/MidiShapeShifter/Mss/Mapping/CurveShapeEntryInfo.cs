﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping
{

    /// <summary>
    ///     "Text" means the equation is being input as text. "Preset" means the equation is being generated by the 
    ///     preset knobs.
    /// </summary>
    public enum EquationInputMode { Text, Preset };

    /// <summary>
    ///     Contains information about a curve shape and how it is being entered.
    /// </summary>
    public class CurveShapeEntryInfo
    {
        public const EquationInputMode DEFAULT_EQUATION_INPUT_MODE = EquationInputMode.Text;
        public const string DEFAULT_EQUATION = "x";
        public const int DEFAULT_PRESET_INDEX = -1;
        public const double DEFAULT_PRESET_VALUE = 0;

        /// <summary>
        ///     Specifies wheather the user is entering the equation as text or using the preset knobs to generate it
        /// </summary>
        public EquationInputMode EqInputMode = DEFAULT_EQUATION_INPUT_MODE;

        /// <summary>
        ///     Equation used to map a MSS message's data3
        /// </summary>
        public string Equation = DEFAULT_EQUATION;

        /// <summary>
        ///     Index if the selected preset
        /// </summary>
        public int PresetIndex = DEFAULT_PRESET_INDEX;

        /// <summary>
        ///     Values of the preset knobs
        /// </summary>
        public double[] PresetParamValues = new double[4] { DEFAULT_PRESET_VALUE, DEFAULT_PRESET_VALUE, 
                                                            DEFAULT_PRESET_VALUE, DEFAULT_PRESET_VALUE };

    }
}