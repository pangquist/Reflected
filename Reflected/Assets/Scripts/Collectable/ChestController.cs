using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Chest
{

    public override void OpenChest(int index)
    {
        if (!isOpen )
        {
            isOpen = true;
            //Debug.Log("Chest is now open...");
            if (spawnedObject == null)
                SpawnItem(index);

            Destroy(gameObject, 20);
        }
    }
}
