using AdditionalAttributes.PostCompiling;

using AdditionalExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace AdditionalAttributes
{
    internal static class GUITesting
    {
        private static readonly Dictionary<Type, List<GUIAttribute>> typesAndAttributes = new Dictionary<Type, List<GUIAttribute>>();

        [ExecuteOnEachFieldOfEachTypeWhenScriptsReloads(ExecuteOnEachFieldOfEachTypeWhenScriptsReloads.FieldFlags.SerializableByUnity, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper")]
        private static void GetFields(FieldInfo fieldInfo)
        {
            if (fieldInfo.GetCustomAttribute<GUIAttribute>() is GUIAttribute attribute)
            {
                Type type = fieldInfo.DeclaringType;
                if (typesAndAttributes.TryGetValue(type, out List<GUIAttribute> list))
                    list.Add(attribute);
                else
                    typesAndAttributes.Add(type, new List<GUIAttribute>() { attribute });
            }
        }

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Static;

        [ExecuteWhenScriptsReloads(1)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper")]
        private static void CheckFields()
        {
            foreach (KeyValuePair<Type, List<GUIAttribute>> classToCheck in typesAndAttributes)
            {
                Type classType = classToCheck.Key;
                List<GUIAttribute> attributes = classToCheck.Value;

                HashSet<string> strings = new HashSet<string>(
                    attributes
                        .Where(e => e.nameMode == GUIAttribute.Mode.Reference)
                        .Select(e => e.name)
                        .Concat(
                            attributes
                                .Where(e => e.tooltipMode == GUIAttribute.Mode.Reference)
                                .Select(e => e.tooltip)
                        )
                        .Where(e => e != null)
                    );

                HashSet<string> members = new HashSet<string>(FieldsPropertiesAndMethodsWithReturnTypeOf<string>(classType));

                strings.ExceptWith(members);

                foreach (string field in strings)
                    Debug.LogException(new ArgumentException($"Type {classType} does not have a field, property (with Get Method) or method (with only optional or params pameters and with a return type other than void) of type {typeof(string)} named {field} necessary for attribute {nameof(GUIAttribute)}."));

                HashSet<string> stringOrGUIContent = new HashSet<string>(
                    attributes
                        .Select(e => e.guiContentOrReferenceName)
                        .Where(e => e != null)
                    );

                members.UnionWith(FieldsPropertiesAndMethodsWithReturnTypeOf<GUIContent>(classType));

                stringOrGUIContent.ExceptWith(members);
            }
        }

        private static IEnumerable<string> FieldsPropertiesAndMethodsWithReturnTypeOf(Type @class, Type @return) => @class
                .GetFields(bindingFlags)
                .Where(field => field.FieldType.IsCastableTo(@return) && field.CanBeSerializedByUnity())
                .Cast<MemberInfo>()
                .Concat(
                    @class
                        .GetProperties(bindingFlags)
                        .Where(property => property.PropertyType.IsCastableTo(@return) && property.CanRead)
                        .Cast<MemberInfo>()
                )
                .Concat(
                    @class
                        .GetMethods(bindingFlags)
                        .Where(method => method.ReturnType.IsCastableTo(@return) && method.HasNoMandatoryParameters())
                        .Cast<MemberInfo>()
                )
                .Select(member => member.Name);

        private static IEnumerable<string> FieldsPropertiesAndMethodsWithReturnTypeOf<T>(Type @class) => FieldsPropertiesAndMethodsWithReturnTypeOf(@class, typeof(T));
    }
}