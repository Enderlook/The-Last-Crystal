using UnityEngine;

namespace AdditionalAttributes
{
    public class ExpandableAttribute : PropertyAttribute
    {
        public readonly bool? isBoxed;

        public readonly float? colorMultiplier;

        public ExpandableAttribute() { }

        public ExpandableAttribute(bool isBoxed) => this.isBoxed = isBoxed;

        public ExpandableAttribute(bool isBoxed, float colorMultiplier)
        {
            this.isBoxed = isBoxed;
            this.colorMultiplier = colorMultiplier;
        }

        public ExpandableAttribute(float colorMultiplier) => this.colorMultiplier = colorMultiplier;
    }
}