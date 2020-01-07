using AdditionalExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Compilation;

using UnityAssembly = UnityEditor.Compilation.Assembly;
using SystemAssembly = System.Reflection.Assembly;

namespace AdditionalAttributes
{
    internal static class AssembliesHelper
    {
        /// <summary>
        /// Get all assemblies from <see cref="AppDomain.CurrentDomain"/> which are in the <see cref="CompilationPipeline.GetAssemblies"/> either <see cref="AssembliesType.Editor"/> and <see cref="AssembliesType.Player"/>.
        /// </summary>
        /// <returns>Assemblies which matches criteria.</returns>
        public static IEnumerable<SystemAssembly> GetAllAssembliesOfPlayerAndEditorAssemblies()
        {
            IEnumerable<UnityAssembly> unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player));
            foreach (SystemAssembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (unityAssemblies.ContainsBy(e => e.name == assembly.GetName().Name))
                    yield return assembly;
        }
    }
}