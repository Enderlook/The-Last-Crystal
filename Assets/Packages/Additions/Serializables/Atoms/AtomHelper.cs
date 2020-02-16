namespace Additions.Serializables.Atoms
{
    public static class AtomHelper
    {
        /// <summary>
        /// Get value of the atom. Only works in atoms derived from <see cref="IGet{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of value to get.</typeparam>
        /// <param name="source">Atom where the value is get.</param>
        /// <returns>Value of the atom.</returns>
        public static T GetValue<T>(this Atom source) => ((IGet<T>)source).Value;
    }
}