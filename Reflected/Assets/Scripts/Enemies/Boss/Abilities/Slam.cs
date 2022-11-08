//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slam description
/// </summary>
public class Slam : Ability
{
    [SerializeField] GameObject hitboxObject;
    [SerializeField] Vector3 hitboxPosition;

    public override bool DoEffect()
    {
        base.DoEffect();

        return true;
    }
}