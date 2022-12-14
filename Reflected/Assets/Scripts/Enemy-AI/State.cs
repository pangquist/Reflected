 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    /// <summary>
    /// Parent class of state behaviors
    /// </summary>
    /// <param name="thisEnemy"></param>
    /// <param name="player"></param>
    /// <param name="agent"></param>
    public virtual void DoState(AiManager2 thisEnemy, Enemy me, Player player, NavMeshAgent agent, EnemyStatSystem enemyStatSystem) { }
}
