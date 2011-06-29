using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Mapping;
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
        //TODO: this is only temporarily public while the GUI is still using it directly
        public MappingManager mappingMgr;
        
        protected DryMssEventRelay _dryMssEventRelay;
        public IDryMssEventReceiver DryMssEventReceiver { get { return this._dryMssEventRelay; } }
        public IDryMssEventEchoer DryMssEventEchoer { get { return this._dryMssEventRelay; } }

        protected WetMssEventRelay _wetMssEventRelay;
        public IWetMssEventReceiver WetMssEventReceiver { get { return this._wetMssEventRelay; } }
        public IWetMssEventEchoer WetMssEventEchoer { get { return this._wetMssEventRelay; } }

        protected HostInfoRelay _hostInfoRelay;
        public IHostInfoReceiver HostInfoReceiver { get { return this._hostInfoRelay; } }
        public IHostInfoEchoer HostInfoEchoer { get { return this._hostInfoRelay; } }

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
            sendEventsToHostTrigger = new SendMssEventsToHostTrigger();
            dryMssEventHandler = new DryMssEventHandler();
            mappingMgr = new MappingManager();

            _dryMssEventRelay = new DryMssEventRelay();
            _wetMssEventRelay = new WetMssEventRelay();
            _hostInfoRelay = new HostInfoRelay();

            _mssParameters = new MssParameters();

            
        }

        /// <summary>
        ///     Initialized members
        /// </summary>
        public void Init()
        {
            _mssParameters.Init();

            sendEventsToHostTrigger.Init(this.HostInfoEchoer, this.WetMssEventReceiver);
            dryMssEventHandler.Init(this.DryMssEventEchoer, this.WetMssEventReceiver, this.mappingMgr);
        }

        public void OpenPluginEditor(IntPtr hWnd)
        {
            EnsurePluginEditorExists();

            SetParent(this._pluginEditorView.Handle, hWnd);

            //TODO: Set knobs to correct values. populate stuff
            this._pluginEditorView.Show();
        }

        public void ClosePluginEditor()
        {
            if (this._pluginEditorView != null)
            {
                this._pluginEditorView.Dispose();
                this._pluginEditorView = null;
            }
        }

        protected void EnsurePluginEditorExists()
        {
            if (this._pluginEditorView == null)
            {
                this._pluginEditorView = new PluginEditorView();
                this._pluginEditorView.Init(this.MssParameters, this.mappingMgr, this.DryMssEventEchoer);
                this._pluginEditorView.CreateControl();
            }
        }
    }
}
