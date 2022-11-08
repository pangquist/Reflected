//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability description
/// </summary>
[RequireComponent(typeof(Player))]
public abstract class Ability : MonoBehaviour
{
    [Header("Ability Stats")]
    [SerializeField] protected Sprite abilityIcon;
    [SerializeField] protected List<AudioClip> abilitySounds = new List<AudioClip>();
    [SerializeField] protected AudioSource abilitySource;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float remainingCooldown;
    [SerializeField] protected string abilityName;
    [SerializeField] protected float damage;
    [SerializeField] protected bool debug;
    [SerializeField] AnimationClip abilityAnimation;
    [SerializeField] GameObject vfxObject;
    [SerializeField] float vfxDuration;
    protected Player player;

    protected AbilityCooldowns cooldownstarter;

    private void Start()
    {
        cooldownstarter = FindObjectOfType<AbilityCooldowns>();
        player = GetComponent<Player>();
    }

    public virtual bool DoEffect()
    {
        remainingCooldown = cooldown * player.GetStats().GetCooldownDecrease();
        if (abilitySounds.Count != 0 && abilitySource)
            abilitySource.PlayOneShot(abilitySounds[Random.Range(0,abilitySounds.Count)]);
        return true;
    }

    public virtual AnimationClip GetAnimation()
    {
        remainingCooldown = cooldown * player.GetStats().GetCooldownDecrease();
        if (abilitySounds.Count != 0 && abilitySource)
            abilitySource.PlayOneShot(abilitySounds[Random.Range(0, abilitySounds.Count)]);

        return abilityAnimation;
    }

    protected virtual void Update()
    {
        if (IsOnCooldown())
            remainingCooldown -= Time.deltaTime;
    }

    public bool IsOnCooldown()
    {
        return remainingCooldown > 0;
    }

    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }

    public virtual float GetCooldownPercentage()
    {
        return remainingCooldown / (cooldown * player.GetStats().GetCooldownDecrease());
    }

    public Sprite GetIcon()
    {
        return abilityIcon;
    }

    public string GetName()
    {
        return abilityName;
    }

    public void PlayVFX()
    {
        StartCoroutine(DoVFX());
    }

    IEnumerator DoVFX()
    {
        vfxObject.SetActive(true);

        yield return new WaitForSeconds(vfxDuration);

        vfxObject.SetActive(false);
    }
}