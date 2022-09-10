using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MirrorShard : MonoBehaviour, ICollectable
{
    public static event Action OnShardCollected;

    Rigidbody rb;
    bool hasTarget;
    Vector3 targetPosition;
    float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Collect()
    {
        Debug.Log("You collected a mirror shard");
        Destroy(gameObject);
        OnShardCollected?.Invoke(); //?. makes sure it's not null and that there are listeners to the event
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
