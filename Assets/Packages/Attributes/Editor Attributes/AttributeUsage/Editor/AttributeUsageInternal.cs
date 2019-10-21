using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorHelper;

namespace AdditionalAttributes.Internal
{
    public static class AttributeUsageHelper
    {
        public static IEnumerable<(Type type, T attribute)> GetAllAttributesWithCustomAttributeInCurrentDomainAssemblies<T>()
        {
            foreach (Type type in AssemblyHelper.GetAllTypesOfCurrentDomainAssemblies())
            {
                // Check if a class came from Attribute before using reflection because otherwise it would be a waste of performance
                if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(T), true) is T attribute)
                {
                    yield return (type, attribute);
                }
            }
        }

        public static IEnumerable<(FieldInfo fieldInfo, Type type, Attribute attribute)> GetAllAttributesOfFieldsOfTypesInCurrentDomainAssemblies()
        {
            foreach (Type type in AssemblyHelper.GetAllTypesOfCurrentDomainAssemblies())
            {
                foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    foreach (Attribute attribute in fieldInfo.GetCustomAttributes())
                    {
                        yield return (fieldInfo, type, attribute);
                    }
                }
            }
        }
    }
}