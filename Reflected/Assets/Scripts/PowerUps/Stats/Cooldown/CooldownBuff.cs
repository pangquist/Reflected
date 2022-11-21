using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Cooldown")]
public class CooldownBuff : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<PlayerStatSystem>().AddCooldownDecrease(amount);
        Debug.Log("Cooldown -" + amount);
    }
}
