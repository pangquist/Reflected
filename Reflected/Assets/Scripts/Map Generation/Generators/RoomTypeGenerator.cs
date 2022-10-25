using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RoomTypeGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private MapGenerator mapGenerator;

    [Header("Room Types")]

    [Range(0f, 1f)]
    [SerializeField] private float peacefulChance;

    public void Generate(Map map)
    {
        // Make all rooms Peaceful or Monster

        foreach (Room room in map.Rooms)
        {
            if (Random.Range(0f, 1f) < peacefulChance)
                room.SetType(RoomType.Peaceful);
            else
                room.SetType(RoomType.Monster);
        }

        DetermineStartRoom(map);
        DetermineBossRoom(map);
    }

    private void DetermineStartRoom(Map map)
    {
        Rect mapRect = new Rect(0, 0, map.SizeX, map.SizeZ);
        Room startRoom = map.Rooms[0];

        foreach (Room room in map.Rooms)
        {
            if (room.Rect.Contains(mapRect.center))
            {
                startRoom = room;
                break;
            }

            if (Vector2.Distance(room.Rect.center, mapRect.center) < Vector2.Distance(startRoom.Rect.center, mapRect.center))
                startRoom = room;
        }

        map.StartRoom = startRoom;
        map.StartRoom.SetType(RoomType.Start);

        mapGenerator.Log("Start room: " + map.StartRoom.name);
    }

    private void DetermineBossRoom(Map map)
    {
        Rect mapRect = new Rect(0, 0, map.SizeX, map.SizeZ);

        Dictionary<Room, float> roomFitness = new Dictionary<Room, float>();
        KeyValuePair<Room, float>[] orderedArray;

        // Give all rooms a score of 0

        foreach (Room room in map.Rooms)
            roomFitness.Add(room, 0);

        // Test 1: Outer edge rooms (outer edge is better)

        for (int i = 1; i > 0; ++i)
        {
            Rect inflatedMapRect = mapRect.Inflated(-i, -i);

            foreach (Room room in map.Rooms)
            {
                if (inflatedMapRect.Contains(room.Rect) == false)
                {
                    roomFitness[room] += 1f;
                    i = -1;
                }
            }       
        }

        // Test 2: Distance to start room (more is better)

        orderedArray = roomFitness.OrderBy(pair => Vector2.Distance(pair.Key.Rect.center, map.StartRoom.Rect.center)).ToArray();
        OrderToFitness(1f);

        // Test 3: Area (more is better)

        orderedArray = roomFitness.OrderBy(pair => pair.Key.Rect.Area()).ToArray();
        OrderToFitness(1f);

        // Test 4: Graph reachability (more is better)

        orderedArray = roomFitness.OrderBy(pair => map.Graph.TraverseGraph(map.StartRoom, pair.Key)).ToArray();
        OrderToFitness(1f);

        // Adds fitness to potential boss rooms based of the order orderedArray (higher index = more fitness)
        void OrderToFitness(float weight)
        {
            for (int i = 0; i < orderedArray.Length; ++i)
            {
                roomFitness[orderedArray[i].Key] += (float)i / (orderedArray.Length - 1) * weight;
            }
        }

        // Find room with highest fitness score

        orderedArray = roomFitness.OrderBy(pair => -pair.Value).ToArray();

        map.BossRoom = orderedArray.ElementAt(0).Key;
        map.BossRoom.SetType(RoomType.Boss);

        mapGenerator.Log("Boss room: " + map.BossRoom.name);
        mapGenerator.Log("Boss room fittness: " + orderedArray.ElementAt(0).Value.ToString("0.00") + " / 4");
    }

}
