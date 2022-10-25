using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Damage")]
public class DamageBuff : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<PlayerStatSystem>().AddDamageIncrease(amount);
        Debug.Log("Damage +" + amount);
    }
}
