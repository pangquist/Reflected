//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// PlayerController description
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    [SerializeField] ThirdPersonMovement movement;

    [SerializeField] Player player;

    [SerializeField] bool movementLocked;
    [SerializeField] bool actionLocked;
    [SerializeField] bool dead;

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
        if (context.performed && !movementLocked && !actionLocked && !dead)
            movement.Jump();
    }

    public void Move(Vector2 movementVector)
    {
        if (!movementLocked && !dead)
            movement.Move(new Vector3(movementVector.x, 0, movementVector.y).normalized);

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !actionLocked && !dead)
            player.Attack();
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !actionLocked && !dead)
            player.SpecialAttack();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !movementLocked && !actionLocked && !dead)
            movement.Dash();
    }

    public void SwapDimension(InputAction.CallbackContext context)
    {
        if (context.performed && !dead)
            player.SwapDimension();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && !dead)
            player.Interact();
    }

    public void MovementLock()
    {
        movementLocked = true;
        player.GetAnim().SetBool("movementLocked", true);
    }

    public void LockPlayer()
    {
        movementLocked = true;
        player.GetAnim().SetBool("movementLocked", true);

        actionLocked = true;
        player.GetAnim().SetBool("actionLocked", true);
    }

    public void UnlockPlayer()
    {
        movementLocked = false;
        player.GetAnim().SetBool("movementLocked", false);

        actionLocked = false;
        player.GetAnim().SetBool("actionLocked", false);
    }

    public void MovementUnlock()
    {
        movementLocked = false;
        player.GetAnim().SetBool("movementLocked", false);
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

    public void SetDead()
    {
        dead = true;
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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GravityOn()
    {
        movement.TurnOnGravity();
    }

    public void GravityOff()
    {
        movement.TurnOffGravity();
    }
}