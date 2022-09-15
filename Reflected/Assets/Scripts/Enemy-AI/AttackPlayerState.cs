using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Player player)
    {
        if (distanceTo(player) >= 5)
        {
            thisEnemy.SetMoveTowardState();
            return;
        }

        DoAttack();
    }

    private void DoAttack()
    {
        Debug.Log("You got smashed mate! uwu");
    }
}
