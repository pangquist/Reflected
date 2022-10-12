using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Attack")]
public class AttackSpeed : PowerUpEffect
{        
    public override void Apply(GameObject target)
    {
        target.GetComponent<StatSystem>().AddAttackSpeed(amount);
        Debug.Log("Attack speed +" + amount);
    }

    public void Awake()
    {
        description = "Increases your attack speed by " + amount;
    }

}
