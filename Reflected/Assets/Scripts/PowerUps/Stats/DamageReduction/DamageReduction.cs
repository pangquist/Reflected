using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/DamageReduction")]
public class DamageReduction : PowerUpEffect
{
    
    public override void Apply(GameObject target, float amount)
    {
        target.GetComponent<StatSystem>().AddDamageReduction(amount);
        Debug.Log("Reduction +" + amount);
        
    }
}
