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
        //if (!playerController.GetActionLock())
        //    return;

        if (!playerController.DamageLocked())
            return;

        Enemy target;

        if (other.GetComponent<Enemy>())
            target = other.GetComponent<Enemy>();
        else
            return;

        if (!hitEnemies.Contains(target))
        {
            target.TakeDamage(GetDamage());
            hitEnemies.Add(target);
            
            if(powerUpIndex > -1 && powerUpIndex < 2)
            {
                target.GetComponent<IEffectable>().ApplyEffect(statusEffectDatas[powerUpIndex], damage);
                Debug.Log(statusEffectDatas[powerUpIndex]);
            }
            else if (powerUpIndex == 2)
            {
                player.GetComponent<IEffectable>().ApplyEffect(statusEffectDatas[powerUpIndex], damage / 2);
                Debug.Log(statusEffectDatas[powerUpIndex]);
            }
        }
    }

    public void AddStatusEffect(StatusEffectData effect)
    {
        statusEffectDatas.Add(effect);
    }
}