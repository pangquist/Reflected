using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] StatSystem stats;

    [Header("Stat Properties")]
    [SerializeField] float speed = 12f;
    [SerializeField] float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float movementPenalty;

    [Header("Jump Properties")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool isGrounded;

    [Header("Dash Properties")]
    [SerializeField] Dash dashAbility;

    private Vector3 velocity;
    // Start is called before the first frame update

    private void Update()
    {
        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        controller.Move(velocity * Time.deltaTime);
    }

    // Update is called once per frame
    public void Move(Vector3 direction)
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * stats.GetMovementSpeed() * CharacterMovementPenalty() * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (dashAbility.IsDashing())
            return;

        if (isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void Dash()
    {
        if (isGrounded)
        {
            dashAbility.DoEffect();
        }
    }

    public Dash GetDash()
    {
        return dashAbility;
    }

    public void AddSpeed(float amount)
    {
        speed += amount;
    }

    private float CharacterMovementPenalty()
    {
        return GetComponent<Character>().MovementPenalty();
    }
}
