//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player description
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Stat Properties")]
    [SerializeField] float maxHealth;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    [Header("Weapon Properties")]
    [SerializeField] Weapon currentWeapon;

    [SerializeField] Animator anim;
    
    float currentHealth;
    // Awake is called when the script instance is being loaded
    void Awake()
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

    }

    public void Attack()
    {
        currentWeapon.DoAttack();
    }

    public void SpecialAttack()
    {
        currentWeapon.DoSpecialAttack();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        anim.Play("Death");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
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