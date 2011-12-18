using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MidiShapeShifter.CSharpUtil
{

    /// <summary>
    /// The C# serialization framework needs to know where the assembly that contains the types that
    /// have been serialized. It will only look in the folder that the application is running in so
    /// it will not find this assembly as it is in the VST folder. This class allows shows the
    /// serialization framework how to find this assembly and how to bind the types in the serialized
    /// data to the types in this assembly.
    /// For more info see this forum post:
    /// http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/e5f0c371-b900-41d8-9a5b-1052739f2521
    /// </summary>
    public class DeserializationBinderForPlugins : 
        System.Runtime.Serialization.SerializationBinder
    {

        static DeserializationBinderForPlugins()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly ayResult = null;

            string sShortAssemblyName = args.Name.Split(',')[0];

            Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly ayAssembly in ayAssemblies)
            {
                if (sShortAssemblyName == ayAssembly.FullName.Split(',')[0])
                {
                    ayResult = ayAssembly;

                    break;
                }
            }
            return ayResult;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type tyType = null;

            string sShortAssemblyName = assemblyName.Split(',')[0];
            Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly ayAssembly in ayAssemblies)
            {
                if (sShortAssemblyName == ayAssembly.FullName.Split(',')[0])
                {
                    tyType = ayAssembly.GetType(typeName);
                    break;
                }
            }

            return tyType;
        }

    }
}
