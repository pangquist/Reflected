using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager2 : MonoBehaviour
{
    public enum CombatBehavior { CloseCombat, RangedCombat, AoeCombat, };
    public CombatBehavior currentCombatBehavior;

    //Initialize movement states
    private State activeState;
    private StartState startState; //Redundant
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;

    //Initialize attack states (use switch to only initialize needed scripts)'

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

        //Set combat behavior depending on tag
        if (gameObject.tag == "Melee") currentCombatBehavior = CombatBehavior.CloseCombat;
        else if (gameObject.tag == "Ranged") currentCombatBehavior = CombatBehavior.RangedCombat;
        else if (gameObject.tag == "AOE") currentCombatBehavior = CombatBehavior.AoeCombat;

        //Instansiate movement state scripts
        startState = gameObject.AddComponent<StartState>(); //Redundant
        moveTowardsPlayerState = gameObject.AddComponent<MoveTowardsPlayerState>();
        moveAwayFromPlayerState = gameObject.AddComponent<MoveAwayFromPlayerState>();

        //Instansiate attack stripts depending on combat behavior.
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

        //Old. Here if nessecary. Remove when fully converted to switch cases.
        if (gameObject.tag == "Melee") closeCombat = true;
        else if (gameObject.tag == "AOE") aoeCombat = true;
        else if (gameObject.tag == "Ranged") rangedCombat = true;

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

    //I plan to have an enum for better stucture for this part. Remove when fully converted to switch cases.
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