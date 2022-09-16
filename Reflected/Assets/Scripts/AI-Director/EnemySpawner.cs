using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;

    List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
    List<Transform> spawnTransforms = new List<Transform>();
    [SerializeField] Transform debugSpawnLocation;
    [SerializeField] int numberOfSpawnpoints;

    void Start()
    {
        GetSpawnlocations();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (spawnTransforms.Count > 0)
            {
                debugSpawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
            }
            else
            {
                GetSpawnlocations();
                debugSpawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
            }

            SpawnEnemy(enemyClose, debugSpawnLocation);

            spawnTransforms.Remove(debugSpawnLocation);

            //Spawn location will randomly choose a location and remove it from the list, if there are no more locations in the list all locations will be added again.
        }
    }

    private void GetSpawnlocations()
    {
        List<GameObject> spawns = new List<GameObject>();
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        foreach (GameObject point in spawns)
        {
            spawnTransforms.Add(point.transform);
        }
    }

    private void SpawnEnemy(GameObject enemy, Transform spawnLocation)
    {
        Instantiate(enemy, spawnLocation.position, Quaternion.Euler(0,0,0));
    }
}
