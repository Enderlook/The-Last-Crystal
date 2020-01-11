namespace Additions.Serializables.Ranges
{
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
}