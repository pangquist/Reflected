using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBlast : SwappingAbility
{
    [Header("Mirror Blast Specifics")]
    [SerializeField] float range;
    public override bool DoEffect()
    {
        base.DoEffect();

        Debug.Log("MIRROR BLAST!");

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                Rigidbody rb = enemy.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(100, transform.position, range);
            }
        }

        return true;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, range);
        }
    }
}
