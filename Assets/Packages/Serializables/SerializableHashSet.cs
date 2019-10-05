using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, ISerializationCallbackReceiver
{
    private HashSet<T> hashSet;
    [SerializeField, HideInInspector]
    private T[] array;

    public SerializableHashSet() => hashSet = new HashSet<T>();
    public SerializableHashSet(IEnumerable<T> enumerable) => hashSet = new HashSet<T>(enumerable);

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        array = hashSet.ToArray();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        hashSet = new HashSet<T>(array);
        array = null;
    }

    public int Count => ((ICollection<T>)hashSet).Count;

    public bool IsReadOnly => ((ICollection<T>)hashSet).IsReadOnly;
    public void Add(T item) => ((ICollection<T>)hashSet).Add(item);
    public void Clear() => ((ICollection<T>)hashSet).Clear();
    public bool Contains(T item) => ((ICollection<T>)hashSet).Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)hashSet).CopyTo(array, arrayIndex);
    public bool Remove(T item) => ((ICollection<T>)hashSet).Remove(item);
    public IEnumerator<T> GetEnumerator() => ((ICollection<T>)hashSet).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((ICollection<T>)hashSet).GetEnumerator();
    bool ISet<T>.Add(T item) => ((ISet<T>)hashSet).Add(item);
    public void ExceptWith(IEnumerable<T> other) => ((ISet<T>)hashSet).ExceptWith(other);
    public void IntersectWith(IEnumerable<T> other) => ((ISet<T>)hashSet).IntersectWith(other);
    public bool IsProperSubsetOf(IEnumerable<T> other) => ((ISet<T>)hashSet).IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<T> other) => ((ISet<T>)hashSet).IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => ((ISet<T>)hashSet).IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<T> other) => ((ISet<T>)hashSet).IsSupersetOf(other);
    public bool Overlaps(IEnumerable<T> other) => ((ISet<T>)hashSet).Overlaps(other);
    public bool SetEquals(IEnumerable<T> other) => ((ISet<T>)hashSet).SetEquals(other);
    public void SymmetricExceptWith(IEnumerable<T> other) => ((ISet<T>)hashSet).SymmetricExceptWith(other);
    public void UnionWith(IEnumerable<T> other) => ((ISet<T>)hashSet).UnionWith(other);
}
