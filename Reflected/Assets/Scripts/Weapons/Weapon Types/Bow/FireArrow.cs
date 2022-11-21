using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Projectile
{
    private void Start()
    {
        currentProjectile = projectileType.fire;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<IEffectable>().ApplyEffect(data, 2);
        }
    }
}
