using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Chest
{

    public override void OpenChest(GameObject gameObject)
    {
        if (!isOpen )
        {
            isOpen = true;
            Debug.Log("Chest is now open...");
            if (spawnedObject == null)
                SpawnItem(1);
        }
    }

}
