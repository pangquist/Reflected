using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public new string name;
    public float DOTAmount;
    public float TickSpeed;
    public float MovementPenalty;
    public float LifeTime;

    //public GameObject EffectParticles;
}
