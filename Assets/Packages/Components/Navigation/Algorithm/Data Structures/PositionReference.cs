namespace Navigation
{
    /// <summary>
    /// How position of <see cref="Node"/>s is calculated.
    /// </summary>
    public enum PositionReference
    {
        /// <summary>
        /// Position is calculated by local coordinates from <see cref="reference"/>.
        /// </summary>
        LOCAL,
        /// <summary>
        /// Position is calculated without <see cref="reference"/>.
        /// </summary>
        WORLD
    }
}