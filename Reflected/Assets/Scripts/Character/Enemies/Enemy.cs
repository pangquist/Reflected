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
    protected Player player;

    bool playerNoticed;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        parent = gameObject.transform.parent.gameObject;
    }

    protected override void Update()
    {
        base.Update();
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
        //player.RemoveEnemy(this);
        anim.Play("Death");
        base.Die();
    }

    public void AdaptiveDifficulty(float extraDifficultyPercentage) //called when instaintiated (from the EnemySpanwer-script)
    {
        currentHealth += maxHealth * extraDifficultyPercentage;

        damage += damage * extraDifficultyPercentage;
    }

    public void LootDrop(Transform lootDropPosition)
    {
        LootDropList = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>().GetCollectablePool();

        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 1, 0);
        Instantiate(LootDropList.GetRandom(), spawnPosition, Quaternion.Euler(0,0,0));
    }
}
