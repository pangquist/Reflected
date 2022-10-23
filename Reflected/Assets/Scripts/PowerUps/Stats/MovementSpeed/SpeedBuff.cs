using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed")]
public class SpeedBuff : PowerUpEffect
{

    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<StatSystem>().AddMovementSpeed(amount);
        Debug.Log("Speed +" + amount);
    }
}
