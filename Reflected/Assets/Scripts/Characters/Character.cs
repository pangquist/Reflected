using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour, IEffectable
{
    //Should these not be "base" properties? As they will be affected by the stat system increases/power-ups. OR are they the final calculation of said stats? -Kevin
    [Header("Character Properties")] 
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;

    protected List<Effect> statusEffects;
    protected Dictionary<StatusEffectData, GameObject> effectParticles;

    [SerializeField] protected Animator anim;

    protected bool isDead;

    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected List<AudioClip> damagedClips;
    [SerializeField] protected List<AudioClip> upgradedDamagedClips;

    protected AudioSource audioSource;

    public UnityEvent HealthChanged = new UnityEvent();
    public UnityEvent Killed = new UnityEvent();

    protected virtual void Start()
    {
        if (this.GetType() != typeof(Player))
            anim = GetComponent<Animator>();
        statusEffects = new List<Effect>();
        effectParticles = new Dictionary<StatusEffectData, GameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (statusEffects.Count > 0) HandleEffect();
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= Mathf.Clamp(damage, 0, currentHealth); 

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.Play("Damaged");
        }

        HealthChanged.Invoke();
        PopUpTextManager.NewDamage(transform.position + Vector3.up * 1.5f, damage);
        PlayDamangedAudioClip();
    }

    protected void PlayDamangedAudioClip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(damagedClips.GetRandom());
    }

    public virtual void Heal(float amount)
    {
        if (!isDead)
        {
            currentHealth += Mathf.Clamp(amount, 0, maxHealth - currentHealth);
            HealthChanged.Invoke();
            PopUpTextManager.NewHeal(transform.position + Vector3.up * 1.5f, amount);
        }
        
    }

    protected virtual void Die()
    {
        anim.Play("Death");
        isDead = true;
        Killed.Invoke();
    }


    public List<Effect> GetStatusEffectList()
    {
        return statusEffects;
    }

    protected void Destroy()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }

    public virtual float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public virtual float GetMaxHealth()
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
        return attackSpeed;
    }
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float MovementPenalty()
    {
        float movementPenalty = 1;
        if (statusEffects.Count > 0)
        {
            foreach (Effect status in statusEffects)
            {
                movementPenalty *= (1 - status.totalSlow);
            }
        }
        //Debug.Log("Movement penalty total: " + movementPenalty);
        return movementPenalty;
    }

    public void ApplyEffect(StatusEffectData data, float scale)
    {
        if (!effectParticles.ContainsKey(data))
        {
            effectParticles.Add(data, Instantiate(data.EffectParticles, transform));
            statusEffects.Add(new Effect(data, scale));
        }           
    }

    public void RemoveEffect(Effect status)
    {
        statusEffects.Remove(status);
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (statusEffects[i].effect == status.effect)
            {
                return;
            }
        }
        Destroy(effectParticles[status.effect]);
        effectParticles.Remove(status.effect);
        //if (effectParticles != null) Destroy(effectParticles);
    }

    public virtual void HandleEffect()
    {        
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
                if (statusEffects[i].effect.DOTAmount > 0)
                    TakeDamage(statusEffects[i].effect.DOTAmount * maxHealth);
                else
                    Heal(-1 * statusEffects[i].effect.DOTAmount * maxHealth);
            }
        }
    }

    public void PlayAnimation(string animName)
    {
        anim.Play(animName);
    }


    public void FocusCamera()
    {
        CinemachineFreeLook freeLook = Object.FindObjectOfType<CinemachineFreeLook>();
        freeLook.Follow = transform;
    }

    public bool Dead() => isDead;
}

[System.Serializable]
public class Effect
{
    public StatusEffectData effect;
    public float currentEffectTime;
    public float nextTickTime;
    public float totalDamage;
    public float totalSlow;


    public Effect(StatusEffectData effect, float scale) //Scale should be removed and everything that uses it
    {
        this.effect = effect;
        currentEffectTime = 0f;
        nextTickTime = 0f;
        totalDamage = effect.DOTAmount * scale;
        totalSlow = effect.MovementPenalty * scale;
        if(totalSlow > 1)
        {
            totalSlow = 1;
        }
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
