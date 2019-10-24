using System;
using System.Collections.Generic;
using System.Reflection;
using AdditionalAttributes.PostCompiling.Execute;

namespace AdditionalAttributes.Internal.PostCompile
{
    public static class AttributeUsageRequireDataTypeAttributeTesting
    {
        private static Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)> checkers = new Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)>();

        [ExecuteOnEachTypeWhenScriptsReloads(ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by AssemblyHelper.")]
        private static void GetAttributesAndTypes(Type type)
        {
            if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(AttributeUsageRequireDataTypeAttribute), true) is AttributeUsageRequireDataTypeAttribute attribute)
            {
                AttributeUsageAttribute attributeUsageAttribute = type.GetCustomAttribute<AttributeUsageAttribute>();
                checkers.Add(type, (attributeUsageAttribute?.ValidOn ?? AttributeTargets.All, (checkType, checkName) => attribute.CheckAllowance(checkType, checkName, type.Name)));
            }
        }

        [ExecuteOnEachTypeWhenScriptsReloads(ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum, 1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by AssemblyHelper.")]
        private static void CheckClasses(Type type)
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (checkers.TryGetValue(attribute.GetType(), out (AttributeTargets targets, Action<Type, string> checker) value))
                {
                    // Check if has the proper flag
                    if ((value.targets & AttributeTargets.Class) != 0)
                        value.checker(type, $"Class {type.Name}");
                }
            }
        }

        private static void CheckSomething(MemberInfo memberInfo, Type type, string memberType)
        {
            foreach (Attribute attribute in memberInfo.GetCustomAttributes())
            {
                if (checkers.TryGetValue(attribute.GetType(), out (AttributeTargets targets, Action<Type, string> checker) value) && (value.targets & AttributeTargets.Field) != 0)
                {
                    value.checker(type, $"{memberType} {memberInfo.Name} in {memberInfo.DeclaringType.Name} class");
                }
            }
        }

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.EitherSerializableOrNotByUnity, 1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by AssemblyHelper.")]
        private static void CheckFields(FieldInfo fieldInfo) => CheckSomething(fieldInfo, fieldInfo.FieldType, "Field");

        [ExecuteOnEachPropertyOfEachTypeWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by AssemblyHelper.")]
        private static void CheckProperties(PropertyInfo propertyInfo) => CheckSomething(propertyInfo, propertyInfo.PropertyType, "Property");

        [ExecuteOnEachMethodOfEachTypeWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by AssemblyHelper.")]
        private static void CheckMethodReturns(MethodInfo methodInfo) => CheckSomething(methodInfo, methodInfo.ReturnType, "Method return");
    }
}
