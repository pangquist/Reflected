//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slam description
/// </summary>
public class Slam : Ability
{
    [SerializeField] GameObject hitboxObject;
    [SerializeField] Bounds bounds;
    [SerializeField] float duration;
    [SerializeField] float stunDuration;

    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;

    [SerializeField] LayerMask groundMask;

    public override bool DoEffect()
    {
        base.DoEffect();

        GetComponent<Animator>().Play("Slam");

        return true;
    }

    public void SlamAttack()
    {
        bounds = hitboxObject.GetComponent<Collider>().bounds;

        if (hitboxObject.GetComponent<Collider>().bounds.Intersects(player.Hitbox().bounds))
        {
            player.TakeDamage(damage);

            player.Stun(stunDuration);
        }

        ParticleSystem particleSystem = Instantiate(vfxObject, hitboxObject.transform.position, hitboxObject.transform.rotation).GetComponent<ParticleSystem>();
        particleSystem.transform.parent = null;

        RaycastHit hit;
        if (Physics.Raycast(vfxObject.transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.material = hit.transform.gameObject.GetComponent<Renderer>().material;
        }
    }
}