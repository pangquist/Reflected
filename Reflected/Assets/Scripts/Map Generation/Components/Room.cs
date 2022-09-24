using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject floorPrefab;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private Floor floor;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    
    // Properties

    public RectInt Rect => rect;
    public Floor Floor => floor;
    public List<Wall> Walls => walls;

    public Room Initialize(RectInt rect, int index)
    {
        this.rect = rect;
        name = "Room " + index;
        return this;
    }

    public void CreateFloor(int wallThickness)
    {
        floor = GameObject.Instantiate(floorPrefab, transform).GetComponent<Floor>().Initialize(rect.Inflated(wallThickness, wallThickness));
    }

    public void LinkChamber(Chamber chamber)
    {
        chambers.Add(chamber);
    }

}
