using System;

using UnityEngine;

namespace Range
{
    public interface IBasicRange<T>
    {
        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        T Value { get; }
    }

    public interface IRange<T> : IBasicRange<T>
    {
        /// <summary>
        /// Return the highest bound of the range.<br/>
        /// </summary>
        T Max { get; }

        /// <summary>
        /// Return the lowest bound of the range.<br/>
        /// </summary>
        T Min { get; }
    }

    [Serializable]
    public abstract class Range<T> : IRange<T>
    {
        [SerializeField, Tooltip("Minimal value (lower bound) of the range.")]
        // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
#pragma warning disable CS0649
        private T min;
        [SerializeField, Tooltip("Maximum value (upper bound) of the range.")]
        // Used in RangeDrawer as string name. Don't forget to change string if this is renamed.
        private T max;
#pragma warning restore CS0649

        /// <summary>
        /// Return the highest bound of the range.<br/>
        /// </summary>
        public T Max => max;

        /// <summary>
        /// Return the lowest bound of the range.<br/>
        /// </summary>
        public T Min => min;

        public abstract T Value { get; }
    }

    public interface IRangeStep<T> : IRange<T>
    {
        /// <summary>
        /// Return a random value between <see cref="IRange{T}.Min"/> and <see cref="IRange{T}Max"/> without using interval <see cref="Step"/>.
        /// </summary>
        T ValueWithoutStep { get; }

        /// <summary>
        /// Return a random value between <see cref="IRange{T}.Min"/> and <see cref="IRange{T}Max"/> using interval <see cref="Step"/>.
        /// </summary>
        new T Value { get; }

        /// <summary>
        /// Step values used when producing random numbers.
        /// </summary>
        T Step { get; }
    }
}