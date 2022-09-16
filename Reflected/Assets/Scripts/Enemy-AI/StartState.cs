using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : State
{
    public override void DoState(AiManager thisEnemy, Player player)
    {
        if (thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) >= 5)
        {
            thisEnemy.SetMoveTowardState();
            return;
        }
        else if(thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) <= 5)
        {
            thisEnemy.SetAttackPlayerState();
            return;
        }
        else if(!thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) <= 25)
        {
            thisEnemy.SetMoveAwayState();
            return;
        }
        else if(!thisEnemy.CloseCombat() && thisEnemy.distanceTo(player) >= 25)
        {
            thisEnemy.SetAttackPlayerState();
            return;
        }
        else
        {
            thisEnemy.SetStartState();
            return;
        }
    }
}
