using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu] //Enables you to create a scriptable object from the asset meny of this type
[Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] public string displayName;
    //public Sprite icon; //Should be added later
}
