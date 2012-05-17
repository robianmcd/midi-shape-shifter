using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    ///     Manages communication between the parameters described in 
    ///     MidiShapeShifter.Mss.MssParameters and the Jacobi framework. In order to do this a 
    ///     VstParameterManager must be created for each parameter.
    /// </summary>
    public class VstParameters
    {
        protected const string DEFAULT_PARAMETER_CATEGORY_NAME = "Parameter";
        protected const int VST_PARAM_MAX_VALUE = 1;
        protected const int VST_PARAM_MIN_VALUE = 0;

        protected PluginPrograms pluginPrograms;

        private Func<MssParameters> getMssParameters;
        protected MssParameters mssParameters { get { return this.getMssParameters(); } }

        protected HashSet<MssParameterID> parametersBeingModified;

        //Used to map MSS parameters to their associated VST parameters.
        public TwoWayDictionary<MssParameterID, VstParameterManager> VstParameterManagerDict = 
            new TwoWayDictionary<MssParameterID, VstParameterManager>();

        public VstParameters()
        {
            parametersBeingModified = new HashSet<MssParameterID>();
        }

        public void Init(Func<MssParameters> getMssParameters, PluginPrograms pluginPrograms)
        {
            this.getMssParameters = getMssParameters;
            this.pluginPrograms = pluginPrograms;

            pluginPrograms.ProgramActivated += new ProgramActivatedEventHandler(OnProgramActivated);
            AttachHandlersToMssParameterEvents();
            InitializeVstParams();
        }

        protected void AttachHandlersToMssParameterEvents()
        {
            //Listens to changes made to parameters from the MSS namespace (E.G. the GUI)
            this.mssParameters.ParameterValueChanged += 
                new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += 
                new ParameterNameChangedEventHandler(MssParameters_NameChanged);
        }

        public void OnMssParametersInstanceReplaced()
        {
            AttachHandlersToMssParameterEvents();
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
        ///     Initialized VstParameterManagerDict by createing a VstParameterManager for each MSS 
        ///     parameter.
        /// </summary>
        protected void InitializeVstParams()
        {
            // all parameter definitions are added to a central list.
            VstParameterInfoCollection parameterInfos = this.pluginPrograms.ParameterInfos;

            //itterate over each MssParameterID
            foreach(MssParameterID paramId in MssParameterID.GetValues(typeof(MssParameterID)))
            {
                VstParameterInfo paramInfo = MssToVstParameterInfo(paramId);
                
                parameterInfos.Add(paramInfo);

                VstParameterManager paramMgr = new VstParameterManager(paramInfo);

                //Adds listener to changes made to a parameter from the host
                paramMgr.PropertyChanged += 
                    new PropertyChangedEventHandler(VstParameterManager_PropertyChanged);
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
            MssParamInfo mssParamInfo = this.mssParameters.GetParameterInfo(paramId);
            paramInfo.Name = GetParamNameFromString(mssParamInfo.Name); 
            paramInfo.Label = "";
            paramInfo.ShortLabel = "";
            paramInfo.MinInteger = VST_PARAM_MIN_VALUE;
            paramInfo.MaxInteger = VST_PARAM_MAX_VALUE;
            paramInfo.LargeStepFloat = ((float)(VST_PARAM_MAX_VALUE - VST_PARAM_MIN_VALUE)) / 8;
            paramInfo.SmallStepFloat = ((float)(VST_PARAM_MAX_VALUE - VST_PARAM_MIN_VALUE)) / 128;
            paramInfo.StepFloat = 0.03125f;
            paramInfo.DefaultValue = (float)this.mssParameters.GetRelativeParamValue(paramId);
            VstParameterNormalizationInfo.AttachTo(paramInfo);

            return paramInfo;
        }

        //Cannot be more then 7 characters
        protected string GetParamNameFromString(string name)
        {
            return name.Substring(0, Math.Min(7, name.Length));
        }

        /// <summary>
        ///     Listens to changes made to VstParameterManagers. This method is used to keep 
        ///     VstParameter_Changed up to date with the active parameter for each VstParameterManager. 
        ///     The active parameter can change when the user changes the program from the host.
        /// </summary>
        /// <param name="sender">The VstParameterManager that changed.</param>
        /// <param name="e">Specifies the type of change made to a VstParameterManager</param>
        private void VstParameterManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            VstParameterManager paramMgr = (VstParameterManager)sender;

            if (e.PropertyName == VstParameterManager.ActiveParameterPropertyName)
            {
                if (paramMgr.ActiveParameter != null)
                {
                    paramMgr.ActiveParameter.PropertyChanged += 
                        new PropertyChangedEventHandler(VstParameter_Changed);
                }
            }
        }

        /// <summary>
        ///     Listens to changes made to parameter values from the host and sends those changes to 
        ///     MssParameters.
        /// </summary>
        /// <param name="sender">The VstParameter that changed.</param>
        private void VstParameter_Changed(object sender, PropertyChangedEventArgs e)
        {
            VstParameter changedParam = (VstParameter)sender;
            MssParameterID paramID;
            //Gets the MssParameterID associated with the changed parameter.
            VstParameterManagerDict.TryGetLeftByRight(out paramID, changedParam.Info.ParameterManager);
            
            if (this.parametersBeingModified.Contains(paramID) == false)
            {
                if (e.PropertyName == VstParameter.ValuePropertyName)
                {
                    this.parametersBeingModified.Add(paramID);
                    //Notifies MssParameters of the change.
                    this.mssParameters.SetParameterRelativeValue(paramID, changedParam.Value);
                    this.parametersBeingModified.Remove(paramID);

                }
            }
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the name of a parameter and modifies 
        ///     the associated VstParameterManager to reflect the change.
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
                paramMgr.ActiveParameter.Info.Name = GetParamNameFromString(name);
            }
        }

        /// <summary>
        ///     Listens to changes made from the MSS namespace to the value of a parameter and modifies 
        ///     the associated VstParameterManager to reflect the change.
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
                    if (this.parametersBeingModified.Contains(paramId) == false)
                    {
                        this.parametersBeingModified.Add(paramId);
                        paramMgr.ActiveParameter.Value = (float)this.mssParameters.GetRelativeParamValue(paramId);
                        this.parametersBeingModified.Remove(paramId);
                    }
                }
            }
        }

        protected void OnProgramActivated()
        {
            //Send the info for each parameter from MssParameters to the host.
            foreach (MssParameterID paramId in MssParameterID.GetValues(typeof(MssParameterID)))
            {
                MssParamInfo mssParamInfo = this.mssParameters.GetParameterInfo(paramId);
                MssParameters_ValueChanged(paramId, mssParamInfo.GetValue());
                MssParameters_NameChanged(paramId, mssParamInfo.Name);
            }
        }

    }
}
