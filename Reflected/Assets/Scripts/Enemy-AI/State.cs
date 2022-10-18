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
    public virtual void DoState(AiManager2 thisEnemy, Player player /*Transform target*/, NavMeshAgent agent) { }

    // Methods and other which all behavior may need.
    //protected float distanceTo(Transform target)
    //{
    //    return Vector3.Distance(gameObject.transform.position, target.position);
    //}
}
