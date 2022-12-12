using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy Specific Properties (If Relevant)")]
    [SerializeField] protected float baseProjectileSpeed;
    [SerializeField] protected Vector3 baseAoeSize;
    //[SerializeField] protected float fleeRange; //Not gonna be adabted
    //[SerializeField] protected float chaseRange; //Not gonna be adabted
    
    //Are these still used? The manager should be taking care of these right? -Kevin
    [SerializeField] protected Vector3 combatTextOffset;
    [SerializeField] protected Canvas combatTextCanvas;

    [SerializeField] protected WeightedRandomList<GameObject> LootDropList;
    [SerializeField] protected List<AudioClip> hitSounds;
    protected bool invurnable;
    GameObject parent;
    protected Player player;

    //protected float aggroRange;

    bool doOnce;

    [SerializeField] MeleeAttackState meleeAttackState;
    [SerializeField] RangedAttackState rangedAttackState;
    [SerializeField] AoeAttackState aoeAttackState;
    [SerializeField] ExplosionAttackState explosionAttackState;

    protected override void Awake()
    {
        currentHealth = maxHealth;
        base.Awake();
        player = FindObjectOfType<Player>();
        parent = gameObject.transform.parent.gameObject;

        //Assign correct components depending on enemy type.
        if (parent.tag == "Melee")
        {
            meleeAttackState = GetComponentInParent<MeleeAttackState>();
        }
        else if (parent.tag == "Ranged")
        {
            rangedAttackState = GetComponentInParent<RangedAttackState>();
        }
        else if (parent.tag == "AOE")
        {
            aoeAttackState = GetComponentInParent<AoeAttackState>();
        }
        else if (parent.tag == "Explosion")
        {
            explosionAttackState = GetComponentInParent<ExplosionAttackState>();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float damage)
    {
        if (invurnable)
            return;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        //Make the enemy look at the player when taking damage (Not needed? -Kevin)
        /*
        if (!GetComponent<Boss>())
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            direction.y = 0;
            parent.transform.rotation = Quaternion.LookRotation(direction);
        }
        */

        //Call base take damage function
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        //Let the director know an enemy in the room has died.
        AiDirector aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
        if (!doOnce)
        {
            aiDirector.killEnemyInRoom();
            doOnce = true;
        }

        //Drop loot if not a boss
        if (!GetComponent<Boss>())
            LootDrop(transform);

        //Call base die function
        base.Die();
    }

    public void AdaptiveDifficulty(float extraDifficultyPercentage) //called when instantiated (from the EnemySpanwer-script)
    {
        maxHealth += maxHealth * extraDifficultyPercentage;
        currentHealth = maxHealth;
        attackSpeed += attackSpeed * (extraDifficultyPercentage * 0.3f);
        movementSpeed += movementSpeed * (extraDifficultyPercentage * 0.1f);
        damage += damage * extraDifficultyPercentage;
    }

    public virtual void LootDrop(Transform lootDropPosition)
    {
        LootDropList = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>().GetCollectablePool();

        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 1, 0);
        Instantiate(LootDropList.GetRandom(), spawnPosition, Quaternion.Euler(0, 0, 0));
    }

    public virtual void ToggleInvurnable()
    {
        invurnable = !invurnable;
    }

    public void DoAttack() //Called from the animation, that then calls the correct attack depending on enemy type.
    {
        if (parent.tag == "Melee")
        {
            meleeAttackState.DoAttack();
        }
        else if (parent.tag == "Ranged")
        {
            rangedAttackState.DoAttack();
        }
        else if (parent.tag == "AOE")
        {
            aoeAttackState.DoAttack();
        }
        else if (parent.tag == "Explosion")
        {
            explosionAttackState.DoAttack(this);
        }
    }

    public float GetProjectileSpeed()
    {
        return baseProjectileSpeed;
    }
    public Vector3 GetAoeSize()
    {
        return baseAoeSize;
    }
}
