using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Root : Enemy
{
    [SerializeField] Collider swipeHitbox;
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        if (invurnable || isDead)
            return;

        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + combatTextOffset, Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (clipName == "Idle")
            anim.Play("Take Damage");

        GetComponent<AudioSource>().PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count)]);

        currentHealth -= Mathf.Clamp(damage, 0, currentHealth);

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

    public void Spawn()
    {
        gameObject.SetActive(true);
        anim.Play("Spawn");
    }

    public void Retract()
    {
        anim.Play("Retract");
    }

    public void Frenzy()
    {
        anim.Play("Swipe");
    }

    public Collider SwipeHitbox() => swipeHitbox;

    public void Slam()
    {
        FindObjectOfType<Boss>().RootSlam(this);
    }
}
