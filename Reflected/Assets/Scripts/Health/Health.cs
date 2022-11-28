using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IMagnetic, IBuyable
{
    public PowerUpEffect powerUpEffect;
  
    Rigidbody rb;
    bool hasTarget, hasProperties;
    Vector3 targetPosition;
    float moveSpeed = 5f;
    float totalplayerhealth;
    int amount;
    Rarity myRarity;
    string description;
    int value;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!hasProperties)
        {
            myRarity = FindObjectOfType<LootPoolManager>().GetRandomRarity();
            totalplayerhealth = FindObjectOfType<PlayerStatSystem>().GetMaxHealthIncrease() + FindObjectOfType<Player>().GetMaxHealth();
            amount = (int)((totalplayerhealth * 0.15f) * myRarity.amountMultiplier);
            value = powerUpEffect.value * myRarity.valueMultiplier;
            description = powerUpEffect.description + " " + amount.ToString();
        }
        //Destroy(gameObject, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Destroy(gameObject);
            Debug.Log(amount);
            powerUpEffect.Apply(other.gameObject, amount); 
        }
    }

    public void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    public void SetProperties(Rarity targetRarity)
    {
        //Debug.Log("Set properties " + targetRarity);
        myRarity = targetRarity;
        totalplayerhealth = FindObjectOfType<PlayerStatSystem>().GetMaxHealthIncrease() + FindObjectOfType<Player>().GetMaxHealth();
        amount = (int)((totalplayerhealth / 4f) * myRarity.amountMultiplier);
        value = powerUpEffect.value * targetRarity.valueMultiplier;
        description = powerUpEffect.description + " " + amount.ToString();
        hasProperties = true;
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
