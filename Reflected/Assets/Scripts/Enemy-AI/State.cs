 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
   public virtual void DoState(AiManager thisEnemy, /*Player player,*/ Transform goal, NavMeshAgent agent) { }


    // Methods and other which all behavior may need.
    protected float distanceTo(Transform goal)
    {
        return Vector3.Distance(gameObject.transform.position, goal.position);
    }
}
