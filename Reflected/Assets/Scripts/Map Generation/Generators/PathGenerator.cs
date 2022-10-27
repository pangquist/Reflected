using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject pathPrefab;

    [Header("Paths")]

    [SerializeField] private float level;
    [SerializeField] private float radius;
    [SerializeField] private float pathPointsFrequency;
    [SerializeField] private Color color;

    private const int anchorPoint1  = 0;
    private const int anchorPoint2  = 3;
    private const int controlPoint1 = 1;
    private const int controlPoint2 = 2;

    // Properties

    public float Level => level;
    public float Radius => radius;
    public Color Color => color;

    public void Generate(Map map)
    {
        Chamber chamber1, chamber2;

        foreach (Room room in map.Rooms)
        {
            if (room.Chambers.Count == 1)
            {
                CreatePoints(room, CreatePath(room, room.Chambers[0]));
                continue;
            }

            for (int i = 0; i < room.Chambers.Count; ++i)
            {
                chamber1 = room.Chambers[i];

                for (int j = i + 1; j < room.Chambers.Count; ++j)
                {
                    chamber2 = room.Chambers[j];
                    CreatePoints(room, CreatePath(room, chamber1, chamber2));
                }
            }
        }
    }

    private PathCreator CreatePath(Room room, Chamber chamber1, Chamber chamber2)
    {
        PathCreator path = Instantiate(pathPrefab, room.PathsChild).GetComponent<PathCreator>();
        path.name = "Path " + room.Paths.Count;
        room.Paths.Add(path);

        path.bezierPath.SetPoint(anchorPoint1, new Vector3(chamber1.Rect.center.x, level, chamber1.Rect.center.y));
        path.bezierPath.SetPoint(anchorPoint2, new Vector3(chamber2.Rect.center.x, level, chamber2.Rect.center.y));

        path.bezierPath.SetPoint(controlPoint1, GetControlPointPosition(chamber1));
        path.bezierPath.SetPoint(controlPoint2, GetControlPointPosition(chamber2));

        Vector3 GetControlPointPosition(Chamber chamber)
        {
            if (chamber.Orientation == Orientation.Horizontal)
                return new Vector3(room.Rect.center.x, level, chamber.Rect.center.y);
            else
                return new Vector3(chamber.Rect.center.x, level, room.Rect.center.y);
        }

        return path;
    }

    private PathCreator CreatePath(Room room, Chamber chamber)
    {
        PathCreator path = Instantiate(pathPrefab, room.PathsChild).GetComponent<PathCreator>();
        path.name = "Path " + room.Paths.Count;
        room.Paths.Add(path);

        path.bezierPath.SetPoint(anchorPoint1, new Vector3(room.Rect.center.x, level, room.Rect.center.y));
        path.bezierPath.SetPoint(anchorPoint2, new Vector3(chamber.Rect.center.x, level, chamber.Rect.center.y));

        path.bezierPath.SetPoint(controlPoint1, GetControlPointPosition(chamber));
        path.bezierPath.SetPoint(controlPoint2, GetControlPointPosition(chamber));

        Vector3 GetControlPointPosition(Chamber chamber)
        {
            if (chamber.Orientation == Orientation.Horizontal)
                return new Vector3(room.Rect.center.x + (chamber.Rect.center.x - room.Rect.center.x) * 0.5f, level, chamber.Rect.center.y);
            else
                return new Vector3(chamber.Rect.center.x, level, room.Rect.center.y + (chamber.Rect.center.y - room.Rect.center.y) * 0.5f);
        }

        return path;
    }

    private void CreatePoints(Room room, PathCreator path)
    {
        float nrOfPoints = path.path.length * pathPointsFrequency;

        for (float percentage = 0f; percentage <= 1f; percentage += 1f / nrOfPoints)
        {
            room.PathPoints.Add(path.path.GetPointAtDistance(path.path.length * percentage));
        }
    }

}
