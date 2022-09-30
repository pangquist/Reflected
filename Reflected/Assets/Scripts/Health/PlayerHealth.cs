using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//OBSOLETE


public class PlayerHealth : MonoBehaviour, ISavable
{
    public float maxHealth = 10;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            //You die
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
    }

    public object SaveState()
    {
        return new SaveData()
        {
            currentHealth = this.currentHealth,
            maxHealth = this.maxHealth
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        currentHealth = saveData.currentHealth;
        maxHealth = saveData.maxHealth;
    }

    [Serializable]
    private struct SaveData
    {
        public float currentHealth;
        public float maxHealth;
    }
}
