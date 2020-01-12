namespace Additions.Serializables.Ranges
{
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
}