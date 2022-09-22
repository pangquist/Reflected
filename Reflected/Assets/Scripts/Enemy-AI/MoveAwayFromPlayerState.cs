using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Player player, Transform goal, NavMeshAgent agent)
    {
        //if (thisEnemy.distanceTo(player) <= 25)
        //{
        //    //thisEnemy.SetMoveAwayState();
        //    return;
        //}

        DoMoveAway();
    }

    private void DoMoveAway()
    {
        //pick a random spot away from the player and move there?
    }
}
