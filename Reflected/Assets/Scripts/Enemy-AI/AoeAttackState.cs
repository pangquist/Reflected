using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AoeAttackState : State
{
    //Object for aoe hitbox
    public GameObject aoeObject;

    //Timer for attack rate
    private float attackTimer = 100f;

    //Base values of the attack stats
    private float baseAttackRate = 4f;

    [Header("Current ATTACK Stats")]
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDamage;
    [SerializeField] private Vector3 aoeSize;

    [Header("Base POSITIONING Values")]
    [SerializeField] private float fleeRange = 7f;
    [SerializeField] private float chaseRange = 20f;

    //Variables for setting up aoe
    private Vector3 playerPos;

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set attack rate, by using default, base, statsystem change as well as debuff.
        attackRate = baseAttackRate / me.GetAttackSpeed() / enemyStatSystem.GetAttackSpeed() / me.MovementPenalty();

        //If ranged and too close, move away from target.
        if (thisEnemy.distanceTo(player.transform) <= fleeRange)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            me.PlayAnimation("Fly Forward In Place");
            return;
        }

        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform) >= chaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            me.PlayAnimation("Fly Forward In Place");
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

            //Set aoe size from base and statsystem
            aoeSize = me.GetAoeSize() * enemyStatSystem.GetAreaOfEffect();

            //Setup parameters for projectile to use
            playerPos = player.transform.position;

            //Play attack animation that will trigger the attack
            me.PlayAnimation("Fire AOE");

            //Reset attack boolTimer
            attackTimer = 0f;
        }
    }

    public void DoAttack()
    {
        //Instantiate the aoe hitbox and set up its variables.
        GameObject currentAOE = Instantiate(aoeObject, playerPos, Quaternion.identity);
        if (currentAOE != null)
        {
            currentAOE.GetComponentInChildren<AOEScript>().SetUp(attackDamage, aoeSize);
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
