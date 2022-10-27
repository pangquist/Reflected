using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/MaxHealth")]
public class HealthBuff : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<PlayerStatSystem>().AddMaxHealth(amount);
        Debug.Log("Health +" + amount);
    }

}
