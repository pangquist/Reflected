using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject waterPrefab;

    [Header("Water")]

    [SerializeField] private float waterLevel;

    public void Generate(Map map)
    {
        Rect waterRect;

        foreach (Room room in map.Rooms)
        {
            waterRect = room.Rect.Inflated(Wall.Thickness, Wall.Thickness);

            GameObject.Instantiate(waterPrefab, room.transform).GetComponent<Water>().Initialize(waterRect, waterLevel);
        }

        foreach (Chamber chamber in map.Chambers)
        {
            if (chamber.Orientation == Orientation.Horizontal)
                waterRect = chamber.Rect.Inflated(0, Wall.Thickness);
            else
                waterRect = chamber.Rect.Inflated(Wall.Thickness, 0);

            GameObject.Instantiate(waterPrefab, chamber.transform).GetComponent<Water>().Initialize(waterRect, waterLevel);
        }
    }
}
