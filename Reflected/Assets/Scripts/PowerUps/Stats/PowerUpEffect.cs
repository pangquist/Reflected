using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    [TextArea(15, 20)]
    [SerializeField] public string description;
    [SerializeField] public int value;
    [SerializeField] public float amount;   
    [SerializeField] public string type;
    [SerializeField] public string powerupName;
    [SerializeField] public string diminishingReturn;

    public abstract void Apply(GameObject target, float amount);
}
