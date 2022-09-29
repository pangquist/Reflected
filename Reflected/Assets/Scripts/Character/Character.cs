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
    protected float currentHealth;
    protected List<StatusEffect> statusEffects;
    protected float currentEffectTime = 0f;
    protected float nextTickTime = 0f;
    protected Animator anim;
    protected List<GameObject> effectParticles;

    protected Weapon currentWeapon;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //foreach (StatusEffect status in statusEffects)
        //{
            
        //    HandleEffect(status);
        //}
        if (statusEffects.Count > 0) HandleEffect();
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

    public virtual void Heal(int amount)
    {
        currentHealth += Mathf.Clamp(amount, 0, maxHealth - currentHealth);
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

    public void ApplyEffect(StatusEffectData data)
    {
        statusEffects.Add(new StatusEffect(data));
        //effectParticles.Add(Instantiate(data.EffectParticles, transform));
    }

    public void RemoveEffect()
    {
        //statusEffects.Remove()
        //statusEffects = null;
        //Need to be reset otherwise the next DOT won't apply
        currentEffectTime = 0;
        nextTickTime = 0;
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
            if (statusEffects[i].currentEffectTime >= statusEffects[i].effect.LifeTime)
            {
                RemoveEffect();
                continue;
            }

            if (statusEffects[i].effect.DOTAmount != 0 && statusEffects[i].currentEffectTime > statusEffects[i].nextTickTime)
            {
                statusEffects[i].SetNextTickTime();
                TakeDamage(statusEffects[i].effect.DOTAmount);
            }
        }
        
    }

    [System.Serializable]
    public struct StatusEffect
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

}
