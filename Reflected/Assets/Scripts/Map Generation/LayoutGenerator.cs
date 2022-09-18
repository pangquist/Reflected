using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LayoutGenerator : MonoBehaviour
{
    private class RoomNode
    {
        public RectInt rect;
        public RoomNode child1 = null;
        public RoomNode child2 = null;

        public RoomNode(int x, int y, int width, int height)
        {
            rect = new RectInt(x, y, width, height);
        }
    }

    // Adjustable fields

    [Header("References")]

    [SerializeField] private GameObject block;

    [Header("Seed")]

    [SerializeField] private int seed;

    [Header("Map Size")]

    [Range(30, 200)]
    [SerializeField] private int minMapWidth;

    [Range(30, 200)]
    [SerializeField] private int minMapHeight;

    [Range(30, 200)]
    [SerializeField] private int maxMapWidth;

    [Range(30, 200)]
    [SerializeField] private int maxMapHeight;

    [Header("Room Size")]

    [Range(3, 20)]
    [SerializeField] private int minRoomLength;

    [Range(4, 30)]
    [SerializeField] private int maxRoomLength;

    [Range(9, 200)]
    [SerializeField] private int minRoomArea;

    [Range(0, 3)]
    [SerializeField] private int roomPadding;

    [Tooltip("Alters the odds of a room being divided along its short or long side, resulting in square or stretched rooms. " +
        "0 = Square (dividing long side), 0.5 = Even mix, 1 = Stretched (dividing short side)")]
    [Range(0f, 1f)]
    [SerializeField] private float divisionBias;

    [Header("Passageways")]

    [SerializeField] private bool addRandomPassageways;

    [Range(0f, 1f)]
    [SerializeField] private float minPassageways;

    [Range(0f, 1f)]
    [SerializeField] private float maxPassageways;

    // Read only fields

    [Header("Read Only")]
    
    [ReadOnly][SerializeField] private int mapWidth;
    [ReadOnly][SerializeField] private int mapHeight;
    [ReadOnly][SerializeField] private List<RectInt> finalRects;

    // Hidden fields

    private List<RoomNode> leafNodes = new List<RoomNode>();

    private void Start()
    {
        NewMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NewMap();
    }

    /// <summary>
    /// Generates a new map using the current settings
    /// </summary>
    public void NewMap()
    {
        // Clear data

        finalRects.Clear();
        leafNodes.Clear();

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        // New seed

        Random.InitState(seed != 0 ? seed : (int)System.DateTime.Now.Ticks);

        // Generate map

        RandomizeMapSize();
        RoomNode firstRoom = new RoomNode(0, 0, mapWidth, mapHeight);
        RecursiveDivision(ref firstRoom);
        ShrinkRooms();
        Build();

        Debug.Log("Layout complete. " + leafNodes.Count + " rooms created.");
    }

    /// <summary>
    /// Randomizes the size of the map
    /// </summary>
    private void RandomizeMapSize()
    {
        mapWidth = Random.Range(minMapWidth, maxMapWidth + 1);
        mapHeight = Random.Range(minMapHeight, maxMapHeight + 1);

        Debug.Log("Generating map with size: " + mapWidth + "x" + mapHeight);
    }

    /// <summary>
    /// Divides the provided room into smaller rooms recursively
    /// </summary>
    private void RecursiveDivision(ref RoomNode room)
    {
        // If the room is too big
        if (room.rect.width - roomPadding * 2 > maxRoomLength || room.rect.height - roomPadding * 2 > maxRoomLength)
        {
            // Try dividing it
            if (TryDivision(ref room))
                return;
            else
                Debug.LogWarning("Room was too big, but could not be divided.");
        }

        // If the room is not too big
        else
        {
            // 50% chance to try dividing it
            if (Random.Range(0f, 1f) < 0.5f && TryDivision(ref room))
                return;
        }

        // This room was not divided: Add as leaf node
        finalRects.Add(room.rect);
        leafNodes.Add(room);
    }

    private bool TryDivision(ref RoomNode room)
    {
        // Check if big enough to be divided

        bool horizontalDivisionPossible =
            room.rect.height >= minRoomLength * 2 + roomPadding * 4 &&
            room.rect.width * room.rect.height >= minRoomArea * 2 + roomPadding * room.rect.width * 2;

        bool verticalDivisionPossible =
            room.rect.width >= minRoomLength * 2 + roomPadding * 4 &&
            room.rect.width * room.rect.height >= minRoomArea * 2 + roomPadding * room.rect.height * 2;

        // Can be divided
        if (horizontalDivisionPossible || verticalDivisionPossible)
        {
            // If vertical not possible: Horizontal
            if (!verticalDivisionPossible)
                DivideHorizontally(ref room);

            // If horizontal not possible: Vertical
            else if (!horizontalDivisionPossible)
                DivideVertically(ref room);

            // If both directions possible: Use division bias
            else if (Random.Range(0f, 1f) < divisionBias)
                DivideLongSide(ref room);

            else
                DivideShortSide(ref room);

            // Continue with recursive division on children
            RecursiveDivision(ref room.child1);
            RecursiveDivision(ref room.child2);

            return true;
        }

        // Can not be divided
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Chooses direction of division randomly
    /// </summary>
    private void DivideRandomly(ref RoomNode room)
    {
        if (Random.Range(0f, 1f) < 0.5f)
            DivideHorizontally(ref room);
        else
            DivideVertically(ref room);
    }

    /// <summary>
    /// Divides the room by its short side
    /// </summary>
    private void DivideShortSide(ref RoomNode room)
    {
        if (room.rect.width == room.rect.height)
            DivideRandomly(ref room);

        else if (room.rect.width > room.rect.height)
            DivideVertically(ref room);

        else
            DivideHorizontally(ref room);
    }

    /// <summary>
    /// Divides the room by its long side
    /// </summary>
    private void DivideLongSide(ref RoomNode room)
    {
        if (room.rect.width == room.rect.height)
            DivideRandomly(ref room);

        else if (room.rect.width > room.rect.height)
            DivideHorizontally(ref room);

        else
            DivideVertically(ref room);
    }

    /// <summary>
    /// Divides the room into two children by dividing horizontally
    /// </summary>
    private void DivideHorizontally(ref RoomNode room)
    {
        int minY = minRoomLength + roomPadding * 2;
        int maxY = room.rect.height - minRoomLength - roomPadding * 2;
        int split = Random.Range(minY, maxY + 1);

        room.child1 = new RoomNode(room.rect.x, room.rect.y, room.rect.width, split);
        room.child2 = new RoomNode(room.rect.x, room.rect.y + split, room.rect.width, room.rect.height - split);
    }

    /// <summary>
    /// Divides the room into two children by dividing vertically
    /// </summary>
    private void DivideVertically(ref RoomNode room)
    {
        int minX = minRoomLength + roomPadding * 2;
        int maxX = room.rect.width - minRoomLength - roomPadding * 2;
        int split = Random.Range(minX, maxX + 1);

        room.child1 = new RoomNode(room.rect.x, room.rect.y, split, room.rect.height);
        room.child2 = new RoomNode(room.rect.x + split, room.rect.y, room.rect.width - split, room.rect.height);
    }

    /// <summary>
    /// Shrinks all rooms based of the room padding
    /// </summary>
    private void ShrinkRooms()
    {
        foreach (RoomNode room in leafNodes)
        {
            room.rect.x += roomPadding;
            room.rect.y += roomPadding;
            room.rect.width -= roomPadding * 2;
            room.rect.height -= roomPadding * 2;
        }
    }

    /// <summary>
    /// Builds the map layout in the world by instantiating blocks
    /// </summary>
    private void Build()
    {
        foreach (RoomNode room in leafNodes)
        {
            GameObject roomBlock = GameObject.Instantiate(block, transform);
            roomBlock.transform.position = new Vector3(room.rect.x, -1, room.rect.y);
            roomBlock.transform.localScale = new Vector3(room.rect.width, 1, room.rect.height);
            roomBlock.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0f, Random.Range(0f, 1f), 0f);
        }
    }

}
