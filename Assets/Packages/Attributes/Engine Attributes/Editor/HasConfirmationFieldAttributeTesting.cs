using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdditionalAttributes.PostCompiling.Execute;
using AdditionalAttributes.PostCompiling.Internal;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AdditionalAttributes.Internal.PostCompile
{
    public static class HasConfirmationFieldAttributeTesting
    {
        private static readonly Dictionary<Type, List<HasConfirmationFieldAttribute>> typesAndAttributes = new Dictionary<Type, List<HasConfirmationFieldAttribute>>();

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.SerializableByUnity, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void GetFields(FieldInfo fieldInfo)
        {
            if (fieldInfo.GetCustomAttribute<HasConfirmationFieldAttribute>() is HasConfirmationFieldAttribute attribute)
            {
                Type type = fieldInfo.DeclaringType;
                if (typesAndAttributes.TryGetValue(type, out List<HasConfirmationFieldAttribute> list))
                    list.Add(attribute);
                else
                    typesAndAttributes.Add(type, new List<HasConfirmationFieldAttribute>() { attribute });
            }
        }

        private static readonly string errorMissingFieldMessage = $"{{0}} does not have a field of type {typeof(bool)} named {{1}} necessary for attribute {nameof(HasConfirmationFieldAttribute)}.";

        [DidReloadScripts(3)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity Editor")]
        private static void CheckFields()
        {
            foreach (KeyValuePair<Type, List<HasConfirmationFieldAttribute>> classToCheck in typesAndAttributes)
            {
                Type classType = classToCheck.Key;
                HashSet<string> confirmFields = new HashSet<string>(classToCheck.Value.Select(e => e.ConfirmFieldName));

                confirmFields.ExceptWith(new HashSet<string>(
                    classType
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        .Where(e => e.FieldType == typeof(bool) && e.CanBeSerializedByUnity())
                        .Select(e => e.Name)
                    )
                );

                foreach (string field in confirmFields)
                {
                    Debug.LogException(new ArgumentException(string.Format(errorMissingFieldMessage, classType, field)));
                }
            }
        }
    }
}