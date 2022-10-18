using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent)
    {
        //Test if this works
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat: //Should be "thisEnemy.Combat..." but it doesnt work??
                Debug.Log("Switch case: CloseCombat entered at position: " + transform.position);
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                Debug.Log("Switch case: RangedCombat entered at position: " + transform.position);
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                Debug.Log("Switch case: AoeCombat entered at position: " + transform.position);
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
