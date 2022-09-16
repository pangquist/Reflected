using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDirector : MonoBehaviour
{
    //Room-stats
    bool activeRoom;
    float timeToClearRoom;
    int enemiesInRoom;
    [SerializeField] int numberOfSpawnpoints;

    //Map-stats
    int numberOfRoomsCleared;
    int numberOfRoomsLeftOnMap;
    int NumberOfRoomsSinceShop;

    //Player-stats
    [SerializeField] Player player;
    float playerCurrentHelathPercentage;
    int playerCurrency;


    EnemySpawner EnemySpawner;


    void Start()
    {
        if(!player) player = GetComponent<Player>();
    }

    void Update()
    {
        if(activeRoom && enemiesInRoom > 0)
        {
            timeToClearRoom += Time.deltaTime;
            playerCurrentHelathPercentage = player.GetHealthPercentage();
        }

        if (!activeRoom)
        {
            ResetRoom();
        }

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

    }
}
