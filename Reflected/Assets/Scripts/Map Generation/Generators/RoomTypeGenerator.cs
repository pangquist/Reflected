using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypeGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private MapGenerator mapGenerator;

    [Header("Room Types")]

    [Range(0f, 1f)]
    [SerializeField] private float peacefulChance;

    [SerializeField] private bool randomizeStartRoom;

    [SerializeField] private bool randomizeBossRoom;

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

        // Set start room

        int maxExclusive = map.Rooms.Count - (randomizeBossRoom ? 0 : 1);
        int startRoom = randomizeStartRoom ? Random.Range(0, maxExclusive) : 0;

        map.StartRoom = map.Rooms[startRoom];
        map.StartRoom.SetType(RoomType.Start);

        // Set boss room

        int bossRoom = startRoom;

        while (bossRoom == startRoom)
            bossRoom = randomizeStartRoom ? Random.Range(0, map.Rooms.Count) : map.Rooms.Count;

        map.BossRoom = map.Rooms[bossRoom];
        map.BossRoom.SetType(RoomType.Boss);

        //foreach (TerrainPlane terrainPlane in map.Rooms[bossRoom].GetComponentsInChildren<TerrainPlane>())
            //terrainPlane.GetComponentInChildren<Material>().color = new Color(150, 60, 20);

        // Log

        mapGenerator.Log("Start room: " + startRoom);
        mapGenerator.Log("Boss room: " + bossRoom);
    }

}
