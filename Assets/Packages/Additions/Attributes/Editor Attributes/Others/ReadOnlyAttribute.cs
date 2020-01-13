using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    [AttributeUsageFieldMustBeSerializableByUnity]
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}