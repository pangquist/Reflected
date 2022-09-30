//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability description
/// </summary>
public abstract class Ability : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected Sprite abilityIcon;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float cooldownTimer;
    [SerializeField] protected string abilityName;

    protected AbilityCooldowns cooldownstarter;

    private void Start()
    {
        cooldownstarter = FindObjectOfType<AbilityCooldowns>();
    }

    public virtual void DoEffect()
    {
        cooldownTimer = cooldown;
    }
    void Update()
    {
        if(IsOnCooldown())
            cooldownTimer -= Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return cooldownTimer > 0;
    }

    public float GetCooldown()
    {
        return cooldown;
    }
}