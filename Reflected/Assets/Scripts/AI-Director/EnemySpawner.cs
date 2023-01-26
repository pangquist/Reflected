using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject tutorialDummy;
    //[SerializeField] GameObject enemyClose;
    //[SerializeField] GameObject enemyRange;
    //[SerializeField] GameObject enemyAOE;
    //[SerializeField] GameObject enemyDOT;
    //[SerializeField] GameObject enemyElite;
    string enemyToSpawn;

    EnemyStatSystem enemyStatSystem;
    ObjectPool objectPool;

    bool spawnElite;

    List<Transform> spawnTransforms = new List<Transform>();
    Transform spawnLocation;

    [SerializeField] int spawnBias = 60;

    void Start()
    {
        //GenerateSpawnLocation();
        enemyStatSystem = GameObject.Find("EnemyStatSystem").GetComponent<EnemyStatSystem>();
        objectPool = GetComponent<ObjectPool>();
        spawnElite = false;
    }

    public void SpawnEnemy(float spawnTime, int enemyAmount, int waveAmount, float enemyadaptiveDifficulty)
    {
        enemyStatSystem.IncreaseHealthBuff();
        StartCoroutine(SpawnWave(spawnTime, enemyAmount, waveAmount, enemyadaptiveDifficulty));
    }

    private IEnumerator SpawnWave(float spawnTime, int enemyAmount, int waveAmount, float enemyAdaptiveDifficulty)
    {
        GenerateSpawnLocation();

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
                StartCoroutine(SpawnEnemy(adaptiveDifficulty, i));
            }
        }
        catch
        {
            if (spawnTransforms.Count == 0)
                Debug.Log("No spawn locations");
        }
    }

    private IEnumerator SpawnEnemy(float adaptiveDifficulty, int frameWait)
    {
        for (int i = 0; i <= frameWait; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("Enemy spawned at frame " + Time.frameCount);

        GetBiasedEnemy();
        if (spawnTransforms.Count <= 0) GenerateSpawnLocation();
        spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
        objectPool.ActivateEnemy(enemyToSpawn, spawnLocation, adaptiveDifficulty);

        spawnTransforms.Remove(spawnLocation);
    }

    public void ActivateEliteEnemy() => spawnElite = true; 

    public Transform GetSpawnLocations()
    {
        if (spawnTransforms.Count <= 0) GenerateSpawnLocation();
        Transform spawnLocation = spawnTransforms[Random.Range(0, spawnTransforms.Count)];
        return spawnLocation;
    }

    private void GenerateSpawnLocation()
    {
        spawnTransforms.Clear();

        foreach (Transform spawnPoint in Map.ActiveRoom.SpawnPointsParent)
        {
            spawnTransforms.Add(spawnPoint);
        }
    }

    private void GetBiasedEnemy()
    {
        int percentage = Random.Range(1, 101);

        if (percentage <= spawnBias)
        {
            enemyToSpawn = "close";
        }
        else
        {
            int percent = Random.Range(1, 101);

            if (spawnElite && percent < 15)
            {
                enemyToSpawn = "elite";
                return;
            }

            if (percent < 40)
            {
                enemyToSpawn = "range";
                return;
            }
            else if (percent > 70)
            {
                enemyToSpawn = "AOE";
                return;
            }
            else
            {
                enemyToSpawn = "DOT";
                return;
            }
        }
    }
}
