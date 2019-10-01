using UnityEngine;

public sealed class LayerAttribute : PropertyAttribute
{
    public static int InvertLayer(int layer) => 1 << layer;
}
