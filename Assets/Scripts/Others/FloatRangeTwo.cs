using UnityEngine;

[System.Serializable]
public class FloatRangeTwo
{
    [Tooltip("Start.")]
    public float start;
    [Tooltip("End.")]
    public float end;
    [Tooltip("Whenever it should return only the highest vector or a random vector made by two vectors.")]
    public bool isRandom = false;

    /// <summary>
    /// Return the highest bound of the range.<br/>
    /// </summary>
    public float Max => Mathf.Max(start, end);

    /// <summary>
    /// Return the lowest bound of the range.<br/>
    /// </summary>
    public float Min => Mathf.Min(start, end);

    /// <summary>
    /// Return a random number between <see cref="start"/> and <see cref="end"/>.
    /// </summary>
    /// <param name="x"><see cref="FloatRangeTwo"/> instance used to determine the random float.</param>
    public static explicit operator float(FloatRangeTwo x) => x.isRandom ? Random.Range(x.Min, x.Max) : x.Max;

    /// <summary>
    /// Return a random number between <see cref="start"/> and <see cref="end"/>.<br/>
    /// The result is always a whole number.  Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="x"><see cref="FloatRangeTwo"/> instance used to determine the random int.</param>
    public static explicit operator int(FloatRangeTwo x) => FloatToIntByChance((float)x);

    /// <summary>
    /// Return an whole number. Decimal numbers are used as chance to increment by one.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int FloatToIntByChance(float number) => (int)number + (Random.value < (number - (int)number) ? 1 : 0);
}