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

    [Range(0, 100)]
    [SerializeField] private int wallHeight;

    [Range(0, 50)]
    [SerializeField] private int wallThickness;

    [Range(0, 1f)][Tooltip("How much to shorten walls adjacent to chambers in order to avoid visual bugs")]
    [SerializeField] private float chamberOverlapInflation;

    [Header("Doors")]

    [Range(0f, 5f)]
    [SerializeField] private float doorThickness;

    [Range(0f, 2f)]
    [SerializeField] private float doorIndent;

    public float ChamberOverlapInflation => chamberOverlapInflation;

    private void Awake()
    {
        Wall.StaticInitialize(wallThickness, wallHeight);
        Door.StaticInitialize(doorThickness);
    }

    public void Generate(Map map)
    {
        Awake();

        Array cardinalDirections = Enum.GetValues(typeof(CardinalDirection));
        Rect roomRect, wallPortion, overlap, portion1, portion2;
        List<Rect> wallPortions = new List<Rect>();

        foreach (Room room in map.Rooms)
        {
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
                            overlap = overlap.Inflated(chamberOverlapInflation, chamberOverlapInflation); // Inflate overlap to avoid visual bugs

                            // Calculate two new portions

                            if (chamber.Orientation == Orientation.Horizontal)
                            {
                                portion1 = new Rect(wallPortion.x, wallPortion.y, wallPortion.width, overlap.y - wallPortion.y);
                                portion2 = new Rect(wallPortion.x, overlap.yMax, wallPortion.width, wallPortion.yMax - overlap.yMax);
                            }

                            else // (chamber.Orientation == Orientation.Vertical)
                            {
                                portion1 = new Rect(wallPortion.x, wallPortion.y, overlap.x - wallPortion.x, wallPortion.height);
                                portion2 = new Rect(overlap.xMax, wallPortion.y, wallPortion.xMax - overlap.xMax, wallPortion.height);
                            }

                            // Change the current portion and add a second
                            wallPortion = wallPortions[i] = portion1;
                            wallPortions.Add(portion2);

                            // Add door to Chamber
                            CardinalDirection doorDirection = direction.GetNext().GetNext();
                            chamber.AddDoor(InstantiateDoor(chamber, room, doorDirection, overlap));
                        }

                        // Continue looking for chambers overlapping this portion
                    }

                    // Move on to the next portion of this wall (if more were created)
                }

                // Add collected portions to the Wall

                foreach (Rect portion in wallPortions)
                    room.Walls[room.Walls.Count - 1].AddPortion(portion);

                // Move on to the next Wall
            }

            // Move on to the next Room
        }

        foreach (Chamber chamber in map.Chambers)
        {
            if (chamber.Orientation == Orientation.Horizontal)
            {
                roomRect = chamber.Rect.Inflated(-wallThickness * 1.5f, 0);
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.North, chamber.transform, roomRect));
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.South, chamber.transform, roomRect));
            }

            else
            {
                roomRect = chamber.Rect.Inflated(0, -wallThickness * 1.5f);
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.West, chamber.transform, roomRect));
                chamber.Walls.Add(InstantiateSinglePortionWall(CardinalDirection.East, chamber.transform, roomRect));
            }
        }
    }

    /// <summary>
    /// Returns the Rect of a Wall as seen from above
    /// </summary>
    private Rect GetWallRect(CardinalDirection direction, Rect roomRect)
    {
        switch (direction)
        {
            case CardinalDirection.North:
                return new Rect(roomRect.x - wallThickness, roomRect.y + roomRect.height, roomRect.width + wallThickness * 2, wallThickness);

            case CardinalDirection.South:
                return new Rect(roomRect.x - wallThickness, roomRect.y - wallThickness, roomRect.width + wallThickness * 2, wallThickness);

            case CardinalDirection.West:
                return new Rect(roomRect.x - wallThickness, roomRect.y - wallThickness, wallThickness, roomRect.height + wallThickness * 2);

            case CardinalDirection.East:
                return new Rect(roomRect.x + roomRect.width, roomRect.y - wallThickness, wallThickness, roomRect.height + wallThickness * 2);

            default:
                return new Rect();
        }
    }

    /// <summary>
    /// Instantiates, initializes, and returns a Wall with one portion
    /// </summary>
    private Wall InstantiateSinglePortionWall(CardinalDirection direction, Transform parentTransform, Rect roomRect)
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
    /// Returns the position of a door based of the overlap of a room and a chamber
    /// </summary>
    private Vector2 GetDoorPosition(CardinalDirection direction, Rect overlap)
    {
        if (direction == CardinalDirection.North)
            return new Vector2(overlap.center.x, overlap.yMax - doorThickness * 0.5f - doorIndent);

        else if (direction == CardinalDirection.East)
            return new Vector2(overlap.xMax - doorThickness * 0.5f - doorIndent, overlap.center.y);

        else if (direction == CardinalDirection.South)
            return new Vector2(overlap.center.x, overlap.yMin + doorThickness * 0.5f + doorIndent);

        else // (direction == CardinalDirection.West)
            return new Vector2(overlap.xMin + doorThickness * 0.5f + doorIndent, overlap.center.y);
    }

    /// <summary>
    /// Instantiates, initializes, and returns a door
    /// </summary>
    private Door InstantiateDoor(Chamber chamber, Room room, CardinalDirection direction, Rect overlap)
    {
        float width = direction == CardinalDirection.North || direction == CardinalDirection.South ? overlap.width : overlap.height;
        width -= mapGenerator.PillarGenerator.Width;

        return GameObject.Instantiate(doorPrefab, chamber.transform).GetComponent<Door>()
            .Initialize(direction, room, GetDoorPosition(direction, overlap), width);
    }

}
