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

    [SerializeField] bool movementLocked;
    [SerializeField] bool actionLocked;

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
        if (context.performed && !movementLocked && !actionLocked)
            movement.Jump();
    }

    public void Move(Vector2 movementVector)
    {
        if (!movementLocked)
            movement.Move(new Vector3(movementVector.x, 0, movementVector.y).normalized);

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !actionLocked)
            player.Attack();
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !actionLocked)
            player.SpecialAttack();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !movementLocked && !actionLocked)
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
    public bool Back()
    {
        return playerControls.Player.Back.triggered;
    }

    public void SetActionLocked(bool state)
    {
        actionLocked = state;
    }


    public void ActionLock()
    {
        actionLocked = true;
        player.GetAnim().SetBool("actionLocked", true);
    }

    public void ActionUnlock()
    {
        actionLocked = false;
        player.GetAnim().SetBool("actionLocked", false);
    }

    public bool GetActionLock()
    {
        return actionLocked;
    }
}