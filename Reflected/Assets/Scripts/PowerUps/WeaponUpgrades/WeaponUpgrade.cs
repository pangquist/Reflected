using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/WeaponUpgrade")]
public class WeaponUpgrade : PowerUpEffect
{
    [SerializeField] public StatusEffectData effect;
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponentInChildren<Sword>().AddStatusEffect(effect);
    }
}
