using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed")]
public class SpeedBuff : PowerUpEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<ThirdPersonMovement>().speed += amount;
    }
}
