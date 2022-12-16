using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCharge : MonoBehaviour, IBuyable, IMagnetic
{
    public PowerUpEffect powerUpEffect;
    //Check ui for health that the player gets back for the description and not the powerup effect

    Rigidbody rb;
    bool hasTarget, hasProperties;
    Vector3 targetPosition;
    float moveSpeed = 5f;
    int amount;
    string description;
    int value;
    Rarity myRarity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!hasProperties)
        {
            amount = (int)powerUpEffect.amount;
            value = powerUpEffect.value;
            description = powerUpEffect.description + " " + amount.ToString();
        }
        Destroy(gameObject, 20);
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
            //moveSpeed += Time.deltaTime * 1;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    public void SetProperties()
    {
        amount = (int)powerUpEffect.amount;
        value = powerUpEffect.value;
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

    public void ScalePrice(int scale)
    {
        value = powerUpEffect.value * scale; 
    }

    public void ApplyOnPurchase()
    {
        Player player = FindObjectOfType<Player>();
        powerUpEffect.Apply(player.gameObject, amount);
    }
}
