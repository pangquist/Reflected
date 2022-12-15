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

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.DamageLocked())
            return;

        Enemy target;

        if (other.GetComponent<Enemy>())
            target = other.GetComponent<Enemy>();
        else if (other.GetComponent<Destructible>())
        {
            other.GetComponent<Destructible>().DestroyAnimation();
            return;
        }
        else
            return;

        if (!hitEnemies.Contains(target))
        {
            target.TakeDamage(GetDamage());
            hitEnemies.Add(target);

            foreach (StatusEffectData effectData in statusEffectDatas)
            {
                if(effectData.name == "Regenerate")
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