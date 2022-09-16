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

    public override void DoAttack()
    {
        base.DoAttack();
        anim.Play(comboClips[currentComboIndex].name);
        currentComboIndex++;
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
            target.TakeDamage(damage);
            hitEnemies.Add(target);
        }
    }
}