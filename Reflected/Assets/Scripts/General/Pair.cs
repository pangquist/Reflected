using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pair<T> where T : class
{
    private T item1;
    private T item2;

    public T Item1 { get { return item1; } set { item1 = value; } }
    public T Item2 { get { return item2; } set { item2 = value; } }

    public Pair(T item1, T item2)
    {
        this.item1 = item1;
        this.item2 = item2;
    }

    public bool Contains(T item)
    {
        return item == item1 || item == item2;
    }
}
