using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAwayFromPlayerState : State
{
    //Range to player in which the enemy will stop fleeing. (Reposition to attack)
    [SerializeField] private float rangedFleeRange = 12.5f;
    [SerializeField] private float aoeFleeRange = 12.5f;

    //Variables for the semi random fleeing behavior.
    private float fleeTimer = 1000f;
    private float changeTime = 1f;

    [Header("Current Movement Speed")]
    [SerializeField] private float movementSpeed;

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        switch (thisEnemy.currentCombatBehavior)
        {
            case AiManager2.CombatBehavior.CloseCombat: //This case SHOULD not be entered as there should be no reason for melee to run away.
                thisEnemy.SetMoveTowardState(); //Here in case it does get entered by a melee enemy.
                break;
            case AiManager2.CombatBehavior.ExplosionCombat: //This case SHOULD not be entered as there should be no reason for explosion enemy to run away.
                thisEnemy.SetMoveTowardState(); //Here in case it does get entered by an explosion enemy.
                break;

            case AiManager2.CombatBehavior.RangedCombat:
                if (thisEnemy.distanceTo(player.transform.position) >= rangedFleeRange)
                {
                    agent.isStopped = true;
                    me.PlayAnimation("Idle");
                    fleeTimer = 1000f;
                    thisEnemy.SetRangedAttackState();
                    return;
                }
                break;

            case AiManager2.CombatBehavior.AoeCombat:
                if (thisEnemy.distanceTo(player.transform.position) >= aoeFleeRange)
                {
                    agent.isStopped = true;
                    me.PlayAnimation("Idle");
                    fleeTimer = 1000f;
                    agent.speed /= 1.2f;
                    thisEnemy.SetAoeAttackState();
                    return;
                }
                break;

            default:
                break;
        }

        //Set movement speed from base, statsystem and debuff.
        movementSpeed = (me.GetMovementSpeed() * enemyStatSystem.GetMovementSpeed() * thisEnemy.me.MovementPenalty()) + 1f;
        agent.speed = movementSpeed;

        DoMoveAway(player.transform, agent, thisEnemy);
    }

    /// <summary>
    /// Will make the enemy turn around at a semi random direction away from the player, and then move there for a second until it finds a new direction.
    /// This will make the enemy seem like it "panics" and runs a wildly.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="agent"></param>
    private void DoMoveAway(Transform target, NavMeshAgent agent, AiManager2 thisEnemy)
    {
        float multiplier = 1.5f;
        Vector3 moveTo = transform.position + (transform.position - target.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)) * multiplier);

        fleeTimer += Time.deltaTime;
        if (fleeTimer >= changeTime || thisEnemy.distanceTo(moveTo) <= 2f)
        {
            agent.destination = moveTo;
            fleeTimer = 0f;
        }
    }
}
