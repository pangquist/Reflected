using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPoolManager : MonoBehaviour
{
    [SerializeField] WeightedRandomList<LootPool> powerupPool;
    [SerializeField] WeightedRandomList<LootPool> collectablePool;

    private void UpdateWeights()
    {
        
    }

}
