using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<T, U> : IDictionary<T, U>, ISerializationCallbackReceiver
{
    private Dictionary<T, U> dictionary;

    private Dictionary<T, U> Dictionary {
        get {
            if (dictionary == null)
                dictionary = new Dictionary<T, U>();
            return dictionary;
        }
        set => dictionary = value;
    }

    [SerializeField]
    private T[] keys;
    [SerializeField]
    private U[] values;

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keys = new T[Dictionary.Count];
        values = new U[Dictionary.Count];
        int i = 0;
        foreach (KeyValuePair<T, U> keyValuePair in Dictionary)
        {
            keys[i] = keyValuePair.Key;
            values[i] = keyValuePair.Value;
            i++;
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Dictionary = new Dictionary<T, U>();
        for (int i = 0; i < keys.Length; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }
        keys = null;
        values = null;
    }

    public ICollection<T> Keys => ((IDictionary<T, U>)Dictionary).Keys;

    public ICollection<U> Values => ((IDictionary<T, U>)Dictionary).Values;

    public int Count => ((IDictionary<T, U>)Dictionary).Count;

    public bool IsReadOnly => ((IDictionary<T, U>)Dictionary).IsReadOnly;

    public U this[T key] { get => ((IDictionary<T, U>)Dictionary)[key]; set => ((IDictionary<T, U>)Dictionary)[key] = value; }

    public void Add(T key, U value) => ((IDictionary<T, U>)Dictionary).Add(key, value);
    public bool ContainsKey(T key) => ((IDictionary<T, U>)Dictionary).ContainsKey(key);
    public bool Remove(T key) => ((IDictionary<T, U>)Dictionary).Remove(key);
    public bool TryGetValue(T key, out U value) => ((IDictionary<T, U>)Dictionary).TryGetValue(key, out value);
    public void Add(KeyValuePair<T, U> item) => ((IDictionary<T, U>)Dictionary).Add(item);
    public void Clear() => ((IDictionary<T, U>)Dictionary).Clear();
    public bool Contains(KeyValuePair<T, U> item) => ((IDictionary<T, U>)Dictionary).Contains(item);
    public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex) => ((IDictionary<T, U>)Dictionary).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<T, U> item) => ((IDictionary<T, U>)Dictionary).Remove(item);
    public IEnumerator<KeyValuePair<T, U>> GetEnumerator() => ((IDictionary<T, U>)Dictionary).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<T, U>)Dictionary).GetEnumerator();
}