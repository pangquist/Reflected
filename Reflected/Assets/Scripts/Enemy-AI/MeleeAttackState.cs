using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : State
{
    public GameObject meleeObject;

    //Timer for attack rate
    private float attackTimer = 0f;

    //Base values of the attack stats
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int attackDamage = 5;

    //Range to player in which the enemy will chase the player. (Reposition to attack)
    [SerializeField] private float chaseRange = 2.5f;


    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set attack rate, chase range?
        
        if (thisEnemy.distanceTo(player.transform) >= chaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        //Make enemy face player and stand still
        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            DoAttack(thisEnemy);
            attackTimer = 0f;
        }
    }

    private void DoAttack(AiManager2 thisEnemy)
    {
        //Set attack damage (attack size?)

        GameObject currentMeleeAttack = Instantiate(meleeObject, thisEnemy.firePoint.position, gameObject.transform.rotation);
        if (currentMeleeAttack != null)
        {
            currentMeleeAttack.GetComponent<MeleeHitboxScript>().SetUp(attackDamage);
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
