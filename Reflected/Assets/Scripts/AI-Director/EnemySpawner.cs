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
        List<GameObject> spawns = new List<GameObject>();
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();
        
        foreach(GameObject point in spawns)
        {
            spawnTransforms.Add(point.transform);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            debugSpawnLocation = spawnTransforms[Random.Range(0,spawnTransforms.Count)];
            SpawnEnemy(enemyClose, debugSpawnLocation);

            //spawnTransforms.Remove(debugSpawnLocation);
            //in the future we might remove the spawnlocation from the list so that enemies don't spawn inside eachother.
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
