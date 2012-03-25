using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Evaluation
{
    public class EvaluationCurveInput : EvaluationInput
    {
        //These fields are only needed if this is input for a curve equation
        public MssMsgDataField PrimaryInputSource { get; set; }

        public double RelData1 { get; set; }
        public double RelData2 { get; set; }
        public double RelData3 { get; set; }

        public List<string> CurveEquations { get; set; }
        public List<XyPoint<string>> PointEquations { get; set; }

        //Can be called multiple times.
        public void Init(double relData1, double relData2, double relData3,
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

        public void Init(MssMsg mssMsg,
                           IMssParameterViewer parameterViewer,
                           IMappingEntry mappingEntry)
        {
            MssMsgInfo inMsgInfo = mappingEntry.InMssMsgRange.MsgInfo;
            double relativeData1 = (double)mssMsg.Data1 / (double)(inMsgInfo.MaxData1Value - inMsgInfo.MinData1Value);
            double relativeData2 = (double)mssMsg.Data2 / (double)(inMsgInfo.MaxData2Value - inMsgInfo.MinData2Value);
            double relativeData3 = (double)mssMsg.Data3 / (double)(inMsgInfo.MaxData3Value - inMsgInfo.MinData3Value);

            this.Init(relativeData1, relativeData2, relativeData3,
                        parameterViewer.GetParameterValue(MssParameterID.VariableA),
                        parameterViewer.GetParameterValue(MssParameterID.VariableB),
                        parameterViewer.GetParameterValue(MssParameterID.VariableC),
                        parameterViewer.GetParameterValue(MssParameterID.VariableD),
                        mappingEntry);
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

        public void setPrimaryInputVal(double primaryInputVal)
        {
            if (this.PrimaryInputSource == MssMsgDataField.Data1)
            {
                this.RelData1 = primaryInputVal;
            }
            else if (this.PrimaryInputSource == MssMsgDataField.Data2)
            {
                this.RelData2 = primaryInputVal;
            }
            else if (this.PrimaryInputSource == MssMsgDataField.Data3)
            {
                this.RelData3 = primaryInputVal;
            }
            else
            {
                //unknown MssMsgDataField
                Debug.Assert(false);
            }
        }
    }
}
