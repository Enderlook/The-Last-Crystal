﻿using UnityEngine;

public static class LayerMaskExtensions
{
    /// <summary>
    /// Convert a <see cref="LayerMask"/> into a layer number.<br/>
    /// This should only be used if the <paramref name="layerMask"/> has a single layer.
    /// </summary>
    /// <param name="layerMask"><see cref="LayerMask"/> to convert.</param>
    /// <returns>Layer number.</returns>
    public static int ToLayer(this LayerMask layerMask)
    // https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/#post-3021162
    {
        int bitMask = layerMask.value;
        int result = bitMask > 0 ? 0 : 31;
        while (bitMask > 1)
        {
            bitMask >>= 1;
            result++;
        }
        return result;
    }

    /// <summary>
    /// Check if the <paramref name="layerToCheck"/> have <paramref name="layerTrigger"/>.
    /// </summary>
    /// <param name="layerToCheck">Layers to check if have <paramref name="layerTrigger"/>.</param>
    /// <param name="layerTrigger">Layers checked in <paramref name="layerToCheck"/>.</param>
    /// <returns>Whenever <paramref name="layerToCheck"/> has <paramref name="layerTrigger"/>.</returns>
    public static bool LayerMatchTest(LayerMask layerToCheck, LayerMask layerTrigger)
    // https://forum.unity.com/threads/making-a-ray-ignore-a-layer.385224/#post-2504429
    {
        return ((1 << layerToCheck) & layerTrigger) != 0;
    }

    /// <summary>
    /// Check if the <c><paramref name="source"/>.layer</c> is one the layers from <paramref name="layerTrigger"/>.
    /// </summary>
    /// <param name="source">Gameobject to check its layers.</param>
    /// <param name="layerTrigger">Layer that must be checked.</param>
    /// <returns>Whenever <c><paramref name="source"/>.layer</c> is one the layers from <paramref name="layerTrigger"/>.</returns>
    public static bool LayerMatchTest(this GameObject source, LayerMask layerTrigger) => LayerMatchTest(source.layer, layerTrigger);
}