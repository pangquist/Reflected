using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{
    // ---- Illegal (Cannot use new for Monobehavior scripts) ----
    //private StartState startState = new StartState();
    //private MoveTowardsPlayerState moveTowardsPlayerState = new MoveTowardsPlayerState();
    //private MoveAwayFromPlayerState moveAwayFromPlayerState = new MoveAwayFromPlayerState();
    //private AttackPlayerState attackPlayerState = new AttackPlayerState();

    //Initialize states
    private State activeState;
    private StartState startState;
    private MoveTowardsPlayerState moveTowardsPlayerState;
    private MoveAwayFromPlayerState moveAwayFromPlayerState;
    private AttackPlayerState attackPlayerState;

    [SerializeField] private GameObject GOplayer;
    private Player player; //This is not assigned correctly
    private Enemy me;

    private NavMeshAgent agent;
    private Transform goal; //Instead of using the player at the moment.

    bool closeCombat = false;

    void Start()
    {
        //Assign components
        //if (!me) me = GetComponent<Enemy>();
        //if (!player) player = GOplayer.GetComponent<Player>();
        //if (!GOplayer) GOplayer = GameObject.FindGameObjectWithTag("Player");

        if (!me)
        {
            me = GetComponent<Enemy>();
            Debug.Log("Assignmed Enemy component");
        }
        if (!GOplayer)
        {
            GOplayer = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Assigned GOplayer from tag");
        }
        if (!player)
        {
            player = GOplayer.GetComponent<Player>();
            Debug.Log("Assigned Player Component from GOplayer");
        }


        //Instansiate state scripts
        startState = gameObject.AddComponent<StartState>();
        moveTowardsPlayerState = gameObject.AddComponent<MoveTowardsPlayerState>();
        moveAwayFromPlayerState = gameObject.AddComponent<MoveAwayFromPlayerState>();
        attackPlayerState = gameObject.AddComponent<AttackPlayerState>();

        //Set active player state
        //activeState = startState;
        activeState = moveTowardsPlayerState;

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();
        goal = GameObject.FindGameObjectWithTag("Player").transform;

        if (gameObject.tag == "Melee") closeCombat = true;
    }
    private void Update()
    {
        //agent.destination = player.gameObject.transform.position;   //Something is wrong with the player object.
        //agent.destination = goal.position;

        //activeState.DoState(this, player);
        activeState.DoState(this, goal, agent);
    }

    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetAttackPlayerState() => activeState = attackPlayerState;
    public void SetStartState() => activeState = startState;

    public bool CloseCombat() => closeCombat;

    public State GetActiveState() => activeState;


    public float distanceTo(Transform goal)
    {
        Debug.Log(Vector3.Distance(this.gameObject.transform.position, goal.position));
        return Vector3.Distance(this.gameObject.transform.position, goal.position);
    }
}
