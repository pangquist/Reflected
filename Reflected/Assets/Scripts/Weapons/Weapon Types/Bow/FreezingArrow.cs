using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingArrow : Projectile
{
    private void Start()
    {
        currentProjectile = projectileType.freeze;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<IEffectable>().ApplyEffect(data, 1);
        }
        Destroy(gameObject);
    }
}
