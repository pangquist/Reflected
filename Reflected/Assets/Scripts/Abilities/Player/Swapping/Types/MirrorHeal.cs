using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHeal : SwappingAbility
{
    public override bool DoEffect()
    {
        base.DoEffect();

        Debug.Log("MIRROR HEAL!");

        gameObject.GetComponent<Player>().Heal(999);

        return true;
    }
}
