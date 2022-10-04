using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Uppgrade/ForcePush")]
public class AuraEffect : Uppgrade
{
    [SerializeField] float pushAmount;
    [SerializeField] float pushRadius;

    public override void Active(GameObject parent)
    {
        Collider[] colliders = Physics.OverlapSphere(parent.transform.position, pushRadius);
        foreach (Collider pushableObject in colliders)
        {
            if (pushableObject.CompareTag("Enemy"))
            {
                Rigidbody pushBody = pushableObject.GetComponent<Rigidbody>();
                var force = parent.transform.position - pushBody.transform.position;
                force.Normalize();
                pushBody.AddForce(-force * pushAmount);
            }
        }
    }
}
