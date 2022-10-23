using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Attack")]
public class AttackSpeed : PowerUpEffect
{        
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<StatSystem>().AddAttackSpeed(amount);
        Debug.Log("Attack speed +" + amount);
    }
}