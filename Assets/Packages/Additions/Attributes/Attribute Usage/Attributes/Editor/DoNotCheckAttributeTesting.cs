
using Additions.Attributes.AttributeUsage.PostCompiling.Attributes;

using System;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes.AttributeUsage
{
    internal static class DoNotCheckAttributeTesting
    {
        [ExecuteOnEachTypeWhenScriptsReloads(0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void GetAttributeFromMember(Type type)
        {
            if (type.GetCustomAttribute<DoNotCheckAttribute>(true) is DoNotCheckAttribute attribute)
                foreach (Type _type in attribute.ignoreTypes)
                    if (!_type.IsSubclassOf(typeof(Attribute)))
                        Debug.LogException(new ArgumentException($"Attribute {nameof(DoNotCheckAttribute)} can only have types that inherit from {nameof(Attribute)} in the field {nameof(DoNotCheckAttribute.ignoreTypes)}. The type {type} is not subclass of {nameof(Attribute)}. Found in type {type.Name}."));
        }

        [ExecuteOnEachMemberOfEachTypeWhenScriptsReloads(0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by PostCompilingAssembliesHelper.")]
        private static void GetAttributeFromMember(MemberInfo memberInfo)
        {
            if (memberInfo.GetCustomAttribute<DoNotCheckAttribute>(true) is DoNotCheckAttribute attribute)
                foreach (Type type in attribute.ignoreTypes)
                    if (!type.IsSubclassOf(typeof(Attribute)))
                        Debug.LogException(new ArgumentException($"Attribute {nameof(DoNotCheckAttribute)} can only have types that inherit from {nameof(Attribute)} in the field {nameof(DoNotCheckAttribute.ignoreTypes)}. The type {type} is not subclass of {nameof(Attribute)}. Found in member {memberInfo.MemberType} {memberInfo.Name} in class {memberInfo.ReflectedType}."));
        }

        /// <summary>
        /// Check if this <paramref name="memberInfo"/> should be ignored when checking if has <paramref name="typeThatMayBeIgnored"/>.
        /// </summary>
        /// <param name="memberInfo">Member to check.</param>
        /// <param name="typeThatMayBeIgnored">Type of attribute that it's being checked.</param>
        /// <returns>Whenever it should be ignored or not.</returns>
        public static bool CheckIfShouldBeIgnored(this MemberInfo memberInfo, Type typeThatMayBeIgnored)
        {
            DoNotCheckAttribute attribute = memberInfo.GetCustomAttribute<DoNotCheckAttribute>(true);
            return attribute?.ignoreTypes.Contains(typeThatMayBeIgnored) ?? false;
        }

        /// <summary>
        /// Check if this <paramref name="type"/> should be ignored when checking if has <paramref name="typeThatMayBeIgnored"/>.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="typeThatMayBeIgnored">Type of attribute that it's being checked.</param>
        /// <returns>Whenever it should be ignored or not.</returns>
        public static bool CheckIfShouldBeIgnored(this Type type, Type typeThatMayBeIgnored)
        {
            DoNotCheckAttribute attribute = type.GetCustomAttribute<DoNotCheckAttribute>(true);
            return attribute?.ignoreTypes.Contains(typeThatMayBeIgnored) ?? false;
        }
    }
}