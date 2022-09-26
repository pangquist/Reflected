using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Transform target, NavMeshAgent agent)
    {
        //if (thisEnemy.distanceTo(player) >= 3)
        //{
        //    thisEnemy.SetAttackPlayerState();
        //    return;
        //}

        if (thisEnemy.distanceTo(target) <= 3 && thisEnemy.CloseCombat())
        {
            thisEnemy.SetAttackPlayerState();
            agent.isStopped = true;
            return;
        }
        else if (thisEnemy.distanceTo(target)<=15 && !thisEnemy.CloseCombat())
        {
            thisEnemy.SetAttackPlayerState();
            agent.isStopped = true;
            return;
        }


        DoMoveToward(target, agent);
    }

    private void DoMoveToward(Transform target, NavMeshAgent agent)
    {
        Debug.Log("MoveTowardsPlayer");
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.gameObject.transform.position;
    }
}
