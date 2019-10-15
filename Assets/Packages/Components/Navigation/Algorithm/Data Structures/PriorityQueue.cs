using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<Tuple<float, T>> list = new List<Tuple<float, T>>();

    public int Count => list.Count;

    public void Enqueue(T item, float priority) => list.Add(new Tuple<float, T>(priority, item));

    public T DequeueMin()
    {
        if (list.Count == 0) return default;
        float min = float.MaxValue;
        int index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            float priority = list[i].Item1;
            if (priority < min)
            {
                min = priority;
                index = i;
            }
        }
        T value = list[index].Item2;
        list.RemoveAt(index);
        return value;
    }

    public T DequeueMax()
    {
        if (list.Count == 0) return default;
        float min = -float.MaxValue;
        int index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            float priority = list[i].Item1;
            if (priority > min)
            {
                min = priority;
                index = i;
            }
        }
        T value = list[index].Item2;
        list.RemoveAt(index);
        return value;
    }

}