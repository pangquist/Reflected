using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : State
{
    private float attackTimer = 0f;
    public float attackRate = 1f;

    public GameObject meleeObject;
    public override void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent)
    {
        if (thisEnemy.distanceTo(player.transform /*target*/) >= 5f)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        attackTimer += Time.deltaTime;

        FaceTarget(player.transform.position /*target.position*/);
        agent.destination = thisEnemy.transform.position;
        if (attackTimer >= attackRate)
        {
            Debug.Log("About to attack");
            DoAttack(thisEnemy);
            attackTimer = 0f;
        }
    }

    private void DoAttack(AiManager2 thisEnemy)
    {
        Instantiate(meleeObject, thisEnemy.firePoint.position, gameObject.transform.rotation);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
}
