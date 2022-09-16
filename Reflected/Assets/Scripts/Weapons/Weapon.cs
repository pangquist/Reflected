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
    [SerializeField] protected AnimationClip specialAttackClip;
    [SerializeField] protected float specialAttackCooldown;
    [SerializeField] float timeSinceLastSpecialAttack;

    [SerializeField] protected List<Enemy> hitEnemies;

    protected PlayerController playerController;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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

    public virtual void DoAttack()
    {
        if (playerController.GetAttackLocked())
            return;

        playerController.SetAttackLocked(true);
    }



    public virtual void DoSpecialAttack()
    {
        if (timeSinceLastSpecialAttack < specialAttackCooldown || playerController.GetAttackLocked())
            return;

        playerController.SetAttackLocked(true);
        anim.Play(specialAttackClip.name);
        timeSinceLastSpecialAttack = 0;
    }

    public virtual void WeaponEffect()
    {

    }

    public virtual void Unlock()
    {
        playerController.SetAttackLocked(false);
        ClearEnemies();
    }

    public virtual void ClearEnemies()
    {
        hitEnemies.Clear();
    }
}
