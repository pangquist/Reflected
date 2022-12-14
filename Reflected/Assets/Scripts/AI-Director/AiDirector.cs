using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] float averageTimeToClearRoom;
    List<float> clearTimesList = new List<float>();
    Queue<float> clearTimesQueue = new Queue<float>();
    [SerializeField] List<GameObject> chests;

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
    UiManager uiManager;
    LootPoolManager lootPool;
    Rarity currentRarity;
    int eliteThreshold = 5;

    public static UnityEvent RoomCleared = new UnityEvent();


    // Properties

    public bool AllEnemiesKilled => aliveEnemiesInRoom <= 0;
    public float GetAverageTime() => averageTimeToClearRoom;
    public int GetKillCount() => numberOfEnemiesKilled;
    public int GetClearedRooms() => numberOfRoomsCleared;

    private void Awake()
    {
        Map.RoomEntered.AddListener(RoomEntered);
    }

    private void Start()
    {
        StartCoroutine(DelayedStart(0.2f));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!player) player = FindObjectOfType<Player>();
        if (!enemySpawner) enemySpawner = GetComponent<EnemySpawner>();
        if (!map) map = GameObject.Find("Map").GetComponent<Map>();
        if (!lootPool) lootPool = GameObject.Find("LootPoolManager").GetComponent<LootPoolManager>();
        if (!uiManager) uiManager = FindObjectOfType<UiManager>();
        waveAmount = 1;
        checkDifficulty();
        activeRoom = false;
        inbetweenRooms = false;
        numberOfRoomsLeftOnMap = map.Rooms.Count;
    }

    private void Update()
    {
        CheckRoomActivity();
    }

    private void checkDifficulty()
    {
        startDiffuculty();
        difficultySteps = numberOfEnemiesKilled / 15;

        for (int i = 0; i < difficultySteps; i++)
        {
            minSpawnAmount++;
            maxSpawnAmount++;
            spawntime -= 0.1f;
        }
        if (numberOfRoomsCleared % 3 == 0 && numberOfRoomsCleared > 0)
        {
            waveAmount++;
        }
        if (numberOfRoomsCleared % 2 == 0 && numberOfRoomsCleared > 0)
        {
            lootPool.IncreaseRarity();
        }

        minSpawnAmount -= waveAmount;
        maxSpawnAmount -= waveAmount;

        amountOfEnemiesToSpawn = Random.Range(minSpawnAmount, maxSpawnAmount);
    }
    private void startDiffuculty()
    {
        spawntime = 2;
        minSpawnAmount = 4;
        maxSpawnAmount = 6;
    }
    private void CheckRoomActivity()
    {
        if (activeRoom) // Player is in a room with enemies
        {
            if (uiManager.GetMenuState() != UiManager.MenuState.Active)
                timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }
        if (activeRoom && aliveEnemiesInRoom == 0) // Player kills last enemy in a room
        {
            activeRoom = false;
            inbetweenRooms = true;
            MusicManager musicManager = FindObjectOfType<MusicManager>();
            musicManager.SetMusic(DimensionManager.CurrentDimension, 0);
        }
        if (inbetweenRooms) //All enemies are killed but player is still in same room
        {
            UpdateRoomStatistics();
            SpawnChest();

            //RoomCleared.Invoke();
            if (numberOfRoomsCleared <= eliteThreshold) enemySpawner.ActivateEliteEnemy();

            inbetweenRooms = false;
        }
    }
    private void UpdateRoomStatistics()
    {
        clearTimesList.Add(timeToClearRoom);
        clearTimesQueue.Enqueue(timeToClearRoom);
        if (clearTimesQueue.Count >= 4) clearTimesQueue.Dequeue();
        calculateAverageTime();

        numberOfRoomsCleared++;
        numberOfRoomsLeftOnMap--;
        NumberOfRoomsSinceShop++;

        timeToClearRoom = 0;
    }
    public void RoomEntered() //called when a room is entered (by Map.RoomEntered event)
    {
        if (Map.ActiveRoom.Cleared || Map.ActiveRoom.Type == RoomType.Boss)
            return;

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
    private float calculateAverageTime() => averageTimeToClearRoom = clearTimesList.Sum() / clearTimesList.Count();
    private float calculateTimeStat() => clearTimesQueue.Sum() / clearTimesQueue.Count();

    private float EnemyStatModifier()
    {
        float extraStats = numberOfRoomsCleared * 0.4f;

        if (averageTimeToClearRoom > 0) extraStats += (5f / calculateTimeStat());

        return extraStats;
    }
    private void SpawnChest()
    {
        currentRarity = lootPool.GetRandomRarity();
        Vector3 spawnPosition = enemySpawner.GetSpawnLocations().position;
        spawnPosition.y = 10;
        switch (currentRarity.rarity)
        {
            case "Common":
                Instantiate(chests[0], spawnPosition, Quaternion.Euler(0, 0, 0));
                break;
            case "Rare":
                Instantiate(chests[1], spawnPosition, Quaternion.Euler(0, 0, 0));
                break;
            case "Epic":
                Instantiate(chests[2], spawnPosition, Quaternion.Euler(0, 0, 0));
                break;
            case "Legendary":
                Instantiate(chests[3], spawnPosition, Quaternion.Euler(0, 0, 0));
                break;
            default:
                Instantiate(chests[0], spawnPosition, Quaternion.Euler(0, 0, 0));
                break;
        }


    }
}
