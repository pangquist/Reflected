using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPoolManager : MonoBehaviour
{
    [SerializeField] WeightedRandomList<GameObject> powerupPool;
    [SerializeField] WeightedRandomList<GameObject> truePowerupPool;
    [SerializeField] WeightedRandomList<GameObject> mirrorPowerupPool;
    [SerializeField] WeightedRandomList<GameObject> collectablePool;
    [SerializeField] WeightedRandomList<Rarity> rarityTiers;
    [SerializeField] Dictionary<PowerUpEffect, int> powerupPickAmount;

    private void Start()
    {
        powerupPickAmount = new Dictionary<PowerUpEffect, int>();
        foreach (var pair in powerupPool.list)
        {
            powerupPickAmount.Add(pair.item.GetComponent<InteractablePowerUp>().powerUpEffect, 0);
        }
        //for (int i = 0; i < powerupPool.Count; i++)
        //{
        //    powerupPickAmount.Add(powerupPool.list[i].item.GetComponent<InteractablePowerUp>().powerUpEffect, 0);
        //}
    }

    private void UpdateWeights()
    {
        
    }

    public void SetRarityTiers(int commonWeight, int rareWeight, int epicWeight)
    {
        rarityTiers.SetWeight(0, commonWeight);
        rarityTiers.SetWeight(1, rareWeight);
        rarityTiers.SetWeight(2, epicWeight);
    }

    public void IncreaseRarity()
    {
        rarityTiers.IncreaseWeight(1);
        rarityTiers.IncreaseWeight(2);
    }

    private void OnEnable()
    {
        InteractablePowerUp.OnPowerUPCollected += AddPowerupPickRate;
    }

    private void OnDisable()
    {
        InteractablePowerUp.OnPowerUPCollected -= AddPowerupPickRate;
    }

    private void AddPowerupPickRate(PowerUpEffect powerupEffectData)
    {
        powerupPickAmount[powerupEffectData] += 1;
        Debug.Log(powerupPickAmount[powerupEffectData]);
    }

    public WeightedRandomList<GameObject> GetPowerupPool(bool dimension)
    {
        if (dimension) return truePowerupPool;
        else return mirrorPowerupPool;
    }

    public WeightedRandomList<GameObject> GetPowerupPool()
    {
        return powerupPool;
    }

    public WeightedRandomList<GameObject> GetCollectablePool()
    {
        return collectablePool;
    }

    public WeightedRandomList<Rarity> GetRarityList()
    {
        return rarityTiers;
    }

    public Rarity GetRandomRarity()
    {
        return rarityTiers.GetRandom();
    }

    public int GetAmountPicked(PowerUpEffect powerupEffectData)
    {
        return powerupPickAmount[powerupEffectData];
    }
}
