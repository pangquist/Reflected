using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionAttackState : State
{
    //Object for explosion hitbox
    public GameObject explosionObject;

    //Bool to check if attack has started
    private bool attackStarted = false;

    [Header("Current ATTACK Stats")]
    [SerializeField] private Vector3 aoeSize;

    [Header("Base POSITIONING Values")]
    [SerializeField] private float chaseRange = 2.5f;

    //Status effects that will be applied on the player when they are hit by the explosion
    [Header("Status Effect Data")]
    [SerializeField] private StatusEffectData dotData;
    [SerializeField] private StatusEffectData slowData;

    //Offset of explosion position
    private Vector3 offSet = new Vector3(0f, 1f, 0f);

    public override void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem)
    {
        if (thisEnemy.distanceTo(player.transform) >= chaseRange && !attackStarted)
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
            //Set aoe size from base and statsystem
            aoeSize = me.GetAoeSize() * enemyStatSystem.GetAreaOfEffect();

            //Play attack animation that will trigger the attack
            me.PlayAnimation("Explosion Attack");

            //Reset attack boolTimer
            attackStarted = true;
        }
    }

    public void DoAttack(Enemy me)
    {
        GameObject currentExplosion = Instantiate(explosionObject, transform.position+offSet, Quaternion.identity);
        if (currentExplosion != null)
        {
            currentExplosion.GetComponent<ExplosionScript>().SetUp(aoeSize, dotData, slowData);
            me.TakeDamage(999f);
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
