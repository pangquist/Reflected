using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should be attached to every savable entity in order to distinguish between them with unique ID for load and save
public class SavableEntity : MonoBehaviour
{
    [SerializeField] private string id;

    public string Id => id;

    [ContextMenu("Generate ID")] //Only for test purposes

    private void GenerateId()
    {
        id = Guid.NewGuid().ToString();
    }

    //Find all savable components on gameobject
    public object SaveState()
    {
        var state = new Dictionary<string, object>();
        foreach(var savable in GetComponents<ISavable>())
        {
            state[savable.GetType().ToString()] = savable.SaveState();
        }
        return state;
    }

    public void LoadState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;
        foreach(var savable in GetComponents<ISavable>())
        {
            string typeName = savable.GetType().ToString();
            if(stateDictionary.TryGetValue(typeName, out object saveState))
            {
                savable.LoadState(saveState);
            }
        }
    }
}
