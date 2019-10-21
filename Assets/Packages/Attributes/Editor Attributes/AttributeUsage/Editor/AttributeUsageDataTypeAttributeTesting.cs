using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Callbacks;

namespace AdditionalAttributes.Internal
{
    public static class AttributeUsageDataTypeAttributeTesting
    {
        [DidReloadScripts]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void Check()
        {
            Dictionary<Type, Action<FieldInfo, Type>> checkers = new Dictionary<Type, Action<FieldInfo, Type>>();

            foreach ((Type type, AttributeUsageDataTypeAttribute attribute) in AttributeUsageHelper.GetAllAttributesWithCustomAttributeInCurrentDomainAssemblies<AttributeUsageDataTypeAttribute>())
            {
                checkers.Add(type, (f, c) => attribute.CheckAllowance(type, f, c));
            }

            foreach ((FieldInfo fieldInfo, Type type, Attribute attribute) in AttributeUsageHelper.GetAllAttributesOfFieldsOfTypesInCurrentDomainAssemblies())
            {
                if (checkers.TryGetValue(attribute.GetType(), out Action<FieldInfo, Type> checker))
                    checker(fieldInfo, type);
            }
        }
    }
}
