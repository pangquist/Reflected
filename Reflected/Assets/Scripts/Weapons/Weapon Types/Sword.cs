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
    [SerializeField] List<StatusEffectData> statusEffectDatas;

    public override void AttackWithoutAnimation()
    {
        base.AttackWithoutAnimation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.GetAttackLocked())
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
            for (int i = 0; i < statusEffectDatas.Capacity; i++)
            {
                GetComponent<WeaponStatusEffect>().ApplyEffectToTarget(target.GetComponent<Collider>(), statusEffectDatas[i]);
            }
        }
    }

    public void AddStatusEffect(StatusEffectData effect)
    {
        statusEffectDatas.Add(effect);
    }
}