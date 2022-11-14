using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/WeaponUpgrade")]
public class WeaponUpgrade : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<Sword>().SetWeaponIndex((int)amount);
    }
}
