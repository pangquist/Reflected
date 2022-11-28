using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] PlayerStatSystem stats;
    [SerializeField] Animator animator;
    [SerializeField] List<AudioClip> footstepSounds;
    [SerializeField] ParticleSystem footstepEffect;
    [SerializeField] List<Transform> feetPositions;
    int feetIndex;

    [Header("Stat Properties")]
    [SerializeField] float speed = 12f;
    [SerializeField] float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float movementPenalty;

    [Header("Jump Properties")]
    [SerializeField] float gravityEffect = -9.81f;
    [SerializeField] float jumpHeight;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool isGrounded;
    bool gravity = true;

    [Header("Dash Properties")]
    [SerializeField] Dash dashAbility;

    private Vector3 velocity;
    // Start is called before the first frame update

    private void Update()
    {
        if (!isGrounded && gravity)
            velocity.y += gravityEffect * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetFloat("velY", velocity.y);
        animator.SetBool("isGrounded", isGrounded);
        controller.Move(velocity * Time.deltaTime);
    }

    // Update is called once per frame
    public void Move(Vector3 direction)
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        animator.SetFloat("velX", direction.x);
        animator.SetFloat("velZ", direction.z);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (moveDir.x != 0 || moveDir.z != 0)
                animator.SetBool("isRunning", true);

            controller.Move(moveDir.normalized * speed * stats.GetMovementSpeed() * CharacterMovementPenalty() * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void Jump()
    {
        if (dashAbility.IsDashing() || !isGrounded)
            return;

        //StartCoroutine(_Jump());
        if (isGrounded && velocity.y <= 0)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityEffect);
        }
    }

    //IEnumerator _Jump()
    //{
    //    float progress = 0;

    //    while(progress < jumpDuration)
    //    {

    //        progress += Time.deltaTime;
    //    }
    //}

    public void Dash()
    {
        if (isGrounded)
        {
            if (dashAbility.DoEffect())
                animator.Play(dashAbility.GetName());
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

    public void TurnOnGravity()
    {
        gravity = true;
    }

    public void TurnOffGravity()
    {
        velocity = new Vector3(velocity.x, 0, velocity.z);
        gravity = false;
    }

    public void TakeStep()
    {
        if (!isGrounded)
            return;

        AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Count)];
        GetComponent<AudioSource>().PlayOneShot(footstepSound);

        SpawnGoundParticle();
    }

    public void SpawnGoundParticle()
    {
        if (!isGrounded)
            return;

        Transform feetTransform = feetPositions[feetIndex++];
        if (feetIndex >= feetPositions.Count)
            feetIndex = 0;

        ParticleSystem stepEffect = Instantiate(footstepEffect, feetTransform.position, footstepEffect.transform.rotation);
        stepEffect.gameObject.transform.parent = null;

        RaycastHit hit;

        if (Physics.Raycast(feetTransform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            ParticleSystemRenderer renderer = stepEffect.GetComponent<ParticleSystemRenderer>();
            renderer.material = hit.transform.gameObject.GetComponent<Renderer>().material;
        }
    }
}
