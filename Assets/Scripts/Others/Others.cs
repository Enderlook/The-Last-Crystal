using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensions
{
    // https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/#post-3021162
    /// <summary>
    /// Convert a <see cref="LayerMask"/> into a layer number.<br/>
    /// This should only be used if the <paramref name="layerMask"/> has a single layer.
    /// </summary>
    /// <param name="layerMask"><see cref="LayerMask"/> to convert.</param>
    /// <returns>Layer number.</returns>
    public static int ToLayer(this LayerMask layerMask)
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
}

public static class OthersExtensions
{
    /// <summary>
    /// Deconstruction of <seealso cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
    // https://stackoverflow.com/questions/42549491/deconstruction-in-foreach-over-dictionary
    {
        key = tuple.Key;
        value = tuple.Value;
    }
}