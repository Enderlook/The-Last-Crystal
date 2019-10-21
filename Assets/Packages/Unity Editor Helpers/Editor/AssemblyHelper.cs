using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Compilation;
using AdditionalExtensions;
using UnityEngine;

namespace UnityEditorHelper
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Get all types of all Player and Editor assemblies.
        /// </summary>
        /// <returns>All types of Player and Editor assemblies.</returns>
        public static IEnumerable<Type> GetAllTypesOfPlayerAndEditorAssemblies()
        {
            IEnumerable<UnityEditor.Compilation.Assembly> unityAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player));
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (unityAssemblies.ContainsBy(e => e.name == assembly.GetName().Name))
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}