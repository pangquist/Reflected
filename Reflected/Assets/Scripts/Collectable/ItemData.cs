using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu] //Enables you to create a scriptable object from the asset meny of this type
[Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] public string displayName;
    [SerializeField] public int amount;
    [TextArea(15, 20)]
    [SerializeField] public string description;
    //public Sprite icon; //Should be added later
}
