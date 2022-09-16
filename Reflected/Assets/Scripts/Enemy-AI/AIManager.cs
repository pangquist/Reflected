using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    private State activeState;
    private StartState startState = new StartState();
    private MoveTowardsPlayerState moveTowardsPlayerState = new MoveTowardsPlayerState();
    private MoveAwayFromPlayerState moveAwayFromPlayerState = new MoveAwayFromPlayerState();
    private AttackPlayerState attackPlayerState = new AttackPlayerState();

    [SerializeField] private GameObject GOplayer;
    private Player player;
    private Enemy me;

    bool closeCombat = false;

    void Start()
    {
        if (!me) me = GetComponent<Enemy>();
        if (!player) player = GOplayer.GetComponent<Player>();
        if(!GOplayer) GOplayer = GameObject.FindGameObjectWithTag("Player");

        activeState = startState;

        if (gameObject.tag == "Melee") closeCombat = true;
    }
    private void Update()
    {
        activeState.DoState(this, player);
    }

    public void SetMoveTowardState() => activeState = moveTowardsPlayerState;
    public void SetMoveAwayState() => activeState = moveAwayFromPlayerState;
    public void SetAttackPlayerState() => activeState = attackPlayerState;
    public void SetStartState() => activeState = startState;

    public bool CloseCombat() => closeCombat;

    public State GetActiveState() => activeState;


    public float distanceTo(Player player)
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position);
    }
}
