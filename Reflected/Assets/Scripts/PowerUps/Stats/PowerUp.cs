using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    [SerializeField] public WeightedRandomList<Rarity> rarityTiers;
    public Rarity myRarity;

    private void Start()
    {
        myRarity = rarityTiers.GetRandom();
        powerUpEffect.amount *= myRarity.amountMultiplier;
        powerUpEffect.value *= myRarity.valueMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerUpEffect.Apply(other.gameObject);
        }        
    }
}
