using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The main manager for the enemy AI. This needs to be coupled with the attack script of the particular enemy to work. Everything else is done in code.
/// </summary>
public class AiManager2 : MonoBehaviour
{
    //Enum for enemy combat behavior.
    public enum CombatBehavior { CloseCombat, RangedCombat, AoeCombat, ExplosionCombat };
    public CombatBehavior currentCombatBehavior;
    [SerializeField] private bool elite;

    //Initialize the currently active state.
    private State activeState;

    //Initialize movement states
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;

    //Initialize attack states
    private MeleeAttackState meleeAttackState;
    private RangedAttackState rangedAttackState;
    private AoeAttackState aoeAttackState;
    private ExplosionAttackState explosionAttackState;

    //Initialize relevant stat/general scripts
    [SerializeField] private Player player;
    [SerializeField] public Enemy me;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyStatSystem enemyStatSystem;

    public Transform firePoint; //Find a better solution (Not gonna happen now, it is too late)

    void Start()
    {
        //If scripts not assigned in inspecter, assign them here.
        if (me == null)
        {
                me = gameObject.GetComponentInChildren<Enemy>();
        }
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        //Set combat behavior depending on tag
        if (gameObject.tag == "Melee") currentCombatBehavior = CombatBehavior.CloseCombat;
        else if (gameObject.tag == "Ranged") currentCombatBehavior = CombatBehavior.RangedCombat;
        else if (gameObject.tag == "AOE") currentCombatBehavior = CombatBehavior.AoeCombat;
        else if (gameObject.tag == "Explosion") currentCombatBehavior = CombatBehavior.ExplosionCombat;

        //Instansiate movement state scripts
        moveTowardsPlayerState = gameObject.AddComponent<MoveTowardsPlayerState>();
        moveAwayFromPlayerState = gameObject.AddComponent<MoveAwayFromPlayerState>();

        //Instansiate attack scripts depending on combat behavior.
        switch (currentCombatBehavior)
        {
            case CombatBehavior.CloseCombat:
                meleeAttackState = gameObject.GetComponent<MeleeAttackState>();
                break;
            case CombatBehavior.RangedCombat:
                rangedAttackState = gameObject.GetComponent<RangedAttackState>();
                break;
            case CombatBehavior.AoeCombat:
                aoeAttackState = gameObject.GetComponent<AoeAttackState>();
                break;
            case CombatBehavior.ExplosionCombat:
                explosionAttackState = gameObject.GetComponent<ExplosionAttackState>();
                break;
        }

        //Set active player state
        activeState = moveTowardsPlayerState;

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();

        //Enemy stat system set up
        enemyStatSystem = GameObject.FindGameObjectWithTag("EnemyStatSystem").GetComponent<EnemyStatSystem>();
    }

    private void Update()
    {
        //Run the currently active state if the enemy is alive. This way the AI will stop when the enemy dies.
        if (!me.Dead() && me.isActive()) 
        {
            activeState.DoState(this, me, player, agent, enemyStatSystem);
        }
    }

    //Setters for behavior states.
    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetMeleeAttackState() => activeState = meleeAttackState;
    public void SetRangedAttackState() => activeState = rangedAttackState;
    public void SetAoeAttackState() => activeState = aoeAttackState;
    public void SetExplosionAttackState() => activeState = explosionAttackState;

    //Get the currently active state
    public State GetActiveState() => activeState;

    //Distance checker that behaviors use to transistion between behaviors.
    public float distanceTo(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    public bool Elite()
    {
        return elite;
    }
}