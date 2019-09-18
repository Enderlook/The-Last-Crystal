using UnityEngine;

public static class VectorExtensions
{
    /// <summary>
    /// Returns absolute <seealso cref="Vector2"/> of <paramref name="source"/>.
    /// </summary>
    /// <param name="source"><seealso cref="Vector2"/> to become absolute.</param>
    /// <returns>Absolute <seealso cref="Vector2"/>.</returns>
    public static Vector2 Abs(this Vector2 source) => new Vector2(Mathf.Abs(source.x), Mathf.Abs(source.y));
    /// <summary>
    /// Returns absolute <seealso cref="Vector3"/> of <paramref name="source"/>.
    /// </summary>
    /// <param name="source"><seealso cref="Vector3"/> to become absolute.</param>
    /// <returns>Absolute <seealso cref="Vector3"/>.</returns>
    public static Vector3 Abs(this Vector3 source) => new Vector3(Mathf.Abs(source.x), Mathf.Abs(source.y), Mathf.Abs(source.z));
    /// <summary>
    /// Returns absolute <seealso cref="Vector4"/> of <paramref name="source"/>.
    /// </summary>
    /// <param name="source"><seealso cref="Vector4"/> to become absolute.</param>
    /// <returns>Absolute <seealso cref="Vector4"/>.</returns>
    public static Vector4 Abs(this Vector4 source) => new Vector4(Mathf.Abs(source.x), Mathf.Abs(source.y), Mathf.Abs(source.z), Mathf.Abs(source.w));
}