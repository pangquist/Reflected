using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, ICollectable, IMagnetic
{
    public static event HandleCoinCollected OnCoinCollected;
    public delegate void HandleCoinCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData coinData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    Rigidbody rb;
    bool hasTarget;
    Vector3 targetPosition;
    float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    private void Start()
    {
        coinData.amount = UnityEngine.Random.Range(1, 10);
    }

    public void Collect()
    {
        Debug.Log("You collected a coin");
        Destroy(gameObject);
        OnCoinCollected?.Invoke(coinData);
    }

    public void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, 0, targetDirection.z) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

}