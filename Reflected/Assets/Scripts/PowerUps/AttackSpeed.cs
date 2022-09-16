using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Attack")]
public class AttackSpeed : PowerUpEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        //target.GetComponent<ThirdPersonMovement>().speed += amount;
        Debug.Log("You attack like a champ now");
    }

}
