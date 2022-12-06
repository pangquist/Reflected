using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSlow : SwappingAbility
{
    [Header("Mirror Slow Specifics")]
    [SerializeField] StatusEffectData effect;
    [SerializeField] float scale;
    [SerializeField] float range;

    public override bool DoEffect()
    {
        base.DoEffect();

        Debug.Log("MIRROR SLOW!");

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy" || collider.tag == "Melee" || collider.tag == "AoE" || collider.tag == "Ranged")
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.ApplyEffect(effect, scale);
            }
        }

        return true;
    }
}