using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
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

    [Header("Warnings")]

    [SerializeField] private bool logWarnigns;

    [Header("References")]

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject roomPrefab;

    [Header("Rooms")]

    [Range(3, 50)]
    [SerializeField] private int minRoomLength;

    [Range(3, 50)]
    [SerializeField] private int maxRoomLength;

    [Range(1, 2500)]
    [SerializeField] private int minRoomArea;

    [Range(1, 10)]
    [SerializeField] private int roomPadding;

    [Tooltip("Alters the odds of a room being split its long or short side, resulting in square or stretched children. " +
        "0 = Square (splitting long side), 0.5 = No bias, 1 = Stretched (splitting short side)")]
    [Range(0f, 1f)]
    [SerializeField] private float splitDirectionBias;

    [Tooltip("Alters the odds of a room being split near its center or edges, resulting in evenly or unevenly sized children. " +
        "-1 = Even (splitting near center), 0 = No bias, 1 = Uneven (splitting near edges)")]
    [Range(-0.98f, 0.98f)]
    [SerializeField] private float splitLocationBias;

    private List<RectInt> rects = new List<RectInt>();

    // Properties

    public int RoomPadding => roomPadding;

    public void Generate(Map map)
    {
        // Clear data

        rects.Clear();

        // Generate rooms

        RoomNode firstRoom = new RoomNode(0, 0, map.SizeX, map.SizeZ);
        RecursiveSplit(ref firstRoom);
        ShrinkRooms();
        InstantiateRooms(map);

        mapGenerator.Log("Rooms: " + rects.Count);
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

            else if (logWarnigns)
                Debug.LogWarning("RooomGenerator: Room was too big, but could not be split. (Room size: " + room.rect.width + "x" + room.rect.height + ")");
        }

        // If the room is not too big
        else
        {
            // 50% chance to try splitting it
            if (Random.Range(0f, 1f) < 0.5f && TrySplit(ref room))
                return;
        }

        // This room was not split: Add as leaf node
        rects.Add(room.rect);
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
        RectInt rect;
        for (int i = rects.Count - 1; i >= 0; --i)
        {
            rect = rects[i];
            rects[i] = new RectInt(
                rect.x + roomPadding,
                rect.y + roomPadding,
                rect.width - roomPadding * 2,
                rect.height - roomPadding * 2);
        }
    }

    /// <summary>
    /// Instantiates all rooms as child objects of the map
    /// </summary>
    private void InstantiateRooms(Map map)
    {
        for (int i = 0; i < rects.Count; ++i)
        {
            map.Rooms.Add(GameObject.Instantiate(roomPrefab, map.transform).GetComponent<Room>().Initialize(rects[i], i));
        }
    }

}
