using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Root : Enemy
{
    public override void TakeDamage(float damage)
    {
        if (invurnable || isDead)
            return;

        GetComponent<AudioSource>().PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count)]);

        currentHealth -= Mathf.Clamp(damage, 0, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        HealthChanged.Invoke();
        PopUpTextManager.NewDamage(transform.position + Vector3.up * 1.5f, damage);
        PlayDamangedAudioClip();
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
