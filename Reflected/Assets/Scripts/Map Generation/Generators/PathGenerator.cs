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

    private const int anchorPoint1  = 0;
    private const int anchorPoint2  = 3;
    private const int controlPoint1 = 1;
    private const int controlPoint2 = 2;

    public void Generate(Map map)
    {
        Chamber chamber1, chamber2;

        foreach (Room room in map.Rooms)
        {
            for (int i = 0; i < room.Chambers.Count; ++i)
            {
                chamber1 = room.Chambers[i];

                for (int j = i + 1; j < room.Chambers.Count; ++j)
                {
                    chamber2 = room.Chambers[j];
                    CreatePath(room, chamber1, chamber2);
                }
            }
        }
    }

    private void CreatePath(Room room, Chamber chamber1, Chamber chamber2)
    {
        PathCreator path = GameObject.Instantiate(pathPrefab, room.transform).GetComponent<PathCreator>();
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
    }

}
