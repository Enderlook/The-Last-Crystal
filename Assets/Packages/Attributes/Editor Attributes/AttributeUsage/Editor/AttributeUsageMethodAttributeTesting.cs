using System;
using System.Collections.Generic;
using System.Reflection;
using AdditionalAttributes.PostCompiling.Execute;

namespace AdditionalAttributes.Internal.PostCompile
{
    public static class AttributeUsageMethodAttributeTesting
    {
        private static Dictionary<Type, Action<MethodInfo, string>> checkers = new Dictionary<Type, Action<MethodInfo, string>>();

        [ExecuteOnEachTypeWhenScriptsReloads(ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void GetAttributesAndTypes(Type type)
        {
            if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(AttributeUsageMethodAttribute), true) is AttributeUsageMethodAttribute attribute)
                checkers.Add(type, attribute.CheckAllowance);
        }

        [ExecuteOnEachMethodOfEachTypeWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void CheckMethods(MethodInfo methodInfo)
        {
            foreach (Attribute attribute in methodInfo.GetCustomAttributes())
            {
                if (checkers.TryGetValue(attribute.GetType(), out Action<MethodInfo, string> check))
                    check(methodInfo, $"method {methodInfo.Name} in {methodInfo.DeclaringType.Name} class");
            }
        }
    }
}