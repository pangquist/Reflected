//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss description
/// </summary>
public class Boss : Enemy
{
    [Header("Boss Specifics")]
    bool aggroed = false;
    [SerializeField] GameObject rotateBody;
    [SerializeField] List<Ability> abilities;
    [SerializeField] float abilityTimer;

    bool rotateLock;

    protected override void Update()
    {
        if (isDead)
            return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < aggroRange && !aggroed)
        {
            anim.Play("Activation");
            aggroed = true;
        }

        if (!aggroed)
            return;

        AbilityTimer();
        if (!rotateLock)
            StartCoroutine(_RotateTowardsPlayer());

        base.Update();
    }

    IEnumerator _RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - rotateBody.transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        while(Quaternion.Angle(rotateBody.transform.rotation, targetRotation) > 0.01f)
        {
            //Quaternion nextRotation = Quaternion.Lerp(rotateBody.transform.rotation, targetRotation, Time.deltaTime);
            rotateBody.transform.rotation = Quaternion.RotateTowards(rotateBody.transform.rotation, targetRotation, Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

    public void AbilityTimer()
    {
        if (!invurnable)
            abilityTimer -= Time.deltaTime;

        while (abilityTimer <= 0)
        {
            //Do Random Ability
            Ability chosenAbility = abilities[Random.Range(0, abilities.Count)];
            if (!chosenAbility.IsOnCooldown())
            {
                chosenAbility.DoEffect();
                abilityTimer = chosenAbility.GetCastTime();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void ToggleRotationLock() => rotateLock = !rotateLock;

    public override void LootDrop(Transform lootDropPosition)
    {
        LootDropList = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>().GetCollectablePool();

        Vector3 spawnPosition = lootDropPosition.position + new Vector3(0, 1, 0);
        Instantiate(LootDropList.GetItem(LootDropList.Count - 1), spawnPosition, Quaternion.Euler(0, 0, 0));
    }
}