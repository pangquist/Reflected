using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWeaponPowerup : InteractablePowerUp
{
    // Start is called before the first frame update
    protected override void Start()
    {
        if (!hasProperties)
        {
            amount = powerUpEffect.amount;
            value = powerUpEffect.value;
            description = powerUpEffect.description;
        }
        //Destroy(gameObject, 20);
    }

    public void SetProperties()
    {
        //Debug.Log("Set properties " + targetRarity);
        amount = powerUpEffect.amount;
        value = powerUpEffect.value;
        description = powerUpEffect.description;
        hasProperties = true;
    }

}
