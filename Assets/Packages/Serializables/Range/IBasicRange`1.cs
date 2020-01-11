namespace Additions.Serializables.Ranges
{
    public interface IBasicRange<T>
    {
        /// <summary>
        /// Return a random value between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        T Value { get; }
    }
}