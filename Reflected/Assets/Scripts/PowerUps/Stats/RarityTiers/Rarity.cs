using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rarity")]
public class Rarity : ScriptableObject
{
    [SerializeField] public string rarity;
    [SerializeField] public int valueMultiplier;
    [SerializeField] public float amountMultiplier;
}
