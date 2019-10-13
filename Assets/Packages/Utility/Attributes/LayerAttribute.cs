using UnityEngine;

namespace AdditionalAttributes
{
    public sealed class LayerAttribute : PropertyAttribute
    {
        public static int InvertLayer(int layer) => 1 << layer;
    }
}