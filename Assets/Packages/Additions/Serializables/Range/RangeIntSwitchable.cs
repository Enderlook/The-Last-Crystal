using System;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeIntSwitchable : RangeSwitchable<RangeInt, int>, IBasicRangeInt<int>
    {
        public int Value => Alternative ? Value2 : Value1.Value;

        public int ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntSwitchable"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeIntSwitchable source) => source.Value;
    }
}