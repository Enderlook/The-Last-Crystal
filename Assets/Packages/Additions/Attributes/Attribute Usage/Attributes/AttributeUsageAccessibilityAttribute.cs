using System;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes.AttributeUsage
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageAccessibilityAttribute : Attribute
    {
        private readonly BindingFlags bindingFlags;

        /// <summary>
        /// Determines which <see cref="BindingFlags"/> must have the decorated.
        /// </summary>
        /// <param name="bindingFlags">Necessary binding flags.</param>
        public AttributeUsageAccessibilityAttribute(BindingFlags bindingFlags) => this.bindingFlags = bindingFlags;

#if UNITY_EDITOR
        /// <summary>
        /// Unity Editor only.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="attributeName"></param>
        /// <remarks>Only use in Unity Editor.</remarks>
        public void CheckAllowance(MemberInfo memberInfo, string attributeName)
        {
            if (memberInfo.ReflectedType.GetMember(memberInfo.Name, bindingFlags).Length == 0)
                Debug.LogException(new ArgumentException($"According to {nameof(AttributeUsageAccessibilityAttribute)}, the attribute {attributeName} can only be applied in members with the following {nameof(BindingFlags)}: {bindingFlags}."));
        }
#endif
    }
}