using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromPlayerState : State
{
    public override void DoState(AIManager thisEnemy, Transform target, NavMeshAgent agent)
    {
        if (thisEnemy.distanceTo(target) >= 15)
        {
            thisEnemy.SetAttackPlayerState();
            return;
        }

        DoMoveAway(target, agent);
    }

    private void DoMoveAway(Transform target, NavMeshAgent agent)
    {
        int multiplier = 1;
        Vector3 moveTo = transform.position + ((transform.position - target.position + new Vector3(Random.Range(-12, 12), 0, Random.Range(-15, 12)) * multiplier));
        agent.destination = moveTo;
    }
}
