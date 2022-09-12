using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayFromPlayerState : State
{
    public override void DoState(AIManager thisEnemy, Player player)
    {
        if (distanceTo(player.gameObject.position) <= 25)
        {
            thisEnemy.SetMoveAwayState();
            return;
        }

        DoMoveAway();
    }

    private void DoMoveAway()
    {
        //pick a random spot away from the player and move there?
    }
}
