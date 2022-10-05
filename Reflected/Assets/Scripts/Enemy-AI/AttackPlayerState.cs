using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Plan is to have different scrips for each type of attack. But I might just put everything in here for the pre-production. Then fix in production.
//Also, make each attack script already attached to the prefab to avoid having to use the resouce folder to spawn at runtime. Then you can just use prefabs for projectiles etc.
public class AttackPlayerState : State
{
    private float attackTimer = 0f;
    public float attackRate = 1f;
    public override void DoState(AIManager thisEnemy, Transform target, NavMeshAgent agent)
    {
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

        attackTimer += Time.deltaTime;

        FaceTarget(target.position);
        agent.destination = thisEnemy.transform.position;
        if(attackTimer >= attackRate)
        {
            DoAttack(thisEnemy, target);
            //Debug.Log("Enemy attacked you!");
            attackTimer = 0f;
        }
    }

    private void DoAttack(AIManager thisEnemy, Transform target)
    {
        if (!thisEnemy.CloseCombat() && !thisEnemy.AOE())
        {
            GameObject projectileObject = (GameObject)Resources.Load("ProjectileTestObject");
            FireProjectile(target, projectileObject);
        }
        else if (thisEnemy.AOE())
        {
            attackRate = 5f;
            //aoeObject = GameObject.Find("AOETestObject");
            GameObject aoeObject = (GameObject)Resources.Load("AOETestObject");
            FireAreaOfEffect(target, aoeObject);
        }
        else if (thisEnemy.CloseCombat())
        {
            GameObject meleeObject = (GameObject)Resources.Load("MeleeHitbox");
            MeleeAttack(meleeObject);
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void MeleeAttack(GameObject meleeObject)
    {
        Instantiate(meleeObject, gameObject.GetComponent<AIManager>().firePoint.position, Quaternion.identity);
    }

    private void FireProjectile(Transform target, GameObject projectileObject)
    {
        GameObject currentProjectile = Instantiate(projectileObject, gameObject.GetComponent<AIManager>().firePoint.position, Quaternion.identity);
        currentProjectile.GetComponent<ProjectileScript>().SetUp(target.position, gameObject.GetComponent<AIManager>().firePoint.position, 2f);
        //Debug.Log("FirePoint POS: " + gameObject.GetComponent<AIManager>().firePoint.position);
    }

    private void FireAreaOfEffect(Transform target, GameObject aoeObject)
    {
        //Debug.Log(target.position);
        Instantiate(aoeObject, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), gameObject.transform.rotation);

        //Instantiate(aoeObject, new Vector3(0, 0.01f, 0), target.rotation);

        //GameObject newObject = (GameObject)Instantiate(Resources.Load("AOETestObject"), target);

        //let prefab handle collision with player

        Debug.Log("AOE spawned");
    }
}
