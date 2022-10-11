using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager2 : MonoBehaviour
{
    //Initialize states
    private State activeState;
    private StartState startState;
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;
    //private AttackPlayerState attackPlayerState; //Transformed into the 3 states below:
    private MeleeAttackState meleeAttackState;
    private RangedAttackState rangedAttackState;
    private AoeAttackState aoeAttackState;

    //[SerializeField] private GameObject GOplayer; //Redunant
    private Player player;
    private Enemy me;

    private NavMeshAgent agent;
    //private Transform target; //Redundat

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
            me = gameObject.GetComponent<Enemy>();
        }
        if (player == null)
        {
            Debug.Log("Player was not assigned. Entered assign in start function");
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Debug.Log("Assiged player position: " + player.transform.position);
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
        //attackPlayerState = gameObject.AddComponent<AttackPlayerState>(); //Transformed into the 3 states below:
        meleeAttackState = gameObject.GetComponent<MeleeAttackState>();
        rangedAttackState = gameObject.GetComponent<RangedAttackState>();
        aoeAttackState = gameObject.GetComponent<AoeAttackState>();

        //Set active player state
        //activeState = startState;
        activeState = moveTowardsPlayerState; //StartState might be redundat atm.

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();
        //target = GameObject.FindGameObjectWithTag("Player").transform;

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
        Debug.Log(player);
        activeState.DoState(this, player, agent);
    }

    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    //public void SetAttackPlayerState() => activeState = attackPlayerState; //Transformed into the 3 states below:
    public void SetMeleeAttackState() => activeState = meleeAttackState;
    public void SetRangedAttackState() => activeState = rangedAttackState;
    public void SetAoeAttackState() => activeState = aoeAttackState;
    //public void SetStartState() => activeState = startState; //Redundant

    //I plan to have an enum for better stucture for this part.
    public bool CloseCombat() => closeCombat;
    public bool AoeCombat() => aoeCombat;
    public bool RangedCombat() => rangedCombat;

    //Get the current active state
    public State GetActiveState() => activeState;


    public float distanceTo(Transform target)
    {
        //Debug.Log(Vector3.Distance(this.gameObject.transform.position, target.position));
        return Vector3.Distance(transform.position, target.position);
    }
}