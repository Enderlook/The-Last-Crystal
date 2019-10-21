using System;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageDataType(typeof(int), typeof(float), typeof(LayerMask), typeof(string))]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class LayerAttribute : PropertyAttribute
    {
        public static int InvertLayer(int layer) => 1 << layer;
    }
}