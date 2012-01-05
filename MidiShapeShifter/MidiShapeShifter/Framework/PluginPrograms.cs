using System;

using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.Mss.Programs;

namespace MidiShapeShifter.Framework
{

    public delegate void ProgramActivatedEventHandler();

    /// <summary>
    /// This object manages the Plugin programs and its parameters.
    /// </summary>
    public class PluginPrograms : VstPluginProgramsBase
    {
        public event ProgramActivatedEventHandler ProgramActivated;

        protected Func<MssProgramMgr> getMssProgramMgr;
        protected MssProgramMgr mssProgramMgr { get { return this.getMssProgramMgr(); } }
        protected IVstHost vstHost;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="plugin">A reference to the plugin root object.</param>
        public PluginPrograms()
        {
            ParameterCategories = new VstParameterCategoryCollection();
            ParameterInfos = new VstParameterInfoCollection();
        }

        public void Init(Func<MssProgramMgr> getMssProgramMgr)
        {
            this.getMssProgramMgr = getMssProgramMgr;
            AttachHandlersToMssProgramMgrEvents();
        }

        protected void AttachHandlersToMssProgramMgrEvents()
        {
            this.getMssProgramMgr().ActiveProgramChanged +=
                new ActiveProgramChangedEventHandler(OnProgramChangeFromPlugin);
        }

        public void OnMssProgramMgrInstanceReplaced()
        {
            AttachHandlersToMssProgramMgrEvents();
        }

        public void InitVstHost(IVstHost vstHost)
        {
            this.vstHost = vstHost;
        }

        /// <summary>
        /// Gets a collection of parameter categories.
        /// </summary>
        /// <remarks>
        /// This is the central list that contains all parameter categories.
        /// </remarks>
        public VstParameterCategoryCollection ParameterCategories { get; private set; }

        /// <summary>
        /// Gets a collection of parameter definitions.
        /// </summary>
        /// <remarks>
        /// This is the central list that contains all parameter definitions for the plugin.
        /// </remarks>
        public VstParameterInfoCollection ParameterInfos { get; private set; }

        /// <summary>
        /// Retrieves a parameter category object for the specified <paramref name="categoryName"/>.
        /// </summary>
        /// <param name="categoryName">The name of the parameter category. Typically the name of the Dsp component.</param>
        /// <returns>Never returns null.</returns>
        public VstParameterCategory GetParameterCategory(string categoryName)
        {
            if (ParameterCategories.Contains(categoryName))
            {
                return ParameterCategories[categoryName];
            }

            // create a new parameter category object
            VstParameterCategory paramCategory = new VstParameterCategory();
            paramCategory.Name = categoryName;

            ParameterCategories.Add(paramCategory);

            return paramCategory;
        }

        /// <summary>
        /// Called to initialize the collection of programs for the plugin.
        /// </summary>
        /// <returns>Never returns null or an empty collection.</returns>
        protected override VstProgramCollection CreateProgramCollection()
        {
            return CreatePrograms();
        }

        public VstProgramCollection CreatePrograms()
        {
            VstProgramCollection newPrograms = new VstProgramCollection();

            foreach(MssProgramInfo progInfo in this.mssProgramMgr.FlatProgramList)
            {
                VstProgram program = CreateProgram(ParameterInfos);
                program.Name = progInfo.Name;
                newPrograms.Add(program);
            }
            return newPrograms;
        }

        // create a program with all parameters.
        private VstProgram CreateProgram(VstParameterInfoCollection parameterInfos)
        {
            VstProgram program = new VstProgram(ParameterCategories);

            CreateParameters(program.Parameters, parameterInfos);

            return program;
        }

        // create all parameters
        private void CreateParameters(VstParameterCollection desitnation, VstParameterInfoCollection parameterInfos)
        {
            foreach (VstParameterInfo paramInfo in parameterInfos)
            {
                desitnation.Add(CreateParameter(paramInfo));
            }
        }

        // create one parameter
        private VstParameter CreateParameter(VstParameterInfo parameterInfo)
        {
            VstParameter parameter = new VstParameter(parameterInfo);

            return parameter;
        }

        protected VstProgram _activeProgram;
        public override VstProgram ActiveProgram
        {
            get
            {
                if (this._activeProgram == null && Programs.Count > 0)
                {
                    this.ActiveProgram = Programs[0];
                }

                return this._activeProgram;
            }
            set
            {
                if (this._activeProgram != value)
                {
                    if (this._activeProgram != null)
                    {
                        this._activeProgram.IsActive = false;
                        this._activeProgram = null;
                    }

                    if (value != null)
                    {
                        ActivateProgram(value);
                    }
                }
            }
        }

        protected void ActivateProgram(VstProgram program)
        {
            this._activeProgram = program;
            this._activeProgram.IsActive = true;

            this.Programs[0].Name = program.Name;
            this.mssProgramMgr.OnProgramChanged(program.Name);

            if (ProgramActivated != null)
            {
                ProgramActivated();
            }

            if (this.vstHost != null)
            {
                this.vstHost.GetInstance<IVstHostShell>().UpdateDisplay();
            }
        }

        protected void OnProgramChangeFromPlugin(string programName)
        {
            if (programName == this.ActiveProgram.Name)
            {
                return;
            }

            bool matchingProgramFound = false;
            foreach (VstProgram prog in this.Programs)
            {
                if (prog.Name == programName)
                {
                    ActivateProgram(prog);
                    matchingProgramFound = true;
                    break;
                }
            }

            if (matchingProgramFound == false)
            {
                this.Programs[0].Name = programName;
                ActivateProgram(this.Programs[0]);
            }
        }

    }
}
