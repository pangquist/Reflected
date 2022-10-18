using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromPlayerState : State
{
    private float fleeTimer = 0f;
    private float changeTime = 1f;
    public override void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent)
    {
        Debug.Log(thisEnemy.distanceTo(player.transform /*target*/));
        Debug.Log(thisEnemy.RangedCombat());
        
        //If ranged attack set to ranged attack
        if (thisEnemy.distanceTo(player.transform /*target*/) >= 15 && thisEnemy.RangedCombat())
        {
            thisEnemy.SetRangedAttackState();
            return;
        }
        //Else if aoe attack set to aoe
        else if (thisEnemy.distanceTo(player.transform /*target*/) >= 15 && thisEnemy.AoeCombat())
        {
            thisEnemy.SetAoeAttackState();
            return;
        }

        DoMoveAway(player.transform /*target*/, agent);
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
