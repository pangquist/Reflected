using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AoeAttackState : State
{
    public GameObject aoeObject;

    //Timer for attack rate
    private float attackTimer = 0f;

    //Base values of the attack stats
    [SerializeField] private float attackRate = 5f;
    [SerializeField] private int attackDamage = 1;

    //Range to player in which the enemy will flee or chase. (Reposition to attack)
    [SerializeField] private float fleeRange = 7f;
    [SerializeField] private float chaseRange = 20f;

    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent)
    {
        //Set attack rate
        
        //If ranged and too close, move away from target.
        if (thisEnemy.distanceTo(player.transform) <= fleeRange)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform) >= chaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        //Make enemy face player and stand still
        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            DoAttack(thisEnemy, player);
            attackTimer = 0f;
        }
    }

    private void DoAttack(AiManager2 thisEnemy, Player player)
    {
        //Set damage
        GameObject currentAOE = Instantiate(aoeObject, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.identity);
        if (currentAOE != null)
        {
            currentAOE.GetComponent<AOEScript>().SetUp(attackDamage);
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
