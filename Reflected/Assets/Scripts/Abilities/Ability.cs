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
    [Header("Ability Stats")]
    [SerializeField] protected Sprite abilityIcon;
    [SerializeField] AudioClip abilitySound;
    [SerializeField] AudioSource abilitySource;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float remainingCooldown;
    [SerializeField] protected string abilityName;
    [SerializeField] protected float damage;
    [SerializeField] protected bool debug;

    protected AbilityCooldowns cooldownstarter;

    private void Start()
    {
        cooldownstarter = FindObjectOfType<AbilityCooldowns>();
    }

    public virtual bool DoEffect()
    {
        remainingCooldown = cooldown;
        abilitySource.PlayOneShot(abilitySound);
        return true;
    }
    void Update()
    {
        if(IsOnCooldown())
            remainingCooldown -= Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return remainingCooldown > 0;
    }

    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }

    public float GetCooldownPercentage()
    {
        return remainingCooldown / cooldown;
    }

    public Sprite GetIcon()
    {
        return abilityIcon;
    }

    public string GetName()
    {
        return abilityName;
    }
}