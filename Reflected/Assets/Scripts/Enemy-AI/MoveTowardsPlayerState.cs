using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayerState : State
{
    public override void DoState(AIManager thisEnemy, Player player)
    {
        if (distanceTo(player.gameObject.position) >= 3)
        {
            thisEnemy.SetAttackPlayerState();
            return;
        }

        DoMoveToward();
    }

    private void DoMoveToward()
    {
        //should we use A*? or some built in unity function? Or something more simple? 
    }
}
