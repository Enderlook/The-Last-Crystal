namespace Additions.Components.Navigation
{
    /// <summary>
    /// Mode at which <see cref="Node"/>s or <see cref="Connection"/>s are set active.
    /// </summary>
    public enum ToggleMode
    {
        /// <summary>
        /// Enable element.
        /// </summary>
        Enable,

        /// <summary>
        /// Disable element.
        /// </summary>
        Disable,

        /// <summary>
        /// If enabled, disable it. If disabled, enable it.
        /// </summary>
        Toggle,
    }
}