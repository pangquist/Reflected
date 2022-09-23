using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDirector : MonoBehaviour
{
    //Difficulty
    string difficultyLevel;
    const string easy = "easy";
    const string medium = "medium";
    const string hard = "hard";
    [SerializeField] float spawntime;
    [SerializeField] int amountOfEnemies;

    //Room-stats
    bool activeRoom;
    bool inbetweenRooms;
    int enemiesInRoom;
    float timeToClearRoom;
    List<float> clearTimesList = new List<float>();

    //Map-stats
    int numberOfRoomsCleared;
    int numberOfRoomsLeftOnMap;
    int NumberOfRoomsSinceShop;
    List<int> numberOfEnemiesKilled = new List<int>();

    //Player-stats
    Player player;
    float playerCurrentHelathPercentage;
    int playerCurrency;


    EnemySpawner enemySpawner;


    void Start()
    {
        //if(!player) player = GetComponent<Player>();
        //if(!enemySpawner) enemySpawner = GetComponent<EnemySpawner>();

        if (!player)
        {
            player = GetComponent<Player>();
            Debug.Log("player found");
        }
        if (!enemySpawner)
        {
            enemySpawner = GetComponent<EnemySpawner>();
            Debug.Log("enemySpawner found");
        }

        difficultyLevel = medium;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            checkDifficulty();
            enemySpawner.SpawnEnemy(spawntime, amountOfEnemies);
        }

        CheckRoomActivity();
    }

    private void ResetRoom()
    {
        enemiesInRoom = 0;
        timeToClearRoom = 0;
    }

    private void ResetMap()
    {
        numberOfRoomsCleared = 0;
        numberOfRoomsLeftOnMap = 0;
        clearTimesList.Clear();
    }

    private void checkDifficulty()
    {
        if (difficultyLevel == easy)
        {
            spawntime = 2;
            amountOfEnemies = 4;
        }
        else if (difficultyLevel == medium)
        {
            spawntime = 1;
            amountOfEnemies = 6;
        }
        else if (difficultyLevel == hard)
        {
            spawntime = 0.5f;
            amountOfEnemies = 9;
        }
    }

    private void CheckRoomActivity()
    {
        if (activeRoom && enemiesInRoom > 0)
        {
            timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }
        if (enemiesInRoom == 0)
        {
            activeRoom = false;
            inbetweenRooms = true;
        }
        if (!activeRoom && inbetweenRooms)
        {
            clearTimesList.Add(timeToClearRoom);
            numberOfEnemiesKilled.Add(amountOfEnemies * 2);
            ResetRoom();
            inbetweenRooms = false;
        }
    }
}
