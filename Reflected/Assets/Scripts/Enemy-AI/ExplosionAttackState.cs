using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionAttackState : State
{
    public GameObject explosionObject;
    public Enemy myStats;

    private bool attackStarted = false;

    //Base values of the attack stats
    [SerializeField] private Vector3 baseAoeSize = new Vector3(6f, 4f, 6f);

    //Current values of the attack stats
    private Vector3 aoeSize;

    //Range to player in which the enemy will chase the player. (Reposition to attack)
    [SerializeField] private float chaseRange = 2.5f;

    //Status effects that will be applied on the player when they are hit by the explosion
    [SerializeField] private StatusEffectData dotData;
    [SerializeField] private StatusEffectData slowData;

    public override void DoState(AiManager2 thisEnemy, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        //Set relevant stats

        if (thisEnemy.distanceTo(player.transform) >= chaseRange)
        {
            thisEnemy.SetMoveTowardState();
            agent.isStopped = false;
            return;
        }

        //Make enemy face player and stand still
        FaceTarget(player.transform.position);
        agent.destination = thisEnemy.transform.position;

        if (!attackStarted)
        {
            DoAttack(thisEnemy, enemyStatSystem);
        }
    }

    private void DoAttack(AiManager2 thisEnemy, EnemyStatSystem enemyStatSystem)
    {
        attackStarted = true;

        //Set relevant stats (damage, projectile speed, )
        aoeSize = baseAoeSize * enemyStatSystem.GetAreaOfEffect();

        GameObject currentExplosion = Instantiate(explosionObject, transform.position, Quaternion.identity);
        if (currentExplosion != null)
        {
            currentExplosion.GetComponent<ExplosionScript>().SetUp(aoeSize, dotData, slowData);
            myStats.TakeDamage(999f);
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
