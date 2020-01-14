using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(UnityEngine.Object), includeEnumerableTypes = true, typeFlags = TypeCasting.CheckSubclassOrAssignable)]
    [AttributeUsageFieldMustBeSerializableByUnityAttribute]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ExpandableAttribute : PropertyAttribute
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