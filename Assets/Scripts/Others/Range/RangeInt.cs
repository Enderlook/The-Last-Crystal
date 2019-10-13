using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RangeInt : Range<int>
{
    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    public override int Value => Random.Range(Min, Max);

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="source"><see cref="RangeInt"/> instance used to determine the random int.</param>
    public static explicit operator int(RangeInt source) => source.Value;
}

[Serializable]
public class RangeIntStep : RangeInt
{
    [SerializeField, Tooltip("Step values used when producing random numbers.")]
    // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
    private int step;
#pragma warning restore CS0649

    /// <summary>
    /// Step values used when producing random numbers.
    /// </summary>
    public int Step => step;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> in intervals of <see cref="step"/>.
    /// </summary>
    public override int Value => base.Value / step * step;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> without using <see cref="step"/>.
    /// </summary>
    public int ValueWithoutStep => Value;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="source"><see cref="RangeIntStep"/> instance used to determine the random float.</param>
    public static explicit operator int(RangeIntStep source) => source.Value;
}