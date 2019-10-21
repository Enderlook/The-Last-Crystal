using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditorHelper
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Get all types of all assemblies of current domain.
        /// </summary>
        /// <returns>All types of all assemblies of current domain.</returns>
        public static IEnumerable<Type> GetAllTypesOfCurrentDomainAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }
    }
}