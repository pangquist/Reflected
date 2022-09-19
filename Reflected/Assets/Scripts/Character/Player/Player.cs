//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player description
/// </summary>
public class Player : Character
{
    [Header("Stat Properties")]
    [SerializeField] float jumpForce;
    [SerializeField] float damageModifier;

    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    [SerializeField] List<float> stats = new List<float> ();
    int weaponIndex = 0;
    bool lightDimension;

    UpgradeManager upgradeManager;

    protected override void Awake()
    {
        base.Awake();
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        lightDimension = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        stats.Add(maxHealth);
        stats.Add(damageModifier);
        stats.Add(movementSpeed);
        stats.Add(jumpForce);

        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
<<<<<<< Updated upstream
        Cursor.lockState = CursorLockMode.Locked;
=======

        ModifyStats();
>>>>>>> Stashed changes
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
            anim.Play(currentWeapon.DoAttack(damageModifier).name);
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

    public void ModifyStats()
    {
        //Resets stat before modifying them
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i] = 1;
        }

        if (lightDimension)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                stats[i] += upgradeManager.ModifyLightStats(i);
            }
        }
        else
        {
            //for (int i = 0; i < stats.Count; i++)
            //{
            //    stats[i] = upgradeManager.ModifyDarkStats(i);
            //}
        }
        //Modify stats when starting the level and swapping dimensions
    }

    public void SwapDimension()
    {

    }
}