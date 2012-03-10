using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    public class MssEvaluatorInput
    {
        public EquationType equationType { get; set; }

        public double varA { get; set; }
        public double varB { get; set; }
        public double varC { get; set; }
        public double varD { get; set; }

        //These fields are only needed if this is input for a curve equation
        public MssMsgDataField PrimaryInputSource { get; set; }

        public double RelData1 { get; set; }
        public double RelData2 { get; set; }
        public double RelData3 { get; set; }

        public List<string> CurveEquations { get; set; }
        public List<XyPoint<string>> PointEquations { get; set; }

        public MssEvaluatorInput()
        {
            setDefaultsForOptionFields();
        }

        protected void setDefaultsForOptionFields()
        {
            RelData1 = double.NaN;
            RelData2 = double.NaN;
            RelData3 = double.NaN;

            CurveEquations = null;
            PointEquations = null;
        }

        //Can be called multiple times.
        public void InitForCurveEquation(double relData1, double relData2, double relData3,
                           double varA, double varB, double varC, double varD,
                           IMappingEntry mappingEntry)
        {
            this.equationType = EquationType.Curve;

            this.varA = varA;
            this.varB = varB;
            this.varC = varC;
            this.varD = varD;

            this.PrimaryInputSource = mappingEntry.CurveShapeInfo.PrimaryInputSource;

            this.RelData1 = relData1;
            this.RelData2 = relData2;
            this.RelData3 = relData3;

            this.CurveEquations = mappingEntry.CurveShapeInfo.CurveEquations;
            this.PointEquations = mappingEntry.CurveShapeInfo.PointEquations;
        }

        public void InitForCurveEquation(MssMsg mssMsg,
                           IMssParameterViewer parameterViewer,
                           IMappingEntry mappingEntry)
        {
            double relativeData1 = (double)mssMsg.Data1 / (double)mappingEntry.InMssMsgRange.MsgInfo.MaxData1Value;
            double relativeData2 = (double)mssMsg.Data2 / (double)mappingEntry.InMssMsgRange.MsgInfo.MaxData2Value;
            double relativeData3 = (double)mssMsg.Data3 / (double)mappingEntry.InMssMsgRange.MsgInfo.MaxData3Value;

            this.InitForCurveEquation(relativeData1, relativeData2, relativeData3,
                        parameterViewer.GetParameterValue(MssParameterID.VariableA),
                        parameterViewer.GetParameterValue(MssParameterID.VariableB),
                        parameterViewer.GetParameterValue(MssParameterID.VariableC),
                        parameterViewer.GetParameterValue(MssParameterID.VariableD),
                        mappingEntry);
        }

        public void InitForPointEquation(double varA, double varB, double varC, double varD)
        {
            this.equationType = EquationType.Point;

            this.varA = varA;
            this.varB = varB;
            this.varC = varC;
            this.varD = varD;
        }

        public double getPrimaryInputVal()
        {
            if (this.PrimaryInputSource == MssMsgDataField.Data1)
            {
                return this.RelData1;
            }
            else if (this.PrimaryInputSource == MssMsgDataField.Data2)
            {
                return this.RelData2;
            }
            else if (this.PrimaryInputSource == MssMsgDataField.Data3)
            {
                return this.RelData3;
            }
            else
            {
                //unknown MssMsgDataField
                Debug.Assert(false);
                return -1;
            }
        }

        public MssEvaluatorInput Clone()
        {
            return (MssEvaluatorInput)this.MemberwiseClone();
        }
    }
}
