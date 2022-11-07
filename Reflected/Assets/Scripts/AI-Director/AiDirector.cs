using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AiDirector : MonoBehaviour
{
    //Difficulty
    [SerializeField] float spawntime;
    [SerializeField] int amountOfEnemiesToSpawn;
    [SerializeField] int waveAmount;
    [SerializeField] int difficultySteps;
    int minSpawnAmount, maxSpawnAmount;

    //Room-stats
    bool inbetweenRooms;
    [SerializeField] bool activeRoom;
    [SerializeField] int aliveEnemiesInRoom;
    [SerializeField] float timeToClearRoom;
    [SerializeField] float avergaeTimeToClearRoom;
    List<float> clearTimesList = new List<float>();

    //Map-stats
    [SerializeField] int numberOfRoomsCleared;
    [SerializeField] int numberOfRoomsLeftOnMap;
    [SerializeField] int NumberOfRoomsSinceShop;
    [SerializeField] int numberOfEnemiesKilled;
    Map map;

    //Player-stats
    Player player;
    [SerializeField] float playerCurrentHelathPercentage;
    [SerializeField] int temporaryCurrency;


    EnemySpawner enemySpawner;
    [SerializeField] GameObject chest;

    // Properties

    public bool AllEnemiesKilled => aliveEnemiesInRoom <= 0;

    void Start()
    {
        StartCoroutine(DelayedStart(0.2f));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!player) player = FindObjectOfType<Player>();
        if (!enemySpawner) enemySpawner = GetComponent<EnemySpawner>();
        if (!map) map = GameObject.Find("Map").GetComponent<Map>();

        waveAmount = 1;
        checkDifficulty();
        activeRoom = false;
        inbetweenRooms = false;
        numberOfRoomsLeftOnMap = map.Rooms.Count;
    }

    void Update()
    {
        CheckRoomActivity();
    }
    private void ResetMap()
    {
        numberOfRoomsCleared = 0;
        numberOfRoomsLeftOnMap = 0;
        clearTimesList.Clear();
    }
    private void checkDifficulty()
    {
        startDiffuculty();
        difficultySteps = numberOfEnemiesKilled / 20;

        for (int i = 0; i < difficultySteps; i++)
        {
            minSpawnAmount++;
            maxSpawnAmount++;
            spawntime -= 0.1f;
        }
        if(numberOfRoomsCleared % 4 == 0 && numberOfRoomsCleared > 0)
        {
            waveAmount++;
        }

        amountOfEnemiesToSpawn = Random.Range(minSpawnAmount, maxSpawnAmount);
    }

    private void startDiffuculty()
    {
        spawntime = 2;
        minSpawnAmount = 3;
        maxSpawnAmount = 6;
    }

    private void CheckRoomActivity()
    {
        if (activeRoom) // Player is in a room with enemies
        {
            timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }
        if (activeRoom && aliveEnemiesInRoom == 0) // Player kills last enemy in a room
        {
            activeRoom = false;
            inbetweenRooms = true;
            MusicManager musicManager = FindObjectOfType<MusicManager>();
            musicManager.ChangeMusicIntensity(-1);
        }
        if (inbetweenRooms) //All enemies are killed but player is still in same room
        {
            UpdateRoomStatistics();
            SpawnChest();

            inbetweenRooms = false;
        }
    }
    private void UpdateRoomStatistics()
    {
        clearTimesList.Add(timeToClearRoom);
        calculateAverageTime();

        numberOfRoomsCleared++;
        numberOfRoomsLeftOnMap--;
        NumberOfRoomsSinceShop++;

        timeToClearRoom = 0;
    }
    public void EnterRoom() //called when a new room activates (from Room-script)
    {
        activeRoom = true;
        aliveEnemiesInRoom = 0;
        checkDifficulty();
        aliveEnemiesInRoom = amountOfEnemiesToSpawn * waveAmount;

        enemySpawner.SpawnEnemy(spawntime, amountOfEnemiesToSpawn, waveAmount, EnemyStatModifier());
    }
    public void killEnemyInRoom() //called when enemy dies (from enemy-script)
    {
        aliveEnemiesInRoom--;
        numberOfEnemiesKilled++;
    }
    private float calculateAverageTime() => avergaeTimeToClearRoom = clearTimesList.Sum() / clearTimesList.Count();
    private float EnemyStatModifier()
    {
        float extraStats = numberOfRoomsCleared * 0.1f;

        if(avergaeTimeToClearRoom > 0) extraStats += 10f / calculateAverageTime();
        
        return extraStats;
    }
    private void SpawnChest()
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(5, 3, 0);
        Instantiate(chest, spawnPosition, Quaternion.Euler(0, 0, 0));
    }
}
