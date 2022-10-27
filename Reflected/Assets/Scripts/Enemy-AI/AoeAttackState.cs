using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AoeAttackState : State
{
    private float attackTimer = 0f;
    public float attackRate = 5f;

    public GameObject aoeObject;
    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent)
    {
        //If ranged and too close, move away from target.
        if (thisEnemy.distanceTo(player.transform) <= 7)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform) >= 20)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        attackTimer += Time.deltaTime;

        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;
        if (attackTimer >= attackRate)
        {
            DoAttack(thisEnemy, player);
            attackTimer = 0f;
        }
    }

    private void DoAttack(AiManager2 thisEnemy, Player player)
    {
        GameObject currentAOE = Instantiate(aoeObject, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.identity);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
