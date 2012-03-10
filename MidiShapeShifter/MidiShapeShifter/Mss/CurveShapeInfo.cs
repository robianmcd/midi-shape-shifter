using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.CSharpUtil;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss
{

    public enum EquationType { Curve, Point };    


    /// <summary>
    ///     Contains information about a curve's shape and how it is being entered.
    /// </summary>
    [Serializable]
    public class CurveShapeInfo
    {
        public const string DEFAULT_EQUATION = "input";
        public const int DEFAULT_PRESET_INDEX = -1;
        public const double DEFAULT_PRESET_VALUE = 0;

        /// <summary>
        ///     This field is obsolete. In old version it was used to map an MSS message's data3
        /// </summary>
        [Obsolete("This field is obsolete. Use CurveEquations instead.", false)]
        public string Equation = "";

        /// <summary>
        ///     This list contains the equation for each line segment.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        public List<string> CurveEquations;

        /// <summary>
        ///     This list contains the coordinates for each user control point. A coordinate can 
        ///     be specified by an equation but the euqation cannot contain variables like x, y or 
        ///     z. The equations can however contain variables that can always be evaluated to a 
        ///     number like a, b or c.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        public List<XyPoint<string>> PointEquations;

        /// <summary>
        /// The index of the currently selected equation. This index corresponds to an index in the 
        /// list CurveEquations or the list PointEquations.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        public int SelectedEquationIndex;

        /// <summary>
        /// Specifies which list the index specified in SelectedEquationIndex referes to.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        public EquationType SelectedEquationType;

        /// <summary>
        /// Specifies the primary input field. E.G. if this cass was for a velocity curve then this 
        /// field would be Data3.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        public MssMsgDataField PrimaryInputSource;

        /// <summary>
        ///     Index if the selected preset
        /// </summary>
        public int PresetIndex;

        /// <summary>
        ///     Values of the preset knobs
        /// </summary>
        public double[] PresetParamValues;

        /// <summary>
        ///     Stores the most up to date version of this class. This should be incrimented every 
        ///     time a serializable field is added or removed.
        /// </summary>
        private const int CURRENT_SERIALIZATION_VERSION = 2;

        /// <summary>
        /// This field is used while deserializing to check if the instance being deserialized is 
        /// up to date. 
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        private int version;

        public CurveShapeInfo()
        {
            this.version = CURRENT_SERIALIZATION_VERSION;
        }

        public void InitWithDefaultValues()
        {
            InitOptionalFieldsWithDefaultValues();

            this.CurveEquations.Add(DEFAULT_EQUATION);

            this.PresetIndex = DEFAULT_PRESET_INDEX;
            this.PresetParamValues = new double[4] { DEFAULT_PRESET_VALUE, DEFAULT_PRESET_VALUE, 
                                                            DEFAULT_PRESET_VALUE, DEFAULT_PRESET_VALUE };
        }

        private void InitOptionalFieldsWithDefaultValues()
        {
            this.CurveEquations = new List<string>();
            this.PointEquations = new List<XyPoint<string>>();

            this.SelectedEquationIndex = 0;
            this.SelectedEquationType = EquationType.Curve;

            this.PrimaryInputSource = MssMsgDataField.Data3;
        }

        [OnDeserializing]
        private void BeforeDeserializing(StreamingContext sc)
        {
            InitOptionalFieldsWithDefaultValues();
        }

        [OnDeserialized]
        private void AfterDeserializing(StreamingContext sc)
        {
            //If version is less then 2 then the eqiation is stored in the obselete Equation member 
            //variable.
            if (this.version < 2)
            {
                
                //Disable the warning message about using the obsolete field "Equation"
                #pragma warning disable 618
                this.CurveEquations.Add(this.Equation);
                #pragma warning restore 618


                this.SelectedEquationIndex = 0;
                this.SelectedEquationType = EquationType.Curve;
            }


            this.version = CURRENT_SERIALIZATION_VERSION;
        }
    }
}
