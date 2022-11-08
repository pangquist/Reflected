using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedRandomList<T>
{
    [System.Serializable]
    public class Pair
    {
        public T item;
        public float weight;

        public Pair(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        public void IncreaseWeight()
        {
            this.weight++;
        }
    }

    public List<Pair> list = new List<Pair>();

    public int Count
    {
        get => list.Count;
    }

    public void Add(T item, float weight)
    {
        list.Add(new Pair(item, weight));
    }

    public void Remove(int index)
    {
        list.RemoveAt(index);
    }

    public T GetRandom()
    {
        float totalWeight = 0;

        foreach (Pair p in list)
        {
            totalWeight += p.weight;
        }

        float value = Random.value * totalWeight;

        float sumWeight = 0;

        foreach(Pair p in list)
        {
            sumWeight += p.weight;

            if(sumWeight >= value)
            {
                return p.item;
            }
        }

        return default(T);
    }

    public void SetWeight(int index, float weight)
    {
        list[index].SetWeight(weight);
    }

    public void IncreaseWeight(int index)
    {
        list[index].IncreaseWeight();
    }
}
