using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Player player, Transform goal, NavMeshAgent agent)
    {
        //if (thisEnemy.distanceTo(player) >= 3)
        //{
        //    thisEnemy.SetAttackPlayerState();
        //    return;
        //}

        if (thisEnemy.distanceTo() <= 3)
        {
            thisEnemy.SetAttackPlayerState();
            agent.isStopped=true;
            return;
        }


        DoMoveToward(player, goal, agent);
    }

    private void DoMoveToward(Player player, Transform goal, NavMeshAgent agent)
    {
        Debug.Log("MoveTowardsPlayer");
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = player.gameObject.transform.position;
    }
}
