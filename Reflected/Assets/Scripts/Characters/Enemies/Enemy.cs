using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] protected Image healthBar;

    [SerializeField] protected Vector3 combatTextOffset;
    [SerializeField] protected Canvas combatTextCanvas;
    [SerializeField] protected float aggroRange;

    [SerializeField] protected WeightedRandomList<GameObject> LootDropList;
    protected bool invurnable;
    GameObject parent;
    protected Player player;

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

        if (currentHealth == maxHealth)
            healthBar.gameObject.SetActive(true);
        else if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if(!GetComponent<Boss>())
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            direction.y = 0;
            parent.transform.rotation = Quaternion.LookRotation(direction);
        }


        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + combatTextOffset, Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        Debug.Log("ENEMY TOOK DAMAGE: " + damage);
        base.TakeDamage(damage);
        healthBar.fillAmount = GetHealthPercentage();
    }

    protected override void Die()
    {
        AiDirector aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
        if (!doOnce)
        {
            aiDirector.killEnemyInRoom();
            doOnce = true;
        }
        
        LootDrop(transform);
        //player.RemoveEnemy(this);
        //anim.Play("Death");
        base.Die();
    }

    public void AdaptiveDifficulty(float extraDifficultyPercentage) //called when instaintiated (from the EnemySpanwer-script)
    {
        currentHealth += maxHealth * extraDifficultyPercentage;

        damage += damage * extraDifficultyPercentage;
    }

    public virtual void LootDrop(Transform lootDropPosition)
    {
        LootDropList = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>().GetCollectablePool();

        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 1, 0);
        Instantiate(LootDropList.GetRandom(), spawnPosition, Quaternion.Euler(0,0,0));
    }

    public virtual void ToggleInvurnable()
    {
        invurnable = !invurnable;
    }

    public void DoAttack()
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
            explosionAttackState.DoAttack();
        }
    }
}
