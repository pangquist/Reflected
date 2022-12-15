using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ChargeIncrease")]
public class ChargeIncrease : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        Debug.Log("Charge effect called for amount: " + amount);
        if (target.GetComponent<Player>())
        {
            DimensionManager dimentionManager = FindObjectOfType<DimensionManager>();
            dimentionManager.GainCharges((int)amount);
        }

    }
}
