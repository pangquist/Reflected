using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager2 : MonoBehaviour
{
    //Initialize movement states
    private State activeState;
    private StartState startState; //Redundant
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;

    //Initialize attack states (use switch to only initialize needed scripts)
    private MeleeAttackState meleeAttackState;
    private RangedAttackState rangedAttackState;
    private AoeAttackState aoeAttackState;

    [SerializeField] private Player player;
    [SerializeField] private Enemy me;
    [SerializeField] private NavMeshAgent agent;

    bool closeCombat = false;
    bool rangedCombat = false;
    bool aoeCombat = false;

    public Transform firePoint; //Find a better solution

    public enum CombatBehavior { CloseCombat, RangedCombat, AoeCombat, };
    public CombatBehavior currentCombatBehavior;

    void Start()
    {
        if (me == null)
        {
            me = GetComponentInChildren<Enemy>();
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        //if (firePoint == null)
        //{
        //    //Debug. Will be better when rewritten into different scripts.
        //    firePoint = gameObject.transform.GetChild(0).GetChild(0).transform;
        //}

        //Instansiate state scripts
        startState = gameObject.AddComponent<StartState>();
        moveTowardsPlayerState = gameObject.AddComponent<MoveTowardsPlayerState>();
        moveAwayFromPlayerState = gameObject.AddComponent<MoveAwayFromPlayerState>();

        //Make an if depending on type of enemy so it does not try to get all components
        meleeAttackState = gameObject.GetComponent<MeleeAttackState>();
        rangedAttackState = gameObject.GetComponent<RangedAttackState>();
        aoeAttackState = gameObject.GetComponent<AoeAttackState>();

        //Set active player state
        activeState = moveTowardsPlayerState;

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();

        //Old. Here if nessecary, but I want to use enum instead.
        if (gameObject.tag == "Melee") closeCombat = true;
        else if (gameObject.tag == "AOE") aoeCombat = true;
        else if (gameObject.tag == "Ranged") rangedCombat = true;

        if (gameObject.tag == "Melee") currentCombatBehavior = CombatBehavior.CloseCombat;
        else if (gameObject.tag == "Ranged") currentCombatBehavior = CombatBehavior.RangedCombat;
        else if (gameObject.tag == "AOE") currentCombatBehavior = CombatBehavior.AoeCombat;

    }
    private void Update()
    {
        activeState.DoState(this, player, agent);
    }

    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetMeleeAttackState() => activeState = meleeAttackState;
    public void SetRangedAttackState() => activeState = rangedAttackState;
    public void SetAoeAttackState() => activeState = aoeAttackState;

    //I plan to have an enum for better stucture for this part.
    public bool CloseCombat() => closeCombat;
    public bool AoeCombat() => aoeCombat;
    public bool RangedCombat() => rangedCombat;

    //Get the current active state
    public State GetActiveState() => activeState;


    public float distanceTo(Transform target)
    {
        return Vector3.Distance(transform.position, target.position);
    }
}