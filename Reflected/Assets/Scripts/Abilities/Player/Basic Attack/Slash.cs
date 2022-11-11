//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slash description
/// </summary>
public class Slash : Ability
{
    float comboTimer;
    protected int currentComboIndex;
    [SerializeField] protected AnimationClip[] comboClips;
    [SerializeField] protected float maxTimeBetweenCombo;
    [SerializeField] Weapon sword;

    protected override void Update()
    {
        base.Update();
        if (currentComboIndex > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > maxTimeBetweenCombo)
            {
                comboTimer = 0;
                currentComboIndex = 0;
            }
        }
    }

    public override AnimationClip GetAnimation()
    {
        sword.ClearEnemies();
        base.GetAnimation();

        if (currentComboIndex == comboClips.Length)
            currentComboIndex = 0;

        return comboClips[currentComboIndex++];

    }

    public override bool DoEffect()
    {
        remainingCooldown = cooldown * player.GetStats().GetAttackSpeed();
        if (abilitySounds.Count != 0 && abilitySource)
            abilitySource.PlayOneShot(abilitySounds[Random.Range(0, abilitySounds.Count)]);
        return true;
    }

    public override float GetCooldownPercentage()
    {
        return remainingCooldown / (cooldown * player.GetStats().GetAttackSpeed());
    }
}