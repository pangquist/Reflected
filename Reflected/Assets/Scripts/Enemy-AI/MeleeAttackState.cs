using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : State
{
    //Object for melee hitbox.
    public GameObject meleeObject;

    //Timer for attack rate.
    private float attackTimer = 100f;

    //Base values of the attack stats
    [SerializeField] private float baseAttackRate = 1f;
    [SerializeField] private float baseAttackDamage = 5;

    //Current values of the attack stats
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDamage;

    //Range to player in which the enemy will chase the player. (Reposition to attack)
    [SerializeField] private float chaseRange = 2.5f;

    private Transform firePoint;

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set attack rate, by using default, base, statsystem change as well as debuf.
        attackRate = baseAttackRate / me.GetAttackSpeed() / enemyStatSystem.GetAttackSpeed() / me.MovementPenalty();

        //If player is too far away, chase the player.
        if (thisEnemy.distanceTo(player.transform) >= chaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            me.PlayAnimation("Walk Forward In Place");
            return;
        }

        //Make enemy face player and stand still
        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            //Set attack damage from base and statsystem
            attackDamage = me.GetDamage() * enemyStatSystem.GetDamageIncrease();

            firePoint = thisEnemy.firePoint;

            //Play attack animation that will trigger attack
            me.PlayAnimation("Bite Attack");
            attackTimer = 0f;
        }
    }

    public void DoAttack()
    {
        //Instantiate the hitbox and set up its variables.
        GameObject currentMeleeAttack = Instantiate(meleeObject, firePoint.position, gameObject.transform.rotation);
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
