using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent)
    {
        //Seems to work as intended even with multiple different enemies, and multiple of the same enemy. Try to use this istead of thisEnemy.CloseCombat().
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat:
                //Debug.Log("Switch case: CloseCombat entered at position: " + transform.position);
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                //Debug.Log("Switch case: RangedCombat entered at position: " + transform.position);
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                //Debug.Log("Switch case: AoeCombat entered at position: " + transform.position);
                break;

            default:
                break;
        }
        
        if (thisEnemy.distanceTo(player.transform /*target*/) <= 2f && thisEnemy.CloseCombat())
        {
            thisEnemy.SetMeleeAttackState();
            agent.isStopped = true;
            return;
        }
        else if (thisEnemy.distanceTo(player.transform /*target*/) <=15f && thisEnemy.RangedCombat())
        {
            thisEnemy.SetRangedAttackState();
            agent.isStopped = true;
            return;
        }
        else if (thisEnemy.distanceTo(player.transform /*target*/) <= 15f && thisEnemy.AoeCombat())
        {
            thisEnemy.SetAoeAttackState();
            agent.isStopped = true;
            return;
        }


        DoMoveToward(player.transform /*target*/, agent);
    }

    private void DoMoveToward(Transform target, NavMeshAgent agent)
    {
        agent.destination = target.position;
    }
}
