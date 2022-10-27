using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent)
    {
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        //                     This class is currently not used, as all transitions between states are handled in those particular states.
        // This was meant to choose the start state for the AI, but no matter what you choose as the start state it will transition to what it should be.
        // ---------------------------------------------------------------------------------------------------------------------------------------------


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
