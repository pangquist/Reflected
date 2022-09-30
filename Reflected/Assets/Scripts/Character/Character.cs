using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Properties")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;
    protected float currentHealth;
    protected Animator anim;

    protected Weapon currentWeapon;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
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

    public virtual void Heal(int amount)
    {
        currentHealth += Mathf.Clamp(amount, 0, maxHealth - currentHealth);
    }

    protected virtual void Die()
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed; //replace with attack speed
    }
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
