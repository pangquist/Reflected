using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Transform target, NavMeshAgent agent)
    {
        Debug.Log(thisEnemy.CloseCombat());
        //If melee and too far away, move towards target.
        if (thisEnemy.distanceTo(target) >= 5 && thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too close, move away from target.
        else if (thisEnemy.distanceTo(target) <= 7 && !thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(target) >= 20 && !thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        FaceTarget(target.position);
        agent.destination = thisEnemy.transform.position;
        DoAttack();
    }

    private void DoAttack()
    {
        
        Debug.Log("Enemy attacked you!");
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.02f);
    }
}
