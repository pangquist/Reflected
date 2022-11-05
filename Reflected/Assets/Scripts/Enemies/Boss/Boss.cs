//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss description
/// </summary>
public class Boss : Enemy
{
    [SerializeField] List<Ability> abilities;
    //[SerializeField] float timeBetweenAbilities;
    [SerializeField] float abilityTimer;

    protected override void Update()
    {
        AbilityTimer();
        base.Update();
    }

    public void AbilityTimer()
    {
        abilityTimer -= Time.deltaTime;

        while (abilityTimer <= 0)
        {
            //Do Random Ability
            Ability chosenAbility = abilities[Random.Range(0, abilities.Count)];
            if(!chosenAbility.IsOnCooldown())
            {
                chosenAbility.DoEffect();
                abilityTimer = chosenAbility.Cooldown();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}