 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
   public virtual void DoState(AiManager thisEnemy, Player player) { }
}
