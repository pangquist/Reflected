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
public class Player : Character, ISavable
{
    [SerializeField] PlayerStatSystem stats;
    [SerializeField] GameObject chargeBar;

    [Header("Stat Properties")]
    [SerializeField] float jumpForce;

    [SerializeField] List<Weapon> weapons;
    int weaponIndex = 0;

    //[SerializeField] List<Enemy> aggroedEnemies = new List<Enemy>();

    DimensionManager dimensionManager;
    MusicManager musicManager;

    Ability currentAbility;
    [SerializeField] Ability basicSwordAbility;
    [SerializeField] Ability basicBowAbility;
    [SerializeField] Ability specialAbility;
    [SerializeField] Ability swapAbility;
    [Range(0f, 1f)]
    [SerializeField] float TimeFlowWhileSwapping;

    public delegate void InteractWithObject();
    public static event InteractWithObject OnObjectInteraction;



    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;

        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
        musicManager = dimensionManager.GetComponentInChildren<MusicManager>();

        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SetDamage(damage);


        dimensionManager.SetStatSystem(stats);

        ChangeStats();

        anim.Play("GetUp");
    }

    protected override void Update()
    {
        base.Update();
        if (Cursor.visible)
            Cursor.visible = false;
    }

    public void Attack()
    {
        if (weaponIndex == 0)
            currentAbility = basicSwordAbility;
        else
            currentAbility = basicBowAbility;

        if (currentAbility.IsOnCooldown())
            return;

        anim.Play(currentAbility.GetAnimation().name);
        currentAbility.DoEffect();
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
        if (dimensionManager.TrySwap())
        {
            ChangeStats();
            if (swapAbility)
                swapAbility.DoEffect();
        }
    }

    public void Interact()
    {
        OnObjectInteraction.Invoke();
    }

    public StatSystem GetStats() //May have to be PlayerStatSystem??
    {
        return stats;
    }

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
        return GetComponent<Collider>();
    }

    public void Stun(float duration)
    {
        if (currentHealth > 0)
            StartCoroutine(_Stun(duration));
    }

    IEnumerator _Stun(float duration)
    {
        GetComponent<PlayerController>().LockPlayer();

        anim.Play("Knocked Down");

        yield return new WaitForSeconds(duration);

        GetComponent<PlayerController>().UnlockPlayer();

        anim.Play("GetUp");
    }

    #region SaveLoad
    [Serializable]
    private struct SaveData
    {
        public float currentHealth;
        public float maxHealth;
    }

    public object SaveState()
    {
        return new SaveData()
        {
            currentHealth = this.currentHealth,
            maxHealth = this.maxHealth
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        currentHealth = saveData.currentHealth;
        maxHealth = saveData.maxHealth;
    }

    #endregion
}