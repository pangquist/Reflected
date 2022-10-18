using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingArrow : Projectile
{
    [SerializeField] float timeFrozen;
    [SerializeField] float slowedMovementSpeed;

    private void Start()
    {
        currentProjectile = projectileType.freeze;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.gameObject.GetComponent<Enemy>().Freeze(slowedMovementSpeed, timeFrozen);
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
