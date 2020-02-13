using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomGetter<T, U> : AtomGet<U>
    {
        [SerializeField]
        protected T value;
    }
}