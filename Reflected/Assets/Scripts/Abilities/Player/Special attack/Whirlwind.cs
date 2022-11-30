//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Whirlwind description
/// </summary>
public class Whirlwind : Ability
{
    [SerializeField] Sword sword;
    public override AnimationClip GetAnimation()
    {
        cooldownstarter.Ability1Use();
        sword.ClearEnemies();
        return base.GetAnimation();
    }
}