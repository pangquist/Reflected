using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LootPool/Common")]
public class CommonLootPool : LootPool
{
    public override GameObject GetItem()
    {
        return lootPool.GetRandom();
    }
}
