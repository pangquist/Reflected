using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    [TextArea(15, 20)]
    [SerializeField] public string description;
    [SerializeField] public int value = 1;

    public abstract void Apply(GameObject target);
}
