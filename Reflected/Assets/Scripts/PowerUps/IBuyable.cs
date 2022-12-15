using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    public int GetValue();
    public string GetDescription();
    public void ScalePrice(int scale);
    //public Rarity GetRarity(); //Does diamods and mirror shards scale or do they have a fixed value?
}
