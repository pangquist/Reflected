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

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
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
}