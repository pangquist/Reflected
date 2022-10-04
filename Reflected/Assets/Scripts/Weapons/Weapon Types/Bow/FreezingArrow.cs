using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingArrow : Projectile
{
    [SerializeField] float timeFrozen;
    [SerializeField] float slowedMovementSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.gameObject.GetComponent<Enemy>().Freeze(slowedMovementSpeed, timeFrozen);
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
