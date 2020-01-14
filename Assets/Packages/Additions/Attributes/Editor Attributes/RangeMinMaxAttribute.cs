using Additions.Attributes.AttributeUsage;
using Additions.Serializables.Ranges;

using System;

using UnityEngine;

using RangeInt = Additions.Serializables.Ranges.RangeInt;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(Vector2), typeof(Vector2Int), typeof(RangeInt), typeof(RangeIntStep), typeof(RangeFloat), typeof(RangeFloatStep), includeEnumerableTypes = true)]
    [AttributeUsageFieldMustBeSerializableByUnityAttribute]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class RangeMinMaxAttribute : RangeAttribute
    {
        public RangeMinMaxAttribute(float min, float max, float step = 0, bool showRandomButton = true) : base(min, max, step, showRandomButton) { }
    }
}