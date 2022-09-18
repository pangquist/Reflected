//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController description
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    ThirdPersonMovement movement;
    
    Player player;
    Rigidbody rb;

    bool movementLocked;
    [SerializeField] bool attackLocked;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        movement = GetComponent<ThirdPersonMovement>();
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        if (playerControls.Player.Movement.ReadValue<Vector2>() != Vector2.zero && !movementLocked)
            Move(playerControls.Player.Movement.ReadValue<Vector2>());

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !movementLocked)
            movement.Jump();
    }

    public void Move(Vector2 movementVector)
    {
        if(!movementLocked)
            movement.Move(new Vector3(movementVector.x, 0, movementVector.y).normalized);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !attackLocked)
            player.Attack();
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !attackLocked)
            player.SpecialAttack();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !movementLocked)
            movement.Dash();
    }

    public void SetMovementLocked(bool state)
    {
        movementLocked = state;
    }

    public bool GetMovementLocked()
    {
        return movementLocked;
    }

    public void SetAttackLocked(bool state)
    {
        attackLocked = state;
    }

    public bool GetAttackLocked()
    {
        return attackLocked;
    }
}