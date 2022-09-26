using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    public enum Orientation { Horizontal, Vertical }

    [Header("References")]

    [SerializeField] private GameObject floorPrefab;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private Room room1;
    [ReadOnly][SerializeField] private Room room2;
    [ReadOnly][SerializeField] private Orientation orientation;
    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private Floor floor;
    [ReadOnly][SerializeField] private List<Wall> walls;

    // Properties

    public Orientation ChamberOrientation => orientation;
    public List<Wall> Walls => walls;
    public RectInt Rect => rect;

    public Chamber Initialize(Room room1, Room room2, RectInt rect, Orientation orientation)
    {
        this.room1 = room1;
        this.room2 = room2;
        this.rect = rect;
        this.orientation = orientation;

        name = "Chamber " + room1.name.Replace("Room ", "") + "-" + room2.name.Replace("Room ", "");

        room1.LinkChamber(this);
        room2.LinkChamber(this);

        return this;
    }

    public void CreateFloor(int wallThickness)
    {
        RectInt floorRect;

        if (orientation == Orientation.Horizontal)
            floorRect = rect.Inflated(-wallThickness, wallThickness);
        else
            floorRect = rect.Inflated(wallThickness, -wallThickness);

        floor = GameObject.Instantiate(floorPrefab, transform).GetComponent<Floor>().Initialize(floorRect);
    }

}
