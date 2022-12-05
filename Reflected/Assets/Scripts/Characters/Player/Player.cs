//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player description
/// </summary>
public class Player : Character
{
    [SerializeField] PlayerStatSystem stats;
    [SerializeField] GameObject chargeBar;
    [SerializeField] Collider hitbox;

    [Header("Stat Properties")]
    [SerializeField] float jumpForce;

    [ReadOnly] Bounds hitBoxBounds;
    DimensionManager dimensionManager;
    MusicManager musicManager;

    Ability currentAbility;
    [SerializeField] Ability basicSwordAbility;
    //[SerializeField] Ability basicBowAbility;
    [SerializeField] Ability specialAbility;
    [SerializeField] Ability swapAbility;
    [Range(0f, 1f)]
    [SerializeField] float TimeFlowWhileSwapping;
    [SerializeField] AudioClip trueSwapSound;
    [SerializeField] AudioClip mirrorSwapSound;

    public delegate void InteractWithObject();
    public static event InteractWithObject OnObjectInteraction;

    bool specialOne;

    protected override void Awake()
    {
        currentHealth = maxHealth + stats.GetMaxHealthIncrease();
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;

        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
        musicManager = dimensionManager.GetComponentInChildren<MusicManager>();
        specialOne = false;

        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SetDamage(damage);
        currentAbility = basicSwordAbility;

        dimensionManager.SetStatSystem(stats);

        ChangeStats();

        anim.Play("GetUp");
    }

    public void Attack()
    {
        currentAbility = basicSwordAbility;

        if (currentAbility.IsOnCooldown())
            return;

        anim.Play(currentAbility.GetAnimation().name);
        currentAbility.DoEffect();
    }

    public override float GetHealthPercentage()
    {
        return currentHealth / (maxHealth + stats.GetMaxHealthIncrease());
    }

    public override float GetMaxHealth()
    {
        return maxHealth + stats.GetMaxHealthIncrease();
    }

    public void SpecialAttack()
    {
        if (specialAbility.IsOnCooldown())
            return;

        currentAbility = specialAbility;
        anim.Play(specialAbility.GetAnimation().name);
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
    public void DoWeaponEffect()
    {
        currentWeapon.WeaponEffect();
    }

    public void ChangeStats()
    {
        try
        {
            if (DimensionManager.True)
            {
                stats.GetLightStats();
            }
            else
            {
                stats.GetDarkStats();
            }
        }
        catch
        {

        }
        currentWeapon.SetDamage(damage);
    }

    public override void Heal(float amount)
    {
        currentHealth += Mathf.Clamp(amount, 0, maxHealth + stats.GetMaxHealthIncrease() - currentHealth);
        HealthChanged.Invoke();
    }

    public void TryDimensionSwap()
    {
        if (dimensionManager.CanSwap())
        {
            Time.timeScale = TimeFlowWhileSwapping;
            anim.Play("DimensionSwap");
        }
    }

    public void DoDimensionSwap()
    {
        SetTimeToNormalFlow();
        
        if (dimensionManager.TrySwap())
        {
            ChangeStats();
            if (specialOne)
                swapAbility.DoEffect();

            if(DimensionManager.CurrentDimension == Dimension.True)
            {
                GetComponent<AudioSource>().PlayOneShot(trueSwapSound);
            }
            else
                GetComponent<AudioSource>().PlayOneShot(mirrorSwapSound);
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isDead)
            return;
        
        currentHealth -= Mathf.Clamp(Mathf.CeilToInt(damage * stats.GetDamageReduction()), 0, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            SaveProgress();
        }
        else
        {
            anim.Play("Damaged");
        }

        HealthChanged.Invoke();
    }

    public void Interact()
    {
        OnObjectInteraction?.Invoke();
    }

    public StatSystem GetStats() //May have to be PlayerStatSystem??
    {
        return stats;
    }

    public bool SpecialOne { get { return specialOne; } set { specialOne = value; }  }

    public Animator GetAnim()
    {
        return anim;
    }

    public Ability GetSpecialAbility()
    {
        return specialAbility;
    }

    public void PlayCurrentAbilityVFX()
    {
        currentAbility.PlayVFX();
    }

    public void SetTimeToNormalFlow()
    {
        Time.timeScale = 1;
    }

    public Collider Hitbox()
    {
        return hitbox;
    }

    public void Stun(float duration)
    {
        if (currentHealth > 0)
        {
            SetTimeToNormalFlow();
            StartCoroutine(_Stun(duration));
        }
    }

    public void Rotate(Vector2 direction)
    {
        Debug.Log("ROTATE: " + direction.normalized);
    }

    public StatSystem playerStats()
    {
        return stats;
    }

    IEnumerator _Stun(float duration)
    {
        GetComponent<PlayerController>().LockPlayer();

        anim.Play("Knocked Down");

        yield return new WaitForSeconds(duration);

        GetComponent<PlayerController>().UnlockPlayer();

        anim.Play("GetUp");
    }

    private void SaveProgress()
    {
        GameObject inventory = GameObject.Find("Inventory");
        if (inventory)
            inventory.GetComponent<Inventory>().ResetTemporaryCollectables();
        else
            Debug.Log("No Inventory found");

        GameObject saveLoadSystem = GameObject.Find("SaveLoadSystem");

        if (saveLoadSystem)
            saveLoadSystem.GetComponent<SaveLoadSystem>().Save();
        else
            Debug.Log("No SaveLoadSystem found");
    }

}