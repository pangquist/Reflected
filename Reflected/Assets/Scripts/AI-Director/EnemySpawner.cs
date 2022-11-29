using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;
    [SerializeField] GameObject enemyAOE;
    [SerializeField] GameObject enemyDOT;
    GameObject enemyToSpawn;

    EnemyStatSystem enemyStatSystem;

    List<Transform> spawnTransforms = new List<Transform>();
    Transform spawnLocation;

    [SerializeField] int spawnBias = 60;

    void Start()
    {
        GetSpawnlocations();
        enemyStatSystem = GameObject.Find("EnemyStatSystem").GetComponent<EnemyStatSystem>();
    }

    public void SpawnEnemy(float spawnTime, int enemyAmount, int waveAmount, float enemyadaptiveDifficulty)
    {
        //enemyStatSystem.SetNewStats(1, 1, true); //might change values in future for more fun gameplay. Needs to be balanced with enemyAdaptiveDiffuculty.
        //enemyStatSystem.ApplyNewStats(DimensionManager.True);
        StartCoroutine(SpawnWave(spawnTime, enemyAmount, waveAmount, enemyadaptiveDifficulty));
    }

    private IEnumerator SpawnWave(float spawnTime, int enemyAmount, int waveAmount, float enemyAdaptiveDifficulty)
    {
        GetSpawnlocations();

        for (int i = 0; i < waveAmount; i++)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnFunction(enemyAmount, enemyAdaptiveDifficulty);
        }
    }

    private void SpawnFunction(int amount, float adaptiveDifficulty)
    {
        try
        {
            for (int i = 0; i < amount; i++)
            {
                GetBiasedEnemy();
                if (spawnTransforms.Count <= 0) GetSpawnlocations();
                spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
                Enemy enemy = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<Enemy>();
                enemy.AdaptiveDifficulty(adaptiveDifficulty);
                spawnTransforms.Remove(spawnLocation);
            }
        }
        catch
        {
            if (spawnTransforms.Count == 0)
                Debug.Log("No spawn locations");
        }
    }

    private void GetSpawnlocations()
    {
        List<GameObject> spawns = new List<GameObject>();
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

        spawnTransforms.Clear();

        foreach (GameObject spawnPoint in spawns)
        {
            if (spawnPoint.activeInHierarchy) spawnTransforms.Add(spawnPoint.transform);
        }
    }

    private void GetBiasedEnemy()
    {
        int percentage = Random.Range(1, 101);

        if (percentage <= spawnBias)
        {
            enemyToSpawn = enemyClose;
        }
        else
        {
            int percent = Random.Range(1, 100);

            if (percent < 33)
            {
                enemyToSpawn = enemyRange;
            }
            else if (percent > 66)
            {
                enemyToSpawn = enemyAOE;
            }
            else
            {
                enemyToSpawn = enemyDOT;
            }
        }
    }
}
