using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemObject/HealthObject")]
public class HealthObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Health;
        amount = 1;
        restoreHealthValue = 5;
    }
}
