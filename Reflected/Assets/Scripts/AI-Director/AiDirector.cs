using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDirector : MonoBehaviour
{
    //Difficulty
    [SerializeField] string difficultyLevel;
    const string easy = "easy";
    const string medium = "medium";
    const string hard = "hard";
    float spawntime;
    int amountOfEnemies;

    //Room-stats
    bool activeRoom;
    bool inbetweenRooms;
    int enemiesInRoom;
    [SerializeField] float timeToClearRoom;
    List<float> clearTimesList = new List<float>();

    //Map-stats
    int numberOfRoomsCleared;
    int numberOfRoomsLeftOnMap;
    int NumberOfRoomsSinceShop;
    int numberOfEnemiesKilled;

    //Player-stats
    Player player;
    [SerializeField] float playerCurrentHelathPercentage;
    int playerCurrency;


    EnemySpawner enemySpawner;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!enemySpawner) enemySpawner = GetComponent<EnemySpawner>();

        difficultyLevel = medium;
        checkDifficulty();
        activeRoom = false;
        inbetweenRooms = false;
        if (player.GetCurrentWeapon().GetType() == typeof(Sword)) enemySpawner.SetMeleePlayer();
    }

    void Update()
    {
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
        if (activeRoom && enemiesInRoom > 0) // Player is in a room with enemies
        {
            timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }
        else if (enemiesInRoom == 0) // Player kills last enemy in a room
        {
            activeRoom = false;
            inbetweenRooms = true;
        }
        else if (!activeRoom && inbetweenRooms) // Player have killed all enemies in a room but have not left the room
        {
            clearTimesList.Add(timeToClearRoom);
            numberOfRoomsCleared++;
            numberOfRoomsLeftOnMap--;
            NumberOfRoomsSinceShop++;
            
            ResetRoom();
            inbetweenRooms = false;
        }
    }

    public void EnterRoom()
    {
        activeRoom = true;
        checkDifficulty();
        enemiesInRoom = amountOfEnemies * 2;
        enemySpawner.SpawnEnemy(spawntime, amountOfEnemies);
    }

    public void killEnemyInRoom()
    {
        enemiesInRoom--;
        numberOfEnemiesKilled++;
    }
}
