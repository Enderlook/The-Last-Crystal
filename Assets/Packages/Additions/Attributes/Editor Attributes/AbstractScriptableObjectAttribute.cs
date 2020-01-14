using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(ScriptableObject), includeEnumerableTypes = true, typeFlags = TypeCasting.CheckCanBeAssignedTypes | TypeCasting.CheckIsAssignableTypes)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AbstractScriptableObjectAttribute : Attribute { }
}