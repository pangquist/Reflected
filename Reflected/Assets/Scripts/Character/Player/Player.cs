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

    [Header("Stat Properties")]
    [SerializeField] float jumpForce;

    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    int weaponIndex = 0;

    UpgradeManager upgradeManager;
    bool lightDimension;

    public delegate void InteractWithObject();
    public static event InteractWithObject OnObjectInteraction;



    protected override void Awake()
    {
        base.Awake();
        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SetDamage(damage);

        Cursor.lockState = CursorLockMode.Locked;
        lightDimension = true;

        ChangeStats();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (!currentWeapon.IsLocked())
            anim.Play(currentWeapon.DoAttack().name);
    }

    public void SpecialAttack()
    {
        if (currentWeapon.IsLocked() || currentWeapon.IsOnCooldown())
            return;

        anim.Play(currentWeapon.DoSpecialAttack().name);
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    public void UnlockWeapon()
    {
        currentWeapon.Unlock();
    }

    public void DoWeaponEffect()
    {
        currentWeapon.WeaponEffect();
    }

    public void ChangeStats()
    {
        if (lightDimension)
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
        lightDimension = !lightDimension;

        DimensionManager dimensionManager = GameObject.Find("DimensionManager").GetComponent<DimensionManager>();

        if (lightDimension)
        {
            dimensionManager.SetTrueDimension();
        }
        else
        {
            dimensionManager.SetMirrorDimension();
        }

        ChangeStats();
    }

    public void Interact()
    {
        OnObjectInteraction.Invoke();
    }

    public StatSystem GetStats()
    {
        return stats;
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