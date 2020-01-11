using System;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeFloatSwitchable : RangeSwitchable<RangeFloat, float>, IBasicRange<float>, IBasicRangeInt<float>
    {
        public float Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Alternative ? RangeFloat.FloatToIntByChance(Value2) : Value1.ValueInt;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloatSwitchable source) => source.Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloatSwitchable source) => source.ValueInt;
    }
}