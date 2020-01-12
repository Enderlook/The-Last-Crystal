using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(ScriptableObject), includeEnumerableTypes = true, typeFlags = TypeFlags.CheckCanBeAssignedTypes | TypeFlags.CheckIsAssignableTypes)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AbstractScriptableObjectAttribute : Attribute { }
}