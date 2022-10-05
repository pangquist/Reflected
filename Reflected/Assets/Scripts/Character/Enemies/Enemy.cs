using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] Image healthBar;

    [SerializeField] Canvas combatTextCanvas;
    [SerializeField] float aggroRange;

    [SerializeField] WeightedRandomList<GameObject> LootDropList;

    GameObject parent;
    Player player;

    bool playerNoticed;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        parent = gameObject.transform.parent.gameObject;
    }

    protected override void Update()
    {
        base.Update();
        //float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        //if (!playerNoticed)
        //{
        //    if (distance <= aggroRange)
        //    {
        //        playerNoticed = true;
        //    }
        //}
        //else
        //{
        //    if (distance > aggroRange)
        //    {
        //        playerNoticed = false;
        //    }
        //}

        //if (playerNoticed)
        //{
        //    Vector3 direction = (transform.position - player.transform.position).normalized;
        //    direction.y = 0;
        //    parent.transform.rotation = Quaternion.LookRotation(direction);
        //}
    }

    public override void TakeDamage(float damage)
    {
        if (currentHealth == maxHealth)
            healthBar.gameObject.SetActive(true);
        else if (currentHealth <= 0)
        {
            Die();
            return;
        }

        Vector3 direction = (transform.position - player.transform.position).normalized;
        direction.y = 0;
        parent.transform.rotation = Quaternion.LookRotation(direction);

        CombatText text = Instantiate(combatTextCanvas.gameObject, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<CombatText>();
        text.SetDamageText(damage);

        base.TakeDamage(damage);
        healthBar.fillAmount = GetHealthPercentage();
    }

    protected override void Die()
    {
        AiDirector aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
        aiDirector.killEnemyInRoom();
        LootDrop(transform);
        base.Die();
    }

    public IEnumerator Freeze(float slowedMovementSpeed ,float timeFrozen)
    {
        float oldMovmentSpeed = movementSpeed;
        movementSpeed *= slowedMovementSpeed;

        yield return new WaitForSeconds(timeFrozen);

        movementSpeed = oldMovmentSpeed;
    }
    public IEnumerator TakeDamageOverTime(float damageOverTime, float time)
    {
        for (int i = 0; i < time; i++)
        {
            currentHealth -= damageOverTime / time;
            yield return new WaitForSeconds(1);
        }
    }

    public void AdaptiveDifficulty(float extraDifficultyPercentage) //called when instaintiated (from the EnemySpanwer-script)
    {
        currentHealth += maxHealth * extraDifficultyPercentage;

        damage += damage * extraDifficultyPercentage;
    }

    public void LootDrop(Transform lootDropPosition)
    {
        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 2, 0);
        Instantiate(LootDropList.GetRandom(), spawnPosition, Quaternion.Euler(90,0,0));
    }
}
