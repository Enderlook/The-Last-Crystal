using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializableList<T> : IList<T>, ISerializationCallbackReceiver
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
}