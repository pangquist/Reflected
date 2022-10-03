using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Uppgrade/dash")]
public class DashAttack : Uppgrade
{
    public float dashVelocity;

    public override void Active(GameObject parent)
    {
        ThirdPersonMovement move = parent.GetComponent<ThirdPersonMovement>();
        
    }
}
