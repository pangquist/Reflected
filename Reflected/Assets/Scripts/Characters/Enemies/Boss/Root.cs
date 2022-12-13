using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Root : Enemy
{
    [SerializeField] Collider swipeHitbox;
    [SerializeField] Transform effectTransform;
    Boss boss;
    protected override void Awake()
    {
        gameObject.SetActive(false);
        base.Awake();
        boss = FindObjectOfType<Boss>();
    }

    public override void TakeDamage(float damage)
    {
        if (invurnable || isDead)
            return;

        string clipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (clipName == "Idle")
            anim.Play("Take Damage");

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

        if(boss.Roots().Count == 1)
        {
            boss.ToggleAbilityLock();
        }
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
    public Transform EffectTransform() => effectTransform;

    public void Slam()
    {
        boss.RootSlam(this);
    }
}
