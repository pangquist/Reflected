using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePowerUp : MonoBehaviour, IBuyable
{
    public PowerUpEffect powerUpEffect;
    [SerializeField] public WeightedRandomList<Rarity> rarityTiers;
    [SerializeField] public Rarity myRarity;
    [SerializeField] public float amount;
    [SerializeField] public int value;
    //public WeightedRandomList<PowerUpEffect> RarityPool;

    private void Start()
    {
        myRarity = rarityTiers.GetRandom();
        amount = powerUpEffect.amount * myRarity.amountMultiplier;
        value = powerUpEffect.value * myRarity.valueMultiplier;
    }

    public void SetProperties(Rarity targetRarity)
    {
        Debug.Log("Set properties " + targetRarity);
        myRarity = targetRarity;
        amount = powerUpEffect.amount * targetRarity.amountMultiplier;
        value = powerUpEffect.value * targetRarity.valueMultiplier;              
    }

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject, amount);
    }

    public int GetValue()
    {
        return value;
    }

    public string GetDescription()
    {
        return powerUpEffect.description;
    }
}
