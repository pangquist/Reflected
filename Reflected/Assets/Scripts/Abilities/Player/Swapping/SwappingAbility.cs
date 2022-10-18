using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwappingAbility : Ability
{
    public override bool DoEffect()
    {
        base.DoEffect();

        return true;
    }
}
