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

    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    int weaponIndex = 0;

    UpgradeManager upgradeManager;
    bool lightDimension;

    // Start is called before the first frame update
    void Start()
    {
        ChangeStats();
        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SetDamage(damage);

        Cursor.lockState = CursorLockMode.Locked;
        lightDimension = true;
        //upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        //upgradeManager.AddPlayer(this);

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
            //maxHealth = upgradeManager.GetLightPieces()[0].GetValue();
            //movementSpeed = upgradeManager.GetLightPieces()[1].GetValue();

        }
        else
        {
            //foreach (MirrorPiece piece in upgradeManager.GetDarkPieces())
            //{

            //}
        }
    }

    public void SwapDimension()
    {

    }
}