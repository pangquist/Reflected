using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDirector : MonoBehaviour
{
    //Room-stats
    bool activeRoom;
    float timeToClearRoom;
    int enemiesInRoom;

    //Map-stats
    int numberOfRoomsCleared;
    int numberOfRoomsLeftOnMap;
    int NumberOfRoomsSinceShop;

    //Player-stats
    float playerHelathPercentage;
    int playerCurrency;



    void Start()
    {
        
    }

    void Update()
    {
        if(activeRoom && enemiesInRoom > 0)
        {
            timeToClearRoom += Time.deltaTime;
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
