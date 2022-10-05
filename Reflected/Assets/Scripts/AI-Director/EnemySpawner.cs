using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;

    GameObject enemyToSpawn;
    List<GameObject> enemyList = new List<GameObject>();

    List<Transform> spawnTransforms = new List<Transform>();
    Transform spawnLocation;

    bool meleePlayer;

    int bias = 70;

    void Start()
    {
        GetSpawnlocations();
        enemyList.Add(enemyClose);
        enemyList.Add(enemyRange);
    }

    void Update()
    {

    }

    public void SpawnEnemy(float spawnTime, int enemyAmount, float enemyadaptiveDifficulty)
    {
        StartCoroutine(SpawnWave(spawnTime, enemyAmount, enemyadaptiveDifficulty));
    }

    private IEnumerator SpawnWave(float spawnTime, int enemyAmount, float enemyAdaptiveDifficulty)
    {
            yield return new WaitForSeconds(spawnTime);

            GetSpawnlocations();

            for (int i = 0; i < enemyAmount; i++)
            {
                GetRandomEnemy();
                if (spawnTransforms.Count <= 0) GetSpawnlocations();
                spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
                Enemy enemy = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<Enemy>();
                enemy.AdaptiveDifficulty(enemyAdaptiveDifficulty);
                spawnTransforms.Remove(spawnLocation);
            }

            yield return new WaitForSeconds(spawnTime);

            for (int i = 0; i < enemyAmount; i++)
            {
                GetBiasedEnemy();
                if (spawnTransforms.Count <= 0) GetSpawnlocations();
                spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
                Enemy enemy = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<Enemy>();
                enemy.AdaptiveDifficulty(enemyAdaptiveDifficulty);
                spawnTransforms.Remove(spawnLocation);
            }
    }

    private void GetSpawnlocations()
    {
        List<GameObject> spawns = new List<GameObject>();
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        spawnTransforms.Clear();

        foreach (GameObject spawnPoint in spawns)
        {
            if(spawnPoint.activeInHierarchy) spawnTransforms.Add(spawnPoint.transform);
        }
    }

    private void GetRandomEnemy()
    {
        int i = Random.Range(0, enemyList.Count);

        enemyToSpawn = enemyList[i];
    }

    private void GetBiasedEnemy()
    {
        int percentage = Random.Range(1, 101);

        if (meleePlayer)
        {
            if (percentage <= bias)
            {
                enemyToSpawn = enemyRange;
            }
            else
            {
                enemyToSpawn = enemyClose;
            }
        }
        else if (!meleePlayer)
        {
            if (percentage <= bias)
            {
                enemyToSpawn = enemyClose;
            }
            else
            {
                enemyToSpawn = enemyRange;
            }
        }
    }

    public void SetMeleePlayer()
    {
        meleePlayer = true;
    }
}
