using AdditionalAttributes.AttributeUsage;
using AdditionalAttributes.AttributeUsage.Internal;

using System;

using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageRequireDataType(typeof(ScriptableObject), includeEnumerableTypes = true, typeFlags = AttributeUsageHelper.TypeFlags.CheckCanBeAssignedTypes | AttributeUsageHelper.TypeFlags.CheckIsAssignableTypes)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AbstractScriptableObjectAttribute : Attribute { }
}