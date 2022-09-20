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

    [Tooltip("Alters the odds of a room being split its long or short side, resulting in square or stretched children. " +
        "0 = Square (splitting long side), 0.5 = No bias, 1 = Stretched (splitting short side)")]
    [Range(0f, 1f)]
    [SerializeField] private float splitDirectionBias;

    [Tooltip("Alters the odds of a room being split near its center or edges, resulting in evenly or unevenly sized children. " +
        "-1 = Even (splitting near center), 0 = No bias, 1 = Uneven (splitting near edges)")]
    [Range(-0.98f, 0.98f)]
    [SerializeField] private float splitLocationBias;

    [Header("Chambers")]

    [SerializeField] private bool addRandomChambers;

    [Range(0f, 1f)]
    [SerializeField] private float minChambers;

    [Range(0f, 1f)]
    [SerializeField] private float maxChambers;

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
        RecursiveSplit(ref firstRoom);
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
    /// Splits the provided room into smaller rooms recursively
    /// </summary>
    private void RecursiveSplit(ref RoomNode room)
    {
        // If the room is too big
        if (room.rect.width - roomPadding * 2 > maxRoomLength || room.rect.height - roomPadding * 2 > maxRoomLength)
        {
            // Try splitting it
            if (TrySplit(ref room))
                return;
            else
                Debug.LogWarning("Room was too big, but could not be split. " + room.rect.width + "x" + room.rect.height);
        }

        // If the room is not too big
        else
        {
            // 50% chance to try splitting it
            if (Random.Range(0f, 1f) < 0.5f && TrySplit(ref room))
                return;
        }

        // This room was not split: Add as leaf node
        finalRects.Add(room.rect);
        leafNodes.Add(room);
    }

    private bool TrySplit(ref RoomNode room)
    {
        // Check if big enough to be split

        bool horizontalSplitPossible = SplitPossible(room.rect.height, room.rect.width);
        bool verticalSplitPossible   = SplitPossible(room.rect.width, room.rect.height);

        // (height, width) = Test for horizontal split
        // (width, height) = Test for vertical split

        bool SplitPossible(int a, int b)
        {
            return a >= minRoomLength * 2 + roomPadding * 4 &&
                (b - roomPadding * 2) * (a * 0.5f - roomPadding * 2) >= minRoomArea;
        }

        // Can be split
        if (horizontalSplitPossible || verticalSplitPossible)
        {
            // If vertical not possible: Horizontal
            if (!verticalSplitPossible)
                SplitHorizontally(ref room);

            // If horizontal not possible: Vertical
            else if (!horizontalSplitPossible)
                SplitVertically(ref room);

            // If both directions possible: Use split bias
            else if (Random.Range(0f, 1f) < splitDirectionBias)
                SplitLongSide(ref room);

            else
                SplitShortSide(ref room);

            // Continue with recursive split on children
            RecursiveSplit(ref room.child1);
            RecursiveSplit(ref room.child2);

            return true;
        }

        // Can not be split
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Chooses direction of split randomly
    /// </summary>
    private void SplitRandomly(ref RoomNode room)
    {
        if (Random.Range(0f, 1f) < 0.5f)
            SplitHorizontally(ref room);
        else
            SplitVertically(ref room);
    }

    /// <summary>
    /// Splits the room by its short side
    /// </summary>
    private void SplitShortSide(ref RoomNode room)
    {
        if (room.rect.width == room.rect.height)
            SplitRandomly(ref room);

        else if (room.rect.width > room.rect.height)
            SplitVertically(ref room);

        else
            SplitHorizontally(ref room);
    }

    /// <summary>
    /// Splits the room by its long side
    /// </summary>
    private void SplitLongSide(ref RoomNode room)
    {
        if (room.rect.width == room.rect.height)
            SplitRandomly(ref room);

        else if (room.rect.width > room.rect.height)
            SplitHorizontally(ref room);

        else
            SplitVertically(ref room);
    }

    /// <summary>
    /// Splits the room into two children by splitting horizontally
    /// </summary>
    private void SplitHorizontally(ref RoomNode room)
    {
        int minRoomHeight = Mathf.Max(minRoomLength, minRoomArea / (room.rect.width - roomPadding * 2));
        int minY = minRoomHeight + roomPadding * 2;
        int maxY = room.rect.height - minRoomHeight - roomPadding * 2;
        int y = GetSplitLocation(minY, maxY);

        room.child1 = new RoomNode(room.rect.x, room.rect.y, room.rect.width, y);
        room.child2 = new RoomNode(room.rect.x, room.rect.y + y, room.rect.width, room.rect.height - y);
    }

    /// <summary>
    /// Splits the room into two children by splitting vertically
    /// </summary>
    private void SplitVertically(ref RoomNode room)
    {
        int minRoomWidth = Mathf.Max(minRoomLength, minRoomArea / (room.rect.height - roomPadding * 2));
        int minX = minRoomWidth + roomPadding * 2;
        int maxX = room.rect.width - minRoomWidth - roomPadding * 2;
        int x = GetSplitLocation(minX, maxX);

        room.child1 = new RoomNode(room.rect.x, room.rect.y, x, room.rect.height);
        room.child2 = new RoomNode(room.rect.x + x, room.rect.y, room.rect.width - x, room.rect.height);
    }

    /// <summary>
    /// Returns a location within the provided range using the location bias variable.
    /// </summary>
    private int GetSplitLocation(int min, int max)
    {
        float x = Random.Range(0f, 1f); // Random input

        float y = x.LerpValueCustomSmoothstep(splitLocationBias); // Biased output

        float floatingLocation = min + (max - min) * y; // Translate to floating point location

        int roundedLocation = (int)(floatingLocation + 0.5f); // Round to nearest integer

        return roundedLocation;
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
    /// Builds the map layout in the world using blocks
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
