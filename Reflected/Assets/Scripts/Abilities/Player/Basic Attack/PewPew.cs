using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewPew : Ability
{
    [SerializeField] Bow bow;

    public override bool DoEffect()
    {
        base.DoEffect();

        bow.WeaponEffect();

        return true;
    }
}
