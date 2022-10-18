using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RangedAttackState : State
{
    private float attackTimer = 0f;
    public float attackRate = 1f;
    private Vector3 offSet = new Vector3(0, 0.5f, 0);

    public GameObject projectileObject;

    public override void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent)
    {
        //If ranged and too close, move away from target.
        if (thisEnemy.distanceTo(player.transform /*target*/) <= 7)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform /*target*/) >= 20)
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
            FireProjectile(thisEnemy, player /*target*/);
            attackTimer = 0f;
        }
    }

    private void FireProjectile(AiManager2 thisEnemy, Player player /*Transform target*/)
    {
        GameObject currentProjectile = Instantiate(projectileObject, gameObject.GetComponent<AIManager>().firePoint.position, Quaternion.identity);
        if (currentProjectile != null)
        {
            currentProjectile.GetComponent<ProjectileScript>().SetUp(player.transform.position /*target.position*/ + offSet, gameObject.GetComponent<AIManager>().firePoint.position, 2f);
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
