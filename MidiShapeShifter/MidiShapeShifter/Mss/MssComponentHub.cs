using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.UI;

using System.Drawing;
using System.Windows.Forms;


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

        protected MappingManager _mappingMgr;
        public MappingManager MappingMgr { get { return this._mappingMgr; } }

        protected MssParameters _mssParameters;
        public MssParameters MssParameters { get { return this._mssParameters; } }

        protected MssMsgProcessor _mssMsgProcessor;
        public MssMsgProcessor MssMsgProcessor { get { return this._mssMsgProcessor; } }

        protected PluginEditorView _pluginEditorView;
        public PluginEditorView PluginEditorView { 
            get 
            {
                EnsurePluginEditorExists();
                return this._pluginEditorView; 
            } 
        }

        protected List<MssEvent> mssEventsForHost = new List<MssEvent>();

        public MssComponentHub()
        {
            _mappingMgr = new MappingManager();
            _mssParameters = new MssParameters();
            _mssMsgProcessor = new MssMsgProcessor(MappingMgr);
        }

        /// <summary>
        ///     Initialized members
        /// </summary>
        public void Init()
        {
            _mssParameters.Init();
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
                this._pluginEditorView.Init(this);
                this._pluginEditorView.CreateControl();
            }
        }

        public void HandleIncomingMssEvent(MssEvent unprocessedEvent)
        {
            List<MssMsg> mssMessages = this.MssMsgProcessor.ProcessMssMsg(unprocessedEvent.mssMsg);

            foreach(MssMsg mssMsg in mssMessages)
            {
                MssEvent processedEvent = new MssEvent();
                processedEvent.mssMsg = mssMsg;
                processedEvent.timestamp = unprocessedEvent.timestamp;
                this.mssEventsForHost.Add(processedEvent);
            }
        }

        public List<MssEvent> TransferMssEventsForHost()
        {
            List<MssEvent> eventsToTransfer = new List<MssEvent>(this.mssEventsForHost.Count);
            eventsToTransfer.AddRange(this.mssEventsForHost);

            this.mssEventsForHost.Clear();

            return eventsToTransfer;
        }
    }
}
