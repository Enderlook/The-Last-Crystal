using Additions.Attributes;
using Additions.Attributes.AttributeUsage;
using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomGetter<T, U> : AtomGet<U>
    {
        // Don't check because generic types can't be serialized by Unity, but this class is just a template
        [DoNotCheck(typeof(ExpandableAttribute))]
        [SerializeField, Expandable]
        protected T value;
    }
}