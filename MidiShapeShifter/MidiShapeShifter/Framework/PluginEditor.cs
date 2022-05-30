using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Relays;
using System;
using System.Drawing;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object manages the custom editor (UI) for your plugin.
    /// </summary>
    /// <remarks>
    /// When you do not implement a custom editor, 
    /// your Parameters will be displayed in an editor provided by the host.
    /// </remarks>
    public class PluginEditor : IVstPluginEditor
    {
        private Func<MssComponentHub> getMssHub;
        protected MssComponentHub MssHub { get { return this.getMssHub(); } }

        protected bool pluginEditorIsOpen = false;
        protected IntPtr pluginHandle;

        public PluginEditor()
        {

        }

        public void Init(Func<MssComponentHub> getMssHub)
        {
            this.getMssHub = getMssHub;
        }

        public Rectangle Bounds
        {
            get
            {
                this.MssHub.EnsurePluginEditorExists();
                return this.MssHub.PluginEditorView.Bounds;
            }
        }

        public void Close()
        {
            this.pluginEditorIsOpen = false;
            this.MssHub.ClosePluginEditor();
        }

        public void KeyDown(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // empty by design
        }

        public void KeyUp(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // empty by design
        }

        public VstKnobMode KnobMode { get; set; }

        public void Open(IntPtr hWnd)
        {
            this.pluginEditorIsOpen = true;
            this.pluginHandle = hWnd;
            this.MssHub.OpenPluginEditor(hWnd);
        }

        public void OnBeforeDeserialized()
        {
            this.MssHub.ClosePluginEditor();
        }

        public void OnDeserialized()
        {
            if (this.pluginEditorIsOpen)
            {
                Open(this.pluginHandle);
            }
        }

        public void ProcessIdle()
        {
            // keep your processing short!

            MssComponentHub mssHub = this.getMssHub();

            if (mssHub != null)
            {
                IHostInfoInputPort hostInputPort = mssHub.HostInfoInputPort;

                if (hostInputPort != null)
                {
                    hostInputPort.TriggerIdleProcessing();
                }
            }

        }


    }
}
