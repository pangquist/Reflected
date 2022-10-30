using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairList<T> where T : class
{
    private List<Pair<T>> list;

    public PairList()
    {
        list = new List<Pair<T>>();
    }

    /// <summary>
    /// Adds the provided pair to the list
    /// </summary>
    public void Add(T item1, T item2)
    {
        list.Add(new Pair<T>(item1, item2));
    }

    /// <summary>
    /// Removes the first occurence of the provided pair 
    /// </summary>
    public void RemoveFirst(T item1, T item2)
    {
        if (list.Remove(new Pair<T>(item1, item2)) == false)
            list.Remove(new Pair<T>(item2, item1));
    }

    /// <summary>
    /// Removes all pairs containing the provided item
    /// </summary>
    public void RemoveAll(T item)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i].Contains(item))
                list.RemoveAt(i--);
    }

    /// <summary>
    /// Returns whether or this list contains the provided item
    /// </summary>
    public bool Contains(T item)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i].Contains(item))
                return true;
        return false;
    }

    /// <summary>
    /// Returns whether or this list contains the provided pair
    /// </summary>
    public bool Contains(T item1, T item2)
    {
        foreach (Pair<T> pair in list)
            if (pair.Contains(item1) && pair.Contains(item2))
                return true;
        return false;
    }

    /// <summary>
    /// Removes all pairs from this list
    /// </summary>
    public void Clear()
    {
        list.Clear();
    }

    /// <summary>
    /// Gets an item from the list at the specified index
    /// </summary>
    public Pair<T> this[int i] => list[i];

    /// <summary>
    /// Gets the number of pairs in this list
    /// </summary>
    public int Count => list.Count;
}

