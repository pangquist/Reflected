 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
   public virtual void DoState(AIManager thisEnemy, Transform target, NavMeshAgent agent) { }


    // Methods and other which all behavior may need.
    //protected float distanceTo(Transform target)
    //{
    //    return Vector3.Distance(gameObject.transform.position, target.position);
    //}
}
