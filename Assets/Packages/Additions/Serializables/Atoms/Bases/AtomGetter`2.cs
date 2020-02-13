using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomGetter<T, U> : Atom
    {
        [SerializeField]
        protected T value;

        public abstract U Value { get; }

        /// <summary>
        /// Boxed value of <see cref="Value"/>
        /// </summary>
        public object ObjectValue => Value;
    }
}