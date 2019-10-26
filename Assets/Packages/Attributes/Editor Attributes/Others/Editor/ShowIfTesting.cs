using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdditionalAttributes.PostCompiling;
using UnityEngine;

namespace AdditionalAttributes
{
    internal class ShowfIfTesting
    {
        private static readonly Dictionary<Type, List<ShowIfAttribute>> typesAndAttributes = new Dictionary<Type, List<ShowIfAttribute>>();

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.SerializableByUnity, 0)]
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

        private static readonly string errorMissingFieldMessage = $"{{0}} does not have a field, property (with Get Method) or method (without mandatory parameters and with return type) of type {typeof(bool)} named {{1}} necessary for attribute {nameof(ShowIfAttribute)}.";

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Static;

        [ExecuteWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper")]
        private static void CheckFields()
        {
            foreach (KeyValuePair<Type, List<ShowIfAttribute>> classToCheck in typesAndAttributes)
            {
                Type classType = classToCheck.Key;
                HashSet<string> confirmFields = new HashSet<string>(classToCheck.Value.Select(e => e.NameOfConditional));

                confirmFields.ExceptWith(new HashSet<string>(
                    classType
                        .GetFields(bindingFlags)
                        .Where(field => CheckCasteability(field.FieldType) && field.CanBeSerializedByUnity())
                        .Cast<MemberInfo>()
                        .Concat(
                            classType.GetProperties(bindingFlags)
                            .Where(property => CheckCasteability(property.PropertyType) && property.CanRead)
                            .Cast<MemberInfo>()
                         )
                        .Concat(
                            classType.GetMethods(bindingFlags)
                            .Where(method => CheckCasteability(method.ReturnType) && method.GetParameters().Count(parameter => parameter.IsOptional == false) > 0)
                            .Cast<MemberInfo>()
                         )
                        .Select(member => member.Name)
                    )
                );

                foreach (string field in confirmFields)
                {
                    Debug.LogException(new ArgumentException(string.Format(errorMissingFieldMessage, classType, field)));
                }
            }
        }

        private static bool CheckCasteability(Type type) => type == typeof(bool) || type.IsAssignableFrom(typeof(bool));
    }
}
