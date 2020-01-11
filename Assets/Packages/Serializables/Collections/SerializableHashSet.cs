using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Additions.Serializables.Collections
{
    [Serializable]
    public class SerializableHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, ISerializationCallbackReceiver
    {
        private HashSet<T> hashSet;

        private HashSet<T> HashSet {
            get => hashSet ?? (hashSet = new HashSet<T>());
            set => hashSet = value;
        }

        [SerializeField, HideInInspector]
        private T[] array;

        public SerializableHashSet() => HashSet = new HashSet<T>();

        public SerializableHashSet(IEnumerable<T> enumerable) => HashSet = new HashSet<T>(enumerable);

        void ISerializationCallbackReceiver.OnBeforeSerialize() => array = HashSet.ToArray();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            HashSet = new HashSet<T>(array);
            array = null;
        }

        public int Count => ((ICollection<T>)HashSet).Count;

        public bool IsReadOnly => ((ICollection<T>)HashSet).IsReadOnly;
        public void Add(T item) => ((ICollection<T>)HashSet).Add(item);
        public void Clear() => ((ICollection<T>)HashSet).Clear();
        public bool Contains(T item) => ((ICollection<T>)HashSet).Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)HashSet).CopyTo(array, arrayIndex);
        public bool Remove(T item) => ((ICollection<T>)HashSet).Remove(item);
        public IEnumerator<T> GetEnumerator() => ((ICollection<T>)HashSet).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((ICollection<T>)HashSet).GetEnumerator();
        bool ISet<T>.Add(T item) => ((ISet<T>)HashSet).Add(item);
        public void ExceptWith(IEnumerable<T> other) => ((ISet<T>)HashSet).ExceptWith(other);
        public void IntersectWith(IEnumerable<T> other) => ((ISet<T>)HashSet).IntersectWith(other);
        public bool IsProperSubsetOf(IEnumerable<T> other) => ((ISet<T>)HashSet).IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => ((ISet<T>)HashSet).IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => ((ISet<T>)HashSet).IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => ((ISet<T>)HashSet).IsSupersetOf(other);
        public bool Overlaps(IEnumerable<T> other) => ((ISet<T>)HashSet).Overlaps(other);
        public bool SetEquals(IEnumerable<T> other) => ((ISet<T>)HashSet).SetEquals(other);
        public void SymmetricExceptWith(IEnumerable<T> other) => ((ISet<T>)HashSet).SymmetricExceptWith(other);
        public void UnionWith(IEnumerable<T> other) => ((ISet<T>)HashSet).UnionWith(other);
    }
}