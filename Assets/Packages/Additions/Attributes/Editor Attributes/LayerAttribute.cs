using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(int), typeof(float), typeof(LayerMask), typeof(string), includeEnumerableTypes = true)]
    [AttributeUsageFieldMustBeSerializableByUnityAttribute]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class LayerAttribute : PropertyAttribute
    {
        public static int InvertLayer(int layer) => 1 << layer;
    }
}