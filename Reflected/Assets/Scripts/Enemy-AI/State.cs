 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
   public virtual void DoState(AIManager thisEnemy, Player player) { }

   protected float distanceTo(Player player)
   {
        return gameObject.transform.position - player.transform.position;
   }
}
