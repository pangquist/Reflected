using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    public override void DoState(AiManager thisEnemy, Transform target, NavMeshAgent agent)
    {
        //if (thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) >= 5)
        //{
        //    thisEnemy.SetMoveTowardState();
        //    return;
        //}
        //else if(thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) <= 5)
        //{
        //    thisEnemy.SetAttackPlayerState();
        //    return;
        //}
        //else if(!thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) <= 25)
        //{
        //    thisEnemy.SetMoveAwayState();
        //    return;
        //}
        //else if(!thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) >= 25)
        //{
        //    thisEnemy.SetAttackPlayerState();
        //    return;
        //}
        //else
        //{
        //    thisEnemy.SetStartState();
        //    return;
        //}
    }
}
