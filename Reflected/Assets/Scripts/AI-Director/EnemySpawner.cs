using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;

    List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
    [SerializeField] Transform debugSpawnLocation;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnEnemy(enemyClose, debugSpawnLocation);
        }
    }

    private void getSpawnlocations()
    {

    }

    private void SpawnEnemy(GameObject enemy, Transform spawnLocation)
    {
        Instantiate(enemy, spawnLocation.position, Quaternion.Euler(0, 0, 0));
    }
}
