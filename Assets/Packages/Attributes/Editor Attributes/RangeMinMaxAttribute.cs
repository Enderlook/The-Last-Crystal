using System;
using AdditionalAttributes.AttributeUsage;
using Range;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageRequireDataType(typeof(Vector2), typeof(Vector2Int), typeof(Range.RangeInt), typeof(RangeIntStep), typeof(RangeFloat), typeof(RangeFloatStep), includeEnumerableTypes = true)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class RangeMinMaxAttribute : RangeAttribute
    {
        public RangeMinMaxAttribute(float min, float max, float step = 0, bool showRandomButton = true) : base(min, max, step, showRandomButton) { }
    }
}