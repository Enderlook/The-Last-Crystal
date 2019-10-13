using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RangeFloat : Range<float>
{
    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    public override float Value => Random.Range(Min, Max);

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    public virtual int ValueInt => FloatToIntByChance(Value);

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random float.</param>
    public static explicit operator float(RangeFloat source) => source.Value;
    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random int.</param>
    public static explicit operator int(RangeFloat source) => source.ValueInt;

    /// <summary>
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int FloatToIntByChance(float number) => (int)number + (Random.value < (number - (int)number) ? 1 : 0);
}

[Serializable]
public class RangeFloatStep : RangeFloat, IRangeStep<float>
{
    [SerializeField, Tooltip("Step values used when producing random numbers.")]
    // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
    private float step;
#pragma warning restore CS0649

    /// <summary>
    /// Step values used when producing random numbers.
    /// </summary>
    public float Step => step;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> in intervals of <see cref="step"/>.
    /// </summary>
    public override float Value => (float)Math.Round(base.Value / step, MidpointRounding.AwayFromZero) * step;
    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> in intervals of <see cref="step"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    public override int ValueInt => base.ValueInt / (int)step * (int)step;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> without using <see cref="step"/>.
    /// </summary>
    public float ValueWithoutStep => base.Value;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/> without using <see cref="step"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    public int ValueIntWithoutStep => base.ValueInt;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random float.</param>
    public static explicit operator float(RangeFloatStep source) => source.Value;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random int.</param>
    public static explicit operator int(RangeFloatStep source) => source.ValueInt;
}