using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Range
{
    public interface IBasicRangeInt<T> : IBasicRange<T>
    {
        /// <summary>
        /// Return a random integer value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        int ValueInt { get; }
    }

    public interface IRangeInt<T> : IRange<T>, IBasicRangeInt<T> { }

    public interface IRangeStepInt<T> : IRangeStep<T>, IRangeInt<T>
    {
        /// <summary>
        /// Return a random integer value between <see cref="IRangeStep{T}.Min"/> and <see cref="IRangeStep{T}.Max"/> without using interval <see cref="IRangeStep{T}.Step"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        int ValueIntWithoutStep { get; }

        /// <summary>
        /// Return a random integer value between <see cref="IRangeStep{T}.Min"/> and <see cref="IRangeStep{T}.Max"/> using interval <see cref="IRangeStep{T}.Step"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        new int ValueInt { get; }
    }

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

    [Serializable]
    public class RangeIntStep : RangeInt, IRangeStep<int>, IRangeStepInt<int>
    {
        [SerializeField, Tooltip("Step values used when producing random numbers.")]
        // Used in RangeStepDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
        private int step;
#pragma warning restore CS0649

        public int Step => step;

        public override int Value => base.Value / step * step;

        public int ValueWithoutStep => base.Value;

        int IRangeStepInt<int>.ValueIntWithoutStep => ValueWithoutStep;

        int IRangeStepInt<int>.ValueInt => Value;

        int IRangeStep<int>.ValueWithoutStep => ValueWithoutStep;

        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="source"><see cref="RangeIntStep"/> instance used to determine the random float.</param>
        public static explicit operator int(RangeIntStep source) => source.Value;
    }
}