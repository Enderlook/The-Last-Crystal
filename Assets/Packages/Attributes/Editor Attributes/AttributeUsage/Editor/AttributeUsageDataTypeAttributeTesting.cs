using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEditorHelper;

namespace AdditionalAttributes.Internal.Testing
{
    public static class AttributeUsageDataTypeAttributeTesting
    {
        private static Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)> checkers = new Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)>();

        [DidReloadScripts(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void SetRules()
        {
            AssemblyHelper.SubscribeOnEachTypeLessEnums(GetAttributesAndTypes, 0);
            AssemblyHelper.SubscribeOnEachTypeLessEnums(CheckClasses, 1);
            AssemblyHelper.SubscribeOnEachFieldOfTypes(CheckFields, 1);
            AssemblyHelper.SubscribeOnEachPropertyOfTypes(CheckProperties, 1);
            AssemblyHelper.SubscribeOnEachMethodOfTypes(CheckMethodReturns, 1);
        }

        private static void GetAttributesAndTypes(Type type)
        {
            if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(AttributeUsageDataTypeAttribute), true) is AttributeUsageDataTypeAttribute attribute)
            {
                AttributeUsageAttribute attributeUsageAttribute = type.GetCustomAttribute<AttributeUsageAttribute>();
                checkers.Add(type, (attributeUsageAttribute?.ValidOn ?? AttributeTargets.All, (checkType, checkName) => attribute.CheckAllowance(checkType, checkName, type.Name)));
            }
        }

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

        private static void CheckFields(FieldInfo fieldInfo) => CheckSomething(fieldInfo, fieldInfo.FieldType, "Field");
        private static void CheckProperties(PropertyInfo propertyInfo) => CheckSomething(propertyInfo, propertyInfo.PropertyType, "Property");
        private static void CheckMethodReturns(MethodInfo methodInfo) => CheckSomething(methodInfo, methodInfo.ReturnType, "Method return");
    }
}
