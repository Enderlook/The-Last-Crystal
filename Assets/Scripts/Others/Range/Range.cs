using System;
using UnityEngine;

[Serializable]
public abstract class Range<T>
{
    [SerializeField]
    // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
    private T min;
    [SerializeField]
    // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
    private T max;
#pragma warning restore CS0649

    /// <summary>
    /// Return the highest bound of the range.<br/>
    /// </summary>
    public T Max => max;

    /// <summary>
    /// Return the lowest bound of the range.<br/>
    /// </summary>
    public T Min => min;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    public abstract T Value { get; }
}