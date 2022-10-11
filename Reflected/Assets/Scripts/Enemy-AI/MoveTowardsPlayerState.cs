using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent)
    {
        //Test if this works
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat: //Should be "thisEnemy.Combat..." but it doesnt work??
                Debug.Log("Switch case: CloseCombat entered");
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                Debug.Log("Switch case: RangedCombat entered");
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                Debug.Log("Switch case: AoeCombat entered");
                break;

            default:
                break;
        }
        
        if (thisEnemy.distanceTo(player.transform) <= 2f && thisEnemy.CloseCombat())
        {
            thisEnemy.SetMeleeAttackState();
            agent.isStopped = true;
            return;
        }
        else if (thisEnemy.distanceTo(player.transform)<=15f && thisEnemy.RangedCombat())
        {
            thisEnemy.SetRangedAttackState();
            agent.isStopped = true;
            return;
        }
        else if (thisEnemy.distanceTo(player.transform)<= 15f && thisEnemy.AoeCombat())
        {
            thisEnemy.SetAoeAttackState();
            agent.isStopped = true;
            return;
        }


        DoMoveToward(player.transform, agent);
    }

    private void DoMoveToward(Transform target, NavMeshAgent agent)
    {
        agent.destination = target.position;
    }
}
