using System;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RangeAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;
        public readonly float step;
        public readonly bool showRandomButton;

        public RangeAttribute(float min, float max, float step = 0, bool showRandomButton = true)
        {
            this.min = min;
            this.max = max;
            this.step = step;
            this.showRandomButton = showRandomButton;
        }
    }
}