using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/DamageReduction")]
public class DamageReduction : PowerUpEffect
{
    
    public override void Apply(GameObject target)
    {
        target.GetComponent<StatSystem>().AddDamageReduction(amount);
        Debug.Log("Reduction +" + amount);
        
    }

    public void Awake()
    {
        //myRarity = rarityTiers.GetRandom();
        //Debug.Log("Rarity +" + myRarity.rarity);
        //amount = amount * myRarity.amountMultiplier;
        //Debug.Log("Reduction +" + amount);
        //value = value * myRarity.valueMultiplier;
        description = "Increases your damage reduction by " + amount;
    }
}
