using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FloatRange
{
    [SerializeField]
    // Used in FloatRangeDrawer as string name. Don't forget to change string if this is renamed.
    private float min;
    [SerializeField]
    // Used in FloatRangeDrawer as string name. Don't forget to change string if this is renamed.
    private float max;

    /// <summary>
    /// Return the highest bound of the range.<br/>
    /// </summary>
    public float Max => max;

    /// <summary>
    /// Return the lowest bound of the range.<br/>
    /// </summary>
    public float Min => min;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    public virtual float Value => Random.Range(Min, Max);

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    public virtual int ValueInt => FloatToIntByChance(Value);

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    /// <param name="source"><see cref="FloatRange"/> instance used to determine the random float.</param>
    public static explicit operator float(FloatRange source) => source.Value;
    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="source"><see cref="FloatRange"/> instance used to determine the random int.</param>
    public static explicit operator int(FloatRange source) => source.ValueInt;

    /// <summary>
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int FloatToIntByChance(float number) => (int)number + (Random.value < (number - (int)number) ? 1 : 0);
}

[Serializable]
public class FloatRangeStep : FloatRange
{
    [SerializeField, Tooltip("Step values used when producing random numbers.")]
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
    /// <param name="source"><see cref="FloatRange"/> instance used to determine the random float.</param>
    public static explicit operator float(FloatRangeStep source) => source.Value;

    /// <summary>
    /// Return a random number between <see cref="Min"/> and <see cref="Max"/>.
    /// The result is always an integer. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="source"><see cref="FloatRange"/> instance used to determine the random int.</param>
    public static explicit operator int(FloatRangeStep source) => source.ValueInt;
}
