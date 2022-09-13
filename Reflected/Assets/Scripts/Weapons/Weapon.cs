using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] protected float damage;
    protected float speed;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public abstract void DoAttack();

    public abstract void DoSpecialAttack();
    public abstract void WeaponEffect();
}
