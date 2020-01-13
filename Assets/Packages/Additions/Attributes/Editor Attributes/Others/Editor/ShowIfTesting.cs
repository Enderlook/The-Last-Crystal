using Additions.Attributes.PostCompiling;
using Additions.Attributes.PostCompiling.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes
{
    internal static class ShowfIfTesting
    {
        private static readonly Dictionary<Type, List<ShowIfAttribute>> typesAndAttributes = new Dictionary<Type, List<ShowIfAttribute>>();

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(FieldSerialization.SerializableByUnity, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper")]
        private static void GetFields(FieldInfo fieldInfo)
        {
            if (fieldInfo.GetCustomAttribute<ShowIfAttribute>() is ShowIfAttribute attribute)
            {
                Type type = fieldInfo.DeclaringType;
                if (typesAndAttributes.TryGetValue(type, out List<ShowIfAttribute> list))
                    list.Add(attribute);
                else
                    typesAndAttributes.Add(type, new List<ShowIfAttribute>() { attribute });
            }
        }

        [ExecuteWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper")]
        private static void CheckFields()
        {
            foreach (KeyValuePair<Type, List<ShowIfAttribute>> classToCheck in typesAndAttributes)
            {
                Type classType = classToCheck.Key;
                HashSet<string> confirmFields = new HashSet<string>(classToCheck.Value.Select(e => e.nameOfConditional));

                confirmFields.ExceptWith(new HashSet<string>(classType.FieldsPropertiesAndMethodsWithReturnTypeOf<bool>()));

                foreach (string field in confirmFields)
                    Debug.LogException(new ArgumentException($"{classType} does not have a field, property (with Get Method) or method (without mandatory parameters and with return type) of type {typeof(bool)} named {field} necessary for attribute {nameof(ShowIfAttribute)}."));
            }
        }
    }
}
