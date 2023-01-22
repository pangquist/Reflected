//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sword description
/// </summary>
public class Sword : Weapon
{
    [Header("Sword Properties")]
    [SerializeField] Collider hitBox;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerController.DamageLocked())
            return;

        other.GetComponent<Destructible>()?.DestroyAnimation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.DamageLocked())
            return;

        GameObject target;

        if (other.GetComponent<Enemy>() || other.GetComponent<tutorialDummy>())
            target = other.gameObject;
        else
            return;

        if (!hitEnemies.Contains(target))
        {
            if (target.GetComponent<Enemy>())
            {
                target.GetComponent<Enemy>().TakeDamage(GetDamage());
            }
            else if (target.GetComponent<tutorialDummy>())
            {
                target.GetComponent<tutorialDummy>().TakeDamage(GetDamage());
            }

            hitEnemies.Add(target);

            foreach (StatusEffectData effectData in statusEffectDatas)
            {
                if (effectData.name == "Regenerate")
                {
                    player.GetComponent<IEffectable>().ApplyEffect(effectData, damage);
                }
                else
                {
                    target.GetComponent<IEffectable>().ApplyEffect(effectData, damage);
                    //Debug.Log(effectData);
                }

            }

        }
    }
}