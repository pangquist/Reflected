using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Enemy
{
    public override void TakeDamage(float damage)
    {
        if (invurnable || isDead)
            return;

        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + combatTextOffset, Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        currentHealth -= Mathf.Clamp(damage, 0, currentHealth);

        Debug.Log("ENEMY TOOK DAMAGE: " + damage + "Health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        HealthChanged.Invoke();
    }

    protected override void Die()
    {
        anim.Play("Death");
        isDead = true;
    }

    public void RemoveRootFromBoss()
    {
        FindObjectOfType<Boss>().RemoveRoot(this);
    }
}
