using System;
using Serializables;

namespace Range
{
    [Serializable]
    public class RangeFloatSwitchable : Switch<RangeFloat, float>, IBasicRange<float>, IBasicRangeInt<float>
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

    [Serializable]
    public class RangeFloatStepSwitchable : Switch<RangeFloatStep, float>, IBasicRange<float>, IBasicRangeInt<float>
    {
        public float Value => Alternative ? Value2 : Value1.Value;
        public int ValueInt => Alternative ? RangeFloat.FloatToIntByChance(Value2) : Value1.ValueInt;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random float.</param>
        public static explicit operator float(RangeFloatStepSwitchable source) => source.Value;
        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        /// <param name="source"><see cref="RangeFloatSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeFloatStepSwitchable source) => source.ValueInt;
    }

    [Serializable]
    public class RangeIntSwitchable : Switch<RangeInt, int>, IBasicRangeInt<int>
    {
        public int Value => Alternative ? Value2 : Value1.Value;
        public int ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeIntSwitchable source) => source.Value;
    }

    [Serializable]
    public class RangeIntStepSwitchable : Switch<RangeIntStep, int>, IBasicRangeInt<int>
    {
        public int Value => Alternative ? Value2 : Value1.Value;
        public int ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntStepSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeIntStepSwitchable source) => source.Value;
    }
}