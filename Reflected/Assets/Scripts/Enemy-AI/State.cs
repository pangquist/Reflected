 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
   public virtual void DoState(AIManager thisEnemy, Player player) { }

   protected float distanceTo(Player player)
   {
         return Vector3.Distance(gameObject.transform.position, player.transform.position);
   }
}
