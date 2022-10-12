using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    [SerializeField] public WeightedRandomList<Rarity> rarityTiers;
    [SerializeField] public Rarity myRarity;
    //[SerializeField] public float amount;
    //[SerializeField] public int value;
    //public WeightedRandomList<PowerUpEffect> RarityPool;

    private void Start()
    {
        myRarity = rarityTiers.GetRandom();
        powerUpEffect.amount *= myRarity.amountMultiplier;
        powerUpEffect.value *= myRarity.valueMultiplier;
    }

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject);
    }
}
