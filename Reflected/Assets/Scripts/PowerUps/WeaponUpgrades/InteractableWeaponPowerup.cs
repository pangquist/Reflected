using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWeaponPowerup : InteractablePowerUp
{
    private string descriptionFire = "fire damage that deals damage over time to enemies";
    private string descriptionFreeze = "freze effect that slows down enemies";
    private string descriptionLife = "life regen that heals you when you hit an enemy";
    // Start is called before the first frame update
    protected override void Start()
    {
        if (!hasProperties)
        {
            amount = Random.Range(0, 3);
            value = powerUpEffect.value;
            SetDescription((int)amount);
        }
        //Destroy(gameObject, 20);
    }

    public void SetProperties(int index)
    {
        //Debug.Log("Set properties " + targetRarity);
        amount = index;
        value = powerUpEffect.value;
        SetDescription((int)index);
        hasProperties = true;
    }

    private void SetDescription(int index)
    {
        switch (index)
        {
            case 0:
                description = powerUpEffect.description + " " + descriptionFire;
                break;
            case 1:
                description = powerUpEffect.description + " " + descriptionFreeze;
                break;
            case 2:
                description = powerUpEffect.description + " " + descriptionLife;
                break;
            default:
                description = powerUpEffect.description + " nothing";
                break;

        }
        
    }
}
