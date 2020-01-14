using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageFieldMustBeSerializableByUnity]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class IsPropertyAttribute : PropertyAttribute { }
}