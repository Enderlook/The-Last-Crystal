using Additions.Extensions;

using System;
using System.Linq;

using UnityEditor.Compilation;

using SystemAssembly = System.Reflection.Assembly;
using UnityAssembly = UnityEditor.Compilation.Assembly;

namespace Additions.Attributes.AttributeUsage.PostCompiling
{
    internal static class AssembliesHelper
    {
        private static SystemAssembly[] assemblies;

        /// <summary>
        /// Get all assemblies from <see cref="AppDomain.CurrentDomain"/> which are in the <see cref="CompilationPipeline.GetAssemblies"/> either <see cref="AssembliesType.Editor"/> and <see cref="AssembliesType.Player"/>.
        /// </summary>
        /// <param name="ingoreCache">Whenever it should recalculate the value regardless the cache.</param>
        /// <returns>Assemblies which matches criteria.</returns>
        public static SystemAssembly[] GetAllAssembliesOfPlayerAndEditorAssemblies(bool ingoreCache = false)
        {
            // Cached because it takes like 100ms to do.
            if (assemblies == null || ingoreCache)
            {
                UnityAssembly[] unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player)).ToArray();
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(e => unityAssemblies.ContainsBy(e2 => e2.name == e.GetName().Name)).ToArray();
            }
            return assemblies;
        }
    }
}