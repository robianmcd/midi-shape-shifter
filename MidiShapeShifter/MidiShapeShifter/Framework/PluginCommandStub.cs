using System;
using Jacobi.Vst.Core.Plugin;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object receives all calls from the (unmanaged) host.
    /// </summary>
    /// <remarks>
    /// An instance of this object is created automatically by the Jacobi.Vst.Interop assembly
    /// when the plugin is loaded into the host. Interop marshals all calls from unmanaged C++
    /// to this object.
    /// </remarks>
    public class PluginCommandStub : StdPluginDeprecatedCommandStub, IVstPluginCommandStub
    {
        /// <summary>
        /// Returns an instance of the VST.NET Plugin root object.
        /// </summary>
        /// <returns>Must never return null.</returns>
        protected override IVstPlugin CreatePluginInstance()
        {
            return new Plugin();
        }

        public override void SetProgram(int programNumber)
        {
            //When you load an instance of MSS this function will get called. ProgramNumber will be 
            //set to the index of the program that was selected when you saved MSS. So if some 
            //programs were added or deleted since MSS was saved then this will be wrong. This try
            //catch block is a quick fix to prevent an exception from being thrown but it doesn't 
            //really fix the problem.
            try
            {
                base.SetProgram(programNumber);
            }
            catch (ArgumentOutOfRangeException)
            { 
                //If nothing is set when the plugin loads then it will default to the first program which is "Blank"
            }
        }
    }
}
