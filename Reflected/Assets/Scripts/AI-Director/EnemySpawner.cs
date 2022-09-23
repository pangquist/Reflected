using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;

    //List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
    List<Transform> spawnTransforms = new List<Transform>();
    Transform spawnLocation;

    void Start()
    {
        GetSpawnlocations();
    }

    void Update()
    {

    }

    public void SpawnEnemy(float spawnTime, int enemyAmount)
    {
        StartCoroutine(SpawnWave(spawnTime, enemyAmount));
    }

    private IEnumerator SpawnWave(float spawnTime, int enemyAmount)
    {
        yield return new WaitForSeconds(spawnTime * 2);

        for (int i = 0; i < enemyAmount; i++)
        {
            if (spawnTransforms.Count <= 0) GetSpawnlocations();
            spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
            Instantiate(enemyClose, spawnLocation.position, Quaternion.Euler(0, 0, 0));
            spawnTransforms.Remove(spawnLocation);
        }

        yield return new WaitForSeconds(spawnTime);

        for (int i = 0; i < enemyAmount; i++)
        {
            if (spawnTransforms.Count <= 0) GetSpawnlocations();
            spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
            Instantiate(enemyClose, spawnLocation.position, Quaternion.Euler(0, 0, 0));
            spawnTransforms.Remove(spawnLocation);
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
}
