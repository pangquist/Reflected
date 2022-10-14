using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IBuyable
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerUpEffect.Apply(other.gameObject, amount);
        }        
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
