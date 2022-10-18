using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IEffectable
{
    [Header("Character Properties")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float currentHealth;
    protected List<StatusEffect> statusEffects;
    protected List<GameObject> effectParticles;
    [SerializeField] protected Animator anim;

    protected Weapon currentWeapon;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        if (this.GetType() != typeof(Player))
            anim = GetComponent<Animator>();

        statusEffects = new List<StatusEffect>();
        effectParticles = new List<GameObject>();
    }

    protected virtual void Update()
    {
        if (statusEffects.Count > 0) HandleEffect();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //Debug.Log("current health: " + currentHealth);

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
        Debug.Log("Character Dead");
    }

    protected void Destroy()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
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

    public float MovementPenalty()
    {
        float movementPenalty = 1;
        if (statusEffects.Count > 0)
        {
            foreach (StatusEffect status in statusEffects)
            {
                movementPenalty *= status.effect.MovementPenalty;
            }

        }
        return movementPenalty;
    }

    public void ApplyEffect(StatusEffectData data)
    {
        statusEffects.Add(new StatusEffect(data));
        //effectParticles.Add(Instantiate(data.EffectParticles, transform));
    }

    public void RemoveEffect(StatusEffect status)
    {
        statusEffects.Remove(status);
        //if (effectParticles != null) Destroy(effectParticles);
    }

    public void HandleEffect()
    {
        //foreach (StatusEffect status in statusEffects)
        //{
        //    status.currentEffectTime += Time.deltaTime;
        //    if (status.currentEffectTime >= status.effect.LifeTime)
        //    {
        //        RemoveEffect();
        //        continue;
        //    }

        //    //if (status == null) 

        //    if (status.effect.DOTAmount != 0 && status.currentEffectTime > status.nextTickTime)
        //    {
        //        status.nextTickTime += status.effect.TickSpeed;
        //        TakeDamage(status.effect.DOTAmount);
        //    }
        //}

        for (int i = 0; i < statusEffects.Count; i++)
        {
            statusEffects[i].SetCurrentEffectTime(Time.deltaTime);
            //Debug.Log(statusEffects[i].effect.name);
            if (statusEffects[i].currentEffectTime >= statusEffects[i].effect.LifeTime)
            {
                RemoveEffect(statusEffects[i]);
                continue;
            }

            if (statusEffects[i].effect.DOTAmount != 0 && statusEffects[i].currentEffectTime > statusEffects[i].nextTickTime)
            {
                statusEffects[i].SetNextTickTime();
                TakeDamage(statusEffects[i].effect.DOTAmount);
            }
        }

    }
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffectData effect;
    public float currentEffectTime;
    public float nextTickTime;

    public StatusEffect(StatusEffectData effect)
    {
        this.effect = effect;
        currentEffectTime = 0f;
        nextTickTime = 0f;
    }

    public void SetCurrentEffectTime(float time)
    {
        currentEffectTime += time;
    }

    public void SetNextTickTime()
    {
        nextTickTime += effect.TickSpeed;
    }
}
