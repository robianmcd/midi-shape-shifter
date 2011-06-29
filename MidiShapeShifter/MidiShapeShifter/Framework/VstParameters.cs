using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Framework
{
    public class VstParameters
    {
        protected const string DEFAULT_PARAMETER_CATEGORY_NAME = "Parameter";

        protected PluginPrograms pluginPrograms;
        protected MssParameters mssParameters;

        public TwoWayDictionary<MssParameterID, VstParameterManager> VstParameterManagerDict = new TwoWayDictionary<MssParameterID, VstParameterManager>();

        public VstParameters()
        {
        }

        public void Init(MssParameters mssParameters, PluginPrograms pluginPrograms)
        {
            this.mssParameters = mssParameters;
            this.pluginPrograms = pluginPrograms;

            InitializeVstParams();

            this.mssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);
        }

        //This cannot be done during Init() because the IVstHost is still null
        public void InitHostAutomation(IVstHost vstHost)
        {
            foreach (VstParameterManager paramMgr in VstParameterManagerDict.RightKeys)
            {
                paramMgr.HostAutomation = vstHost.GetInstance<IVstHostAutomation>();
            }
        }

        protected void InitializeVstParams()
        {
            //itterate over each MssParameterID
            foreach(MssParameterID paramId in MssParameterID.GetValues(typeof(MssParameterID)))
            {
                VstParameterInfo paramInfo = MssToVstParameterInfo(paramId);
                VstParameterManager paramMgr = new VstParameterManager(paramInfo);

                paramMgr.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(VstParameterManager_PropertyChanged);
                VstParameterManagerDict.Add(paramId, paramMgr);
            }
        }

        protected VstParameterInfo MssToVstParameterInfo(MssParameterID paramId)
        {
            MssParameters mssParameters = this.mssParameters;

            // all parameter definitions are added to a central list.
            VstParameterInfoCollection parameterInfos = this.pluginPrograms.ParameterInfos;

            // retrieve the category for all variable parameters.
            VstParameterCategory paramCategory =
                this.pluginPrograms.GetParameterCategory(DEFAULT_PARAMETER_CATEGORY_NAME);

            // Variable parameter
            VstParameterInfo paramInfo = new VstParameterInfo();
            paramInfo.Category = paramCategory;
            paramInfo.CanBeAutomated = true;
            string paramName = mssParameters.GetParameterName(paramId);
            paramInfo.Name = paramName.Substring(0, Math.Min(7, paramName.Length)); //Cannot be more then 7 characters
            paramInfo.Label = "";
            paramInfo.ShortLabel = "";
            paramInfo.MinInteger = (int)mssParameters.GetParameterMinValue(paramId);
            paramInfo.MaxInteger = (int)mssParameters.GetParameterMaxValue(paramId);
            paramInfo.LargeStepFloat = ((float)(paramInfo.MaxInteger - paramInfo.MinInteger)) / 8;
            paramInfo.SmallStepFloat = ((float)(paramInfo.MaxInteger - paramInfo.MinInteger)) / 128;
            paramInfo.StepFloat = 0.03125f;
            paramInfo.DefaultValue = (float)mssParameters.GetParameterValue(paramId);
            VstParameterNormalizationInfo.AttachTo(paramInfo);

            parameterInfos.Add(paramInfo);

            return paramInfo;
        }

        private void VstParameterManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VstParameterManager paramMgr = (VstParameterManager)sender;

            if (e.PropertyName == VstParameterManager.ActiveParameterPropertyName)
            {
                paramMgr.ActiveParameter.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(VstParameter_Changed);
            }
        }

        private void VstParameter_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VstParameter changedParam = (VstParameter)sender;
            MssParameterID paramID;
            VstParameterManagerDict.TryGetLeftByRight(out paramID, changedParam.Info.ParameterManager);

            this.mssParameters.SetParameterValue(paramID, changedParam.Value);
        }

        private void MssParameters_NameChanged(MssParameterID paramId, string name)
        {
            VstParameterManager paramMgr;
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.Name != name)
            {
                paramMgr.ActiveParameter.Info.Name = name;
            }
        }

        private void MssParameters_ValueChanged(MssParameterID paramId, double value)
        {
            VstParameterManager paramMgr;
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Value != value)
            {
                paramMgr.ActiveParameter.Value = (float)value;
            }
        }

        private void MssParameters_MinValueChanged(MssParameterID paramId, int minValue)
        {
            VstParameterManager paramMgr;
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.MinInteger != minValue)
            {
                paramMgr.ActiveParameter.Info.MinInteger = minValue;
            }
        }

        private void MssParameters_MaxValueChanged(MssParameterID paramId, int maxValue)
        {
            VstParameterManager paramMgr;
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.MaxInteger != maxValue)
            {
                paramMgr.ActiveParameter.Info.MaxInteger = maxValue;
            }
        }

    }
}
