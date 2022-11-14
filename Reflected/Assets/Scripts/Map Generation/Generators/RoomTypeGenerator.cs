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

    //[Header("Start Room")]

    [Header("Boss Room")]

    [Tooltip("Outer edge")]
    [SerializeField] private float testWeightBoss1;

    [Tooltip("Distance to start room")]
    [SerializeField] private float testWeightBoss2;

    [Tooltip("Area")]
    [SerializeField] private float testWeightBoss3;

    [Tooltip("Graph reachability")]
    [SerializeField] private float testWeightBoss4;

    [Header("Shop Room")]

    [SerializeField] GameObject shopStructurePrefab;

    [SerializeField] private int nrOfShopRooms;

    [Range(0f, 1f)]
    [SerializeField] private float shopRoomFitnessThreshold;

    [Tooltip("Distance to start room")]
    [SerializeField] private float testWeightShop1;

    [Tooltip("Distance to boss room")]
    [SerializeField] private float testWeightShop2;

    [Tooltip("Distance to other shop rooms")]
    [SerializeField] private float testWeightShop3;

    public void Generate(Map map)
    {
        foreach (Room room in map.Rooms)
            room.SetType(RoomType.Monster);

        DetermineStartRoom(map);
        DetermineBossRoom(map);
        DetermineShopRooms(map);
    }

    private void DetermineStartRoom(Map map)
    {
        Rect mapRect = new Rect(0, 0, map.SizeX * MapGenerator.ChunkSize, map.SizeZ * MapGenerator.ChunkSize);
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

        startRoom.SetType(RoomType.Start);
        mapGenerator.Log("Start room: " + Map.StartRoom.name);
    }

    private void DetermineBossRoom(Map map)
    {
        Rect mapRect = new Rect(0, 0, map.SizeX * MapGenerator.ChunkSize, map.SizeZ * MapGenerator.ChunkSize);

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
                    roomFitness[room] += testWeightBoss1;
                    i = -1;
                }
            }       
        }

        // Test 2: Distance to start room (more is better)

        orderedArray = roomFitness.OrderBy(pair => Vector2.Distance(pair.Key.Rect.center, Map.StartRoom.Rect.center)).ToArray();
        OrderToFitness(testWeightBoss2);

        // Test 3: Area (more is better)

        orderedArray = roomFitness.OrderBy(pair => pair.Key.Rect.Area()).ToArray();
        OrderToFitness(testWeightBoss3);

        // Test 4: Graph reachability (more is better)

        orderedArray = roomFitness.OrderBy(pair => map.Graph.TraverseGraph(Map.StartRoom, pair.Key)).ToArray();
        OrderToFitness(testWeightBoss4);

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
        orderedArray.ElementAt(0).Key.SetType(RoomType.Boss);

        float maxScore = testWeightBoss1 + testWeightBoss2 + testWeightBoss3 + testWeightBoss4;
        mapGenerator.Log("Boss room: " + Map.BossRoom.name);
        mapGenerator.Log("Boss room fittness: " + orderedArray.ElementAt(0).Value.ToString("0.00") + " / " + maxScore.ToString("0.00"));
    }

    private void DetermineShopRooms(Map map)
    {
        List<Room> shopRooms = new List<Room>();

        for (int i = 0; i < nrOfShopRooms; ++i)
        {
            Dictionary<Room, float> roomFitness = new Dictionary<Room, float>();
            KeyValuePair<Room, float>[] orderedArray;

            // Give all relevant rooms a score of 0

            foreach (Room room in map.Rooms)
                roomFitness.Add(room, 0);

            roomFitness.Remove(Map.StartRoom);
            roomFitness.Remove(Map.BossRoom);

            foreach (Room room in shopRooms)
                roomFitness.Remove(room);

            if (roomFitness.Count == 0)
                break;

            // Test 1: Distance to start room (more is better)

            orderedArray = roomFitness.OrderBy(pair => Vector2.Distance(pair.Key.Rect.center, Map.StartRoom.Rect.center)).ToArray();
            OrderToFitness(testWeightShop1);

            // Test 2: Distance to boss room (more is better)

            orderedArray = roomFitness.OrderBy(pair => Vector2.Distance(pair.Key.Rect.center, Map.BossRoom.Rect.center)).ToArray();
            OrderToFitness(testWeightShop2);

            // Test 3: Distance to other shop rooms (more is better)

            foreach (Room room in shopRooms)
            {
                orderedArray = roomFitness.OrderBy(pair => Vector2.Distance(pair.Key.Rect.center, room.Rect.center)).ToArray();
                OrderToFitness(testWeightShop3);
            }

            // Adds fitness to potential shop rooms based of the order orderedArray (higher index = more fitness)
            void OrderToFitness(float weight)
            {
                for (int i = 0; i < orderedArray.Length; ++i)
                {
                    roomFitness[orderedArray[i].Key] += (float)i / (orderedArray.Length - 1) * weight;
                }
            }

            // Find room with highest fitness score

            orderedArray = roomFitness.OrderBy(pair => -pair.Value).ToArray();
            int index = Random.Range(0, (int)(orderedArray.Length * (1 - shopRoomFitnessThreshold)));
            KeyValuePair<Room, float> shopRoom = orderedArray.ElementAt(index);

            float maxScore = testWeightShop1 + testWeightShop2 + testWeightShop3 * shopRooms.Count;
            mapGenerator.Log("Shop room" + (i+1) + ": " + shopRoom.Key.name);
            mapGenerator.Log("Shop room fitness: " + shopRoom.Value.ToString("0.00") + " / " + maxScore.ToString("0.00") + " (index " + index + ")");

            shopRoom.Key.SetType(RoomType.Shop);
            shopRooms.Add(shopRoom.Key);

            // Instantiate shop structure

            Transform shop = Instantiate(shopStructurePrefab, shopRoom.Key.transform).transform;
            shop.position = new Vector3(shopRoom.Key.Rect.center.x, 0, shopRoom.Key.Rect.center.y);
        }
    }
}
