using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RangedAttackState : State
{
    //Object for projectile prefab.
    public GameObject projectileObject;

    //Offset so the enemy does not shoot at the players feet.
    private Vector3 offSet = new Vector3(0, 0.5f, 0);

    //Timer for attack rate
    private float attackTimer = 100f;

    private float baseAttackRate = 1;

    [Header("Current ATTACK Stats")]
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDamage;
    [SerializeField] private float projectileForce; //Projectile speed

    [Header("Base POSITIONING Values")]
    [SerializeField] private float baseFleeRange = 7f;
    [SerializeField] private float baseChaseRange = 20f;

    //Variables for setting up projectile
    private Transform firePoint;
    private Vector3 playerPos;

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set attack rate, by using default, base, statsystem change as well as debuff.
        attackRate = baseAttackRate / me.GetAttackSpeed() / enemyStatSystem.GetAttackSpeed() / me.MovementPenalty();

        //If too close, move away from target.
        if (thisEnemy.distanceTo(player.transform) <= baseFleeRange)
        {
            thisEnemy.SetMoveAwayState();
            agent.isStopped = false;
            me.PlayAnimation("Walk Forward In Place");
            return;
        }

        //If too far away, move towards target.
        else if (thisEnemy.distanceTo(player.transform) >= baseChaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            me.PlayAnimation("Walk Forward In Place");
            return;
        }

        //Make enemy face player and stand still
        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            //Set attack damage from base and statsystem
            attackDamage = me.GetDamage() * enemyStatSystem.GetDamageIncrease();

            //Set projectile speed from base
            projectileForce = me.GetProjectileSpeed(); // * enemyStatSystem if we add that

            //Setup parameters for projectile to use
            firePoint = thisEnemy.firePoint;
            playerPos = player.transform.position;

            //Play attack animation that will trigger the attack
            me.PlayAnimation("Projectile Attack");

            //Reset attack boolTimer
            attackTimer = 0f;
        }
    }

    public void DoAttack()
    {
        //Instantiate the projectile and set up its variables.
        GameObject currentProjectile = Instantiate(projectileObject, firePoint.position, Quaternion.identity);
        if (currentProjectile != null)
        {
            currentProjectile.GetComponent<ProjectileScript>().SetUp(playerPos + offSet, firePoint.position, projectileForce, attackDamage);
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
