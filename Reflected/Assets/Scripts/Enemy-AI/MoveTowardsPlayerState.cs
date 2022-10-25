using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    //Range to player in which the enemy will attack
    [SerializeField] private float meleeAttackRange = 2f;
    [SerializeField] private float rangedAttackRange = 15f;
    [SerializeField] private float aoeAttackRange = 15;

    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent)
    {
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat:
                if (thisEnemy.distanceTo(player.transform) <= meleeAttackRange)
                {
                    thisEnemy.SetMeleeAttackState();
                    agent.isStopped = true;
                    return;
                }
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                if (thisEnemy.distanceTo(player.transform) <= rangedAttackRange)
                {
                    thisEnemy.SetRangedAttackState();
                    agent.isStopped = true;
                    return;
                }
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                if (thisEnemy.distanceTo(player.transform) <= aoeAttackRange)
                {
                    thisEnemy.SetAoeAttackState();
                    agent.isStopped = true;
                    return;
                }
                break;

            default:
                break;
        }

        DoMoveToward(player.transform, agent);
    }

    private void DoMoveToward(Transform target, NavMeshAgent agent)
    {
        agent.destination = target.position;
    }
}
