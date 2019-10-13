using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public abstract class Range<T>
{
    [SerializeField]
    // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
    private T min;
    [SerializeField]
    // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
    private T max;

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
    private int step;

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
public class RangeFloatStep : RangeFloat
{
    [SerializeField, Tooltip("Step values used when producing random numbers.")]
    // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
    private float step;

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