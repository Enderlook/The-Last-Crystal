using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomGetter<T, U> : Atom, IGet<U> where T : IGet<U>
    {
        [SerializeField]
        private T value;

        /// <summary>
        /// Boxed value of <see cref="Value"/>
        /// </summary>
        public object ObjectValue => Value;

        /// <summary>
        /// <see cref="value"/> as property.
        /// </summary>
        public U Value => value.Value;
    }
}