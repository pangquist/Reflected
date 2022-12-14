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
    [SerializeField] private AudioClip audioClip;
    protected bool hasProperties;
    public static event HandlePowerupCollected OnPowerUPCollected;
    public delegate void HandlePowerupCollected(PowerUpEffect powerupData);
    //public WeightedRandomList<PowerUpEffect> RarityPool;

    protected virtual void Start() //Might not need this
    {
        if (!hasProperties)
        {
            myRarity = rarityTiers.GetRandom();
            amount = powerUpEffect.amount * myRarity.amountMultiplier;
            value = powerUpEffect.value * myRarity.valueMultiplier;
            if(powerUpEffect.powerupName == "Max_Health")
            {
                description = powerUpEffect.description + " " + amount.ToString();
            }
            else if(powerUpEffect.diminishingReturn == "false")
            {
                description = powerUpEffect.description + " " + (amount * 100).ToString() + "% of players base value.";
            }
            else
            {
                description = powerUpEffect.description + " " + (amount * 100).ToString() + "%. (This effect has diminishing returns)";
            }
            
        }
        //Destroy(gameObject, 20);
    }

    public virtual void SetProperties(Rarity targetRarity)
    {
        //Debug.Log("Set properties " + targetRarity);
        myRarity = targetRarity;
        amount = powerUpEffect.amount * targetRarity.amountMultiplier;
        value = powerUpEffect.value * targetRarity.valueMultiplier;
        if (powerUpEffect.powerupName == "Max_Health")
        {
            description = powerUpEffect.description + " " + amount.ToString();
        }
        else if (powerUpEffect.diminishingReturn == "false")
        {
            description = powerUpEffect.description + " " + (amount * 100).ToString() + "% of players base value.";
        }
        else
        {
            description = powerUpEffect.description + " " + (amount * 100).ToString() + "%. (This effect deminishing returns)";
        }
        hasProperties = true;
    }

    public void ApplyOnInteraction()
    {
        Player player = FindObjectOfType<Player>();
        player.GetComponentInParent<AudioSource>().PlayOneShot(audioClip);
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject, amount);
        OnPowerUPCollected?.Invoke(powerUpEffect);
    }

    public void ApplyOnPurchase()
    {
        Player player = FindObjectOfType<Player>();
        player.GetComponentInParent<AudioSource>().PlayOneShot(audioClip);
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

    public void ScalePrice(int scale)
    {
        value = powerUpEffect.value * myRarity.valueMultiplier * scale;
    }
}
