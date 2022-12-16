using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject tutorialDummy;
    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;
    [SerializeField] GameObject enemyAOE;
    [SerializeField] GameObject enemyDOT;
    [SerializeField] GameObject enemyElite;
    GameObject enemyToSpawn;

    EnemyStatSystem enemyStatSystem;

    bool spawnElite;

    List<Transform> spawnTransforms = new List<Transform>();
    Transform spawnLocation;

    [SerializeField] int spawnBias = 60;

    void Start()
    {
        GenerateSpawnLocation();
        enemyStatSystem = GameObject.Find("EnemyStatSystem").GetComponent<EnemyStatSystem>();
    }

    public void SpawnTutorialDummy(Transform playerPos)
    {
        Vector3 spawnPos = playerPos.position;
        spawnPos.x += 3; spawnPos.y += 3; spawnPos.z += 3;
        Instantiate(tutorialDummy, spawnPos, Quaternion.Euler(0, 225, 0));
    }

    public void SpawnEnemy(float spawnTime, int enemyAmount, int waveAmount, float enemyadaptiveDifficulty)
    {
        //enemyStatSystem.SetNewStats(1, 1, true); //might change values in future for more fun gameplay. Needs to be balanced with enemyAdaptiveDiffuculty.
        //enemyStatSystem.ApplyNewStats(DimensionManager.True);
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
        Enemy enemy = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.Euler(0, 0, 0), Map.ActiveRoom.EnemiesParent).GetComponentInChildren<Enemy>();
        enemy.AdaptiveDifficulty(adaptiveDifficulty);

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
            enemyToSpawn = enemyClose;
        }
        else
        {
            int percent = Random.Range(1, 101);

            if (spawnElite && percent < 10)
            {
                enemyToSpawn = enemyElite;
                return;
            }

            if (percent < 33)
            {
                enemyToSpawn = enemyRange;
                return;
            }
            else if (percent > 66)
            {
                enemyToSpawn = enemyAOE;
                return;
            }
            else
            {
                enemyToSpawn = enemyDOT;
                return;
            }
        }
    }
}
