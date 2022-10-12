using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IMagnetic
{
    public PowerUpEffect powerUpEffect;
  
    Rigidbody rb;
    bool hasTarget;
    Vector3 targetPosition;
    float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerUpEffect.Apply(other.gameObject, powerUpEffect.amount); //Should be changed from
        }
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
