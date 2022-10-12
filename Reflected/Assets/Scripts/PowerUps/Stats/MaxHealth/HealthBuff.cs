using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/MaxHealth")]
public class HealthBuff : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<StatSystem>().AddMaxHealth(amount);
        Debug.Log("Health +" + amount);
    }

    public void Awake()
    {
        description = "Increases your total max health by " + amount;
    }
}
