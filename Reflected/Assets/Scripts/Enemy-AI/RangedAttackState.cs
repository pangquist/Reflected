using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RangedAttackState : State
{
    public GameObject projectileObject;

    //Offset so the enemy does not shoot at the players feet.
    private Vector3 offSet = new Vector3(0, 0.5f, 0);

    //Timer for attack rate
    private float attackTimer = 0f;

    //Base values of the attack stats
    [SerializeField] private float baseAttackRate = 1f;
    [SerializeField] private float baseAttackDamage = 3;
    [SerializeField] private float baseProjectileForce = 2f; //Projectile speed

    //Current value of the attack stats
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDamage;
    [SerializeField] private float projectileForce; //Projectile speed

    //Range to player in which the enemy will flee or chase. (Reposition to attack)
    [SerializeField] private float baseFleeRange = 7f;
    [SerializeField] private float baseChaseRange = 20f;

    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set relevant stats
        attackRate = baseAttackRate * enemyStatSystem.GetAttackSpeed() / thisEnemy.me.MovementPenalty();
        //Attack range?


        //If too close, move away from target.
        if (thisEnemy.distanceTo(player.transform) <= baseFleeRange)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            return;
        }

        //If too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform) >= baseChaseRange)
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
            FireProjectile(thisEnemy, player, enemyStatSystem);
            attackTimer = 0f;
        }
    }

    private void FireProjectile(AiManager2 thisEnemy, Player player, EnemyStatSystem enemyStatSystem)
    {
        //Set relevant stats (damage, projectile speed, )
        attackDamage = baseAttackDamage * enemyStatSystem.GetDamageIncrease();
        projectileForce = baseProjectileForce;
        
        //Instantiate the projectile and set up it variables.
        GameObject currentProjectile = Instantiate(projectileObject, thisEnemy.firePoint.position, Quaternion.identity);
        if (currentProjectile != null)
        {
            currentProjectile.GetComponent<ProjectileScript>().SetUp(player.transform.position + offSet, thisEnemy.firePoint.position, projectileForce, attackDamage);
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
