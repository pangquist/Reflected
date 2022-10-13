using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Health")]
public class Healing : PowerUpEffect
{
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<Player>().Heal((int)amount);
    }

    public void Awake()
    {
        description = "Increases your Health by " + amount;
    }
}
