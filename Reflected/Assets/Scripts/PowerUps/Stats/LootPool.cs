using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LootPool : ScriptableObject
{
    [SerializeField] protected WeightedRandomList<GameObject> lootPool;

    public abstract GameObject GetItem();    
}
