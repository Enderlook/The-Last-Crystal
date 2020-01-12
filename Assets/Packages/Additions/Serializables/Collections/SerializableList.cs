using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Additions.Serializables.Collections
{
    [Serializable]
    public class SerializableList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList, ISerializationCallbackReceiver
    {
        private List<T> list;
        private List<T> List {
            get => list ?? (list = new List<T>());
            set => list = value;
        }

        [SerializeField, HideInInspector]
        private T[] array;

        public SerializableList() => List = new List<T>();

        public SerializableList(IEnumerable<T> enumerable) => List = new List<T>(enumerable);

        public SerializableList(int capacity) => List = new List<T>(capacity);

        public SerializableList(List<T> list) => List = list;

        void ISerializationCallbackReceiver.OnBeforeSerialize() => array = List.ToArray();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            List = array.ToList();
            array = null;
        }

        public List<T> GetList() => List;

        public T this[int index] { get => ((IList<T>)List)[index]; set => ((IList<T>)List)[index] = value; }

        public int Count => ((IList<T>)List).Count;

        public bool IsReadOnly => ((IList<T>)List).IsReadOnly;

        public bool IsSynchronized => ((ICollection)List).IsSynchronized;

        public object SyncRoot => ((ICollection)List).SyncRoot;

        public bool IsFixedSize => ((IList)List).IsFixedSize;

        object IList.this[int index] { get => ((IList)List)[index]; set => ((IList)List)[index] = value; }

        public void Add(T item) => ((IList<T>)List).Add(item);
        public void Clear() => ((IList<T>)List).Clear();
        public bool Contains(T item) => ((IList<T>)List).Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => ((IList<T>)List).CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => ((IList<T>)List).GetEnumerator();
        public int IndexOf(T item) => ((IList<T>)List).IndexOf(item);
        public void Insert(int index, T item) => ((IList<T>)List).Insert(index, item);
        public bool Remove(T item) => ((IList<T>)List).Remove(item);
        public void RemoveAt(int index) => ((IList<T>)List).RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => ((IList<T>)List).GetEnumerator();
        public void CopyTo(Array array, int index) => ((ICollection)List).CopyTo(array, index);
        public int Add(object value) => ((IList)List).Add(value);
        public bool Contains(object value) => ((IList)List).Contains(value);
        public int IndexOf(object value) => ((IList)List).IndexOf(value);
        public void Insert(int index, object value) => ((IList)List).Insert(index, value);
        public void Remove(object value) => ((IList)List).Remove(value);
    }

    public sealed class ShowListAttribute : PropertyAttribute { }
}