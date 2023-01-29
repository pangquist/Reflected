using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    //Range to player in which the enemy will attack
    [SerializeField] private float meleeAttackRange = 2f;
    [SerializeField] private float rangedAttackRange = 20f;
    [SerializeField] private float aoeAttackRange = 20f;
    [SerializeField] private float explosionAttackRange = 2f; 
    
    [Header("Current Movement Speed")]
    [SerializeField] private float movementSpeed;

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat:
                if (thisEnemy.Elite())
                {
                    if (thisEnemy.distanceTo(player.transform.position) <= meleeAttackRange + 2f)
                    {
                        thisEnemy.SetMeleeAttackState();
                        agent.isStopped = true;
                        me.PlayAnimation("Idle");
                        return;
                    }
                }
                else if (thisEnemy.distanceTo(player.transform.position) <= meleeAttackRange)
                {
                    thisEnemy.SetMeleeAttackState();
                    agent.isStopped = true;
                    me.PlayAnimation("Idle");
                    return;
                }
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                if (thisEnemy.distanceTo(player.transform.position) <= rangedAttackRange)
                {
                    thisEnemy.SetRangedAttackState();
                    agent.isStopped = true;
                    me.PlayAnimation("Idle");
                    return;
                }
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                if (thisEnemy.distanceTo(player.transform.position) <= aoeAttackRange)
                {
                    thisEnemy.SetAoeAttackState();
                    agent.isStopped = true;
                    me.PlayAnimation("Idle");
                    return;
                }
                break;

            case AiManager2.CombatBehavior.ExplosionCombat:
                if(thisEnemy.distanceTo(player.transform.position) <= explosionAttackRange)
                {
                    thisEnemy.SetExplosionAttackState();
                    agent.isStopped = true;
                    return;
                }
                break;

            default:
                break;
        }

        //Set movement speed from base, statsystem and debuff.
        movementSpeed = me.GetMovementSpeed() * enemyStatSystem.GetMovementSpeed() * thisEnemy.me.MovementPenalty();
        agent.speed = movementSpeed;

        DoMoveToward(player.transform, agent);
    }

    private void DoMoveToward(Transform target, NavMeshAgent agent)
    {
        agent.destination = target.position;
    }
}
