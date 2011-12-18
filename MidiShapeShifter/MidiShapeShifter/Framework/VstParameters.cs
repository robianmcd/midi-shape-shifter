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
    /// <summary>
    ///     Manages communication between the parameters described in MidiShapeShifter.Mss.MssParameters and the Jacobi
    ///     framework. In order to do this a VstParameterManager must be created for each parameter.
    /// </summary>
    public class VstParameters
    {
        protected const string DEFAULT_PARAMETER_CATEGORY_NAME = "Parameter";

        protected PluginPrograms pluginPrograms;
        protected MssParameters mssParameters;

        //Used to map MSS parameters to their associated VST parameters.
        public TwoWayDictionary<MssParameterID, VstParameterManager> VstParameterManagerDict = new TwoWayDictionary<MssParameterID, VstParameterManager>();

        public VstParameters()
        {
        }

        public void Init(MssParameters mssParameters, PluginPrograms pluginPrograms)
        {
            this.mssParameters = mssParameters;
            this.pluginPrograms = pluginPrograms;

            InitializeVstParams();
            
            //Listens to changes made to parameters from the MSS namespace (E.G. the GUI)
            this.mssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);
        }

        /// <summary>
        ///     Enables the ability to change parameters in this plugin from the host.
        /// <remarks>This cannot be done during Init() because the IVstHost is still null</remarks>
        public void InitHostAutomation(IVstHost vstHost)
        {
            foreach (VstParameterManager paramMgr in VstParameterManagerDict.RightKeys)
            {
                paramMgr.HostAutomation = vstHost.GetInstance<IVstHostAutomation>();
            }
        }

        /// <summary>
        ///     Initialized VstParameterManagerDict by createing a VstParameterManager for each MSS parameter.
        /// </summary>
        protected void InitializeVstParams()
        {
            //itterate over each MssParameterID
            foreach(MssParameterID paramId in MssParameterID.GetValues(typeof(MssParameterID)))
            {
                VstParameterInfo paramInfo = MssToVstParameterInfo(paramId);

                // all parameter definitions are added to a central list.
                VstParameterInfoCollection parameterInfos = this.pluginPrograms.ParameterInfos;
                parameterInfos.Add(paramInfo);

                VstParameterManager paramMgr = new VstParameterManager(paramInfo);

                //Adds listener to changes made to a parameter from the host
                paramMgr.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(VstParameterManager_PropertyChanged);
                VstParameterManagerDict.Add(paramId, paramMgr);
            }
        }

        /// <summary>
        ///     Generates an instance of VstParameterInfo using information about an MSS parameter.
        /// </summary>
        /// <param name="paramId">Specifies the MSS parameter to get information from</param>
        /// <returns></returns>
        protected VstParameterInfo MssToVstParameterInfo(MssParameterID paramId)
        {
            // retrieve the category for all variable parameters.
            VstParameterCategory paramCategory =
                this.pluginPrograms.GetParameterCategory(DEFAULT_PARAMETER_CATEGORY_NAME);

            // Variable parameter
            VstParameterInfo paramInfo = new VstParameterInfo();
            paramInfo.Category = paramCategory;
            paramInfo.CanBeAutomated = true;
            string paramName = this.mssParameters.GetParameterName(paramId);
            paramInfo.Name = paramName.Substring(0, Math.Min(7, paramName.Length)); //Cannot be more then 7 characters
            paramInfo.Label = "";
            paramInfo.ShortLabel = "";
            paramInfo.MinInteger = (int)this.mssParameters.GetParameterMinValue(paramId);
            paramInfo.MaxInteger = (int)this.mssParameters.GetParameterMaxValue(paramId);
            paramInfo.LargeStepFloat = ((float)(paramInfo.MaxInteger - paramInfo.MinInteger)) / 8;
            paramInfo.SmallStepFloat = ((float)(paramInfo.MaxInteger - paramInfo.MinInteger)) / 128;
            paramInfo.StepFloat = 0.03125f;
            paramInfo.DefaultValue = (float)this.mssParameters.GetParameterValue(paramId);
            VstParameterNormalizationInfo.AttachTo(paramInfo);

            return paramInfo;
        }

        /// <summary>
        ///     Listens to changes made to VstParameterManagers. This method is used to keep VstParameter_Changed up to
        ///     date with the active parameter for each VstParameterManager. The active parameter can change when the 
        ///     user changes the program from the host.
        /// </summary>
        /// <param name="sender">The VstParameterManager that changed.</param>
        /// <param name="e">Specifies the type of change made to a VstParameterManager</param>
        private void VstParameterManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VstParameterManager paramMgr = (VstParameterManager)sender;

            if (e.PropertyName == VstParameterManager.ActiveParameterPropertyName)
            {
                if (paramMgr.ActiveParameter != null)
                {
                    paramMgr.ActiveParameter.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(VstParameter_Changed);
                }
            }
        }

        /// <summary>
        ///     Listens to changes made to parameter values from the host and sends those changes to MssParameters.
        /// </summary>
        /// <param name="sender">The VstParameter that changed.</param>
        private void VstParameter_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VstParameter changedParam = (VstParameter)sender;
            MssParameterID paramID;
            //Gets the MssParameterID associated with the changed parameter.
            VstParameterManagerDict.TryGetLeftByRight(out paramID, changedParam.Info.ParameterManager);

            //Notifies MssParameters of the change.
            this.mssParameters.SetParameterValue(paramID, changedParam.Value);
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the name of a parameter and modifies the 
        ///     associated VstParameterManager to reflect the change.
        /// </summary>
        /// <param name="paramId">ID of the changed parameter.</param>
        /// <param name="name">New name of the changed parameter.</param>
        private void MssParameters_NameChanged(MssParameterID paramId, string name)
        {
            VstParameterManager paramMgr;
            //Looks up the VstParameterManager associated with the changed parameter
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.Name != name)
            {
                paramMgr.ActiveParameter.Info.Name = name;
            }
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the value of a parameter and modifies the 
        ///     associated VstParameterManager to reflect the change.
        /// </summary>
        /// <param name="paramId">ID of the changed parameter.</param>
        /// <param name="value">New value of the changed parameter.</param>
        private void MssParameters_ValueChanged(MssParameterID paramId, double value)
        {
            VstParameterManager paramMgr;
            //Looks up the VstParameterManager associated with the changed parameter
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter != null)
            {
                if (paramMgr.ActiveParameter.Value != value)
                {
                    paramMgr.ActiveParameter.Value = (float)value;
                }
            }
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the min value for a parameter and modifies the 
        ///     associated VstParameterManager to reflect the change.
        /// </summary>
        /// <param name="paramId">ID of the changed parameter.</param>
        /// <param name="minValue">New min value of the changed parameter.</param>
        private void MssParameters_MinValueChanged(MssParameterID paramId, int minValue)
        {
            VstParameterManager paramMgr;
            //Looks up the VstParameterManager associated with the changed parameter
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.MinInteger != minValue)
            {
                paramMgr.ActiveParameter.Info.MinInteger = minValue;
            }
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the max value for a parameter and modifies the 
        ///     associated VstParameterManager to reflect the change.
        /// </summary>
        /// <param name="paramId">ID of the changed parameter.</param>
        /// <param name="maxValue">New max value of the changed parameter.</param>
        private void MssParameters_MaxValueChanged(MssParameterID paramId, int maxValue)
        {
            VstParameterManager paramMgr;
            //Looks up the VstParameterManager associated with the changed parameter
            VstParameterManagerDict.TryGetRightByLeft(paramId, out paramMgr);
            if (paramMgr.ActiveParameter.Info.MaxInteger != maxValue)
            {
                paramMgr.ActiveParameter.Info.MaxInteger = maxValue;
            }
        }

    }
}
