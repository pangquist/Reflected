using System.Collections;
using System.Collections.Generic;
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

    public void Generate(Map map)
    {
        Wall.StaticInitialize(wallThickness, wallHeight);

        RectInt rect;

        foreach (Room room in map.Rooms)
        {
            room.CreateFloor(wallThickness);

            rect = room.Rect;

            room.Walls.Add(GameObject.Instantiate(wallPrefab, room.transform).GetComponent<Wall>().InitializeNorth(rect, true));
            room.Walls.Add(GameObject.Instantiate(wallPrefab, room.transform).GetComponent<Wall>().InitializeSouth(rect, true));
            room.Walls.Add(GameObject.Instantiate(wallPrefab, room.transform).GetComponent<Wall>().InitializeWest(rect, true));
            room.Walls.Add(GameObject.Instantiate(wallPrefab, room.transform).GetComponent<Wall>().InitializeEast(rect, true));
        }

        foreach (Chamber chamber in map.Chambers)
        {
            chamber.CreateFloor(wallThickness);

            if (chamber.ChamberOrientation == Chamber.Orientation.Horizontal)
            {
                rect = chamber.Rect.Inflated(-wallThickness, 0);
                chamber.Walls.Add(GameObject.Instantiate(wallPrefab, chamber.transform).GetComponent<Wall>().InitializeNorth(rect, true));
                chamber.Walls.Add(GameObject.Instantiate(wallPrefab, chamber.transform).GetComponent<Wall>().InitializeSouth(rect, true));
            }

            else
            {
                rect = chamber.Rect.Inflated(0, -wallThickness);
                chamber.Walls.Add(GameObject.Instantiate(wallPrefab, chamber.transform).GetComponent<Wall>().InitializeWest(rect, true));
                chamber.Walls.Add(GameObject.Instantiate(wallPrefab, chamber.transform).GetComponent<Wall>().InitializeEast(rect, true));
            }
        }
    }

}
