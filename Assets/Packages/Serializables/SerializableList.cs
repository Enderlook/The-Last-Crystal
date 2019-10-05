using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializableList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList, ISerializationCallbackReceiver
{
    private List<T> list;
    [SerializeField, HideInInspector]
    private T[] array;

    public SerializableList() => list = new List<T>();
    public SerializableList(IEnumerable<T> enumerable) => list = new List<T>(enumerable);
    public SerializableList(int capacity) => list = new List<T>(capacity);
    public SerializableList(List<T> list) => this.list = list;

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        array = list.ToArray();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        list = array.ToList();
        array = null;
    }

    public List<T> GetList() => list;

    public T this[int index] { get => ((IList<T>)list)[index]; set => ((IList<T>)list)[index] = value; }

    public int Count => ((IList<T>)list).Count;

    public bool IsReadOnly => ((IList<T>)list).IsReadOnly;

    public bool IsSynchronized => ((ICollection)list).IsSynchronized;

    public object SyncRoot => ((ICollection)list).SyncRoot;

    public bool IsFixedSize => ((IList)list).IsFixedSize;

    object IList.this[int index] { get => ((IList)list)[index]; set => ((IList)list)[index] = value; }

    public void Add(T item) => ((IList<T>)list).Add(item);
    public void Clear() => ((IList<T>)list).Clear();
    public bool Contains(T item) => ((IList<T>)list).Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => ((IList<T>)list).CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => ((IList<T>)list).GetEnumerator();
    public int IndexOf(T item) => ((IList<T>)list).IndexOf(item);
    public void Insert(int index, T item) => ((IList<T>)list).Insert(index, item);
    public bool Remove(T item) => ((IList<T>)list).Remove(item);
    public void RemoveAt(int index) => ((IList<T>)list).RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => ((IList<T>)list).GetEnumerator();
    public void CopyTo(Array array, int index) => ((ICollection)list).CopyTo(array, index);
    public int Add(object value) => ((IList)list).Add(value);
    public bool Contains(object value) => ((IList)list).Contains(value);
    public int IndexOf(object value) => ((IList)list).IndexOf(value);
    public void Insert(int index, object value) => ((IList)list).Insert(index, value);
    public void Remove(object value) => ((IList)list).Remove(value);
}