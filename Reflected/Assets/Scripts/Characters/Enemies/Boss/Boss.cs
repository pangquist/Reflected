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
    [Header("Boss Specifics")]
    [SerializeField] GameObject rotateBody;
    [SerializeField] List<Ability> abilities;
    [SerializeField] float timeBetweenAbilities;
    [SerializeField] float abilityTimer;

    protected override void Update()
    {
        AbilityTimer();
        RotateTowardsPlayer();
        base.Update();
    }

    public void RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - rotateBody.transform.position).normalized;
        direction.y = 0;
        rotateBody.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void AbilityTimer()
    {
        if (!invurnable)
            abilityTimer -= Time.deltaTime;

        while (abilityTimer <= 0)
        {
            //Do Random Ability
            Ability chosenAbility = abilities[Random.Range(0, abilities.Count)];
            if (!chosenAbility.IsOnCooldown())
            {
                chosenAbility.DoEffect();
                abilityTimer = timeBetweenAbilities;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}