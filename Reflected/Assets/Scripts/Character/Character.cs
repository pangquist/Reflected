using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Charcter Properties")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float movementSpeed;
    protected float currentHealth;
    protected Animator anim;

    [Header("Weapon Properties")]
    [SerializeField] protected Weapon currentWeapon;


    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.Play("Damaged");
        }
    }

    public void Attack()
    {
        currentWeapon.DoAttack();
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
}
