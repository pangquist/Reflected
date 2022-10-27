using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LootPool/PowerUp")]
public class LootPoolPowerUp : LootPool
{
    public override GameObject GetItem()
    {
        return lootPool.GetRandom();
    }
}
