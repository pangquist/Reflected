using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : Projectile
{
    [SerializeField] float colleteralDamage;
    [SerializeField] float blastRadius;
    [SerializeField] float explosionforce = 10;

    private void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        var surrondingObjects = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (var obj in surrondingObjects)
        {
            if (obj.GetComponent<Enemy>())
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionforce, transform.position, blastRadius);
                obj.GetComponent<Enemy>().TakeDamage(colleteralDamage);
            }
        }
    }
}
