using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uppgrade : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float avtiveTime;

    public virtual void Active() { }
}
