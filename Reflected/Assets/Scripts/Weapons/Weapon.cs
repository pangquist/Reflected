using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    protected Animator anim;
    [SerializeField] protected float damage;

    protected float speed;

    [Header("Combo")]
    float comboTimer;
    protected int currentComboIndex;
    [SerializeField] protected AnimationClip[] comboClips;
    [SerializeField] protected float maxTimeBetweenCombo;

    [Header("Special Attack")]
    [SerializeField] protected float specialAttackCooldown;
    [SerializeField] float timeSinceLastSpecialAttack;

    PlayerController playerController;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        currentComboIndex = 0;
        timeSinceLastSpecialAttack = specialAttackCooldown;
    }

    public virtual void Update()
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

    public virtual void DoAttack()
    {
        if (playerController.GetAttackLocked())
            return;

        playerController.SetAttackLocked(true);
    }

    public virtual void DoSpecialAttack()
    {
        if (timeSinceLastSpecialAttack < specialAttackCooldown)
            return;

        anim.Play("SpecialAttack");
        timeSinceLastSpecialAttack = 0;
    }

    public abstract void WeaponEffect();

    public virtual void Unlock()
    {
        playerController.SetAttackLocked(false);
    }
}
