using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Read Only")]

    [ReadOnly][SerializeField] private int sizeX;
    [ReadOnly][SerializeField] private int sizeZ;
    [ReadOnly][SerializeField] private List<Room> rooms;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private Room activeRoom;

    // Properties

    public int SizeX => sizeX;
    public int SizeZ => sizeZ;
    public List<Room> Rooms => rooms;
    public List<Chamber> Chambers => chambers;

    public Room ActiveRoom { get { return activeRoom; } set { activeRoom = value; } }

    public void Initialize(int sizeX, int sizeZ)
    {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        name = "Map";
    }

    public void DeactivateAll()
    {
        foreach (Room room in rooms)
            room.Deactivate(null);
    }

    public void SetStartRoom(int roomIndex)
    {
        Room startRoom = rooms[roomIndex];
        startRoom.gameObject.SetActive(true);
        startRoom.Activate();
        startRoom.SetCleared(true);
    }
}
