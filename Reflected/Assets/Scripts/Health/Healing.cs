using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Health")]
public class Healing : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        if(target.GetComponent<Player>()) target.GetComponent<Player>().Heal((int)amount);
    }
}
