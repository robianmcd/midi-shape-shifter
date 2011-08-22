using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.UI;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     MssComponentHub manages all aspects of the plugin that do not rely on the jacobi framework. This class and 
    ///     all of its members do not have any references to the Jacobi framework or classes in the 
    ///     MidiShapeShifter.Framework namespace. This will make it much easier to extend this plugin to other 
    ///     frameworks or to a standalone application.
    /// </summary>
    public class MssComponentHub
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        protected SendMssEventsToHostTrigger sendEventsToHostTrigger;
        protected DryMssEventHandler dryMssEventHandler;
        protected MappingManager mappingMgr;

        protected MssEventGenerator mssEventGenrator;
        protected GeneratorMappingManager genMappingMgr;
        
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

        protected PluginEditorView _pluginEditorView;
        public PluginEditorView PluginEditorView { 
            get 
            {
                EnsurePluginEditorExists();
                return this._pluginEditorView; 
            } 
        }

        public MssComponentHub()
        {
            this.sendEventsToHostTrigger = new SendMssEventsToHostTrigger();
            this.dryMssEventHandler = new DryMssEventHandler();
            this.mappingMgr = new MappingManager();

            this._dryMssEventRelay = new DryMssEventRelay();
            this._wetMssEventRelay = new WetMssEventRelay();
            this._hostInfoRelay = new HostInfoRelay();

            this.genMappingMgr = new GeneratorMappingManager();
            this.mssEventGenrator = new MssEventGenerator();

            this._mssParameters = new MssParameters();

            
        }

        /// <summary>
        ///     Initialized members
        /// </summary>
        public void Init()
        {
            _mssParameters.Init();

            this.sendEventsToHostTrigger.Init(this.HostInfoOutputPort, this.WetMssEventInputPort);
            this.dryMssEventHandler.Init(this.DryMssEventOutputPort, this.WetMssEventInputPort, this.mappingMgr);
            this.mssEventGenrator.Init(this.HostInfoOutputPort, 
                                       this.WetMssEventOutputPort, 
                                       this.DryMssEventInputPort, 
                                       this.genMappingMgr);
        }

        /// <summary>
        ///     Creates an displays the PluginEditorView.
        /// </summary>
        public void OpenPluginEditor(IntPtr hWnd)
        {
            EnsurePluginEditorExists();

            SetParent(this._pluginEditorView.Handle, hWnd);

            this._pluginEditorView.Show();
        }

        /// <summary>
        ///     Hides and disposes of the PluginEditorView.
        /// </summary>
        public void ClosePluginEditor()
        {
            if (this._pluginEditorView != null)
            {
                this._pluginEditorView.Dispose();
                this._pluginEditorView = null;
            }
        }

        /// <summary>
        ///     Creates and initializes the PluginEditorView if it does not already exist.
        /// </summary>
        protected void EnsurePluginEditorExists()
        {
            if (this._pluginEditorView == null)
            {
                this._pluginEditorView = new PluginEditorView();
                this._pluginEditorView.Init(this.MssParameters, this.mappingMgr, this.genMappingMgr, this.DryMssEventOutputPort);
                this._pluginEditorView.CreateControl();
            }
        }
    }
}
