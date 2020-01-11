namespace Additions.Serializables.Ranges
{
    public interface IBasicRangeInt<T> : IBasicRange<T>
    {
        /// <summary>
        /// Return a random integer value between <see cref="Min"/> and <see cref="Max"/>.
        /// The result is always an integer. Decimal numbers are used as chance to increment by one.
        /// </summary>
        int ValueInt { get; }
    }
}