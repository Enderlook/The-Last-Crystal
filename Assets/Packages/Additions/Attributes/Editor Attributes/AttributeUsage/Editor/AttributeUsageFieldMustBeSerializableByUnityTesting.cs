using Additions.Attributes.PostCompiling;
using Additions.Attributes.PostCompiling.Attributes;

using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes.AttributeUsage
{
    public static class AttributeUsageFieldMustBeSerializableByUnityTesting
    {
        private static HashSet<Type> types = new HashSet<Type>();

        [ExecuteOnEachTypeWhenScriptsReloads(ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void GetAttributesAndTypes(Type type)
        {
            if (type.IsSubclassOf(typeof(Attribute)) && type.GetCustomAttribute(typeof(AttributeUsageFieldMustBeSerializableByUnity), true) is AttributeUsageFieldMustBeSerializableByUnity attribute)
                types.Add(type);
        }

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(FieldSerialization.EitherSerializableOrNotByUnity, loop: 1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void CheckFields(FieldInfo fieldInfo)
        {
            foreach (Attribute attribute in fieldInfo.GetCustomAttributes())
                if (types.Contains(attribute.GetType()) && !fieldInfo.CanBeSerializedByUnity())
                    Debug.LogException(new ArgumentException($"The attribute {attribute.GetType().Name} in field {fieldInfo.FieldType.Name} from class {fieldInfo.DeclaringType.Name}."));
        }
    }
}