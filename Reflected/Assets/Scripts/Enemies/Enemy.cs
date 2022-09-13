using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class Enemy : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] protected float maxHealth;

    [SerializeField] Canvas combatTextCanvas;
    //CombatText combatText;
    protected float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
        //combatText = combatTextCanvas.GetComponent<CombatText>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.value = GetHealthPercentage();

        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        if(currentHealth <= 0)
            Destroy(gameObject);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
