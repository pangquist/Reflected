using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : Projectile
{
    [SerializeField] float colleteralDamage;
    [SerializeField] float blastRadius;
    [SerializeField] float explosionforce;

    private void Start()
    {
        currentProjectile = projectileType.fire;
    }

    void Update()
    {
        Gizmos.DrawSphere(transform.position, blastRadius);
        Gizmos.DrawSphere(transform.position, blastRadius / 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var outerSphere = Physics.OverlapSphere(transform.position, blastRadius);
        var innerSphere = Physics.OverlapSphere(transform.position, blastRadius/2);
        foreach (var obj in outerSphere)
        {
            if (obj.GetComponent<Enemy>())
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionforce, transform.position, blastRadius);
                obj.GetComponent<Enemy>().TakeDamage(colleteralDamage);
            }
        }

        foreach (var obj in innerSphere)
        {
            if (obj.GetComponent<Enemy>())
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionforce * 3, transform.position, blastRadius / 2);
                obj.GetComponent<Enemy>().TakeDamage(colleteralDamage * 2);
            }
        }

        Destroy(gameObject);
    }
}
