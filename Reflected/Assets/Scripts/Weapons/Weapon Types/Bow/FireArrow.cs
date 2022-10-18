using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Projectile
{
    [SerializeField] float damageOverTime;
    [SerializeField] float time;

    private void Start()
    {
        currentProjectile = projectileType.fire;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>())
        {
            other.gameObject.GetComponent<Enemy>().TakeDamageOverTime(damageOverTime, time);
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
