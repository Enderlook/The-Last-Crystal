﻿using Additions.Attributes.AttributeUsage.PostCompiling;
using Additions.Attributes.AttributeUsage.PostCompiling.Attributes;

using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes.AttributeUsage
{
    internal static class AttributeUsageFieldMustBeSerializableByUnityTesting
    {
        private static HashSet<Type> types = new HashSet<Type>();

        [ExecuteOnEachTypeWhenScriptsReloads(ExecuteOnEachTypeWhenScriptsReloads.TypeFlags.IsNonEnum, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void GetAttributesAndTypes(Type type)
        {
            if (type.IsSubclassOf(typeof(Attribute))
                && type.GetCustomAttribute<AttributeUsageFieldMustBeSerializableByUnityAttribute>(true) is AttributeUsageFieldMustBeSerializableByUnityAttribute attribute)
                types.Add(type);
        }

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(FieldSerialization.EitherSerializableOrNotByUnity, loop: 1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void CheckFields(FieldInfo fieldInfo)
        {
            foreach (Attribute attribute in fieldInfo.GetCustomAttributes())
            {
                Type attributeType = attribute.GetType();
                if (types.Contains(attributeType)
                    && !fieldInfo.CanBeSerializedByUnity()
                    && !fieldInfo.CheckIfShouldBeIgnored(attributeType))
                    Debug.LogException(new ArgumentException($"The attribute {attribute.GetType().Name} can only be used in fields that can be serialized by Unity, but field {fieldInfo.Name} from class {fieldInfo.DeclaringType.Name} (type {fieldInfo.FieldType}) can't be serialized."));
            }
        }
    }
}