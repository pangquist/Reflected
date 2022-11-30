using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    public string SavePath => $"{Application.persistentDataPath}/save.txt";

    private void Awake()
    {
        DontDestroyOnLoad(this);

        SaveLoadSystem[] array = FindObjectsOfType<SaveLoadSystem>();
        if (array.Length > 1)
            Destroy(gameObject);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Debug.Log(Application.persistentDataPath);
        var state = LoadFile();
        SaveState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        LoadState(state);
    }

    public void SaveFile(object state) //Create the file
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found");
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream); 
            //Pulls all the values from the file and make a dictionary out of them to be used in the game
        }
    }

    //Find all gameobjects with an SavableEntiry component and tells them to save
    void SaveState(Dictionary<string, object> state)
    {
        foreach(var savable in FindObjectsOfType<SavableEntity>())
        {
            state[savable.Id] = savable.SaveState();
        }
    }

    //Find all gameobjects with an SavableEntiry component and tells them to load
    void LoadState(Dictionary<string, object> state)
    {
        foreach(var savable in FindObjectsOfType<SavableEntity>())
        {
            if(state.TryGetValue(savable.Id, out object saveState))
            {
                savable.LoadState(saveState);
            }
        }
    }
}
