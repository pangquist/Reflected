using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalArrow : Projectile
{
    private void Start()
    {
        currentProjectile = projectileType.normal;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
