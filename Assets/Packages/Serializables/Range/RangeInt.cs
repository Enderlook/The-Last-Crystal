using System;

using Random = UnityEngine.Random;

namespace Additions.Serializables.Ranges
{
    [Serializable]
    public class RangeInt : Range<int>, IRangeInt<int>
    {
        public override int Value => Random.Range(Min, Max);

        int IBasicRangeInt<int>.ValueInt => Value;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeInt"/> instance used to determine the random int.</param>
        public static explicit operator int(RangeInt source) => source.Value;
    }
}