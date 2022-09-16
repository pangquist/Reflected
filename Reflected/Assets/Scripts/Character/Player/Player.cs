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

    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        Cursor.visible = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible)
            Cursor.visible = false;
    }



    public void SpecialAttack()
    {
        currentWeapon.DoSpecialAttack();
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
}