using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromPlayerState : State
{
    private float fleeTimer = 0f;
    private float changeTime = 1f;
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

        fleeTimer += Time.deltaTime;
        if (fleeTimer >= changeTime)
        {
            Vector3 moveTo = transform.position + ((transform.position - target.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)) * multiplier));
            agent.destination = moveTo;
            fleeTimer = 0f;
        }

    }
}
