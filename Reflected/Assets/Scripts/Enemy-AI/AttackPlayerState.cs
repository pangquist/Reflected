using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Plan is to have different scrips for each type of attack. But I might just put everything in here for the pre-production. Then fix in production.
public class AttackPlayerState : State
{
    public override void DoState(AiManager thisEnemy, Transform target, NavMeshAgent agent)
    {
        Debug.Log(thisEnemy.CloseCombat());
        //If melee and too far away, move towards target.
        if (thisEnemy.distanceTo(target) >= 5 && thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too close, move away from target.
        else if (thisEnemy.distanceTo(target) <= 7 && !thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }
        //If ranged and too far away, move towards target.
        else if (thisEnemy.distanceTo(target) >= 20 && !thisEnemy.CloseCombat())
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        FaceTarget(target.position);
        agent.destination = thisEnemy.transform.position;
        DoAttack(thisEnemy, target);
    }

    private void DoAttack(AiManager thisEnemy, Transform target)
    {
        if (thisEnemy.AOE())
        {
            //aoeObject = GameObject.Find("AOETestObject");
            GameObject aoeObject = (GameObject)Resources.Load("AOETestObject");
            FireAreaOfEffect(target, aoeObject);
        }
        Debug.Log("Enemy attacked you!");
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.02f);
    }

    private void MeleeAttack()
    {

    }

    private void FireProjectile()
    {

    }

    private void FireAreaOfEffect(Transform target, GameObject aoeObject)
    {
        Debug.Log(target.position);
        Instantiate(aoeObject, new Vector3(target.transform.position.x, target.transform.position.y - 0.499f, target.transform.position.z), target.rotation);

        //Instantiate(aoeObject, new Vector3(0, 0.01f, 0), target.rotation);

        //GameObject newObject = (GameObject)Instantiate(Resources.Load("AOETestObject"), target);

        //let prefab handle collision with player

        Debug.Log("AOE spawned");
    }
}
