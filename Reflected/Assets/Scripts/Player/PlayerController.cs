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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        movement = GetComponent<ThirdPersonMovement>();
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    void Start()
    {

    }

    void Update()
    {
        if (playerControls.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
            Move(playerControls.Player.Movement.ReadValue<Vector2>());

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            movement.Jump();
    }

    public void Move(Vector2 movementVector)
    {
        movement.Move(new Vector3(movementVector.x, 0, movementVector.y).normalized);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.Attack();
    }

    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.SpecialAttack();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
            movement.Dash();
    }
}