namespace Additions.Components.SoundSystem
{
    /// <summary>
    /// Playmodes.
    /// </summary>
    public enum PlayMode
    {
        /// <summary>
        /// Display form first to last and them go back to beginning.
        /// </summary>
        Next,

        /// <summary>
        /// Display form first to last and from last to first.
        /// </summary>
        PingPong,

        /// <summary>
        /// Display randomly.
        /// </summary>
        Random
    };
}