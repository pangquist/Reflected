using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IMagnetic, IBuyable
{
    public PowerUpEffect powerUpEffect;
    //Check ui for health that the player gets back for the description and not the powerup effect
  
    Rigidbody rb;
    bool hasTarget, hasProperties;
    Vector3 targetPosition;
    float moveSpeed = 5f;
    public int amount;
    public string description;

    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!hasProperties)
        {
            float totalplayerhealth = FindObjectOfType<PlayerStatSystem>().GetMaxHealthIncrease() + FindObjectOfType<Player>().GetMaxHealth();
            amount = (int)((totalplayerhealth * powerUpEffect.amount));
            description = powerUpEffect.description + " " + (amount * 100).ToString() + "% of your current HP."; 
        }
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() )
        {
            if (other.GetComponent<Player>().GetMaxHealth() != other.GetComponent<Player>().GetMaxHealth())
            {
                GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);
                Destroy(gameObject);
                Debug.Log(amount);
                powerUpEffect.Apply(other.gameObject, amount);
            }            
        }
    }


    public void FixedUpdate()
    {
        Player player = FindObjectOfType<Player>();
        if (hasTarget && player.GetMaxHealth() != player.GetCurrentHealth())
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
            moveSpeed *= 1.01f;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    public void SetProperties()
    {
        float totalplayerhealth = FindObjectOfType<PlayerStatSystem>().GetMaxHealthIncrease() + FindObjectOfType<Player>().GetMaxHealth();
        amount = (int)((totalplayerhealth * powerUpEffect.amount));
        description = powerUpEffect.description + " " + (powerUpEffect.amount * 100).ToString() + "% of your current HP.";
        hasProperties = true;
    }

    public int GetValue()
    {
        return powerUpEffect.value;
    }

    public string GetDescription()
    {
        return description;
    }

    public void ScalePrice(int scale)
    {
        throw new NotImplementedException();
    }

    public void ApplyOnPurchase()
    {
        Player player = FindObjectOfType<Player>();
        powerUpEffect.Apply(player.gameObject, amount);
    }
}
