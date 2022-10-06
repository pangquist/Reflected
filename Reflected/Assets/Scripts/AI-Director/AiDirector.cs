using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AiDirector : MonoBehaviour
{
    //Difficulty
    [SerializeField] string difficultyLevel;
    const string superEasy = "superEasy";
    const string easy = "easy";
    const string medium = "medium";
    const string hard = "hard";
    [SerializeField] float spawntime;
    [SerializeField] int amountOfEnemiesToSpawn;

    //Room-stats
    [SerializeField] bool activeRoom;
    bool inbetweenRooms;
    [SerializeField] int enemiesInRoom;
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


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!enemySpawner) enemySpawner = GetComponent<EnemySpawner>();
        //if(!map) map = GameObject.Find("Map Generator").GetComponent<Map>();

        difficultyLevel = easy;
        checkDifficulty();
        activeRoom = false;
        inbetweenRooms = false;
        if (player.GetCurrentWeapon().GetType() == typeof(Sword)) enemySpawner.SetMeleePlayer();
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
        if(difficultyLevel == superEasy)
        {
            spawntime = 2;
            amountOfEnemiesToSpawn = Random.Range(1, 3);
        }
        if (difficultyLevel == easy)
        {
            spawntime = 2;
            amountOfEnemiesToSpawn = Random.Range(4,6);
        }
        else if (difficultyLevel == medium)
        {
            spawntime = 1;
            amountOfEnemiesToSpawn = Random.Range(6, 9);
        }
        else if (difficultyLevel == hard)
        {
            spawntime = 0.5f;
            amountOfEnemiesToSpawn = Random.Range(9, 12);
        }
    }
    private void CheckRoomActivity()
    {
        if (activeRoom) // Player is in a room with enemies
        {
            timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }
        if (activeRoom && enemiesInRoom == 0) // Player kills last enemy in a room
        {
            activeRoom = false;
            inbetweenRooms = true;
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
    public void EnterRoom() //is called when a new room activates (from Room-script)
    {
        activeRoom = true;
        checkDifficulty();
        enemiesInRoom = amountOfEnemiesToSpawn * 2;

        enemySpawner.SpawnEnemy(spawntime, amountOfEnemiesToSpawn, EnemyStatModifier());
    }
    public void killEnemyInRoom() //is called when enemy dies (from enemy-script)
    {
        enemiesInRoom--;
        numberOfEnemiesKilled++;
    }
    private float calculateAverageTime() => avergaeTimeToClearRoom = clearTimesList.Sum() / clearTimesList.Count();
    private float EnemyStatModifier()
    {
        float extraStats = numberOfRoomsCleared * 0.02f;

        if(avergaeTimeToClearRoom < 0) extraStats += 10f / calculateAverageTime();
        
        return extraStats;
    }

    private void SpawnChest()
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(5, 5, 0);
        Instantiate(chest, spawnPosition, Quaternion.Euler(0, 0, 0));
    }
}
