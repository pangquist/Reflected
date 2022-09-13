using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class Enemy : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.value = GetHealthPercentage();

        if(currentHealth <= 0)
            Destroy(gameObject);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
