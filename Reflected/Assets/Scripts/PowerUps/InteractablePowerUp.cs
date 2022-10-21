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
    [SerializeField] public string description;
    bool hasProperties;
    public static event HandlePowerupCollected OnPowerUPCollected;
    public delegate void HandlePowerupCollected(PowerUpEffect powerupData);
    //public WeightedRandomList<PowerUpEffect> RarityPool;

    private void Start() //Might not need this
    {
        if (!hasProperties)
        {
            myRarity = rarityTiers.GetRandom();
            amount = powerUpEffect.amount * myRarity.amountMultiplier;
            value = powerUpEffect.value * myRarity.valueMultiplier;
            description = powerUpEffect.description + amount.ToString();
        }        
    }

    public void SetProperties(Rarity targetRarity)
    {
        Debug.Log("Set properties " + targetRarity);
        myRarity = targetRarity;
        amount = powerUpEffect.amount * targetRarity.amountMultiplier;
        value = powerUpEffect.value * targetRarity.valueMultiplier;
        description = powerUpEffect.description + amount.ToString();
        hasProperties = true;
    }

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject, amount);
        OnPowerUPCollected?.Invoke(powerUpEffect);
    }

    public int GetValue()
    {
        return value;
    }

    public string GetDescription()
    {
        return description;
    }

    public Rarity GetRarity()
    {
        return myRarity;
    }
}
