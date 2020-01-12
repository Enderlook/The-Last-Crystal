namespace Additions.Serializables.Ranges
{
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