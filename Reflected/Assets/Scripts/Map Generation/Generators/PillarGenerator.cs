using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PillarGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject pillarPrefab;

    [Header("Pillars")]

    [Range(0f, 10f)]
    [SerializeField] private float width;

    [Range(0f, 5f)]
    [SerializeField] private float protrudingDepth;

    [Range(0f, 5f)]
    [SerializeField] private float protrudingHeight;

    [Min(1)]
    [SerializeField] private int distance;

    private Vector3 wallPillarScale;
    private Vector3 chamberPillarScale;

    // Properties

    public float Width => width;

    public void Generate(Map map)
    {
        wallPillarScale = new Vector3(
            width,
            Wall.Height + protrudingHeight,
            Wall.Thickness + protrudingDepth * 2f);

        chamberPillarScale = new Vector3(
            width,
            Wall.Height + protrudingHeight,
            Mathf.Max(map.Chambers[0].Rect.width, map.Chambers[0].Rect.height) + protrudingDepth * 2f);

        float halfWallThickness = Wall.Thickness * 0.5f;

        Rect portionRect;
        Vector2 marker;
        bool edgePillars;

        foreach (Room room in map.Rooms)
        {
            foreach (Wall wall in room.Walls)
            {
                foreach (GameObject portion in wall.Portions)
                {
                    portionRect = new Rect(
                        portion.transform.position.x,
                        portion.transform.position.z,
                        portion.transform.localScale.x,
                        portion.transform.localScale.z);

                    // Clamp portionRect

                    if (wall.Orientation == Orientation.Horizontal)
                    {
                        portionRect.xMin = Mathf.Clamp(portionRect.xMin, room.Rect.xMin, room.Rect.xMax);
                        portionRect.xMax = Mathf.Clamp(portionRect.xMax, room.Rect.xMin, room.Rect.xMax);
                    }
                    else
                    {
                        portionRect.yMin = Mathf.Clamp(portionRect.yMin, room.Rect.yMin, room.Rect.yMax);
                        portionRect.yMax = Mathf.Clamp(portionRect.yMax, room.Rect.yMin, room.Rect.yMax);
                    }

                    // Set marker

                    if (wall.Orientation == Orientation.Horizontal)
                        marker = new Vector2(portionRect.x, portionRect.center.y);
                    else
                        marker = new Vector2(portionRect.center.x, portionRect.y);

                    // Instantiate pillars symmetrically, and contue doing who while there is space for it
                    edgePillars = true;
                    while (true)
                    {
                        InstantiatePillar(room, wall, edgePillars, marker);
                        InstantiatePillar(room, wall, edgePillars, marker.RotateAroundPivot(portionRect.center, Mathf.PI));
                        edgePillars = false;

                        if (Vector2.Distance(marker, portionRect.center) <= distance * 1.5f)
                            break;

                        marker += (portionRect.center - marker).normalized * distance;
                    }

                    // If there is space for exactly one more pillar, instansiate it
                    if (Vector2.Distance(marker, portionRect.center) >= distance)
                    {
                        InstantiatePillar(room, wall, edgePillars, portionRect.center);
                    }

                } // foreach portion

            } // foreach wall

        } // foreach room

        /*
        // Places pillars as walls to chambers
        foreach (Chamber chamber in map.Chambers)
        {
            if (chamber.Orientation == Orientation.Horizontal)
            {
                InstantiateChamberPillar(chamber, new Vector2(chamber.Rect.center.x, chamber.Rect.yMin));
                InstantiateChamberPillar(chamber, new Vector2(chamber.Rect.center.x, chamber.Rect.yMax));
            }
            else
            {
                InstantiateChamberPillar(chamber, new Vector2(chamber.Rect.xMin, chamber.Rect.center.y));
                InstantiateChamberPillar(chamber, new Vector2(chamber.Rect.xMax, chamber.Rect.center.y));
            }
        }
        */
    }

    private void InstantiatePillar(Room room, Wall wall, bool edgePillars, Vector2 position)
    {
        Transform parent = wall.transform;

        // If this is an edge pillar and this wall has multiple portions
        if (edgePillars && wall.Portions.Count > 1)
        {
            // Check if in chamber
            foreach (Chamber chamber in room.Chambers)
            {
                if (chamber.Rect.Inflated(0.01f, 0.01f).Contains(position))
                {
                    // Alt 1: Skip if chambers generate their own pillars
                    //return;

                    // Alt 2: Instantiate to pillar
                    parent = chamber.transform;
                    break;
                }
            }
        }

        Pillar pillar = GameObject.Instantiate(pillarPrefab, parent).GetComponent<Pillar>().Initialize();
        pillar.transform.position = new Vector3(position.x, 0f, position.y);
        pillar.transform.localScale = wallPillarScale;
        wall.Pillars.Add(pillar);

        if (wall.Orientation == Orientation.Vertical)
            pillar.transform.Rotate(0f, 90f, 0f);
    }

    private void InstantiateChamberPillar(Chamber chamber, Vector2 position)
    {
        Pillar pillar = GameObject.Instantiate(pillarPrefab, chamber.transform).GetComponent<Pillar>().Initialize();
        pillar.transform.position = new Vector3(position.x, 0f, position.y);
        pillar.transform.localScale = chamberPillarScale;
        chamber.Pillars.Add(pillar);

        if (chamber.Orientation == Orientation.Horizontal)
            pillar.transform.Rotate(0f, 90f, 0f);
    }

}
