using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour, ICollectable
{
    public static event HandleDiamondCollected OnDiamondCollected;
    public delegate void HandleDiamondCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData diamondData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    public void Collect()
    {
        Debug.Log("You collected a Diamond");
        Destroy(gameObject);
        OnDiamondCollected?.Invoke(diamondData);
    }
}
