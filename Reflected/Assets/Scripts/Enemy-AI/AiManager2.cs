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
    public enum CombatBehavior { CloseCombat, RangedCombat, AoeCombat, };
    public CombatBehavior currentCombatBehavior;

    //Initialize the currently active state.
    private State activeState;

    //Initialize movement states
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;

    //Initialize attack states
    private MeleeAttackState meleeAttackState;
    private RangedAttackState rangedAttackState;
    private AoeAttackState aoeAttackState;

    //Initialize relevant stat/general scripts
    [SerializeField] private Player player;
    [SerializeField] private Enemy me;
    [SerializeField] private NavMeshAgent agent;

    public Transform firePoint; //Find a better solution

    void Start()
    {
        //If scripts not assigned in inspecter, assign them here.
        if (me == null)
        {
            me = GetComponentInChildren<Enemy>();
        }
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        //Should have a way to assign firePoint without the inspector. But I don't have one atm.
        //if (firePoint == null)
        //{
        //    //Debug. Will be better when rewritten into different scripts.
        //    firePoint = gameObject.transform.GetChild(0).GetChild(0).transform;
        //}

        //Set combat behavior depending on tag
        if (gameObject.tag == "Melee") currentCombatBehavior = CombatBehavior.CloseCombat;
        else if (gameObject.tag == "Ranged") currentCombatBehavior = CombatBehavior.RangedCombat;
        else if (gameObject.tag == "AOE") currentCombatBehavior = CombatBehavior.AoeCombat;

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
        }

        //Set active player state
        activeState = moveTowardsPlayerState;

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Run the currently active state
        activeState.DoState(this, player, agent);
    }

    //Setters for behavior states.
    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetMeleeAttackState() => activeState = meleeAttackState;
    public void SetRangedAttackState() => activeState = rangedAttackState;
    public void SetAoeAttackState() => activeState = aoeAttackState;

    //Get the currently active state
    public State GetActiveState() => activeState;

    //Distance checker that behaviors use to transistion between behaviors.
    public float distanceTo(Transform target)
    {
        return Vector3.Distance(transform.position, target.position);
    }
}