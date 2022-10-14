using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    protected Animator anim;
    protected float damage;

    protected float speed;

    [Header("Combo")]
    float comboTimer;
    protected int currentComboIndex;
    [SerializeField] protected AnimationClip[] comboClips;
    [SerializeField] protected float maxTimeBetweenCombo;

    [Header("Special Attack")]
    [SerializeField] protected AnimationClip specialAttackClip;
    [SerializeField] protected float specialAttackCooldown;
    [SerializeField] float timeSinceLastSpecialAttack;

    [SerializeField] protected List<Enemy> hitEnemies;

    protected PlayerController playerController;

    Player player;

    private AbilityCooldowns cooldownstarter;



    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Playerbody").GetComponent<PlayerController>();

        player = GameObject.Find("Player").GetComponent<Player>();

        cooldownstarter = FindObjectOfType<AbilityCooldowns>();

        hitEnemies = new List<Enemy>();
        currentComboIndex = 0;
        timeSinceLastSpecialAttack = specialAttackCooldown;
    }

    protected virtual void Update()
    {
        if(currentComboIndex > 0)
        {
            comboTimer += Time.deltaTime;
            if(comboTimer > maxTimeBetweenCombo)
            {
                comboTimer = 0;
                currentComboIndex = 0;
            }
        }

        if (timeSinceLastSpecialAttack < specialAttackCooldown)
            timeSinceLastSpecialAttack += Time.deltaTime;
    }

    //public virtual AnimationClip[] GetBasicAttack()
    //{
    //    return comboClips;
    //}

    public virtual AnimationClip DoAttack()
    {
        if (playerController.GetAttackLocked())
            return null;

        ClearEnemies();
        playerController.SetAttackLocked(true);

        if (currentComboIndex == comboClips.Length)
            currentComboIndex = 0;

        return comboClips[currentComboIndex++];

    }

    public virtual void AttackWithoutAnimation()
    {

    }

    public virtual AnimationClip DoSpecialAttack()
    {
        ClearEnemies();
        playerController.SetAttackLocked(true);
        timeSinceLastSpecialAttack = 0;
        cooldownstarter.Ability1Use();
        return specialAttackClip;
    }

    public virtual void WeaponEffect()
    {

    }

    public bool IsLocked()
    {
        return playerController.GetAttackLocked();
    }

    public bool IsOnCooldown()
    {
        return timeSinceLastSpecialAttack < specialAttackCooldown;
    }
    public float GetCooldown()
    {
        return specialAttackCooldown;
    }
    public float GetCurrentCooldown()
    {
        return specialAttackCooldown - timeSinceLastSpecialAttack;
    }

    public virtual void Unlock()
    {
        playerController.SetAttackLocked(false);
    }

    public virtual void ClearEnemies()
    {
        hitEnemies.Clear();
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public float GetDamage()
    {
        return damage * player.GetStats().GetDamageIncrease();
    }
}
