using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEditorHelper;

namespace AdditionalAttributes.Internal
{
    public static class AttributeUsageDataTypeAttributeTesting
    {
        [DidReloadScripts]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void Check()
        {
            Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)> checkers = new Dictionary<Type, (AttributeTargets targets, Action<Type, string> checker)>();

            foreach ((Type type, AttributeUsageDataTypeAttribute attribute) in AttributeUsageHelper.GetAllAttributesWithCustomAttributeInPlayerAndEditorAssemblies<AttributeUsageDataTypeAttribute>())
            {
                AttributeUsageAttribute attributeUsageAttribute = type.GetCustomAttribute<AttributeUsageAttribute>();
                checkers.Add(type, (attributeUsageAttribute?.ValidOn ?? AttributeTargets.All, (checkType, checkName) => attribute.CheckAllowance(checkType, checkName, type.Name)));
            }

            foreach (Type classType in AssemblyHelper.GetAllTypesOfPlayerAndEditorAssemblies())
            {
                foreach (Attribute attribute in classType.GetCustomAttributes())
                {
                    if (checkers.TryGetValue(attribute.GetType(), out (AttributeTargets targets, Action<Type, string> checker) value))
                    {
                        // Check if has the proper flag
                        if ((value.targets & AttributeTargets.Class) != 0)
                            value.checker(classType, $"Class {classType.Name}");
                    }
                }

                foreach ((MemberInfo memberInfo, Type type, Attribute attribute) in AttributeUsageHelper.GettAllAttributesOfMembersOf(classType))
                {
                    if (checkers.TryGetValue(attribute.GetType(), out (AttributeTargets targets, Action<Type, string> checker) value))
                    {
                        switch (memberInfo.MemberType)
                        {
                            case MemberTypes.Field:
                                if ((value.targets & AttributeTargets.Field) != 0)
                                    value.checker(((FieldInfo)memberInfo).FieldType, $"Field {memberInfo.Name} in {type.Name} class");
                                break;
                            case MemberTypes.Property:
                                if ((value.targets & AttributeTargets.Property) != 0)
                                    value.checker(((PropertyInfo)memberInfo).PropertyType, $"Property {memberInfo.Name} in {type.Name} class");
                                break;
                            case MemberTypes.Method:
                                if ((value.targets & AttributeTargets.Method) != 0)
                                    value.checker(((MethodInfo)memberInfo).ReturnType, $"Method {memberInfo.Name} in {type.Name} class");
                                break;
                        }
                    }
                }
            }
        }
    }
}
