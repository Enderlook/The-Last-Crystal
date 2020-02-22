namespace Additions.Serializables.Atoms
{
    public abstract class AtomGet<T> : Atom, IGet<T>
    {
        /// <summary>
        /// Boxed value of <see cref="Value"/>.
        /// </summary>
        public object ObjectValue => Value;

        /// <summary>
        /// Value stored in this <see cref="Atom"/>.
        /// </summary>
        public abstract T Value { get; }
    }
}