using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.UI;
using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Parameters;
using System.Threading;
using System.Globalization;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     MssComponentHub manages all aspects of the plugin that do not rely on the jacobi framework. This class and 
    ///     all of its members do not have any references to the Jacobi framework or classes in the 
    ///     MidiShapeShifter.Framework namespace. This will make it much easier to extend this plugin to other 
    ///     frameworks or to a standalone application.
    /// </summary>
    [DataContract]
    public class MssComponentHub
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        protected SendMssEventsToHostTrigger sendEventsToHostTrigger;
        protected ParameterMsgHandler paramMsgHandler;
        protected DryMssEventHandler dryMssEventHandler;
        [DataMember(Name = "MappingMgr")]
        protected IMappingManager mappingMgr;

        protected MssEventGenerator mssEventGenrator;
        [DataMember(Name = "GenMappingMgr")]
        protected GeneratorMappingManager genMappingMgr;

        public static readonly Object criticalSectioinLock = new Object();

        /// <summary>
        ///     Passes unprocessed MssEvents from the "Framework" namespace to the "Mss" namespace.
        /// </summary>
        protected DryMssEventRelay _dryMssEventRelay;
        public IDryMssEventInputPort DryMssEventInputPort { get { return this._dryMssEventRelay; } }
        public IDryMssEventOutputPort DryMssEventOutputPort { get { return this._dryMssEventRelay; } }

        /// <summary>
        ///     Passes processed MssEvents from the "Mss" namespace to the "Framework" namespace.
        /// </summary>
        protected WetMssEventRelay _wetMssEventRelay;
        public IWetMssEventInputPort WetMssEventInputPort { get { return this._wetMssEventRelay; } }
        public IWetMssEventOutputPort WetMssEventOutputPort { get { return this._wetMssEventRelay; } }

        /// <summary>
        ///     Passes information about the host from the "Framework" namespace to the "Mss" namespace
        /// </summary>
        protected HostInfoRelay _hostInfoRelay;
        public IHostInfoInputPort HostInfoInputPort { get { return this._hostInfoRelay; } }
        public IHostInfoOutputPort HostInfoOutputPort { get { return this._hostInfoRelay; } }

        protected MssParameters _mssParameters;
        public MssParameters MssParameters { get { return this._mssParameters; } }

        [DataMember(Name = "MssProgramMgr")]
        protected MssProgramMgr _mssProgramMgr;
        public MssProgramMgr MssProgramMgr { get { return this._mssProgramMgr; } }

        protected TransformPresetMgr transformPresetMgr;

        protected Factory_MssMsgRangeEntryMetadata msgEntryMetadataFactory;
        protected IFactory_MssMsgInfo msgInfoFactory;

        public PluginEditorView PluginEditorView;

        [DataMember(Name = "ActiveMappingInfo")]
        protected ActiveMappingInfo activeMappingInfo;

        [DataMember(Name = "VariableParamMgr")]
        protected VariableParamMgr variableParamMgr;

        protected EventLogger eventLogger;

        static MssComponentHub() {
            //Some countries use a comma instead of a decimal point. This is not supported by NCalc so we 
            //need to ensure everyone is using a "."
            if (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
            }
        }

        public MssComponentHub()
        {
            Logger.Info(1, "Initializing MssComponentHub");

            ConstructNonSerializableMembers();

            //Construct Serializable members
            this._mssProgramMgr = new MssProgramMgr();
            this.mappingMgr = new MappingManager();
            this.genMappingMgr = new GeneratorMappingManager();
            this.activeMappingInfo = new ActiveMappingInfo();
            this.variableParamMgr = new VariableParamMgr();
        }

        protected void ConstructNonSerializableMembers()
        {
            this.sendEventsToHostTrigger = new SendMssEventsToHostTrigger();
            this.paramMsgHandler = new ParameterMsgHandler();
            this.dryMssEventHandler = new DryMssEventHandler();

            this._dryMssEventRelay = new DryMssEventRelay();
            this._wetMssEventRelay = new WetMssEventRelay();
            this._hostInfoRelay = new HostInfoRelay();

            this.mssEventGenrator = new MssEventGenerator();
            this._mssParameters = new MssParameters();
            this.msgEntryMetadataFactory = new Factory_MssMsgRangeEntryMetadata();
            this.msgInfoFactory = new Factory_MssMsgInfo();
            this.transformPresetMgr = new TransformPresetMgr();
            this.eventLogger = new EventLogger();
        }

        /// <summary>
        ///     Initialized members
        /// </summary>
        public void Init()
        {
            InitializeNonSerializableMembers();
            //Initialize serializable members
            this.MssProgramMgr.Init();
            this.variableParamMgr.Init();
        }

        protected void InitializeNonSerializableMembers()
        {
            //This should be initialized first so that it get's triggered first when wet or dry 
            //events are recieved. A better solution would be to have a beforeEventRecieved event.
            this.eventLogger.Init(this.DryMssEventOutputPort, this.WetMssEventOutputPort);

            this.msgEntryMetadataFactory.Init(this.genMappingMgr, 
                                              (IMssParameterViewer)this.MssParameters);
            this.msgInfoFactory.Init(this.genMappingMgr, (IMssParameterViewer)this.MssParameters);

            this.sendEventsToHostTrigger.Init(this.HostInfoOutputPort, this.WetMssEventInputPort);
            this.paramMsgHandler.Init(this.MssParameters, 
                                      this.WetMssEventOutputPort, 
                                      this.DryMssEventInputPort, 
                                      this.HostInfoOutputPort);
            this.dryMssEventHandler.Init(this.DryMssEventOutputPort, 
                                         this.WetMssEventInputPort, 
                                         this.mappingMgr, 
                                         this.MssParameters);
            this.mssEventGenrator.Init(this.HostInfoOutputPort,
                                       this.WetMssEventOutputPort,
                                       this.DryMssEventInputPort,
                                       this.genMappingMgr,
                                       this.MssParameters);


            this.activeMappingInfo.InitNonserializableMembers(this.mappingMgr, this.genMappingMgr);
            transformPresetMgr.Init(this.activeMappingInfo);
            this._mssParameters.Init(this.activeMappingInfo, this.variableParamMgr);
        }


        [OnDeserializing]
        protected void OnDeserializing(StreamingContext context)
        {
            ConstructNonSerializableMembers();
        }

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            InitializeNonSerializableMembers();
        }

        /// <summary>
        ///     Creates an displays the PluginEditorView.
        /// </summary>
        public void OpenPluginEditor(IntPtr hWnd)
        {
            EnsurePluginEditorExists();

            SetParent(this.PluginEditorView.Handle, hWnd);

            this.PluginEditorView.Show();
        }

        /// <summary>
        ///     Hides and disposes of the PluginEditorView.
        /// </summary>
        public void ClosePluginEditor()
        {
            if (this.PluginEditorView != null)
            {
                this.PluginEditorView.Dispose();
                this.PluginEditorView = null;
            }
        }

        /// <summary>
        ///     Creates and initializes the PluginEditorView if it does not already exist.
        /// </summary>
        public void EnsurePluginEditorExists()
        {
            if (this.PluginEditorView == null)
            {
                this.PluginEditorView = new PluginEditorView();

                this.PluginEditorView.CreateControl();
                this.PluginEditorView.Init(this.MssParameters, 
                                            this.mappingMgr, 
                                            this.genMappingMgr,
                                            this.MssProgramMgr,
                                            this.transformPresetMgr,
                                            this.DryMssEventOutputPort,
                                            this.HostInfoOutputPort, 
                                            this.activeMappingInfo);
            }
        }

        public override string ToString()
        {
            return String.Format("MssComponentHub, numMappings: {0}, numGenMappings {1}", this.mappingMgr.GetNumEntries(), this.genMappingMgr.GetNumEntries());
        }

    }
}
