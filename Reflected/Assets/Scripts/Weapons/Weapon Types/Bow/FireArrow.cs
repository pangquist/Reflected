using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Projectile
{
    private void Start()
    {
        currentProjectile = projectileType.fire;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<IEffectable>().ApplyEffect(data, damage * 2);
        }
        Destroy(gameObject);
    }
}
