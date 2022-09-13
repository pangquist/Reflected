using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    protected Animator anim;
    [SerializeField] protected float damage;

    protected float speed;

    float comboTimer;
    protected int currentComboIndex;
    [SerializeField] protected AnimationClip[] comboClips;
    [SerializeField] protected float maxTimeBetweenCombo;

    PlayerController playerController;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        currentComboIndex = 0;
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
    }

    public virtual void DoAttack()
    {
        if (playerController.GetAttackLocked())
            return;

        playerController.SetAttackLocked(true);
    }

    public abstract void DoSpecialAttack();
    public abstract void WeaponEffect();

    public virtual void Unlock()
    {
        playerController.SetAttackLocked(false);
    }
}
