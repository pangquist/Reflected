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

    //public override void DoSpecialAttack()
    //{
    //    base.DoSpecialAttack();
    //}

    public override void WeaponEffect()
    {
        Collider[] targets = Physics.OverlapBox(hitBox.gameObject.transform.position, hitBox.bounds.size/2);

        foreach(Collider collider in targets)
        {
            if(collider.GetComponent<Enemy>())
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            }
        }
    }
}