using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    protected Animator anim;
    protected float damage;

    protected float speed;

    [SerializeField] protected List<Enemy> hitEnemies;

    protected PlayerController playerController;

    Player player;

    private AbilityCooldowns cooldownstarter;

    [SerializeField] protected int powerUpIndex;
    //[SerializeField] List<Weapons> projectiles = new List<Weapons>();

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.Find("Playerbody").GetComponent<PlayerController>();

        player = GameObject.Find("Player").GetComponent<Player>();

        hitEnemies = new List<Enemy>();
    }

    protected virtual void Update()
    {

    }

    public virtual void WeaponEffect()
    {

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

    public void SetWeaponIndex(int index)
    {
        powerUpIndex = index;        
    }
}
