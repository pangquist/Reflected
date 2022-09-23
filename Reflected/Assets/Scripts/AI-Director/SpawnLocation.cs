using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    GameObject spawnPoint;
    Transform spawnPosition;

    void Start()
    {
        spawnPosition = spawnPoint.transform;
    }

    private void GetSpawnLocation()
    {
        
    }
}
