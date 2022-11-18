using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AoeAttackState : State
{
    public GameObject aoeObject;

    //Timer for attack rate
    private float attackTimer = 100f;

    //Base values of the attack stats
    [SerializeField] private float baseAttackRate = 4f;
    [SerializeField] private float baseAttackDamage = 1f;
    [SerializeField] private Vector3 baseAoeSize = new Vector3(5f, 3f, 5f);

    //Current values of the attack stats
    private float attackRate;
    private float attackDamage;
    private Vector3 aoeSize;

    //Range to player in which the enemy will flee or chase. (Reposition to attack)
    [SerializeField] private float fleeRange = 7f;
    [SerializeField] private float chaseRange = 20f;

    //Variables for setting up aoe
    private Transform firePoint;
    private Vector3 playerPos;

    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set relevant stats
        attackRate = baseAttackRate / enemyStatSystem.GetAttackSpeed();
        //Attack range?

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
            //Set relevant stats (damage, projectile speed, )
            attackDamage = baseAttackDamage * enemyStatSystem.GetDamageIncrease();
            aoeSize = baseAoeSize * enemyStatSystem.GetAreaOfEffect();

            //Setup parameters for projectile to use
            firePoint = thisEnemy.firePoint;
            playerPos = player.transform.position;

            //DoAttack(thisEnemy, player, enemyStatSystem);

            thisEnemy.SendAnimation("Fire AOE");

            attackTimer = 0f;
        }
    }

    public void DoAttack(/*AiManager2 thisEnemy, Player player, EnemyStatSystem enemyStatSystem*/)
    {
        GameObject currentAOE = Instantiate(aoeObject, playerPos, Quaternion.identity);

        //new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)   ^^^^^ istedet for playerpos


        if (currentAOE != null)
        {
            currentAOE.GetComponent<AOEScript>().SetUp(attackDamage, aoeSize);
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
