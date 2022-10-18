using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//This class is currently depricated. It was the old code used for pre-production. Use the 'AiManager2' script for the current script.

public class AIManager : MonoBehaviour
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
    private Player player; //This is not assigned correctly. Ignore for now and just use goal for the agent.
    private Enemy me;
    //private PlaceholderEnemyScript me;

    private NavMeshAgent agent;
    private Transform target; //Instead of using the player at the moment.

    bool closeCombat = false;
    bool areaOfEffect = false;

    public Transform firePoint;

    void Start()
    {
        //Assign components
        //if (!me) me = GetComponent<Enemy>();
        //if (!player) player = GOplayer.GetComponent<Player>();
        //if (!GOplayer) GOplayer = GameObject.FindGameObjectWithTag("Player");

        if (!me)
        {
            me = GetComponent<Enemy>();
            //me = GetComponent<PlaceholderEnemyScript>();
            //Debug.Log("Assignmed Enemy component");
        }
        if (!GOplayer)
        {
            GOplayer = GameObject.FindGameObjectWithTag("Player");
            //Debug.Log("Assigned GOplayer from tag");
        }
        if (!player)
        {
            player = GOplayer.GetComponent<Player>();
            //Debug.Log("Assigned Player Component from GOplayer");
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
        attackPlayerState = gameObject.AddComponent<AttackPlayerState>();
        //Need: Melee attack & Ranged attack
        //Need: Touch attack

        //Set active player state
        //activeState = startState;
        activeState = attackPlayerState; //For specific states

        //AI Navmesh setup
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //I plan to have an enum for better stucture for this part.
        if (gameObject.tag == "Melee") closeCombat = true;
        else if (gameObject.tag == "AOE") areaOfEffect = true;


    }
    private void Update()
    {
        //agent.destination = player.gameObject.transform.position;   //Something is wrong with the player object.
        //agent.destination = goal.position;

        //activeState.DoState(this, player);
        //Debug.Log("FirePoint POS: " + firePoint.position);
        //activeState.DoState(this, target, agent);
    }

    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetAttackPlayerState() => activeState = attackPlayerState;
    public void SetStartState() => activeState = startState;

    //I plan to have an enum for better stucture for this part.
    public bool CloseCombat() => closeCombat;
    public bool AOE() => areaOfEffect;
    public State GetActiveState() => activeState;


    public float distanceTo(Transform target)
    {
        //TEMPORARY MUSIC FIX ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        if(Vector3.Distance(this.gameObject.transform.position, target.position) < 15)
        {
            player.AddEnemy(me);
        }
        else if(Vector3.Distance(this.gameObject.transform.position, target.position) > 25 && player.GetEnemies().Contains(me))
        {
            //player.RemoveEnemy(me);
        }
        //TEMPORARY MUSIC FIX ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //Debug.Log(Vector3.Distance(this.gameObject.transform.position, target.position));
        return Vector3.Distance(this.gameObject.transform.position, target.position);
    }
}
