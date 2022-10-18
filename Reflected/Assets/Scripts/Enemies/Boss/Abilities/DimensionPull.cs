//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DimensionPull description
/// </summary>
public class DimensionPull : Ability
{
    DimensionManager dimensionManager;

    private void Awake()
    {
        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
    }

    public override bool DoEffect()
    {
        base.DoEffect();
        dimensionManager.ForcedSwap();
        return true;
    }
}