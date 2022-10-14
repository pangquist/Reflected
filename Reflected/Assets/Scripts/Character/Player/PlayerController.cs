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
    [SerializeField] ThirdPersonMovement movement;

    [SerializeField] Player player;
    //[SerializeField] Rigidbody rb;

    bool movementLocked;
    bool attackLocked;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        if (player)
            Move(playerControls.Player.Movement.ReadValue<Vector2>());
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !movementLocked)
            movement.Jump();
    }

    public void Move(Vector2 movementVector)
    {
        if (!movementLocked)
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

    public void SwapDimension(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.SwapDimension();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.Interact();
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

    public void LockAttack()
    {
        attackLocked = true;
    }

    public void UnlockAttack()
    {
        attackLocked = false;
    }

    public bool GetAttackLocked()
    {
        return attackLocked;
    }

    public bool Back()
    {
        return playerControls.Player.Back.triggered;
    }

}