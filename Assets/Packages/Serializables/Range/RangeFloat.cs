using System;

using Random = UnityEngine.Random;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeFloat : Range<float>, IRangeInt<float>
    {
        public override float Value => Random.Range(Min, Max);

        /// <summary>
        /// Return a random integer value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        public virtual int ValueInt => FloatToIntByChance(Value);

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloat source) => source.Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloat"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloat source) => source.ValueInt;

        /// <summary>
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="number">Number to round by chance.</param>
        /// <returns>Rounded number by chance.</returns>
        public static int FloatToIntByChance(float number) => (int)number + (Random.value < number - (int)number ? 1 : 0);
    }
}