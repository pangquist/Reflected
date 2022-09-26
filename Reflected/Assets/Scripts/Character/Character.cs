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
        return currentWeapon.GetDamage();
    }

    public float GetAttackSpeed()
    {
        return 1; //replace with attack speed
    }
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
