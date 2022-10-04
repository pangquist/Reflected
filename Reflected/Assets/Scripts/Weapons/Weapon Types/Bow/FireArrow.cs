using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Projectile
{
    [SerializeField] float damageOverTime;
    [SerializeField] float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.gameObject.GetComponent<Enemy>().TakeDamageOverTime(damageOverTime, time);
        }
    }
}
