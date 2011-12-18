using System;
using System.Drawing;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Common;
using MidiShapeShifter.Mss.UI;
using System.Collections.Generic;

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
        private Plugin _plugin;
        protected bool pluginEditorIsOpen = false;
        protected IntPtr pluginHandle;

        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
        }

        public Rectangle Bounds
        {
            get { return _plugin.MssHub.PluginEditorView.Bounds; }
        }

        public void Close()
        {
            this.pluginEditorIsOpen = false;
            _plugin.MssHub.ClosePluginEditor();
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
            _plugin.MssHub.OpenPluginEditor(hWnd);
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
        }
    }
}
