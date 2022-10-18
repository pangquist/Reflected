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
    [SerializeField] StatSystem stats;
    [SerializeField] GameObject chargeBar;

    [Header("Stat Properties")]
    [SerializeField] float jumpForce;

    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    int weaponIndex = 0;

    [SerializeField] List<Enemy> aggroedEnemies = new List<Enemy>();

    DimensionManager dimensionManager;
    MusicManager musicManager;

    [SerializeField] Ability basicAbility;
    [SerializeField] Ability specialAbility;
    [SerializeField] Ability swapAbility;

    public delegate void InteractWithObject();
    public static event InteractWithObject OnObjectInteraction;



    protected override void Awake()
    {
        base.Awake();
        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SetDamage(damage);

        Cursor.lockState = CursorLockMode.Locked;

        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
        musicManager = dimensionManager.GetComponentInChildren<MusicManager>();

        dimensionManager.SetStatSystem(stats);

        ChangeStats();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Cursor.visible)
            Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon.gameObject.SetActive(false);
            if (++weaponIndex >= weapons.Count)
                weaponIndex = 0;

            Debug.Log(weaponIndex);

            currentWeapon = weapons[weaponIndex];
            currentWeapon.gameObject.SetActive(true);
        }

    }
    public void Attack()
    {
        if (basicAbility.IsOnCooldown())
            return;

        anim.Play(basicAbility.GetAnimation().name);
    }

    public void SpecialAttack()
    {
        if (specialAbility.IsOnCooldown())
            return;

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
        if (DimensionManager.True)
        {
            stats.GetLightStats();
        }
        else
        {
            stats.GetDarkStats();
        }

        currentWeapon.SetDamage(damage);
    }

    public void SwapDimension()
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

    public StatSystem GetStats()
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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //anim.Play("TakeDamage");
    }

    public void AddEnemy(Enemy enemy)
    {
        if (aggroedEnemies.Contains(enemy))
            return;

        if (aggroedEnemies.Count == 0)
        {
            musicManager.ChangeMusicIntensity(1);
        }

        aggroedEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (!aggroedEnemies.Contains(enemy))
            return;


        aggroedEnemies.Remove(enemy);

        if (aggroedEnemies.Count == 0)
        {
            musicManager.ChangeMusicIntensity(-1);
        }
    }

    public List<Enemy> GetEnemies()
    {
        return aggroedEnemies;
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