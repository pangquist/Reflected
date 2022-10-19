using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Cooldown")]
public class CooldownBuff : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<StatSystem>().AddCooldownDecrease(amount);
        Debug.Log("Damage +" + amount);
    }

    public void Awake()
    {
        description = "Decreases your cooldown by " + amount;
    }
}
