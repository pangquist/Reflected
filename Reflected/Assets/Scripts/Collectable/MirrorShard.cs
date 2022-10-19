using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MirrorShard : MonoBehaviour, ICollectable, IBuyable
{
    public static event HandleShardCollected OnShardCollected;
    public delegate void HandleShardCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData mirroShardData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    public void Collect()
    {
        Debug.Log("You collected a mirror shard");
        Destroy(gameObject);
        OnShardCollected?.Invoke(mirroShardData); //?. makes sure it's not null and that there are listeners to the event
    }

    public int GetValue()
    {
        return mirroShardData.value;
    }

    public string GetDescription()
    {
        return mirroShardData.description;
    }
}
