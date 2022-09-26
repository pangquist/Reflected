using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [Header("Warnings")]

    [SerializeField] private bool logWarnigns;

    [Header("References")]

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject doorPrefab;

    [Header("Walls")]

    [Range(1, 10)]
    [SerializeField] private int wallHeight;

    [Range(1, 5)]
    [SerializeField] private int wallThickness;

    [Header("Doors")]

    [Range(0f, 1f)]
    [SerializeField] private float doorThickness;

    [Range(0f, 5f)]
    [SerializeField] private float doorTransitionDuration;

    public void Generate(Map map)
    {
        Wall.StaticInitialize(wallThickness, wallHeight);
        Door.StaticInitialize(doorThickness, doorTransitionDuration);

        Array cardinalDirections = Enum.GetValues(typeof(CardinalDirection));
        RectInt roomRect, wallPortion, overlap, portion1, portion2;
        List<RectInt> wallPortions = new List<RectInt>();

        foreach (Room room in map.Rooms)
        {
            room.CreateFloor(wallThickness);
            roomRect = room.Rect;

            foreach (CardinalDirection direction in cardinalDirections)
            {
                // Instantiate a wall without any portions
                room.Walls.Add(InstantiateWall(direction, room.transform));
                wallPortion = GetWallRect(direction, roomRect);

                // Start a new collection of wall portions
                wallPortions.Clear();
                wallPortions.Add(wallPortion);

                // Check all portions of this wall
                for (int i = 0; i < wallPortions.Count; ++i)
                {
                    wallPortion = wallPortions[i];

                    // Check connecting chambers
                    foreach (Chamber chamber in room.Chambers)
                    {
                        // If they overlap with the wall portion
                        if (wallPortion.Overlaps(chamber.Rect, out overlap))
                        {
                            // Calculate two new portions

                            if (chamber.Orientation == Orientation.Horizontal)
                            {
                                portion1 = new RectInt(wallPortion.x, wallPortion.y, wallPortion.width, overlap.y - wallPortion.y);
                                portion2 = new RectInt(wallPortion.x, overlap.Top(), wallPortion.width, wallPortion.Top() - overlap.Top());
                            }

                            else // (chamber.Orientation == Orientation.Vertical)
                            {
                                portion1 = new RectInt(wallPortion.x, wallPortion.y, overlap.x - wallPortion.x, wallPortion.height);
                                portion2 = new RectInt(overlap.Right(), wallPortion.y, wallPortion.Right() - overlap.Right(), wallPortion.height);
                            }

                            // Change the current portion and add a second
                            wallPortion = wallPortions[i] = portion1;
                            wallPortions.Add(portion2);

                            // Add door to Chamber
                            CardinalDirection doorDirection = (CardinalDirection)cardinalDirections.GetValue(((int)direction + 2) % 4);
                            chamber.Doors.Add(InstantiateDoor(chamber, room, doorDirection, overlap));
                        }

                        // Continue looking for chambers overlapping this portion
                    }

                    // Move on to the next portion of this wall (if more were created)
                }

                // Add collected portions to the Wall

                foreach (RectInt portion in wallPortions)
                {
                    room.Walls[room.Walls.Count - 1].AddPortion(portion);
                }

                // Move on to the next Wall
            }

            // Move on to the next Room
        }

        foreach (Chamber chamber in map.Chambers)
        {
            chamber.CreateFloor(wallThickness);

            if (chamber.Orientation == Orientation.Horizontal)
            {
                roomRect = chamber.Rect.Inflated(-wallThickness, 0);
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.North, chamber.transform, roomRect));
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.South, chamber.transform, roomRect));
            }

            else
            {
                roomRect = chamber.Rect.Inflated(0, -wallThickness);
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.West, chamber.transform, roomRect));
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.East, chamber.transform, roomRect));
            }
        }
    }

    /// <summary>
    /// Returns the RectInt of a Wall as seen from above
    /// </summary>
    private RectInt GetWallRect(CardinalDirection direction, RectInt roomRect)
    {
        switch (direction)
        {
            case CardinalDirection.North:
                return new RectInt(roomRect.x - wallThickness, roomRect.y + roomRect.height, roomRect.width + wallThickness * 2, wallThickness);

            case CardinalDirection.South:
                return new RectInt(roomRect.x - wallThickness, roomRect.y - wallThickness, roomRect.width + wallThickness * 2, wallThickness);

            case CardinalDirection.West:
                return new RectInt(roomRect.x - wallThickness, roomRect.y - wallThickness, wallThickness, roomRect.height + wallThickness * 2);

            case CardinalDirection.East:
                return new RectInt(roomRect.x + roomRect.width, roomRect.y - wallThickness, wallThickness, roomRect.height + wallThickness * 2);

            default:
                return new RectInt();
        }
    }

    /// <summary>
    /// Instantiates, initializes, and returns a Wall with one portion
    /// </summary>
    private Wall InstantiateSinglePortionWall(CardinalDirection direction, Transform parentTransform, RectInt roomRect)
    {
        return InstantiateWall(direction, parentTransform).AddPortion(GetWallRect(direction, roomRect));
    }

    /// <summary>
    /// Instantiates, initializes, and returns a Wall without any portions
    /// </summary>
    private Wall InstantiateWall(CardinalDirection direction, Transform parentTransform)
    {
        Wall wall = GameObject.Instantiate(wallPrefab, parentTransform).GetComponent<Wall>();
        wall.Initialize(direction);
        return wall;
    }

    /// <summary>
    /// Returns the Rect of a door as seen from above
    /// </summary>
    private Rect GetDoorRect(CardinalDirection direction, RectInt overlap)
    {
        Rect doorRect = new Rect();

        // Set position

        if (direction == CardinalDirection.South || direction == CardinalDirection.West)
            doorRect.position = new Vector2(overlap.x, overlap.y);

        else if (direction == CardinalDirection.North)
            doorRect.position = new Vector2(overlap.x, overlap.Top() - doorThickness * wallThickness);

        else // (direction == CardinalDirection.East)
            doorRect.position = new Vector2(overlap.Right() - doorThickness * wallThickness, overlap.y);

        // Set size

        if (direction == CardinalDirection.North || direction == CardinalDirection.South)
            doorRect.size = new Vector2(overlap.width, doorThickness * wallThickness);

        else // (direction == CardinalDirection.West || direction == CardinalDirection.East)
            doorRect.size = new Vector2(doorThickness * wallThickness, overlap.height);

        return doorRect;
    }

    /// <summary>
    /// Instantiates, initializes, and returns a door
    /// </summary>
    private Door InstantiateDoor(Chamber chamber, Room room, CardinalDirection direction, RectInt overlap)
    {
        return GameObject.Instantiate(doorPrefab, chamber.transform).GetComponent<Door>().Initialize(direction, GetDoorRect(direction, overlap), room);
    }

}
