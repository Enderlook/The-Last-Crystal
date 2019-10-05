using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, ISerializationCallbackReceiver
{
    private Dictionary<TKey, TValue> dictionary;

    private Dictionary<TKey, TValue> Dictionary {
        get {
            if (dictionary == null)
                dictionary = new Dictionary<TKey, TValue>();
            return dictionary;
        }
        set => dictionary = value;
    }

    [SerializeField]
    private TKey[] keys;
    [SerializeField]
    private TValue[] values;

    public SerializableDictionary() => Dictionary = new Dictionary<TKey, TValue>();
    public SerializableDictionary(int capacity) => Dictionary = new Dictionary<TKey, TValue>(capacity);
    public SerializableDictionary(IDictionary<TKey, TValue> dictionary) => Dictionary = new Dictionary<TKey, TValue>(dictionary);

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keys = new TKey[Dictionary.Count];
        values = new TValue[Dictionary.Count];
        int i = 0;
        foreach (KeyValuePair<TKey, TValue> keyValuePair in Dictionary)
        {
            keys[i] = keyValuePair.Key;
            values[i] = keyValuePair.Value;
            i++;
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Length; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }
        keys = null;
        values = null;
    }

    public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)Dictionary).Keys;

    public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)Dictionary).Values;

    public int Count => ((IDictionary<TKey, TValue>)Dictionary).Count;

    public bool IsReadOnly => ((IDictionary<TKey, TValue>)Dictionary).IsReadOnly;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Values;

    public bool IsSynchronized => ((ICollection)Dictionary).IsSynchronized;

    public object SyncRoot => ((ICollection)Dictionary).SyncRoot;

    public bool IsFixedSize => ((IDictionary)Dictionary).IsFixedSize;

    ICollection IDictionary.Keys => ((IDictionary)Dictionary).Keys;

    ICollection IDictionary.Values => ((IDictionary)Dictionary).Values;

    public object this[object key] { get => ((IDictionary)Dictionary)[key]; set => ((IDictionary)Dictionary)[key] = value; }
    public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)Dictionary)[key]; set => ((IDictionary<TKey, TValue>)Dictionary)[key] = value; }

    public void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>)Dictionary).Add(key, value);
    public bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>)Dictionary).ContainsKey(key);
    public bool Remove(TKey key) => ((IDictionary<TKey, TValue>)Dictionary).Remove(key);
    public bool TryGetValue(TKey key, out TValue value) => ((IDictionary<TKey, TValue>)Dictionary).TryGetValue(key, out value);
    public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)Dictionary).Add(item);
    public void Clear() => ((IDictionary<TKey, TValue>)Dictionary).Clear();
    public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)Dictionary).Contains(item);
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)Dictionary).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)Dictionary).Remove(item);
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IDictionary<TKey, TValue>)Dictionary).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<TKey, TValue>)Dictionary).GetEnumerator();
    public void CopyTo(Array array, int index) => ((ICollection)Dictionary).CopyTo(array, index);
    public void Add(object key, object value) => ((IDictionary)Dictionary).Add(key, value);
    public bool Contains(object key) => ((IDictionary)Dictionary).Contains(key);
    IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)Dictionary).GetEnumerator();
    public void Remove(object key) => ((IDictionary)Dictionary).Remove(key);
}