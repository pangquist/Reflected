using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class PathGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject pathPrefab;

    [Header("Paths")]

    [SerializeField] private float level;
    [SerializeField] private float radius;
    [SerializeField] private float pathPointsFrequency;
    [SerializeField] private Color color;
    [SerializeField] private AnimationCurve amountBias;

    private const int anchorPoint1  = 0;
    private const int anchorPoint2  = 3;
    private const int controlPoint1 = 1;
    private const int controlPoint2 = 2;

    // Properties

    public static float Level { get; private set; }
    public static float Radius { get; private set; }
    public static Color Color { get; private set; }

    private void Awake()
    {
        Level = level;
        Radius = radius;
        Color = color;
    }

    public void Generate(Map map)
    {
        Chamber chamber1 = null;
        Chamber chamber2 = null;

        foreach (Room room in map.Rooms)
        {
            foreach (PathCreator path in room.GetComponentsInChildren<PathCreator>())
            {
                room.Paths.Add(path);
                CreatePoints(room, path);
            }

            if (room.Chambers.Count == 1)
            {
                CreatePoints(room, CreatePath(room, room.Chambers[0]));
                continue;
            }

            List<Chamber> unusedChambers = new List<Chamber>(room.Chambers);
            PairList<Chamber> chamberPairs = new PairList<Chamber>();
            int min = room.Chambers.Count - 1;
            int max = room.Chambers.Count * (room.Chambers.Count - 1) / 2;
            int pathsToCreate = (int)(min + (max + 0.9999f - min) * amountBias.Evaluate(Random.Range(0f, 1f)));

            while (chamberPairs.Count < pathsToCreate || unusedChambers.Count > 0)
            {
                do
                {
                    if (unusedChambers.Count > 0)
                        chamber1 = unusedChambers.GetRandom();
                    else
                        chamber1 = room.Chambers.GetRandom();

                    chamber2 = room.Chambers.GetRandom();
                }
                while (chamber1 == chamber2 || chamberPairs.Contains(chamber1, chamber2));

                unusedChambers.Remove(chamber1);
                unusedChambers.Remove(chamber2);
                chamberPairs.Add(chamber1, chamber2);
                CreatePoints(room, CreatePath(room, chamber1, chamber2));
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
            Vector3 pathPoint = path.path.GetPointAtDistance(path.path.length * percentage);
            room.PathPoints.Add(new Vector3(pathPoint.x, level, pathPoint.z));
        }
    }

}
