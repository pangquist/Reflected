using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerState : State
{
    public override void DoState(AiManager thisEnemy, /*Player player,*/ Transform goal, NavMeshAgent agent)
    {
        if (thisEnemy.distanceTo(goal) >= 5)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        DoAttack();
    }

    private void DoAttack()
    {
        Debug.Log("You got smashed mate! uwu");
    }
}
