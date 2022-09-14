using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class Enemy : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] protected float maxHealth;

    [SerializeField] Canvas combatTextCanvas;
    [SerializeField] float aggroRange;

    GameObject parent;
    Player player;
    Animator anim;

    protected float currentHealth;
    bool playerNoticed;

    public void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        parent = gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if(!playerNoticed)
        {
            if(aggroRange <= distance)
            {
                playerNoticed = true;
            }
        }
        else
        {
            if (aggroRange > distance)
            {
                playerNoticed = false;
            }
        }

        if(playerNoticed)
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            direction.y = 0;
            parent.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = GetHealthPercentage();

        Vector3 direction = (transform.position - player.transform.position).normalized;
        direction.y = 0;
        parent.transform.rotation = Quaternion.LookRotation(direction);

        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        if(currentHealth <= 0)
        {
            anim.Play("Death");
        }
        else
        {
            anim.Play("Damaged");
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
